#!/bin/bash

# test-imgui-components.sh - Test ImGui component functionality
# Validates ImGui components for common issues and best practices

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}🧪 Testing ImGui Components...${NC}"

# Function to count issues
ISSUES=0
add_issue() {
    echo -e "${RED}❌ $1${NC}"
    ((ISSUES++))
}

add_success() {
    echo -e "${GREEN}✅ $1${NC}"
}

add_warning() {
    echo -e "${YELLOW}⚠️  $1${NC}"
}

# Test 1: Check for basic ImGui patterns
echo -e "${BLUE}📋 Testing Basic ImGui Patterns:${NC}"

check_imgui_patterns() {
    local file="$1"
    
    # Check for ImGui.Begin/End pairs
    if grep -q "ImGui.Begin" "$file" 2>/dev/null; then
        if grep -q "ImGui.End" "$file" 2>/dev/null; then
            add_success "ImGui.Begin/End pattern found in $file"
        else
            add_issue "ImGui.Begin without ImGui.End in $file"
        fi
    else
        add_warning "No ImGui.Begin found in $file (may be intentional)"
    fi
    
    # Check for proper ID handling
    if grep -q "ImGui.Button" "$file" 2>/dev/null; then
        if grep -q "PushID\|##" "$file" 2>/dev/null; then
            add_success "ID handling found in $file"
        else
            add_warning "Potential ID conflicts in $file - consider using PushID or ##suffix"
        fi
    fi
    
    # Check for proper styling
    if grep -q "PushStyle" "$file" 2>/dev/null; then
        if grep -q "PopStyle" "$file" 2>/dev/null; then
            add_success "Proper style push/pop found in $file"
        else
            add_issue "PushStyle without PopStyle in $file"
        fi
    fi
}

# Test all C# files
for csfile in assets/templates/*.cs assets/examples/*.cs; do
    if [[ -f "$csfile" ]]; then
        check_imgui_patterns "$csfile"
    fi
done

echo -e "${BLUE}📊 Testing Performance Patterns:${NC}"

# Test 2: Performance considerations
check_performance_patterns() {
    local file="$1"
    
    # Check for expensive operations in Draw methods
    if grep -A 10 -B 2 "Draw()" "$file" 2>/dev/null | grep -q "File\|GC\|Task\.Run"; then
        add_warning "Potentially expensive operations in Draw method in $file"
    fi
    
    # Check for update throttling
    if grep -q "DateTime\|TimeSpan" "$file" 2>/dev/null; then
        add_success "Update throttling patterns found in $file"
    else
        add_warning "No update throttling found in $file - may impact performance"
    fi
    
    # Check for IDisposable implementation
    if grep -q "class.*:" "$file" 2>/dev/null; then
        if grep -q "IDisposable" "$file" 2>/dev/null; then
            if grep -q "Dispose()" "$file" 2>/dev/null; then
                add_success "Proper IDisposable implementation in $file"
            else
                add_issue "IDisposable without Dispose method in $file"
            fi
        else
            add_warning "Class may not implement IDisposable in $file"
        fi
    fi
}

for csfile in assets/templates/*.cs assets/examples/*.cs; do
    if [[ -f "$csfile" ]]; then
        check_performance_patterns "$csfile"
    fi
done

echo -e "${BLUE}🎨 Testing UI/UX Patterns:${NC}"

# Test 3: User experience considerations
check_ux_patterns() {
    local file="$1"
    
    # Check for tooltips
    if grep -q "SetTooltip\|IsItemHovered" "$file" 2>/dev/null; then
        add_success "Tooltip support found in $file"
    else
        add_warning "No tooltips found in $file - consider adding for better UX"
    fi
    
    # Check for error handling
    if grep -q "try\|catch\|Exception" "$file" 2>/dev/null; then
        add_success "Error handling found in $file"
    else
        add_warning "No error handling found in $file"
    fi
    
    # Check for user feedback
    if grep -q "Logger\|ChatGui\|Print" "$file" 2>/dev/null; then
        add_success "User feedback mechanisms found in $file"
    else
        add_warning "No user feedback found in $file - consider adding notifications"
    fi
}

for csfile in assets/templates/*.cs assets/examples/*.cs; do
    if [[ -f "$csfile" ]]; then
        check_ux_patterns "$csfile"
    fi
done

echo -e "${BLUE}🔗 Testing Dalamud Integration:${NC}"

# Test 4: Dalamud-specific patterns
check_dalamud_patterns() {
    local file="$1"
    
    # Check for Dalamud imports
    if grep -q "using Dalamud" "$file" 2>/dev/null; then
        add_success "Dalamud imports found in $file"
    else
        add_warning "No Dalamud imports found in $file"
    fi
    
    # Check for Dalamud helpers
    if grep -q "ImGuiHelpers\|ImGuiExtensions" "$file" 2>/dev/null; then
        add_success "Dalamud ImGui helpers used in $file"
    else
        add_warning "No Dalamud ImGui helpers found in $file - consider using them for better integration"
    fi
    
    # Check for proper service usage
    if grep -q "Service\.\|_clientState\|_dataManager" "$file" 2>/dev/null; then
        add_success "Dalamud services used in $file"
    else
        add_warning "No Dalamud service usage found in $file"
    fi
}

for csfile in assets/templates/*.cs assets/examples/*.cs; do
    if [[ -f "$csfile" ]]; then
        check_dalamud_patterns "$csfile"
    fi
done

echo -e "${BLUE}📝 Code Quality Tests:${NC}"

# Test 5: Code quality and structure
check_code_quality() {
    local file="$1"
    
    # Check for documentation comments
    if grep -q "///" "$file" 2>/dev/null; then
        add_success "Documentation comments found in $file"
    else
        add_warning "No documentation comments found in $file"
    fi
    
    # Check for region usage
    if grep -q "#region" "$file" 2>/dev/null; then
        add_success "Code organization with regions found in $file"
    fi
    
    # Check for const/readonly usage
    if grep -q "const\|readonly" "$file" 2>/dev/null; then
        add_success "Immutable field usage found in $file"
    fi
    
    # Check for magic numbers
    local magic_numbers=$(grep -oE '\b[0-9]+\b' "$file" | grep -v -E '[0-9]{1,2}\b' | wc -l)
    if [[ $magic_numbers -gt 10 ]]; then
        add_warning "Many magic numbers found in $file - consider using constants"
    fi
}

for csfile in assets/templates/*.cs assets/examples/*.cs; do
    if [[ -f "$csfile" ]]; then
        check_code_quality "$csfile"
    fi
done

echo -e "${BLUE}🔧 Template Generation Test:${NC}"

# Test 6: Template generation script
test_template_script() {
    if [[ -f "scripts/generate-ui-template.sh" ]]; then
        if [[ -x "scripts/generate-ui-template.sh" ]]; then
            add_success "Template generation script is executable"
            
            # Test script with different parameters
            echo -e "${BLUE}Testing template generation...${NC}"
            
            # Test basic template generation
            if timeout 10s scripts/generate-ui-template.sh basic-window --level 1 --namespace Test.UI --class TestWindow > /dev/null 2>&1; then
                add_success "Basic template generation works"
            else
                add_issue "Basic template generation failed"
            fi
            
            # Clean up generated test file
            rm -f TestWindow.cs
        else
            add_issue "Template generation script is not executable"
        fi
    else
        add_issue "Template generation script not found"
    fi
}

test_template_script

echo -e "${BLUE}📊 Component Statistics:${NC}"

# Generate statistics
TOTAL_FILES=$(find assets/ -name "*.cs" | wc -l)
TEMPLATE_FILES=$(find assets/templates/ -name "*.cs" 2>/dev/null | wc -l)
EXAMPLE_FILES=$(find assets/examples/ -name "*.cs" 2>/dev/null | wc -l)
TOTAL_LINES=$(find assets/ -name "*.cs" -exec wc -l {} + 2>/dev/null | tail -1 | awk '{print $1}' || echo 0)

echo -e "${BLUE}📈 Component Library Statistics:${NC}"
echo "  Total C# files: $TOTAL_FILES"
echo "  Template files: $TEMPLATE_FILES"
echo "  Example files: $EXAMPLE_FILES"
echo "  Total lines of code: $TOTAL_LINES"

# Count unique ImGui widgets used
WIDGET_TYPES=$(grep -h -o "ImGui\.[A-Za-z]*" assets/*.cs assets/**/*.cs 2>/dev/null | sort -u | wc -l)
echo "  Unique ImGui widgets: $WIDGET_TYPES"

# Count Dalamud helpers used
DALAMUD_HELPERS=$(grep -h -c "ImGuiHelpers\|ImGuiExtensions" assets/*.cs assets/**/*.cs 2>/dev/null | awk '{sum+=$1} END {print sum}' || echo 0)
echo "  Dalamud helper usages: $DALAMUD_HELPERS"

echo -e "${BLUE}🎯 Recommendations:${NC}"

if [[ $ISSUES -eq 0 ]]; then
    echo -e "${GREEN}🎉 All tests passed! Components look great.${NC}"
else
    echo -e "${YELLOW}📋 Found $ISSUES issues to address:${NC}"
    echo "   • Review ID handling in ImGui components"
    echo "   • Add proper error handling and user feedback"
    echo "   • Implement update throttling for performance"
    echo "   • Ensure proper resource disposal"
    echo "   • Add tooltips and documentation"
fi

echo -e "${BLUE}📚 Next Steps:${NC}"
echo "   • Review all warnings and consider improvements"
echo "   • Test components in actual Dalamud environment"
echo "   • Validate component behavior with real game data"
echo "   • Consider adding more advanced examples"
echo "   • Document any custom patterns or conventions used"

echo -e "${GREEN}✅ Component testing completed!${NC}"

exit $ISSUES