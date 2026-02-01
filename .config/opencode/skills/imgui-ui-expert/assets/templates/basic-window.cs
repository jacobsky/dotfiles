using System;
using System.Numerics;
using ImGuiNET;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Plugin.Services;

namespace MyPlugin.UI
{
    /// <summary>
    /// Basic ImGui window with Dalamud integration
    /// Level 1 complexity - Beginner friendly
    /// </summary>
    public class BasicWindow : IDisposable
    {
        private readonly IDataManager _dataManager;
        private bool _visible = true;
        private bool _settingsOpen = false;
        
        public BasicWindow(IDataManager dataManager)
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
            ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Vector2(300, 200));
            
            if (ImGui.Begin("Basic Window", ref _visible, flags))
            {
                DrawContent();
            }
            
            ImGui.End();
            ImGui.PopStyleVar();
        }

        private void DrawContent()
        {
            // Level 1: Basic content
            ImGui.Text("Hello from BasicWindow!");
            ImGui.Text("This is a beginner-friendly ImGui window.");
            ImGui.Separator();
            
            // Basic button
            if (ImGui.Button("Click Me"))
            {
                // Button action
                Service.Logger.Info("Button clicked!");
            }
            
            ImGui.SameLine();
            
            // Toggle window button
            if (ImGui.Button("Toggle Settings"))
            {
                _settingsOpen = !_settingsOpen;
            }
            
            // Settings panel (basic)
            if (_settingsOpen)
            {
                ImGui.Separator();
                ImGui.Text("Basic Settings:");
                
                static bool enableFeature = true;
                if (ImGui.Checkbox("Enable Basic Feature", ref enableFeature))
                {
                    Service.Logger.Info($"Feature {(enableFeature ? "enabled" : "disabled")}");
                }
                
                static string name = "";
                if (ImGui.InputText("Your Name", ref name, 256))
                {
                    // Text input changed
                }
            }
            
            ImGui.Separator();
            
            // Information display
            ImGui.Text("Window Information:");
            ImGui.Text($"Position: {ImGui.GetWindowPos()}");
            ImGui.Text($"Size: {ImGui.GetWindowSize()}");
            ImGui.Text($"FPS: {ImGui.GetIO().Framerate:F1}");
            
            // Close button
            if (ImGui.Button("Close Window"))
            {
                _visible = false;
            }
        }

        public void Dispose()
        {
            // Cleanup resources
            Service.Logger.Info("BasicWindow disposed");
        }
    }
}