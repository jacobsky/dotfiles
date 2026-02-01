using System;
using System.Numerics;
using ImGuiNET;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Plugin.Services;

namespace MyPlugin.UI
{
    /// <summary>
    /// Configuration panel with settings management
    /// Level 2-3 complexity - Intermediate to Advanced
    /// </summary>
    public class ConfigPanel : IDisposable
    {
        private readonly PluginConfig _config;
        private bool _visible = false;
        private bool _hasChanges = false;
        private int _selectedTab = 0;
        
        // Feature settings
        private bool _enableMainFeature = true;
        private bool _enableAdvancedMode = false;
        private float _updateInterval = 1.0f;
        private int _maxHistoryItems = 100;
        
        // Display settings
        private Vector4 _primaryColor = new(0.2f, 0.6f, 1.0f, 1.0f);
        private Vector4 _accentColor = new(1.0f, 0.6f, 0.2f, 1.0f);
        private bool _showTooltips = true;
        private bool _compactMode = false;
        
        // Hotkey settings
        private string _toggleHotkey = "F1";
        private string _quickActionHotkey = "F2";
        
        public ConfigPanel(PluginConfig config)
        {
            _config = config;
            LoadSettings();
        }

        public bool Visible
        {
            get => _visible;
            set => _visible = value;
        }

        public void Draw()
        {
            if (!_visible) return;

            var windowFlags = ImGuiWindowFlags.None;
            ImGui.SetNextWindowSize(new Vector2(500, 400), ImGuiCond.FirstUseEver);
            
            if (ImGui.Begin("Configuration", ref _visible, windowFlags))
            {
                DrawTabBar();
                DrawSelectedTab();
                DrawActionButtons();
            }
            
            ImGui.End();
        }

        private void DrawTabBar()
        {
            if (ImGui.BeginTabBar("ConfigTabs"))
            {
                if (ImGui.BeginTabItem("Features"))
                {
                    _selectedTab = 0;
                    ImGui.EndTabItem();
                }
                
                if (ImGui.BeginTabItem("Display"))
                {
                    _selectedTab = 1;
                    ImGui.EndTabItem();
                }
                
                if (ImGui.BeginTabItem("Hotkeys"))
                {
                    _selectedTab = 2;
                    ImGui.EndTabItem();
                }
                
                if (ImGui.BeginTabItem("Advanced"))
                {
                    _selectedTab = 3;
                    ImGui.EndTabItem();
                }
                
                ImGui.EndTabBar();
            }
        }

        private void DrawSelectedTab()
        {
            ImGui.Separator();
            
            switch (_selectedTab)
            {
                case 0:
                    DrawFeaturesTab();
                    break;
                case 1:
                    DrawDisplayTab();
                    break;
                case 2:
                    DrawHotkeysTab();
                    break;
                case 3:
                    DrawAdvancedTab();
                    break;
            }
        }

        private void DrawFeaturesTab()
        {
            ImGui.Text("Feature Configuration");
            ImGui.Separator();
            
            // Main feature toggle
            if (ImGui.Checkbox("Enable Main Feature", ref _enableMainFeature))
            {
                MarkChanged();
            }
            
            if (_enableMainFeature)
            {
                ImGui.Indent();
                
                // Update interval
                if (ImGui.SliderFloat("Update Interval (seconds)", ref _updateInterval, 0.1f, 10.0f, "%.1f"))
                {
                    MarkChanged();
                }
                
                // History items limit
                if (ImGui.SliderInt("Max History Items", ref _maxHistoryItems, 10, 1000))
                {
                    MarkChanged();
                }
                
                ImGui.Unindent();
            }
            
            // Advanced mode
            if (ImGui.Checkbox("Enable Advanced Mode", ref _enableAdvancedMode))
            {
                MarkChanged();
            }
            
            if (_enableAdvancedMode)
            {
                ImGui.Indent();
                ImGui.TextColored(new Vector4(1, 1, 0, 1), "⚠ Advanced features enabled");
                ImGui.Text("These options provide additional functionality");
                ImGui.Text("but may impact performance.");
                ImGui.Unindent();
            }
        }

        private void DrawDisplayTab()
        {
            ImGui.Text("Display Settings");
            ImGui.Separator();
            
            // Color settings
            ImGui.Text("Colors:");
            
            if (ImGui.ColorEdit3("Primary Color", ref _primaryColor))
            {
                MarkChanged();
            }
            
            if (ImGui.ColorEdit3("Accent Color", ref _accentColor))
            {
                MarkChanged();
            }
            
            ImGui.Separator();
            
            // UI behavior
            ImGui.Text("Interface:");
            
            if (ImGui.Checkbox("Show Tooltips", ref _showTooltips))
            {
                MarkChanged();
            }
            
            if (ImGui.Checkbox("Compact Mode", ref _compactMode))
            {
                MarkChanged();
            }
            
            if (_showTooltips)
            {
                ImGui.Indent();
                ImGui.Text("Tooltips will show additional information");
                ImGui.Text("when hovering over UI elements.");
                ImGui.Unindent();
            }
            
            // Preview section
            ImGui.Separator();
            ImGui.Text("Preview:");
            
            ImGui.PushStyleColor(ImGuiCol.Button, _primaryColor);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, _primaryColor * 1.2f);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, _primaryColor * 0.8f);
            
            if (ImGui.Button("Primary Button"))
            {
                // Preview action
            }
            
            ImGui.PopStyleColor(3);
            
            ImGui.SameLine();
            
            ImGui.PushStyleColor(ImGuiCol.Button, _accentColor);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, _accentColor * 1.2f);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, _accentColor * 0.8f);
            
            if (ImGui.Button("Accent Button"))
            {
                // Preview action
            }
            
            ImGui.PopStyleColor(3);
        }

        private void DrawHotkeysTab()
        {
            ImGui.Text("Hotkey Configuration");
            ImGui.Separator();
            
            ImGui.Text("Define hotkeys for quick access to plugin features.");
            ImGui.Text("Click in the input field and press the desired key.");
            
            ImGui.Separator();
            
            // Toggle UI hotkey
            ImGui.Text("Toggle UI:");
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100);
            if (ImGui.InputText("##toggle", ref _toggleHotkey, 16))
            {
                MarkChanged();
            }
            
            if (ImGui.IsItemHovered() && _showTooltips)
            {
                ImGui.SetTooltip("Opens/closes the main plugin window");
            }
            
            ImGui.SameLine();
            if (ImGui.Button("Set##settoggle"))
            {
                // Wait for key press logic would go here
                Service.Logger.Info("Press desired key for toggle hotkey");
            }
            
            // Quick action hotkey
            ImGui.Text("Quick Action:");
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100);
            if (ImGui.InputText("##quickaction", ref _quickActionHotkey, 16))
            {
                MarkChanged();
            }
            
            if (ImGui.IsItemHovered() && _showTooltips)
            {
                ImGui.SetTooltip("Executes the quick action feature");
            }
            
            ImGui.SameLine();
            if (ImGui.Button("Set##setquick"))
            {
                Service.Logger.Info("Press desired key for quick action hotkey");
            }
            
            ImGui.Separator();
            
            // Hotkey testing
            ImGui.Text("Test Hotkeys:");
            if (ImGui.Button("Test Toggle Hotkey"))
            {
                Service.Logger.Info($"Toggle hotkey: {_toggleHotkey}");
            }
            
            ImGui.SameLine();
            if (ImGui.Button("Test Quick Action"))
            {
                Service.Logger.Info($"Quick action hotkey: {_quickActionHotkey}");
            }
        }

        private void DrawAdvancedTab()
        {
            ImGui.Text("Advanced Configuration");
            ImGui.Separator();
            
            ImGui.TextColored(new Vector4(1, 1, 0, 1), "⚠ Warning:");
            ImGui.Text("These settings are for advanced users only.");
            ImGui.Text("Incorrect values may cause instability.");
            
            ImGui.Separator();
            
            // Debug options
            ImGui.Text("Debug Options:");
            
            static bool enableDebugLogging = false;
            if (ImGui.Checkbox("Enable Debug Logging", ref enableDebugLogging))
            {
                MarkChanged();
            }
            
            static bool enablePerformanceMonitor = false;
            if (ImGui.Checkbox("Enable Performance Monitor", ref enablePerformanceMonitor))
            {
                MarkChanged();
            }
            
            if (enablePerformanceMonitor)
            {
                ImGui.Indent();
                ImGui.Text("Performance monitoring will impact");
                ImGui.Text("overall performance slightly.");
                ImGui.Unindent();
            }
            
            ImGui.Separator();
            
            // Experimental features
            ImGui.Text("Experimental Features:");
            
            static bool enableBetaFeatures = false;
            if (ImGui.Checkbox("Enable Beta Features", ref enableBetaFeatures))
            {
                MarkChanged();
            }
            
            if (enableBetaFeatures)
            {
                ImGui.Indent();
                ImGui.TextColored(new Vector4(0, 1, 0, 1), "Beta features enabled");
                ImGui.Text("These features are under development");
                ImGui.Text("and may contain bugs.");
                ImGui.Unindent();
                
                static bool enableExperimentalUI = false;
                if (ImGui.Checkbox("Experimental UI", ref enableExperimentalUI))
                {
                    MarkChanged();
                }
            }
            
            ImGui.Separator();
            
            // Reset button
            if (ImGui.Button("Reset to Defaults"))
            {
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Reset all settings to default values");
                }
                
                // Confirmation dialog would go here
                ResetToDefaults();
                MarkChanged();
            }
            
            ImGui.SameLine();
            
            // Export/Import
            if (ImGui.Button("Export Settings"))
            {
                ExportSettings();
            }
            
            ImGui.SameLine();
            
            if (ImGui.Button("Import Settings"))
            {
                ImportSettings();
            }
        }

        private void DrawActionButtons()
        {
            ImGui.Separator();
            
            if (_hasChanges)
            {
                ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0, 0.8f, 0, 1));
                if (ImGui.Button("Save Changes"))
                {
                    SaveSettings();
                    _hasChanges = false;
                    Service.Logger.Info("Settings saved successfully");
                }
                ImGui.PopStyleColor();
                
                ImGui.SameLine();
                
                if (ImGui.Button("Revert"))
                {
                    LoadSettings();
                    _hasChanges = false;
                    Service.Logger.Info("Settings reverted");
                }
            }
            else
            {
                ImGui.PushStyleVar(ImGuiStyleVar.Alpha, 0.5f);
                ImGui.Button("No Changes to Save");
                ImGui.PopStyleVar();
            }
        }

        private void MarkChanged()
        {
            _hasChanges = true;
        }

        private void LoadSettings()
        {
            // Load settings from configuration
            _enableMainFeature = _config.EnableMainFeature;
            _enableAdvancedMode = _config.EnableAdvancedMode;
            _updateInterval = _config.UpdateInterval;
            _maxHistoryItems = _config.MaxHistoryItems;
            _primaryColor = _config.PrimaryColor;
            _accentColor = _config.AccentColor;
            _showTooltips = _config.ShowTooltips;
            _compactMode = _config.CompactMode;
            _toggleHotkey = _config.ToggleHotkey;
            _quickActionHotkey = _config.QuickActionHotkey;
        }

        private void SaveSettings()
        {
            // Save settings to configuration
            _config.EnableMainFeature = _enableMainFeature;
            _config.EnableAdvancedMode = _enableAdvancedMode;
            _config.UpdateInterval = _updateInterval;
            _config.MaxHistoryItems = _maxHistoryItems;
            _config.PrimaryColor = _primaryColor;
            _config.AccentColor = _accentColor;
            _config.ShowTooltips = _showTooltips;
            _config.CompactMode = _compactMode;
            _config.ToggleHotkey = _toggleHotkey;
            _config.QuickActionHotkey = _quickActionHotkey;
            
            _config.Save();
        }

        private void ResetToDefaults()
        {
            _enableMainFeature = true;
            _enableAdvancedMode = false;
            _updateInterval = 1.0f;
            _maxHistoryItems = 100;
            _primaryColor = new Vector4(0.2f, 0.6f, 1.0f, 1.0f);
            _accentColor = new Vector4(1.0f, 0.6f, 0.2f, 1.0f);
            _showTooltips = true;
            _compactMode = false;
            _toggleHotkey = "F1";
            _quickActionHotkey = "F2";
        }

        private void ExportSettings()
        {
            // Export settings to file
            var filename = $"plugin_config_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            Service.Logger.Info($"Settings exported to {filename}");
        }

        private void ImportSettings()
        {
            // Import settings from file
            Service.Logger.Info("Import settings - file dialog would open here");
        }

        public void Dispose()
        {
            if (_hasChanges)
            {
                Service.Logger.Warning("Config panel disposed with unsaved changes");
            }
            
            Service.Logger.Info("ConfigPanel disposed");
        }
    }

    /// <summary>
    /// Plugin configuration class (simplified)
    /// </summary>
    public class PluginConfig
    {
        public bool EnableMainFeature { get; set; } = true;
        public bool EnableAdvancedMode { get; set; } = false;
        public float UpdateInterval { get; set; } = 1.0f;
        public int MaxHistoryItems { get; set; } = 100;
        public Vector4 PrimaryColor { get; set; } = new(0.2f, 0.6f, 1.0f, 1.0f);
        public Vector4 AccentColor { get; set; } = new(1.0f, 0.6f, 0.2f, 1.0f);
        public bool ShowTooltips { get; set; } = true;
        public bool CompactMode { get; set; } = false;
        public string ToggleHotkey { get; set; } = "F1";
        public string QuickActionHotkey { get; set; } = "F2";

        public void Save()
        {
            // Save implementation would go here
        }
    }
}