#!/bin/bash

# skill-creator validation script
# Validates skill naming conventions, structure, and required fields

set -euo pipefail

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Global variables
VALIDATION_ERRORS=0
VALIDATION_WARNINGS=0
SKILL_PATH=""
SKILL_NAME=""

# Logging functions
log_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

log_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

log_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
    ((VALIDATION_WARNINGS++))
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
    ((VALIDATION_ERRORS++))
}

# Print usage information
print_usage() {
    cat << EOF
Usage: $0 <skill_path>

Validates an OpenCode skill structure and compliance.

Arguments:
    skill_path    Path to the skill directory to validate

Example:
    $0 /path/to/my-skill
    $0 ./my-skill

EOF
}

# Check if required arguments are provided
check_arguments() {
    if [[ $# -eq 0 ]]; then
        log_error "No skill path provided"
        print_usage
        exit 1
    fi
    
    SKILL_PATH="$1"
    
    if [[ ! -d "$SKILL_PATH" ]]; then
        log_error "Skill directory does not exist: $SKILL_PATH"
        exit 1
    fi
    
    # Extract skill name from directory (using absolute path to avoid basename issues)
    SKILL_NAME=$(basename "$(realpath "$SKILL_PATH")")
    log_info "Validating skill: $SKILL_NAME"
    log_info "Skill path: $SKILL_PATH"
}

# Validate skill naming conventions
validate_naming() {
    log_info "Validating naming conventions..."
    
    # Check skill name pattern (lowercase alphanumeric with hyphens)
    if [[ ! "$SKILL_NAME" =~ ^[a-z0-9]+(-[a-z0-9]+)*$ ]]; then
        log_error "Skill name '$SKILL_NAME' does not match required pattern: ^[a-z0-9]+(-[a-z0-9]+)*$"
        log_error "Name must be lowercase alphanumeric with single hyphen separators"
    else
        log_success "Skill name follows naming convention"
    fi
    
    # Check name length (1-64 characters)
    if [[ ${#SKILL_NAME} -lt 1 ]] || [[ ${#SKILL_NAME} -gt 64 ]]; then
        log_error "Skill name length must be between 1 and 64 characters (current: ${#SKILL_NAME})"
    else
        log_success "Skill name length is appropriate"
    fi
}

# Validate directory structure
validate_structure() {
    log_info "Validating directory structure..."
    
    # Check for required SKILL.md file
    local skill_file="$SKILL_PATH/SKILL.md"
    if [[ ! -f "$skill_file" ]]; then
        log_error "Required SKILL.md file not found in skill directory"
        return
    else
        log_success "SKILL.md file exists"
    fi
    
    # Check for optional directories
    local optional_dirs=("scripts" "references" "assets")
    for dir in "${optional_dirs[@]}"; do
        local dir_path="$SKILL_PATH/$dir"
        if [[ -d "$dir_path" ]]; then
            log_success "Optional $dir/ directory exists"
            
            # Check for common subdirectories in assets/
            if [[ "$dir" == "assets" ]]; then
                local asset_subdirs=("templates" "examples")
                for subdir in "${asset_subdirs[@]}"; do
                    local subdir_path="$dir_path/$subdir"
                    if [[ -d "$subdir_path" ]]; then
                        log_success "Assets $subdir/ subdirectory exists"
                    fi
                done
            fi
        fi
    done
    
    # Check for unexpected files/directories in root
    local allowed_root_files=("SKILL.md")
    local allowed_root_dirs=("scripts" "references" "assets")
    
    while IFS= read -r -d '' item; do
        local item_name=$(basename "$item")
        if [[ -f "$item" ]]; then
            if [[ ! " ${allowed_root_files[*]} " =~ " ${item_name} " ]]; then
                log_warning "Unexpected file in skill root: $item_name"
            fi
        elif [[ -d "$item" ]]; then
            if [[ ! " ${allowed_root_dirs[*]} " =~ " ${item_name} " ]]; then
                log_warning "Unexpected directory in skill root: $item_name/"
            fi
        fi
    done < <(find "$SKILL_PATH" -maxdepth 1 -mindepth 1 -print0)
}

# Validate frontmatter in SKILL.md
validate_frontmatter() {
    log_info "Validating frontmatter..."
    
    local skill_file="$SKILL_PATH/SKILL.md"
    
    # Check if file has frontmatter (starts with ---)
    if ! head -n 1 "$skill_file" | grep -q "^---$"; then
        log_error "SKILL.md does not start with frontmatter (---)"
        return
    fi
    
    # Extract frontmatter
    local frontmatter_end=false
    local frontmatter=""
    local line_num=0
    
    while IFS= read -r line; do
        ((line_num++))
        
        if [[ $line_num -eq 1 ]]; then
            continue  # Skip first line (already checked)
        fi
        
        if [[ "$line" == "---" ]]; then
            frontmatter_end=true
            break
        fi
        
        frontmatter="${frontmatter}${line}\n"
    done < "$skill_file"
    
    if [[ "$frontmatter_end" == false ]]; then
        log_error "Frontmatter not properly closed (missing closing ---)"
        return
    fi
    
    # Validate YAML structure
    if command -v python3 >/dev/null 2>&1; then
        echo -e "$frontmatter" | python3 -c "
import yaml
import sys
try:
    yaml.safe_load(sys.stdin.read())
    print('YAML syntax is valid')
except yaml.YAMLError as e:
    print(f'YAML syntax error: {e}', file=sys.stderr)
    sys.exit(1)
" 2>/dev/null || log_error "Invalid YAML syntax in frontmatter"
    fi
    
    # Check required fields
    local required_fields=("name" "description")
    for field in "${required_fields[@]}"; do
        if echo -e "$frontmatter" | grep -q "^[[:space:]]*$field[[:space:]]*:"; then
            log_success "Required field '$field' found in frontmatter"
        else
            log_error "Required field '$field' missing from frontmatter"
        fi
    done
    
    # Check optional common fields
    local optional_fields=("license" "compatibility" "metadata")
    for field in "${optional_fields[@]}"; do
        if echo -e "$frontmatter" | grep -q "^[[:space:]]*$field[[:space:]]*:"; then
            log_success "Optional field '$field' found in frontmatter"
        fi
    done
    
    # Validate name field matches directory name
    local frontmatter_name=$(echo -e "$frontmatter" | grep "^[[:space:]]*name[[:space:]]*:" | cut -d':' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' | tr -d '"')
    if [[ "$frontmatter_name" == "$SKILL_NAME" ]]; then
        log_success "Frontmatter name matches directory name"
    else
        log_error "Frontmatter name '$frontmatter_name' does not match directory name '$SKILL_NAME'"
    fi
    
    # Validate description length (should be 1-1024 characters)
    local description=$(echo -e "$frontmatter" | grep "^[[:space:]]*description[[:space:]]*:" | cut -d':' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' | tr -d '"')
    if [[ ${#description} -ge 1 ]] && [[ ${#description} -le 1024 ]]; then
        log_success "Description length is appropriate (${#description} characters)"
    else
        log_error "Description length must be between 1 and 1024 characters (current: ${#description})"
    fi
}

# Validate SKILL.md content structure
validate_content() {
    log_info "Validating SKILL.md content structure..."
    
    local skill_file="$SKILL_PATH/SKILL.md"
    
    # Check for main sections
    local sections=("## What I do" "## When to use me" "## How I work")
    for section in "${sections[@]}"; do
        if grep -q "^$section$" "$skill_file"; then
            log_success "Required section found: $section"
        else
            log_warning "Recommended section missing: $section"
        fi
    done
    
    # Check for bullet points in "What I do" section
    if grep -A 10 "^## What I do$" "$skill_file" | grep -q "^[[:space:]]*- "; then
        log_success "What I do section contains bullet points"
    else
        log_warning "What I do section should contain bullet points"
    fi
    
    # Check file size (should be reasonable for context)
    local file_size=$(wc -l < "$skill_file")
    if [[ $file_size -le 500 ]]; then
        log_success "SKILL.md length is appropriate for context optimization ($file_size lines)"
    else
        log_warning "SKILL.md is quite long ($file_size lines). Consider moving detailed content to references/ directory"
    fi
}

# Validate scripts directory
validate_scripts() {
    local scripts_dir="$SKILL_PATH/scripts"
    
    if [[ ! -d "$scripts_dir" ]]; then
        return  # scripts/ is optional
    fi
    
    log_info "Validating scripts directory..."
    
    # Check script permissions
    while IFS= read -r -d '' script; do
        if [[ -f "$script" ]]; then
            if [[ -x "$script" ]]; then
                log_success "Script is executable: $(basename "$script")"
            else
                log_warning "Script is not executable: $(basename "$script")"
            fi
        fi
    done < <(find "$scripts_dir" -type f -print0)
}

# Generate validation summary
generate_summary() {
    echo
    log_info "Validation Summary"
    echo "===================="
    
    if [[ $VALIDATION_ERRORS -eq 0 ]]; then
        log_success "✓ Skill validation passed with no errors"
    else
        log_error "✗ Skill validation failed with $VALIDATION_ERRORS error(s)"
    fi
    
    if [[ $VALIDATION_WARNINGS -gt 0 ]]; then
        log_warning "⚠ $VALIDATION_WARNINGS warning(s) found"
    fi
    
    echo
    
    if [[ $VALIDATION_ERRORS -eq 0 ]]; then
        log_info "The skill '$SKILL_NAME' is ready for use with OpenCode!"
    else
        log_info "Please fix the errors above before using this skill with OpenCode."
    fi
    
    # Exit with appropriate code
    if [[ $VALIDATION_ERRORS -gt 0 ]]; then
        exit 1
    else
        exit 0
    fi
}

# Main validation function
main() {
    echo "OpenCode Skill Validator"
    echo "======================="
    echo
    
    check_arguments "$@"
    validate_naming
    validate_structure
    validate_frontmatter
    validate_content
    validate_scripts
    generate_summary
}

# Run main function with all arguments
main "$@"