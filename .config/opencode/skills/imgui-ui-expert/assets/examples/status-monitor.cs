using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ImGuiNET;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Plugin.Services;

namespace MyPlugin.Examples
{
    /// <summary>
    /// Real-time Status Monitor - Live tracking of game performance and player stats
    /// Advanced example with real-time data updates and visualization
    /// </summary>
    public class StatusMonitor : IDisposable
    {
        private bool _isVisible = true;
        private readonly Queue<float> _fpsHistory = new();
        private readonly Queue<float> _memoryHistory = new();
        private readonly Queue<StatusSnapshot> _statusHistory = new();
        private DateTime _lastUpdate = DateTime.MinValue;
        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(100); // 10 FPS for updates
        private const int MaxHistory = 60; // 6 seconds of history at 10 FPS
        
        // Status data
        private StatusSnapshot _currentStatus = new();
        
        public struct StatusSnapshot
        {
            public DateTime Timestamp;
            public float FPS;
            public float MemoryMB;
            public float CPUUsage;
            public string PlayerJob;
            public int PlayerLevel;
            public int PlayerHP;
            public int PlayerMaxHP;
            public int PlayerMP;
            public int PlayerMaxMP;
        }
        
        // Monitor settings
        private bool _showPerformanceGraph = true;
        private bool _showPlayerStats = true;
        private bool _showSystemInfo = true;
        private bool _compactMode = false;
        
        // Graph settings
        private Vector4 _fpsColor = new(0, 1, 0, 1);
        private Vector4 _memoryColor = new(0, 0.5f, 1, 1);
        private Vector4 _cpuColor = new(1, 1, 0, 1);
        private float _graphHeight = 100f;

        public StatusMonitor(IClientState clientState, IFramework framework)
        {
            _clientState = clientState;
            _framework = framework;
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => _isVisible = value;
        }

        private readonly IClientState _clientState;
        private readonly IFramework _framework;

        public void Draw()
        {
            if (!_isVisible) return;

            UpdateStatusData();
            
            var windowFlags = ImGuiWindowFlags.None;
            var windowSize = _compactMode ? new Vector2(300, 200) : new Vector2(400, 350);
            ImGui.SetNextWindowSize(windowSize, ImGuiCond.FirstUseEver);
            
            if (ImGui.Begin("Status Monitor", ref _isVisible, windowFlags))
            {
                DrawHeader();
                
                if (_showPlayerStats)
                    DrawPlayerStats();
                
                if (_showPerformanceGraph)
                    DrawPerformanceGraph();
                
                if (_showSystemInfo)
                    DrawSystemInfo();
                
                DrawControls();
            }
            
            ImGui.End();
        }

        private void UpdateStatusData()
        {
            var now = DateTime.Now;
            if ((now - _lastUpdate) < _updateInterval)
                return;
            
            // Collect performance data
            var currentFPS = ImGui.GetIO().Framerate;
            var memoryMB = GC.GetTotalMemory(false) / (1024f * 1024f);
            var cpuUsage = GetCpuUsage(); // Simplified CPU measurement
            
            // Update history
            _fpsHistory.Enqueue(currentFPS);
            if (_fpsHistory.Count > MaxHistory)
                _fpsHistory.Dequeue();
            
            _memoryHistory.Enqueue(memoryMB);
            if (_memoryHistory.Count > MaxHistory)
                _memoryHistory.Dequeue();
            
            // Collect player data
            var player = _clientState.LocalPlayer;
            _currentStatus = new StatusSnapshot
            {
                Timestamp = now,
                FPS = currentFPS,
                MemoryMB = memoryMB,
                CPUUsage = cpuUsage,
                PlayerJob = player?.ClassJob.Value?.Name ?? "None",
                PlayerLevel = player?.Level ?? 0,
                PlayerHP = player?.CurrentHp ?? 0,
                PlayerMaxHP = player?.MaxHp ?? 0,
                PlayerMP = player?.CurrentMp ?? 0,
                PlayerMaxMP = player?.MaxMp ?? 0
            };
            
            _statusHistory.Enqueue(_currentStatus);
            if (_statusHistory.Count > MaxHistory)
                _statusHistory.Dequeue();
            
            _lastUpdate = now;
        }

        private void DrawHeader()
        {
            ImGui.Text("Real-time Status Monitor");
            
            ImGui.SameLine();
            
            // Status indicator
            var statusColor = GetPerformanceColor(_currentStatus.FPS);
            ImGui.TextColored(statusColor, "●");
            
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip($"Performance: {GetPerformanceGrade(_currentStatus.FPS)}");
            }
            
            ImGui.Separator();
        }

        private void DrawPlayerStats()
        {
            if (!_compactMode)
                ImGui.Text("Player Status:");
            
            if (_clientState.IsLoggedIn && _clientState.LocalPlayer != null)
            {
                if (_compactMode)
                {
                    // Compact layout - single line
                    ImGui.Text($"{_currentStatus.PlayerJob} Lvl {_currentStatus.PlayerLevel}");
                    ImGui.SameLine();
                    ImGui.Text($"HP: {_currentStatus.PlayerHP}/{_currentStatus.PlayerMaxHP}");
                    ImGui.SameLine();
                    ImGui.Text($"MP: {_currentStatus.PlayerMP}/{_currentStatus.PlayerMaxMP}");
                }
                else
                {
                    // Full layout - detailed info
                    ImGui.Text($"Job: {_currentStatus.PlayerJob}");
                    ImGui.SameLine();
                    ImGui.Text($"Level: {_currentStatus.PlayerLevel}");
                    
                    // HP bar
                    var hpPercentage = _currentStatus.PlayerMaxHP > 0 ? 
                        (float)_currentStatus.PlayerHP / _currentStatus.PlayerMaxHP : 0;
                    ImGui.Text("HP:");
                    ImGui.SameLine();
                    ImGui.ProgressBar(hpPercentage, new Vector2(-1, 0), 
                        $"{_currentStatus.PlayerHP}/{_currentStatus.PlayerMaxHP}");
                    
                    // HP color coding
                    var hpColor = hpPercentage switch
                    {
                        > 0.5f => new Vector4(0, 1, 0, 1),    // Green
                        > 0.25f => new Vector4(1, 1, 0, 1),   // Yellow
                        _ => new Vector4(1, 0, 0, 1)      // Red
                    };
                    
                    var drawList = ImGui.GetWindowDrawList();
                    var pos = ImGui.GetCursorScreenPos() - new Vector2(0, ImGui.GetFontSize() * 1.5f);
                    var barSize = new Vector2(ImGui.GetContentRegionAvail().X, ImGui.GetFontSize() * 0.8f);
                    var filledWidth = barSize.X * hpPercentage;
                    
                    drawList.AddRectFilled(pos, pos + new Vector2(filledWidth, barSize.Y), 
                        ImGui.ColorConvertFloat4ToU32(hpColor));
                    
                    // MP bar
                    var mpPercentage = _currentStatus.PlayerMaxMP > 0 ? 
                        (float)_currentStatus.PlayerMP / _currentStatus.PlayerMaxMP : 0;
                    ImGui.Text("MP:");
                    ImGui.SameLine();
                    ImGui.ProgressBar(mpPercentage, new Vector2(-1, 0), 
                        $"{_currentStatus.PlayerMP}/{_currentStatus.PlayerMaxMP}");
                    
                    // MP color
                    var mpColor = new Vector4(0, 0.5f, 1, 1); // Blue
                    
                    pos = ImGui.GetCursorScreenPos() - new Vector2(0, ImGui.GetFontSize() * 1.5f);
                    filledWidth = barSize.X * mpPercentage;
                    
                    drawList.AddRectFilled(pos, pos + new Vector2(filledWidth, barSize.Y), 
                        ImGui.ColorConvertFloat4ToU32(mpColor));
                }
            }
            else
            {
                ImGui.TextColored(new Vector4(1, 0.5f, 0.5f, 1), "Not logged in");
            }
            
            if (!_compactMode)
                ImGui.Separator();
        }

        private void DrawPerformanceGraph()
        {
            if (!_compactMode)
                ImGui.Text("Performance Graph:");
            
            var graphSize = new Vector2(ImGui.GetContentRegionAvail().X, _graphHeight);
            
            if (_fpsHistory.Count > 1 && _memoryHistory.Count > 1)
            {
                // Custom graph drawing
                var drawList = ImGui.GetWindowDrawList();
                var pos = ImGui.GetCursorScreenPos();
                
                // Background
                drawList.AddRectFilled(pos, pos + graphSize, 
                    ImGui.ColorConvertFloat4ToU32(new Vector4(0.1f, 0.1f, 0.1f, 1.0f)));
                
                // Draw grid lines
                DrawGraphGrid(drawList, pos, graphSize);
                
                // Draw FPS line
                DrawGraphLine(drawList, pos, graphSize, _fpsHistory.ToArray(), _fpsColor, 120.0f);
                
                // Draw Memory line
                DrawGraphLine(drawList, pos, graphSize, _memoryHistory.ToArray(), _memoryColor, 500.0f);
                
                // Border
                drawList.AddRect(pos, pos + graphSize, 
                    ImGui.ColorConvertFloat4ToU32(new Vector4(1, 1, 1, 1.0f)));
                
                ImGui.Dummy(graphSize);
            }
            else
            {
                ImGui.Text("Collecting data...");
            }
            
            if (!_compactMode)
                ImGui.Separator();
        }

        private void DrawGraphGrid(ImDrawListPtr drawList, Vector2 pos, Vector2 size)
        {
            var gridColor = ImGui.ColorConvertFloat4ToU32(new Vector4(0.3f, 0.3f, 0.3f, 0.5f));
            
            // Horizontal lines
            for (int i = 0; i <= 4; i++)
            {
                var y = pos.Y + (size.Y / 4) * i;
                drawList.AddLine(new Vector2(pos.X, y), new Vector2(pos.X + size.X, y), gridColor);
            }
            
            // Vertical lines
            for (int i = 0; i <= 6; i++)
            {
                var x = pos.X + (size.X / 6) * i;
                drawList.AddLine(new Vector2(x, pos.Y), new Vector2(x, pos.Y + size.Y), gridColor);
            }
        }

        private void DrawGraphLine(ImDrawListPtr drawList, Vector2 pos, Vector2 size, float[] data, Vector4 color, float maxValue)
        {
            if (data.Length < 2) return;
            
            var points = new List<Vector2>();
            
            for (int i = 0; i < data.Length; i++)
            {
                var x = pos.X + (size.X / (data.Length - 1)) * i;
                var normalizedValue = Math.Min(1.0f, data[i] / maxValue);
                var y = pos.Y + size.Y - (normalizedValue * size.Y);
                points.Add(new Vector2(x, y));
            }
            
            // Draw line
            for (int i = 0; i < points.Count - 1; i++)
            {
                drawList.AddLine(points[i], points[i + 1], ImGui.ColorConvertFloat4ToU32(color), 2.0f);
            }
            
            // Draw points
            foreach (var point in points)
            {
                drawList.AddCircleFilled(point, 2.0f, ImGui.ColorConvertFloat4ToU32(color));
            }
        }

        private void DrawSystemInfo()
        {
            if (!_compactMode)
                ImGui.Text("System Information:");
            
            var uptime = DateTime.Now - _framework.StartTime;
            
            if (_compactMode)
            {
                // Compact single-line display
                ImGui.Text($"FPS: {_currentStatus.FPS:F1}");
                ImGui.SameLine();
                ImGui.Text($"Mem: {_currentStatus.MemoryMB:F0}MB");
                ImGui.SameLine();
                ImGui.Text($"CPU: {_currentStatus.CPUUsage:F0}%");
                ImGui.SameLine();
                ImGui.Text($"Up: {uptime:hh\\:mm\\:ss}");
            }
            else
            {
                // Full display with details
                if (ImGui.BeginTable("SystemInfo", 2, ImGuiTableFlags.Borders))
                {
                    ImGui.TableSetupColumn("Metric");
                    ImGui.TableSetupColumn("Value");
                    ImGui.TableHeadersRow();
                    
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("FPS");
                    ImGui.TableSetColumnIndex(1);
                    var fpsColor = GetPerformanceColor(_currentStatus.FPS);
                    ImGui.TextColored(fpsColor, $"{_currentStatus.FPS:F1} ({GetPerformanceGrade(_currentStatus.FPS)})");
                    
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("Memory Usage");
                    ImGui.TableSetColumnIndex(1);
                    var memColor = GetMemoryColor(_currentStatus.MemoryMB);
                    ImGui.TextColored(memColor, $"{_currentStatus.MemoryMB:F2} MB");
                    
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("CPU Usage");
                    ImGui.TableSetColumnIndex(1);
                    var cpuColor = GetCpuColor(_currentStatus.CPUUsage);
                    ImGui.TextColored(cpuColor, $"{_currentStatus.CPUUsage:F1}%");
                    
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("Game Uptime");
                    ImGui.TableSetColumnIndex(1);
                    ImGui.Text(uptime.ToString(@"hh\:mm\:ss"));
                    
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("Frame Time");
                    ImGui.TableSetColumnIndex(1);
                    var frameTime = 1000.0f / _currentStatus.FPS;
                    ImGui.Text($"{frameTime:F2} ms");
                    
                    ImGui.EndTable();
                }
            }
            
            if (!_compactMode)
                ImGui.Separator();
        }

        private void DrawControls()
        {
            // Display options
            ImGui.Text("Display Options:");
            
            ImGui.Checkbox("Show Player Stats", ref _showPlayerStats);
            ImGui.SameLine();
            ImGui.Checkbox("Show Performance Graph", ref _showPerformanceGraph);
            
            if (!_compactMode)
            {
                ImGui.Checkbox("Show System Info", ref _showSystemInfo);
                ImGui.SameLine();
                ImGui.Checkbox("Compact Mode", ref _compactMode);
                
                ImGui.Separator();
                
                // Graph settings
                ImGui.Text("Graph Settings:");
                
                ImGui.Text("Graph Height:");
                ImGui.SameLine();
                ImGui.SetNextItemWidth(100);
                ImGui.SliderFloat("##graphheight", ref _graphHeight, 50f, 200f, "%.0fpx");
                
                // Color settings
                ImGui.Text("Graph Colors:");
                
                if (ImGui.ColorEdit3("FPS Color", ref _fpsColor))
                {
                    // Color updated
                }
                
                ImGui.SameLine();
                
                if (ImGui.ColorEdit3("Memory Color", ref _memoryColor))
                {
                    // Color updated
                }
                
                ImGui.SameLine();
                
                if (ImGui.ColorEdit3("CPU Color", ref _cpuColor))
                {
                    // Color updated
                }
            }
            else
            {
                ImGui.SameLine();
                ImGui.Checkbox("Compact Mode", ref _compactMode);
            }
        }

        private Vector4 GetPerformanceColor(float fps)
        {
            return fps switch
            {
                >= 60 => new Vector4(0, 1, 0, 1),    // Green
                >= 45 => new Vector4(1, 1, 0, 1),   // Yellow
                >= 30 => new Vector4(1, 0.5f, 0, 1), // Orange
                _ => new Vector4(1, 0, 0, 1)       // Red
            };
        }

        private Vector4 GetMemoryColor(float memoryMB)
        {
            return memoryMB switch
            {
                < 500 => new Vector4(0, 1, 0, 1),    // Green
                < 1000 => new Vector4(1, 1, 0, 1),   // Yellow
                < 2000 => new Vector4(1, 0.5f, 0, 1), // Orange
                _ => new Vector4(1, 0, 0, 1)       // Red
            };
        }

        private Vector4 GetCpuColor(float cpuUsage)
        {
            return cpuUsage switch
            {
                < 25 => new Vector4(0, 1, 0, 1),    // Green
                < 50 => new Vector4(1, 1, 0, 1),   // Yellow
                < 75 => new Vector4(1, 0.5f, 0, 1), // Orange
                _ => new Vector4(1, 0, 0, 1)       // Red
            };
        }

        private string GetPerformanceGrade(float fps)
        {
            return fps switch
            {
                >= 60 => "A+",
                >= 50 => "A",
                >= 40 => "B",
                >= 30 => "C",
                _ => "D"
            };
        }

        private float GetCpuUsage()
        {
            // Simplified CPU usage measurement
            // In a real implementation, you would use system APIs or performance counters
            return 20.0f + (float)(new Random().NextDouble() * 30.0f);
        }

        public void Dispose()
        {
            Service.Logger.Info("StatusMonitor disposed");
        }
    }
}