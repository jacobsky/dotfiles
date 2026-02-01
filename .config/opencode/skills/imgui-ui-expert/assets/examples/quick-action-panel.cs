using System;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Plugin.Services;

namespace MyPlugin.Examples
{
    /// <summary>
    /// Quick Action Panel - Floating toolbar for frequently used actions
    /// Interactive utility example with drag support and customizable actions
    /// </summary>
    public class QuickActionPanel : IDisposable
    {
        private readonly List<QuickAction> _actions = new();
        private bool _isVisible = true;
        private Vector2 _position = new(200, 200);
        private bool _isDragging = false;
        private Vector2 _dragOffset = Vector2.Zero;
        private bool _isVertical = false;
        private int _iconSize = 32;
        
        public record QuickAction(string Name, Action OnClick, FontAwesomeIcon Icon, Vector4 Color, string Hotkey = "");
        
        public QuickActionPanel()
        {
            InitializeDefaultActions();
        }

        private void InitializeDefaultActions()
        {
            _actions.Add(new QuickAction("Heal", () => ExecuteHeal(), FontAwesomeIcon.Heart, new(0, 1, 0, 1), "F1"));
            _actions.Add(new QuickAction("Buff", () => ExecuteBuff(), FontAwesomeIcon.Star, new(1, 1, 0, 1), "F2"));
            _actions.Add(new QuickAction("Teleport", () => ExecuteTeleport(), FontAwesomeIcon.Compass, new(0, 1, 1, 1), "F3"));
            _actions.Add(new QuickAction("Repair", () => ExecuteRepair(), FontAwesomeIcon.Wrench, new(1, 0.5f, 0, 1), "F4"));
            _actions.Add(new QuickAction("Settings", () => ExecuteSettings(), FontAwesomeIcon.Cog, new(0.5f, 0.5f, 0.5f, 1), "F5"));
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => _isVisible = value;
        }

        public void Draw()
        {
            if (!_isVisible) return;
            
            // Set window position
            ImGui.SetNextWindowPos(_position, ImGuiCond.Always);
            
            // Minimal window style for floating panel
            var flags = ImGuiWindowFlags.NoTitleBar | 
                       ImGuiWindowFlags.NoResize | 
                       ImGuiWindowFlags.AlwaysAutoResize |
                       ImGuiWindowFlags.NoScrollbar |
                       ImGuiWindowFlags.NoBackground; // Transparent background
            
            if (ImGui.Begin("QuickActionPanel", ref _isVisible, flags))
            {
                DrawActionButtons();
                HandleDragging();
                DrawContextMenu();
            }
            
            ImGui.End();
        }

        private void DrawActionButtons()
        {
            var buttonSize = new Vector2(_iconSize, _iconSize);
            var spacing = 4f;
            
            // Custom background for panel
            var drawList = ImGui.GetWindowDrawList();
            var pos = ImGui.GetCursorScreenPos() - new Vector2(spacing, spacing);
            
            // Calculate panel size based on layout
            var (cols, rows) = CalculateLayout(_actions.Count);
            var panelSize = new Vector2(
                cols * (buttonSize.X + spacing) + spacing,
                rows * (buttonSize.Y + spacing) + spacing
            );
            
            // Draw semi-transparent background
            drawList.AddRectFilled(pos, pos + panelSize, ImGui.ColorConvertFloat4ToU32(new Vector4(0.1f, 0.1f, 0.1f, 0.8f)));
            drawList.AddRect(pos, pos + panelSize, ImGui.ColorConvertFloat4ToU32(new Vector4(0.5f, 0.5f, 0.5f, 1.0f)));
            
            // Layout buttons
            for (int i = 0; i < _actions.Count; i++)
            {
                var action = _actions[i];
                
                if (i > 0 && i % cols != 0)
                    ImGui.SameLine(0, spacing);
                
                // Draw button with custom styling
                ImGui.PushStyleColor(ImGuiCol.Button, action.Color);
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, action.Color * 1.2f);
                ImGui.PushStyleColor(ImGuiCol.ButtonActive, action.Color * 0.8f);
                ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 4.0f);
                
                if (ImGui.Button($"{(char)action.Icon}##{i}", buttonSize))
                {
                    action.OnClick.Invoke();
                    ShowActionFeedback(action.Name);
                }
                
                ImGui.PopStyleVar();
                ImGui.PopStyleColor(3);
                
                // Tooltip with action info
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip($"{action.Name}\nHotkey: {action.Hotkey}");
                }
            }
        }

        private (int cols, int rows) CalculateLayout(int itemCount)
        {
            if (_isVertical)
            {
                // Vertical layout - single column
                return (1, itemCount);
            }
            else
            {
                // Horizontal layout - try to create a square-ish grid
                var cols = (int)Math.Ceiling(Math.Sqrt(itemCount));
                var rows = (int)Math.Ceiling((float)itemCount / cols);
                return (cols, rows);
            }
        }

        private void HandleDragging()
        {
            // Start dragging on right-click
            if (ImGui.IsWindowHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Right))
            {
                _isDragging = true;
                _dragOffset = ImGui.GetMousePos() - _position;
            }
            
            // Continue dragging
            if (_isDragging && ImGui.IsMouseDragging(ImGuiMouseButton.Right))
            {
                _position = ImGui.GetMousePos() - _dragOffset;
                
                // Keep within screen bounds
                var viewport = ImGui.GetMainViewport();
                var screenSize = viewport.WorkSize;
                _position = Vector2.Clamp(_position, Vector2.Zero, screenSize - new Vector2(100, 100));
            }
            
            // Stop dragging
            if (_isDragging && ImGui.IsMouseReleased(ImGuiMouseButton.Right))
            {
                _isDragging = false;
            }
        }

        private void DrawContextMenu()
        {
            // Show context menu on middle-click
            if (ImGui.IsWindowHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Middle))
            {
                ImGui.OpenPopup("QuickActionContextMenu");
            }
            
            if (ImGui.BeginPopup("QuickActionContextMenu"))
            {
                if (ImGui.MenuItem("Toggle Layout"))
                {
                    _isVertical = !_isVertical;
                }
                
                if (ImGui.BeginMenu("Icon Size"))
                {
                    if (ImGui.MenuItem("Small (24px)", _iconSize == 24))
                    {
                        _iconSize = 24;
                    }
                    
                    if (ImGui.MenuItem("Medium (32px)", _iconSize == 32))
                    {
                        _iconSize = 32;
                    }
                    
                    if (ImGui.MenuItem("Large (48px)", _iconSize == 48))
                    {
                        _iconSize = 48;
                    }
                    
                    ImGui.EndMenu();
                }
                
                ImGui.Separator();
                
                if (ImGui.MenuItem("Reset Position"))
                {
                    _position = new Vector2(200, 200);
                }
                
                if (ImGui.MenuItem("Hide Panel"))
                {
                    _isVisible = false;
                }
                
                ImGui.EndPopup();
            }
        }

        private void ShowActionFeedback(string actionName)
        {
            // Create a temporary notification or visual feedback
            Service.ChatGui.Print($"[Quick Action] {actionName} executed");
            
            // Could also play a sound or show a floating text
        }

        private void ExecuteHeal()
        {
            Service.Logger.Info("Heal action executed");
            // Implement heal logic
        }

        private void ExecuteBuff()
        {
            Service.Logger.Info("Buff action executed");
            // Implement buff logic
        }

        private void ExecuteTeleport()
        {
            Service.Logger.Info("Teleport action executed");
            // Implement teleport logic
        }

        private void ExecuteRepair()
        {
            Service.Logger.Info("Repair action executed");
            // Implement repair logic
        }

        private void ExecuteSettings()
        {
            Service.Logger.Info("Settings action executed");
            // Open settings window
        }

        public void AddAction(QuickAction action)
        {
            _actions.Add(action);
        }

        public void RemoveAction(string name)
        {
            _actions.RemoveAll(a => a.Name == name);
        }

        public void Dispose()
        {
            Service.Logger.Info("QuickActionPanel disposed");
        }
    }
}