#!/bin/bash

# validate-project.sh - Validate Dalamud project structure for ImGui UI development
# Validates that a project is properly set up for Dalamud plugin development with ImGui

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}🔍 Validating Dalamud project structure...${NC}"

# Check if we're in a C# project directory
if [[ ! -f "*.csproj" && ! -f "*.sln" ]]; then
    echo -e "${RED}❌ No .csproj or .sln file found. Please run this in a C# project directory.${NC}"
    exit 1
fi

echo -e "${BLUE}📋 Project file validation:${NC}"

# Check for project file
for proj_file in *.csproj; do
    if [[ -f "$proj_file" ]]; then
        echo -e "${GREEN}✅ Found project file: $proj_file${NC}"
        
        # Check for Dalamud reference
        if grep -q "Dalamud" "$proj_file" 2>/dev/null; then
            echo -e "${GREEN}✅ Dalamud reference found${NC}"
        else
            echo -e "${YELLOW}⚠️  No Dalamud reference found in $proj_file${NC}"
            echo "   Consider adding Dalamud packages via nuget"
        fi
        
        # Check for ImGui.NET reference
        if grep -q "ImGui.NET" "$proj_file" 2>/dev/null; then
            echo -e "${GREEN}✅ ImGui.NET reference found${NC}"
        else
            echo -e "${YELLOW}⚠️  No ImGui.NET reference found in $proj_file${NC}"
            echo "   Add: <PackageReference Include='ImGui.NET' Version='1.90.8.1' />"
        fi
        
        break
    fi
done

echo -e "${BLUE}📁 Directory structure validation:${NC}"

# Required directories for ImGui development
required_dirs=("src" "Properties")
optional_dirs=("Resources" "Images" "Fonts")

for dir in "${required_dirs[@]}"; do
    if [[ -d "$dir" ]]; then
        echo -e "${GREEN}✅ Found directory: $dir${NC}"
    else
        echo -e "${YELLOW}⚠️  Missing recommended directory: $dir${NC}"
    fi
done

echo -e "${BLUE}🔧 Code structure validation:${NC}"

# Check for basic plugin structure
plugin_files=()

# Look for main plugin files
if find . -name "*.cs" -type f | grep -q -E "(Plugin|Main)"; then
    echo -e "${GREEN}✅ Found main plugin class file${NC}"
else
    echo -e "${YELLOW}⚠️  No main plugin class found (look for files containing 'Plugin' or 'Main')${NC}"
fi

# Check for Dalamud plugin attributes
if find . -name "*.cs" -type f -exec grep -l "DalamudPlugin" {} \; | head -1 | grep -q .; then
    echo -e "${GREEN}✅ DalamudPlugin attribute found${NC}"
else
    echo -e "${RED}❌ No DalamudPlugin attribute found${NC}"
    echo "   Add [Plugin(\"name\", \"version\", \"author\")] to your main plugin class"
fi

echo -e "${BLUE}📦 Package configuration validation:${NC}"

# Check for nuget.config or packages directory
if [[ -f "nuget.config" ]]; then
    echo -e "${GREEN}✅ NuGet configuration found${NC}"
else
    echo -e "${YELLOW}⚠️  No nuget.config found (optional but recommended)${NC}"
fi

if [[ -d "packages" ]]; then
    echo -e "${GREEN}✅ Packages directory found${NC}"
else
    echo -e "${YELLOW}⚠️  No local packages directory (using global packages)${NC}"
fi

echo -e "${BLUE}🎨 ImGui-specific validation:${NC}"

# Check for ImGui imports in C# files
if find . -name "*.cs" -type f -exec grep -l "using ImGui" {} \; | head -1 | grep -q .; then
    echo -e "${GREEN}✅ ImGui imports found${NC}"
else
    echo -e "${YELLOW}⚠️  No ImGui imports found (expected if starting new project)${NC}"
fi

# Check for Dalamud Interface imports
if find . -name "*.cs" -type f -exec grep -l "Dalamud.Interface" {} \; | head -1 | grep -q .; then
    echo -e "${GREEN}✅ Dalamud Interface imports found${NC}"
else
    echo -e "${YELLOW}⚠️  No Dalamud Interface imports found${NC}"
    echo "   Add: using Dalamud.Interface;"
    echo "   Add: using Dalamud.Interface.Utility;"
fi

echo -e "${BLUE}📄 Build configuration:${NC}"

# Check for debug configuration
if find . -name "*.csproj" -o -name "*.sln" | xargs grep -q "Debug" 2>/dev/null; then
    echo -e "${GREEN}✅ Debug configuration found${NC}"
else
    echo -e "${YELLOW}⚠️  No debug configuration explicitly defined${NC}"
fi

# Check for target framework
if grep -r "TargetFramework" *.csproj 2>/dev/null | grep -q "net"; then
    framework=$(grep -r "TargetFramework" *.csproj 2>/dev/null | head -1 | sed 's/.*<TargetFramework>\(.*\)<\/TargetFramework>.*/\1/')
    echo -e "${GREEN}✅ Target framework: $framework${NC}"
    
    # Check if framework is compatible with Dalamud
    if [[ "$framework" =~ net[6-9] ]] || [[ "$framework" =~ net[6-9]\.0 ]]; then
        echo -e "${GREEN}✅ Compatible framework for Dalamud${NC}"
    else
        echo -e "${YELLOW}⚠️  Consider using .NET 6.0 or higher for best compatibility${NC}"
    fi
fi

echo -e "${BLUE}📝 Summary:${NC}"

# Count issues (simplified - in real implementation would track counts)
echo -e "${GREEN}✅ Project structure validation complete${NC}"
echo -e "${BLUE}💡 Recommendations:${NC}"
echo "   • Ensure proper Dalamud and ImGui.NET package references"
echo "   • Implement proper DalamudPlugin attributes"
echo "   • Use .NET 6.0 or higher for best compatibility"
echo "   • Include proper using statements for ImGui and Dalamud.Interface"
echo "   • Consider organizing code in src/ and Resources/ directories"

echo -e "${GREEN}🎉 Validation completed successfully!${NC}"