#!/bin/bash

# generate-ui-template.sh - Generate boilerplate ImGui UI code for Dalamud plugins
# Creates template files based on specified UI pattern and complexity level

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

show_usage() {
    echo "Usage: $0 <template_type> [output_file] [options]"
    echo ""
    echo "Template types:"
    echo "  basic-window     - Basic ImGui window with Dalamud integration"
    echo "  config-panel     - Configuration panel with settings"
    echo "  interactive-tool - Interactive utility with buttons and actions"
    echo "  data-viz        - Data visualization dashboard"
    echo "  quick-actions    - Quick action toolbar"
    echo "  status-monitor   - Real-time status monitor"
    echo ""
    echo "Options:"
    echo "  --level <1-4>   - Complexity level (1=basic, 4=expert)"
    echo "  --namespace <ns> - Custom namespace (default: MyPlugin.UI)"
    echo "  --class <name>   - Custom class name"
    echo "  --help          - Show this help"
    echo ""
    echo "Examples:"
    echo "  $0 basic-window MyUI.cs --level 1"
    echo "  $0 config-panel SettingsPanel.cs --namespace MyPlugin.Config --level 2"
}

# Parse arguments
TEMPLATE_TYPE=""
OUTPUT_FILE=""
NAMESPACE="MyPlugin.UI"
CLASS_NAME=""
COMPLEXITY_LEVEL=2

while [[ $# -gt 0 ]]; do
    case $1 in
        --level)
            COMPLEXITY_LEVEL="$2"
            shift 2
            ;;
        --namespace)
            NAMESPACE="$2"
            shift 2
            ;;
        --class)
            CLASS_NAME="$2"
            shift 2
            ;;
        --help)
            show_usage
            exit 0
            ;;
        -*)
            echo "Unknown option: $1"
            show_usage
            exit 1
            ;;
        *)
            if [[ -z "$TEMPLATE_TYPE" ]]; then
                TEMPLATE_TYPE="$1"
            elif [[ -z "$OUTPUT_FILE" ]]; then
                OUTPUT_FILE="$1"
            else
                echo "Too many arguments"
                show_usage
                exit 1
            fi
            shift
            ;;
    esac
done

# Validate arguments
if [[ -z "$TEMPLATE_TYPE" ]]; then
    echo "Error: Template type is required"
    show_usage
    exit 1
fi

if [[ -z "$OUTPUT_FILE" ]]; then
    # Generate default output filename based on template type
    case $TEMPLATE_TYPE in
        basic-window) OUTPUT_FILE="BasicWindow.cs" ;;
        config-panel) OUTPUT_FILE="ConfigPanel.cs" ;;
        interactive-tool) OUTPUT_FILE="InteractiveTool.cs" ;;
        data-viz) OUTPUT_FILE="DataVisualization.cs" ;;
        quick-actions) OUTPUT_FILE="QuickActions.cs" ;;
        status-monitor) OUTPUT_FILE="StatusMonitor.cs" ;;
        *) OUTPUT_FILE="${TEMPLATE_TYPE}.cs" ;;
    esac
fi

if [[ -z "$CLASS_NAME" ]]; then
    # Generate class name from filename
    CLASS_NAME=$(basename "$OUTPUT_FILE" .cs)
fi

# Validate complexity level
if [[ ! "$COMPLEXITY_LEVEL" =~ ^[1-4]$ ]]; then
    echo "Error: Complexity level must be between 1 and 4"
    exit 1
fi

echo -e "${BLUE}🎨 Generating ImGui UI template...${NC}"
echo -e "${BLUE}📋 Template: $TEMPLATE_TYPE${NC}"
echo -e "${BLUE}📁 Output: $OUTPUT_FILE${NC}"
echo -e "${BLUE}📊 Complexity: Level $COMPLEXITY_LEVEL${NC}"
echo -e "${BLUE}🏷️  Namespace: $NAMESPACE${NC}"
echo -e "${BLUE}🏛️  Class: $CLASS_NAME${NC}"

# Function to generate based on template type
generate_template() {
    local template_type="$1"
    local complexity="$2"
    local namespace="$3"
    local class_name="$4"
    
    case $template_type in
        basic-window)
            generate_basic_window "$complexity" "$namespace" "$class_name"
            ;;
        config-panel)
            generate_config_panel "$complexity" "$namespace" "$class_name"
            ;;
        interactive-tool)
            generate_interactive_tool "$complexity" "$namespace" "$class_name"
            ;;
        data-viz)
            generate_data_viz "$complexity" "$namespace" "$class_name"
            ;;
        quick-actions)
            generate_quick_actions "$complexity" "$namespace" "$class_name"
            ;;
        status-monitor)
            generate_status_monitor "$complexity" "$namespace" "$class_name"
            ;;
        *)
            echo -e "${RED}❌ Unknown template type: $template_type${NC}"
            exit 1
            ;;
    esac
}

generate_basic_window() {
    local complexity="$1"
    local namespace="$2"
    local class_name="$3"
    
    cat > "$OUTPUT_FILE" << EOF
using System;
using System.Numerics;
using ImGuiNET;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Plugin.Services;

namespace $namespace
{
    /// <summary>
    /// Basic ImGui window with Dalamud integration
    /// Level $complexity complexity
    /// </summary>
    public class $class_name : IDisposable
    {
        private readonly IDataManager _dataManager;
        private bool _visible = true;
        private bool _settingsOpen = false;
        
EOF

    if [[ $complexity -ge 2 ]]; then
        cat >> "$OUTPUT_FILE" << EOF
        // Level 2+: Basic state management
        private string _inputText = "";
        private bool _checkboxValue = false;
        private float _sliderValue = 0.0f;
        private int _selectedOption = 0;
        private readonly string[] _options = { "Option 1", "Option 2", "Option 3" };
        
EOF
    fi

    if [[ $complexity -ge 3 ]]; then
        cat >> "$OUTPUT_FILE" << EOF
        // Level 3+: Advanced features
        private Vector4 _colorValue = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        private bool _customDrawing = false;
        private readonly System.Random _random = new System.Random();
        
EOF
    fi

    cat >> "$OUTPUT_FILE" << EOF
        public $class_name(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public bool Visible
        {
            get => _visible;
            set => _visible = value;
        }

        public void Draw()
        {
            if (!_visible) return;

            var flags = ImGuiWindowFlags.None;
EOF

    if [[ $complexity -ge 2 ]]; then
        cat >> "$OUTPUT_FILE" << EOF
            // Level 2+: Configurable window flags
            if (ImGui.Checkbox("Settings", ref _settingsOpen))
            {
                // Settings toggled
            }
            ImGui.SameLine();
            
EOF
    fi

    cat >> "$OUTPUT_FILE" << EOF
            ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Vector2(300, 200));
            
            if (ImGui.Begin("$class_name", ref _visible, flags))
            {
                DrawContent();
            }
            
            ImGui.End();
            ImGui.PopStyleVar();
        }

        private void DrawContent()
        {
EOF

    if [[ $complexity -eq 1 ]]; then
        cat >> "$OUTPUT_FILE" << EOF
            // Level 1: Basic content
            ImGui.Text("Hello from $class_name!");
            ImGui.Separator();
            
            if (ImGui.Button("Click Me"))
            {
                // Button action
                Service.Logger.Info("Button clicked!");
            }
            
            if (ImGui.Button("Toggle Window"))
            {
                _visible = !_visible;
            }
EOF
    elif [[ $complexity -eq 2 ]]; then
        cat >> "$OUTPUT_FILE" << EOF
            // Level 2: Interactive widgets
            ImGui.Text("Interactive Elements");
            ImGui.Separator();
            
            // Input field
            ImGui.Text("Input Field:");
            if (ImGui.InputText("##input", ref _inputText, 256))
            {
                // Input changed
            }
            
            // Checkbox
            if (ImGui.Checkbox("Enable Feature", ref _checkboxValue))
            {
                // Checkbox toggled
            }
            
            // Slider
            if (ImGui.SliderFloat("Value", ref _sliderValue, 0.0f, 100.0f))
            {
                // Slider changed
            }
            
            // Combo box
            ImGui.Text("Select Option:");
            if (ImGui.Combo("##combo", ref _selectedOption, _options, _options.Length))
            {
                // Selection changed
            }
            
            ImGui.Separator();
            
            // Action buttons
            if (ImGui.Button("Save Settings"))
            {
                SaveSettings();
            }
            
            ImGui.SameLine();
            
            if (ImGui.Button("Reset"))
            {
                ResetSettings();
            }
EOF
    elif [[ $complexity -ge 3 ]]; then
        cat >> "$OUTPUT_FILE" << EOF
            // Level 3+: Advanced features
            if (ImGui.BeginTabBar("##tabs"))
            {
                if (ImGui.BeginTabItem("Basic"))
                {
                    DrawBasicTab();
                    ImGui.EndTabItem();
                }
                
                if (ImGui.BeginTabItem("Advanced"))
                {
                    DrawAdvancedTab();
                    ImGui.EndTabItem();
                }
                
                if (_customDrawing && ImGui.BeginTabItem("Custom"))
                {
                    DrawCustomTab();
                    ImGui.EndTabItem();
                }
                
                ImGui.EndTabBar();
            }
EOF
    fi

    cat >> "$OUTPUT_FILE" << EOF
        }
EOF

    if [[ $complexity -ge 2 ]]; then
        cat >> "$OUTPUT_FILE" << EOF

        private void SaveSettings()
        {
            // Save settings implementation
            Service.Logger.Info("Settings saved");
        }

        private void ResetSettings()
        {
            // Reset to defaults
            _inputText = "";
            _checkboxValue = false;
            _sliderValue = 0.0f;
            _selectedOption = 0;
EOF
    if [[ $complexity -ge 3 ]]; then
        cat >> "$OUTPUT_FILE" << EOF
            _colorValue = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
EOF
    fi
        cat >> "$OUTPUT_FILE" << EOF
            Service.Logger.Info("Settings reset");
        }
EOF
    fi

    if [[ $complexity -ge 3 ]]; then
        cat >> "$OUTPUT_FILE" << EOF

        private void DrawBasicTab()
        {
            ImGui.Text("Basic interactive elements");
            ImGui.Separator();
            
            // Basic controls from Level 2
            if (ImGui.InputText("Input", ref _inputText, 256)) { }
            if (ImGui.Checkbox("Feature", ref _checkboxValue)) { }
            if (ImGui.SliderFloat("Slider", ref _sliderValue, 0f, 100f)) { }
        }

        private void DrawAdvancedTab()
        {
            ImGui.Text("Advanced features");
            ImGui.Separator();
            
            // Color picker
            if (ImGui.ColorEdit4("Color", ref _colorValue))
            {
                // Color changed
            }
            
            // Custom drawing toggle
            if (ImGui.Checkbox("Enable Custom Drawing", ref _customDrawing))
            {
                // Custom drawing toggled
            }
            
            // Progress bar
            ImGui.Text("Progress:");
            ImGui.ProgressBar(_sliderValue / 100f, new Vector2(-1, 0), $"{_sliderValue:F1}%");
        }

        private void DrawCustomTab()
        {
            ImGui.Text("Custom drawing example");
            ImGui.Separator();
            
            var drawList = ImGui.GetWindowDrawList();
            var pos = ImGui.GetCursorScreenPos();
            var size = new Vector2(200, 100);
            
            // Draw custom rectangle
            drawList.AddRectFilled(pos, pos + size, ImGui.ColorConvertFloat4ToU32(_colorValue));
            
            // Draw text
            drawList.AddText(pos + new Vector2(10, 40), 0xFFFFFFFF, "Custom Drawing!");
            
            // Dummy widget for spacing
            ImGui.Dummy(size);
        }
EOF
    fi

    cat >> "$OUTPUT_FILE" << EOF

        public void Dispose()
        {
            // Cleanup resources
        }
    }
}
EOF
}

# Add more template generation functions as needed...
generate_config_panel() {
    # Similar implementation for config panel
    echo "Config panel template generation - Coming soon!"
}

generate_interactive_tool() {
    # Similar implementation for interactive tool
    echo "Interactive tool template generation - Coming soon!"
}

generate_data_viz() {
    # Similar implementation for data visualization
    echo "Data visualization template generation - Coming soon!"
}

generate_quick_actions() {
    # Similar implementation for quick actions
    echo "Quick actions template generation - Coming soon!"
}

generate_status_monitor() {
    # Similar implementation for status monitor
    echo "Status monitor template generation - Coming soon!"
}

# Generate the template
generate_template "$TEMPLATE_TYPE" "$COMPLEXITY_LEVEL" "$NAMESPACE" "$CLASS_NAME"

echo -e "${GREEN}✅ Template generated successfully: $OUTPUT_FILE${NC}"
echo -e "${BLUE}💡 Next steps:${NC}"
echo "   • Add the generated class to your Dalamud plugin"
echo "   • Initialize in your main plugin class"
echo "   • Call Draw() method in your UI render loop"
echo "   • Customize the implementation for your needs"