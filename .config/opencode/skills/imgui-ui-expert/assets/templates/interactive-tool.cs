using System;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Plugin.Services;

namespace MyPlugin.UI
{
    /// <summary>
    /// Interactive utility with buttons and action panels
    /// Level 2-3 complexity - Intermediate to Advanced
    /// </summary>
    public class InteractiveTool : IDisposable
    {
        private bool _visible = true;
        private readonly List<QuickAction> _quickActions = new();
        private readonly List<ToolAction> _toolActions = new();
        private int _selectedCategory = 0;
        private bool _showHelp = false;
        private string _searchText = "";
        
        // Tool state
        private bool _isProcessing = false;
        private string _lastAction = "";
        private readonly Queue<string> _actionHistory = new();
        private const int MaxHistory = 20;
        
        public record QuickAction(string Name, Action OnClick, FontAwesomeIcon Icon, Vector4 Color, string Description = "");
        public record ToolAction(string Name, Action OnClick, string Category, string Hotkey = "");
        
        public InteractiveTool()
        {
            InitializeActions();
        }

        public bool Visible
        {
            get => _visible;
            set => _visible = value;
        }

        private void InitializeActions()
        {
            // Quick actions for instant access
            _quickActions.Add(new QuickAction("Execute", ExecuteAction, FontAwesomeIcon.Play, new(0, 1, 0, 1), "Execute the current action"));
            _quickActions.Add(new QuickAction("Stop", StopAction, FontAwesomeIcon.Stop, new(1, 0, 0, 1), "Stop current execution"));
            _quickActions.Add(new QuickAction("Reset", ResetAction, FontAwesomeIcon.Refresh, new(1, 1, 0, 1), "Reset tool state"));
            _quickActions.Add(new QuickAction("Settings", OpenSettings, FontAwesomeIcon.Cog, new(0.5f, 0.5f, 0.5f, 1), "Open tool settings"));
            
            // Tool actions organized by category
            _toolActions.Add(new ToolAction("Quick Export", () => ExecuteTool("Export"), "File", "Ctrl+E"));
            _toolActions.Add(new ToolAction("Import Data", () => ExecuteTool("Import"), "File", "Ctrl+I"));
            _toolActions.Add(new ToolAction("Save State", () => ExecuteTool("Save"), "File", "Ctrl+S"));
            _toolActions.Add(new ToolAction("Load State", () => ExecuteTool("Load"), "File", "Ctrl+L"));
            
            _toolActions.Add(new ToolAction("Analyze", () => ExecuteTool("Analyze"), "Analysis", "F5"));
            _toolActions.Add(new ToolAction("Process Data", () => ExecuteTool("Process"), "Analysis", "F6"));
            _toolActions.Add(new ToolAction("Generate Report", () => ExecuteTool("Report"), "Analysis", "F7"));
            _toolActions.Add(new ToolAction("Validate", () => ExecuteTool("Validate"), "Analysis", "F8"));
            
            _toolActions.Add(new ToolAction("Clear Cache", () => ExecuteTool("Clear Cache"), "System", ""));
            _toolActions.Add(new ToolAction("Refresh", () => ExecuteTool("Refresh"), "System", "F9"));
            _toolActions.Add(new ToolAction("Repair", () => ExecuteTool("Repair"), "System", ""));
            _toolActions.Add(new ToolAction("Optimize", () => ExecuteTool("Optimize"), "System", ""));
        }

        public void Draw()
        {
            if (!_visible) return;

            var windowFlags = ImGuiWindowFlags.None;
            ImGui.SetNextWindowSize(new Vector2(600, 500), ImGuiCond.FirstUseEver);
            
            if (ImGui.Begin("Interactive Tool", ref _visible, windowFlags))
            {
                DrawHeader();
                DrawQuickActions();
                DrawToolCategories();
                DrawToolActions();
                DrawStatusPanel();
            }
            
            ImGui.End();
            
            // Draw help overlay if enabled
            if (_showHelp)
            {
                DrawHelpOverlay();
            }
        }

        private void DrawHeader()
        {
            // Title and search
            ImGui.Text("Interactive Utility Tool");
            
            ImGui.SameLine();
            ImGuiHelpers.RightAlign("Help");
            
            if (ImGui.Button("?"))
            {
                _showHelp = !_showHelp;
            }
            
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Toggle help overlay");
            }
            
            // Search bar
            ImGui.SetNextItemWidth(-1);
            if (ImGui.InputTextWithHint("##search", "Search actions...", ref _searchText, 256))
            {
                // Filter actions based on search
            }
            
            ImGui.Separator();
        }

        private void DrawQuickActions()
        {
            ImGui.Text("Quick Actions:");
            
            var buttonSize = new Vector2(80, 40);
            var spacing = 5f;
            
            for (int i = 0; i < _quickActions.Count; i++)
            {
                var action = _quickActions[i];
                
                if (i > 0 && i % 4 != 0)
                    ImGui.SameLine(0, spacing);
                
                // Colored button with icon
                ImGui.PushStyleColor(ImGuiCol.Button, action.Color);
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, action.Color * 1.2f);
                ImGui.PushStyleColor(ImGuiCol.ButtonActive, action.Color * 0.8f);
                
                if (ImGui.Button($"{(char)action.Icon}\n{action.Name}##{i}", buttonSize))
                {
                    action.OnClick.Invoke();
                    AddToHistory($"Quick: {action.Name}");
                }
                
                ImGui.PopStyleColor(3);
                
                // Tooltip
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip(action.Description);
                }
            }
            
            ImGui.Separator();
        }

        private void DrawToolCategories()
        {
            ImGui.Text("Tool Categories:");
            
            var categories = new[] { "File", "Analysis", "System" };
            
            for (int i = 0; i < categories.Length; i++)
            {
                var isSelected = _selectedCategory == i;
                var category = categories[i];
                
                if (isSelected)
                {
                    ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.2f, 0.6f, 1.0f, 1.0f));
                }
                
                if (ImGui.Button(category))
                {
                    _selectedCategory = i;
                }
                
                if (isSelected)
                {
                    ImGui.PopStyleColor();
                }
                
                if (i < categories.Length - 1)
                    ImGui.SameLine();
            }
            
            ImGui.Separator();
        }

        private void DrawToolActions()
        {
            ImGui.Text("Available Actions:");
            
            // Get current category
            var categoryName = _selectedCategory switch
            {
                0 => "File",
                1 => "Analysis",
                2 => "System",
                _ => "File"
            };
            
            // Filter actions by category and search
            var filteredActions = _toolActions.FindAll(action => 
                action.Category == categoryName &&
                (string.IsNullOrEmpty(_searchText) || action.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase))
            );
            
            if (filteredActions.Count == 0)
            {
                ImGui.TextColored(new Vector4(0.5f, 0.5f, 0.5f, 1f), "No actions found");
                return;
            }
            
            // Action list with hotkeys
            if (ImGui.BeginTable("ToolActions", 3, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
            {
                ImGui.TableSetupColumn("Action");
                ImGui.TableSetupColumn("Hotkey");
                ImGui.TableSetupColumn("Execute");
                ImGui.TableHeadersRow();
                
                foreach (var action in filteredActions)
                {
                    ImGui.TableNextRow();
                    
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text(action.Name);
                    
                    ImGui.TableSetColumnIndex(1);
                    ImGui.Text(action.Hotkey);
                    
                    ImGui.TableSetColumnIndex(2);
                    var buttonText = _isProcessing ? "Processing..." : "Execute";
                    var isDisabled = _isProcessing;
                    
                    if (isDisabled)
                    {
                        ImGui.PushStyleVar(ImGuiStyleVar.Alpha, 0.5f);
                    }
                    
                    if (ImGui.Button($"{buttonText}##{action.Name}") && !_isProcessing)
                    {
                        ExecuteToolAction(action);
                    }
                    
                    if (isDisabled)
                    {
                        ImGui.PopStyleVar();
                    }
                }
                
                ImGui.EndTable();
            }
        }

        private void DrawStatusPanel()
        {
            ImGui.Separator();
            
            // Status display
            ImGui.Text("Status:");
            
            var statusColor = _isProcessing ? new Vector4(1, 1, 0, 1) : new Vector4(0, 1, 0, 1);
            var statusText = _isProcessing ? "Processing..." : "Ready";
            
            ImGui.SameLine();
            ImGui.TextColored(statusColor, statusText);
            
            // Last action
            if (!string.IsNullOrEmpty(_lastAction))
            {
                ImGui.Text($"Last Action: {_lastAction}");
            }
            
            // Action history
            if (_actionHistory.Count > 0)
            {
                ImGui.Separator();
                ImGui.Text("Action History:");
                
                var historyHeight = ImGui.GetContentRegionAvail().Y;
                if (ImGui.BeginChild("History", new Vector2(0, historyHeight)))
                {
                    foreach (var action in _actionHistory.Reverse().Take(10))
                    {
                        ImGui.Text($"• {action}");
                    }
                    
                    if (ImGui.Button("Clear History"))
                    {
                        _actionHistory.Clear();
                    }
                }
                
                ImGui.EndChild();
            }
        }

        private void DrawHelpOverlay()
        {
            var viewport = ImGui.GetMainViewport();
            ImGui.SetNextWindowPos(viewport.WorkPos);
            ImGui.SetNextWindowSize(viewport.WorkSize);
            
            var flags = ImGuiWindowFlags.NoDecoration | 
                       ImGuiWindowFlags.NoMove | 
                       ImGuiWindowFlags.NoSavedSettings |
                       ImGuiWindowFlags.AlwaysAutoResize;
            
            if (ImGui.Begin("HelpOverlay", ref _showHelp, flags))
            {
                ImGui.SetWindowFontScale(1.2f);
                ImGui.Text("Interactive Tool Help");
                ImGui.SetWindowFontScale(1.0f);
                
                ImGui.Separator();
                
                ImGui.Text("Quick Actions:");
                ImGui.Text("• Execute - Run the selected tool action");
                ImGui.Text("• Stop - Cancel current execution");
                ImGui.Text("• Reset - Clear tool state and history");
                ImGui.Text("• Settings - Open configuration panel");
                
                ImGui.Separator();
                
                ImGui.Text("Tool Categories:");
                ImGui.Text("• File - Import, export, save, and load operations");
                ImGui.Text("• Analysis - Data processing and validation tools");
                ImGui.Text("• System - Maintenance and optimization utilities");
                
                ImGui.Separator();
                
                ImGui.Text("Hotkeys:");
                ImGui.Text("• ? - Toggle this help overlay");
                ImGui.Text("• Esc - Close help overlay");
                
                ImGui.Separator();
                
                if (ImGui.Button("Close (Esc)"))
                {
                    _showHelp = false;
                }
            }
            
            // Close on ESC
            if (ImGui.IsKeyPressed(ImGuiKey.Escape))
            {
                _showHelp = false;
            }
            
            ImGui.End();
        }

        private void ExecuteAction()
        {
            Service.Logger.Info("Execute action triggered");
            _lastAction = "Execute completed";
            AddToHistory("Execute action");
        }

        private void StopAction()
        {
            _isProcessing = false;
            Service.Logger.Info("Stop action triggered");
            _lastAction = "Execution stopped";
            AddToHistory("Stop execution");
        }

        private void ResetAction()
        {
            _isProcessing = false;
            _lastAction = "";
            _actionHistory.Clear();
            Service.Logger.Info("Tool reset completed");
            AddToHistory("Tool reset");
        }

        private void OpenSettings()
        {
            Service.Logger.Info("Settings opened");
            _lastAction = "Settings opened";
            AddToHistory("Open settings");
        }

        private void ExecuteToolAction(ToolAction action)
        {
            _isProcessing = true;
            
            // Simulate async execution
            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(1000); // Simulate work
                    action.OnClick.Invoke();
                    
                    _lastAction = $"{action.Name} completed";
                    AddToHistory($"{action.Name} executed");
                }
                catch (Exception ex)
                {
                    Service.Logger.Error(ex, $"Failed to execute {action.Name}");
                    _lastAction = $"{action.Name} failed";
                    AddToHistory($"{action.Name} failed");
                }
                finally
                {
                    _isProcessing = false;
                }
            });
        }

        private void ExecuteTool(string toolName)
        {
            Service.Logger.Info($"Executing tool: {toolName}");
            _lastAction = $"{toolName} executed";
            AddToHistory($"{toolName} executed");
        }

        private void AddToHistory(string action)
        {
            _actionHistory.Enqueue($"[{DateTime.Now:HH:mm:ss}] {action}");
            
            // Keep history limited
            while (_actionHistory.Count > MaxHistory)
            {
                _actionHistory.Dequeue();
            }
        }

        public void Dispose()
        {
            Service.Logger.Info("InteractiveTool disposed");
        }
    }
}