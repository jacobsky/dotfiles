#!/bin/bash

# skill-creator skeleton generation script
# Creates proper directory structure for new OpenCode skills

set -euo pipefail

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Default values
DEFAULT_LICENSE="MIT"
DEFAULT_COMPATIBILITY="opencode"

# Global variables
SKILL_NAME=""
SKILL_DESCRIPTION=""
SKILL_LICENSE="$DEFAULT_LICENSE"
SKILL_COMPATIBILITY="$DEFAULT_COMPATIBILITY"
OUTPUT_DIR=""
INCLUDE_EXAMPLES=false

# Logging functions
log_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

log_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

log_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Print usage information
print_usage() {
    cat << EOF
Usage: $0 [options] <skill_name> <description>

Creates a new OpenCode skill directory structure with valid SKILL.md file.

Arguments:
    skill_name     Name of the skill (lowercase alphanumeric with hyphens)
    description    Brief description of what the skill does

Options:
    -o, --output DIR       Output directory (default: current directory)
    -l, --license LICENSE   License type (default: MIT)
    -c, --compatibility COMP   Compatibility (default: opencode)
    -e, --examples         Include example files and directories
    -h, --help            Show this help message

Examples:
    $0 my-skill "Analyzes code for security vulnerabilities"
    $0 -o ./skills/ -e audit-tool "Performs security audits on repositories"
    $0 --license Apache-2.0 doc-generator "Generates API documentation from code"

EOF
}

# Parse command line arguments
parse_arguments() {
    while [[ $# -gt 0 ]]; do
        case $1 in
            -o|--output)
                OUTPUT_DIR="$2"
                shift 2
                ;;
            -l|--license)
                SKILL_LICENSE="$2"
                shift 2
                ;;
            -c|--compatibility)
                SKILL_COMPATIBILITY="$2"
                shift 2
                ;;
            -e|--examples)
                INCLUDE_EXAMPLES=true
                shift
                ;;
            -h|--help)
                print_usage
                exit 0
                ;;
            -*)
                log_error "Unknown option: $1"
                print_usage
                exit 1
                ;;
            *)
                if [[ -z "$SKILL_NAME" ]]; then
                    SKILL_NAME="$1"
                elif [[ -z "$SKILL_DESCRIPTION" ]]; then
                    SKILL_DESCRIPTION="$1"
                else
                    log_error "Too many arguments"
                    print_usage
                    exit 1
                fi
                shift
                ;;
        esac
    done
    
    # Validate required arguments
    if [[ -z "$SKILL_NAME" ]]; then
        log_error "Skill name is required"
        print_usage
        exit 1
    fi
    
    if [[ -z "$SKILL_DESCRIPTION" ]]; then
        log_error "Skill description is required"
        print_usage
        exit 1
    fi
    
    # Set default output directory
    if [[ -z "$OUTPUT_DIR" ]]; then
        OUTPUT_DIR="."
    fi
    
    log_info "Skill name: $SKILL_NAME"
    log_info "Description: $SKILL_DESCRIPTION"
    log_info "Output directory: $OUTPUT_DIR"
    log_info "License: $SKILL_LICENSE"
    log_info "Compatibility: $SKILL_COMPATIBILITY"
}

# Validate skill name
validate_skill_name() {
    log_info "Validating skill name..."
    
    # Check naming pattern
    if [[ ! "$SKILL_NAME" =~ ^[a-z0-9]+(-[a-z0-9]+)*$ ]]; then
        log_error "Skill name must match pattern: ^[a-z0-9]+(-[a-z0-9]+)*$"
        log_error "Name must be lowercase alphanumeric with single hyphen separators"
        exit 1
    fi
    
    # Check name length
    if [[ ${#SKILL_NAME} -lt 1 ]] || [[ ${#SKILL_NAME} -gt 64 ]]; then
        log_error "Skill name length must be between 1 and 64 characters (current: ${#SKILL_NAME})"
        exit 1
    fi
    
    log_success "Skill name is valid"
}

# Check if skill directory already exists
check_existing_skill() {
    local skill_path="$OUTPUT_DIR/$SKILL_NAME"
    
    if [[ -d "$skill_path" ]]; then
        log_error "Skill directory already exists: $skill_path"
        log_info "Choose a different name or remove the existing directory"
        exit 1
    fi
    
    log_info "Skill directory location: $skill_path"
}

# Create directory structure
create_directory_structure() {
    local skill_path="$OUTPUT_DIR/$SKILL_NAME"
    
    log_info "Creating directory structure..."
    
    # Create main skill directory
    mkdir -p "$skill_path"
    log_success "Created: $skill_path/"
    
    # Create optional directories
    local optional_dirs=("scripts" "references" "assets")
    for dir in "${optional_dirs[@]}"; do
        mkdir -p "$skill_path/$dir"
        log_success "Created: $skill_path/$dir/"
    done
    
    # Create assets subdirectories
    mkdir -p "$skill_path/assets/templates"
    mkdir -p "$skill_path/assets/examples"
    log_success "Created: $skill_path/assets/templates/"
    log_success "Created: $skill_path/assets/examples/"
}

# Generate frontmatter YAML
generate_frontmatter() {
    cat << EOF
---
name: $SKILL_NAME
description: $SKILL_DESCRIPTION
license: $SKILL_LICENSE
compatibility: $SKILL_COMPATIBILITY
metadata:
  workflow: interactive-guidance
  parallel: false
  output: structured-response
---

EOF
}

# Generate basic SKILL.md content
generate_skill_content() {
    cat << 'EOF'
## What I do

- [Add bullet points describing what this skill does]
- [Focus on specific capabilities and actions]
- [Use action-oriented language]
- [Be specific about the skill's purpose]

## When to use me

Use this skill when you need to:
- [Describe specific scenarios where this skill is valuable]
- [List the problems this skill solves]
- [Explain when this skill is the right choice]

## How I work

### Phase 1: Initial Analysis

- [Describe the first phase of work]
- [Explain what happens during this phase]
- [Mention any tools or approaches used]

### Phase 2: Processing

- [Describe the main processing phase]
- [Explain how the skill operates on inputs]
- [Detail any intermediate steps]

### Phase 3: Output Generation

- [Describe how results are produced]
- [Explain the output format and structure]
- [Mention any post-processing or validation]

## Execution Pattern

### Agent Selection

- **Primary Agent**: [Specify which agent handles the main work]
- **Support Agents**: [List any additional agents used]

### Tool Usage

- [List the main tools this skill uses]
- [Explain how tools are combined]
- [Mention any special tool configurations]

## Output Format

[Describe the structure and format of the skill's output]

## Error Handling

- [Describe how errors are handled]
- [Explain fallback mechanisms]
- [Mention any graceful degradation strategies]

## Notes

- [Add any additional notes or constraints]
- [Mention any dependencies or requirements]
- [Include any tips for effective usage]

EOF
}

# Create SKILL.md file
create_skill_file() {
    local skill_path="$OUTPUT_DIR/$SKILL_NAME"
    local skill_file="$skill_path/SKILL.md"
    
    log_info "Creating SKILL.md file..."
    
    # Generate the complete file content
    {
        generate_frontmatter
        generate_skill_content
    } > "$skill_file"
    
    log_success "Created: $skill_file"
}

# Create example files if requested
create_example_files() {
    if [[ "$INCLUDE_EXAMPLES" == false ]]; then
        return
    fi
    
    local skill_path="$OUTPUT_DIR/$SKILL_NAME"
    
    log_info "Creating example files..."
    
    # Create example script
    cat << 'EOF' > "$skill_path/scripts/example-task.sh"
#!/bin/bash

# Example task script for the skill
# Replace with your actual implementation

set -euo pipefail

echo "Running example task for skill..."
echo "Replace this with your actual implementation logic"

# Add your script logic here
# For example:
# - File processing
# - API calls
# - Data analysis
# - Output generation

exit 0
EOF
    chmod +x "$skill_path/scripts/example-task.sh"
    log_success "Created: $skill_path/scripts/example-task.sh"
    
    # Create example reference documentation
    cat << 'EOF' > "$skill_path/references/domain-knowledge.md"
# Domain Knowledge

This file contains domain-specific information that the skill can reference.

## Key Concepts

- **Concept 1**: Description of important concept
- **Concept 2**: Another relevant concept with details
- **Concept 3**: Third concept with examples

## Best Practices

1. Practice 1 with explanation
2. Practice 2 with rationale
3. Practice 3 with implementation notes

## Resources

- [Link to external documentation]
- [Reference to relevant specifications]
- [Links to helpful tutorials or guides]
EOF
    log_success "Created: $skill_path/references/domain-knowledge.md"
    
    # Create example template
    cat << 'EOF' > "$skill_path/assets/templates/output-template.md"
# Output Template

This is a template for formatting the skill's output.

## Section 1

[Content for section 1]

## Section 2

[Content for section 2]

## Summary

[Summary of results]

## Next Steps

[Recommended next actions]
EOF
    log_success "Created: $skill_path/assets/templates/output-template.md"
    
    # Create README for examples
    cat << 'EOF' > "$skill_path/assets/examples/README.md"
# Skill Examples

This directory contains examples and demos of the skill in action.

## Example 1: Basic Usage

Description of basic usage example with expected output.

## Example 2: Advanced Usage

Description of advanced usage example with configuration options.

## Example 3: Integration

Description of how to integrate this skill with other tools or workflows.
EOF
    log_success "Created: $skill_path/assets/examples/README.md"
}

# Create validation wrapper script
create_validation_wrapper() {
    local skill_path="$OUTPUT_DIR/$SKILL_NAME"
    local skill_creator_path="$(dirname "$(dirname "$(realpath "$0")")")"
    
    # Create a simple validation script that calls the main validator
    cat << EOF > "$skill_path/scripts/validate.sh"
#!/bin/bash

# Validation wrapper for $SKILL_NAME skill
# This script validates the skill structure and compliance

# Get the path to the skill-creator validation script
SKILL_CREATOR_PATH="$skill_creator_path"
VALIDATOR_SCRIPT="\$SKILL_CREATOR_PATH/scripts/validate-structure.sh"

if [[ -f "\$VALIDATOR_SCRIPT" ]]; then
    exec "\$VALIDATOR_SCRIPT" "\$(realpath "\$(dirname "\$0")/..")"
else
    echo "Error: Validation script not found at \$VALIDATOR_SCRIPT"
    exit 1
fi
EOF
    chmod +x "$skill_path/scripts/validate.sh"
    log_success "Created: $skill_path/scripts/validate.sh"
}

# Print completion message
print_completion_message() {
    local skill_path="$OUTPUT_DIR/$SKILL_NAME"
    
    echo
    log_success "✓ Skill '$SKILL_NAME' created successfully!"
    echo
    log_info "Skill Location: $skill_path"
    echo
    log_info "Next Steps:"
    echo "1. Edit $skill_path/SKILL.md to customize the skill content"
    echo "2. Add your implementation to the scripts/ directory"
    echo "3. Add documentation to references/ if needed"
    echo "4. Test your skill with: $skill_path/scripts/validate.sh"
    echo "5. Use your skill with: skill({name: \"$SKILL_NAME\"})"
    echo
    
    if [[ "$INCLUDE_EXAMPLES" == true ]]; then
        log_info "Example files have been included to help you get started."
        log_info "You can remove or modify them as needed."
    fi
    
    echo
    log_info "Happy skill building! 🚀"
}

# Main function
main() {
    echo "OpenCode Skill Creator"
    echo "====================="
    echo
    
    parse_arguments "$@"
    validate_skill_name
    check_existing_skill
    create_directory_structure
    create_skill_file
    create_example_files
    create_validation_wrapper
    print_completion_message
}

# Run main function with all arguments
main "$@"