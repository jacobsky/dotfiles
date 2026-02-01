using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ImGuiNET;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Plugin.Services;

namespace MyPlugin.UI
{
    /// <summary>
    /// Data visualization dashboard with real-time updates
    /// Level 3-4 complexity - Advanced to Expert
    /// </summary>
    public class DataVisualization : IDisposable
    {
        private bool _visible = true;
        private readonly Dictionary<string, DataSeries> _dataSeries = new();
        private readonly List<ChartType> _availableCharts = new();
        private int _selectedChart = 0;
        private bool _isRecording = false;
        private DateTime _startTime = DateTime.Now;
        private float _updateInterval = 0.5f;
        private DateTime _lastUpdate = DateTime.MinValue;
        
        // Chart settings
        private int _maxDataPoints = 100;
        private bool _showGrid = true;
        private bool _showLegend = true;
        private Vector4[] _chartColors = new[]
        {
            new Vector4(1, 0, 0, 1),    // Red
            new Vector4(0, 1, 0, 1),    // Green
            new Vector4(0, 0, 1, 1),    // Blue
            new Vector4(1, 1, 0, 1),    // Yellow
            new Vector4(1, 0, 1, 1),    // Magenta
            new Vector4(0, 1, 1, 1),    // Cyan
        };
        
        public record DataSeries(string Name, List<float> Values, Vector4 Color);
        public enum ChartType { Line, Bar, Area, Scatter }
        
        public DataVisualization()
        {
            InitializeDataSeries();
            InitializeCharts();
        }

        public bool Visible
        {
            get => _visible;
            set => _visible = value;
        }

        private void InitializeDataSeries()
        {
            _dataSeries["FPS"] = new DataSeries("FPS", new List<float>(), _chartColors[0]);
            _dataSeries["Memory"] = new DataSeries("Memory (MB)", new List<float>(), _chartColors[1]);
            _dataSeries["CPU"] = new DataSeries("CPU (%)", new List<float>(), _chartColors[2]);
            _dataSeries["Network"] = new DataSeries("Network", new List<float>(), _chartColors[3]);
            _dataSeries["Response Time"] = new DataSeries("Response Time", new List<float>(), _chartColors[4]);
        }

        private void InitializeCharts()
        {
            _availableCharts.Add(ChartType.Line);
            _availableCharts.Add(ChartType.Bar);
            _availableCharts.Add(ChartType.Area);
            _availableCharts.Add(ChartType.Scatter);
        }

        public void Draw()
        {
            if (!_visible) return;

            UpdateData();
            
            var windowFlags = ImGuiWindowFlags.None;
            ImGui.SetNextWindowSize(new Vector2(800, 600), ImGuiCond.FirstUseEver);
            
            if (ImGui.Begin("Data Visualization Dashboard", ref _visible, windowFlags))
            {
                DrawHeader();
                DrawChartControls();
                DrawMainChart();
                DrawStatistics();
                DrawDataSeriesPanel();
            }
            
            ImGui.End();
        }

        private void UpdateData()
        {
            var now = DateTime.Now;
            if ((now - _lastUpdate).TotalSeconds < _updateInterval)
                return;
            
            if (_isRecording)
            {
                // Collect performance data
                var fps = ImGui.GetIO().Framerate;
                var memory = GC.GetTotalMemory(false) / (1024f * 1024f);
                var cpu = GetCpuUsage(); // Simplified
                var network = GetNetworkUsage(); // Simplified
                var responseTime = GetResponseTime(); // Simplified
                
                // Add data points
                AddDataPoint("FPS", fps);
                AddDataPoint("Memory", memory);
                AddDataPoint("CPU", cpu);
                AddDataPoint("Network", network);
                AddDataPoint("Response Time", responseTime);
            }
            
            _lastUpdate = now;
        }

        private void AddDataPoint(string seriesName, float value)
        {
            if (_dataSeries.TryGetValue(seriesName, out var series))
            {
                series.Values.Add(value);
                
                // Limit data points
                while (series.Values.Count > _maxDataPoints)
                {
                    series.Values.RemoveAt(0);
                }
            }
        }

        private void DrawHeader()
        {
            // Title and recording status
            ImGui.Text("Data Visualization Dashboard");
            
            ImGui.SameLine();
            
            var recordingColor = _isRecording ? new Vector4(1, 0, 0, 1) : new Vector4(0.5f, 0.5f, 0.5f, 1);
            ImGui.TextColored(recordingColor, _isRecording ? "● Recording" : "○ Stopped");
            
            // Time display
            ImGui.SameLine();
            ImGuiHelpers.RightAlign($"Runtime: {DateTime.Now - _startTime:hh\\:mm\\:ss}");
            
            ImGui.Separator();
        }

        private void DrawChartControls()
        {
            // Recording controls
            if (_isRecording)
            {
                ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(1, 0, 0, 1));
                if (ImGui.Button("Stop Recording"))
                {
                    _isRecording = false;
                }
                ImGui.PopStyleColor();
            }
            else
            {
                ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0, 1, 0, 1));
                if (ImGui.Button("Start Recording"))
                {
                    _isRecording = true;
                    _startTime = DateTime.Now;
                }
                ImGui.PopStyleColor();
            }
            
            ImGui.SameLine();
            
            if (ImGui.Button("Clear Data"))
            {
                ClearAllData();
            }
            
            ImGui.SameLine();
            
            if (ImGui.Button("Export Data"))
            {
                ExportData();
            }
            
            // Chart type selection
            ImGui.SameLine();
            ImGuiHelpers.RightAlign("Chart Type: ");
            
            if (ImGui.BeginCombo("##charttype", _availableCharts[_selectedChart].ToString()))
            {
                for (int i = 0; i < _availableCharts.Count; i++)
                {
                    var isSelected = i == _selectedChart;
                    if (ImGui.Selectable(_availableCharts[i].ToString(), isSelected))
                    {
                        _selectedChart = i;
                    }
                }
                ImGui.EndCombo();
            }
            
            // Update interval
            ImGui.SameLine();
            ImGui.Text("Update Rate:");
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100);
            if (ImGui.SliderFloat("##updaterate", ref _updateInterval, 0.1f, 5.0f, "%.1fs"))
            {
                // Update interval changed
            }
            
            ImGui.Separator();
        }

        private void DrawMainChart()
        {
            var chartType = _availableCharts[_selectedChart];
            var chartTitle = $"{chartType} Chart - Real-time Data";
            
            ImGui.Text(chartTitle);
            
            // Calculate chart dimensions
            var availableSize = ImGui.GetContentRegionAvail();
            var chartSize = new Vector2(availableSize.X, 300);
            
            switch (chartType)
            {
                case ChartType.Line:
                    DrawLineChart(chartSize);
                    break;
                case ChartType.Bar:
                    DrawBarChart(chartSize);
                    break;
                case ChartType.Area:
                    DrawAreaChart(chartSize);
                    break;
                case ChartType.Scatter:
                    DrawScatterChart(chartSize);
                    break;
            }
            
            ImGui.Separator();
        }

        private void DrawLineChart(Vector2 size)
        {
            var drawList = ImGui.GetWindowDrawList();
            var pos = ImGui.GetCursorScreenPos();
            
            // Background
            drawList.AddRectFilled(pos, pos + size, ImGui.ColorConvertFloat4ToU32(new Vector4(0.1f, 0.1f, 0.1f, 1.0f)));
            
            if (_showGrid)
            {
                DrawGrid(drawList, pos, size);
            }
            
            // Draw each data series
            foreach (var (name, series) in _dataSeries)
            {
                if (series.Values.Count < 2) continue;
                
                DrawLineSeries(drawList, pos, size, series, name);
            }
            
            // Border
            drawList.AddRect(pos, pos + size, ImGui.ColorConvertFloat4ToU32(new Vector4(1, 1, 1, 1.0f)));
            
            ImGui.Dummy(size);
            
            if (_showLegend)
            {
                DrawLegend();
            }
        }

        private void DrawBarChart(Vector2 size)
        {
            var drawList = ImGui.GetWindowDrawList();
            var pos = ImGui.GetCursorScreenPos();
            
            // Background
            drawList.AddRectFilled(pos, pos + size, ImGui.ColorConvertFloat4ToU32(new Vector4(0.1f, 0.1f, 0.1f, 1.0f)));
            
            if (_showGrid)
            {
                DrawGrid(drawList, pos, size);
            }
            
            // Group bars by series
            var barWidth = size.X / (_dataSeries.Count * 2); // Spacing between groups
            var groupIndex = 0;
            
            foreach (var (name, series) in _dataSeries)
            {
                if (series.Values.Count == 0) continue;
                
                var latestValue = series.Values.LastOrDefault();
                var normalizedValue = NormalizeValue(latestValue, name);
                var barHeight = normalizedValue * (size.Y - 20); // Leave margin for labels
                var barX = pos.X + groupIndex * barWidth * 2 + barWidth / 2;
                var barY = pos.Y + size.Y - barHeight - 10;
                
                // Draw bar
                drawList.AddRectFilled(
                    new Vector2(barX, barY),
                    new Vector2(barX + barWidth, barY + barHeight),
                    ImGui.ColorConvertFloat4ToU32(series.Color)
                );
                
                // Value label
                var valueText = latestValue.ToString("F1");
                var textSize = ImGui.CalcTextSize(valueText);
                var textPos = new Vector2(barX + (barWidth - textSize.X) / 2, barY - textSize.Y - 5);
                drawList.AddText(textPos, ImGui.ColorConvertFloat4ToU32(new Vector4(1, 1, 1, 1)), valueText);
                
                groupIndex++;
            }
            
            // Border
            drawList.AddRect(pos, pos + size, ImGui.ColorConvertFloat4ToU32(new Vector4(1, 1, 1, 1.0f)));
            
            ImGui.Dummy(size);
            
            if (_showLegend)
            {
                DrawLegend();
            }
        }

        private void DrawAreaChart(Vector2 size)
        {
            var drawList = ImGui.GetWindowDrawList();
            var pos = ImGui.GetCursorScreenPos();
            
            // Background
            drawList.AddRectFilled(pos, pos + size, ImGui.ColorConvertFloat4ToU32(new Vector4(0.1f, 0.1f, 0.1f, 1.0f)));
            
            if (_showGrid)
            {
                DrawGrid(drawList, pos, size);
            }
            
            // Draw each data series as filled area
            foreach (var (name, series) in _dataSeries)
            {
                if (series.Values.Count < 2) continue;
                
                DrawAreaSeries(drawList, pos, size, series, name);
            }
            
            // Border
            drawList.AddRect(pos, pos + size, ImGui.ColorConvertFloat4ToU32(new Vector4(1, 1, 1, 1.0f)));
            
            ImGui.Dummy(size);
            
            if (_showLegend)
            {
                DrawLegend();
            }
        }

        private void DrawScatterChart(Vector2 size)
        {
            var drawList = ImGui.GetWindowDrawList();
            var pos = ImGui.GetCursorScreenPos();
            
            // Background
            drawList.AddRectFilled(pos, pos + size, ImGui.ColorConvertFloat4ToU32(new Vector4(0.1f, 0.1f, 0.1f, 1.0f)));
            
            if (_showGrid)
            {
                DrawGrid(drawList, pos, size);
            }
            
            // Draw scatter points for each series
            foreach (var (name, series) in _dataSeries)
            {
                if (series.Values.Count == 0) continue;
                
                DrawScatterSeries(drawList, pos, size, series, name);
            }
            
            // Border
            drawList.AddRect(pos, pos + size, ImGui.ColorConvertFloat4ToU32(new Vector4(1, 1, 1, 1.0f)));
            
            ImGui.Dummy(size);
            
            if (_showLegend)
            {
                DrawLegend();
            }
        }

        private void DrawGrid(ImDrawListPtr drawList, Vector2 pos, Vector2 size)
        {
            var gridColor = ImGui.ColorConvertFloat4ToU32(new Vector4(0.3f, 0.3f, 0.3f, 1.0f));
            
            // Vertical lines
            for (int i = 0; i <= 10; i++)
            {
                var x = pos.X + (size.X / 10) * i;
                drawList.AddLine(new Vector2(x, pos.Y), new Vector2(x, pos.Y + size.Y), gridColor);
            }
            
            // Horizontal lines
            for (int i = 0; i <= 5; i++)
            {
                var y = pos.Y + (size.Y / 5) * i;
                drawList.AddLine(new Vector2(pos.X, y), new Vector2(pos.X + size.X, y), gridColor);
            }
        }

        private void DrawLineSeries(ImDrawListPtr drawList, Vector2 pos, Vector2 size, DataSeries series, string name)
        {
            var points = new List<Vector2>();
            
            for (int i = 0; i < series.Values.Count; i++)
            {
                var x = pos.X + (size.X / _maxDataPoints) * i;
                var normalizedValue = NormalizeValue(series.Values[i], name);
                var y = pos.Y + size.Y - (normalizedValue * (size.Y - 20)) - 10;
                points.Add(new Vector2(x, y));
            }
            
            // Draw line
            if (points.Count >= 2)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    drawList.AddLine(points[i], points[i + 1], ImGui.ColorConvertFloat4ToU32(series.Color), 2.0f);
                }
            }
            
            // Draw points
            foreach (var point in points)
            {
                drawList.AddCircleFilled(point, 3.0f, ImGui.ColorConvertFloat4ToU32(series.Color));
            }
        }

        private void DrawAreaSeries(ImDrawListPtr drawList, Vector2 pos, Vector2 size, DataSeries series, string name)
        {
            var points = new List<Vector2>();
            
            for (int i = 0; i < series.Values.Count; i++)
            {
                var x = pos.X + (size.X / _maxDataPoints) * i;
                var normalizedValue = NormalizeValue(series.Values[i], name);
                var y = pos.Y + size.Y - (normalizedValue * (size.Y - 20)) - 10;
                points.Add(new Vector2(x, y));
            }
            
            // Create filled polygon
            if (points.Count >= 2)
            {
                var polygonPoints = new List<Vector2>();
                
                // Add the area points
                foreach (var point in points)
                {
                    polygonPoints.Add(point);
                }
                
                // Close the polygon at the bottom
                polygonPoints.Add(new Vector2(points.Last().X, pos.Y + size.Y - 10));
                polygonPoints.Add(new Vector2(points.First().X, pos.Y + size.Y - 10));
                
                // Draw filled polygon with transparency
                var color = series.Color with { W = 0.5f };
                drawList.AddConvexPolyFilled(ref polygonPoints.ToArray(), polygonPoints.Count, ImGui.ColorConvertFloat4ToU32(color));
                
                // Draw the line on top
                for (int i = 0; i < points.Count - 1; i++)
                {
                    drawList.AddLine(points[i], points[i + 1], ImGui.ColorConvertFloat4ToU32(series.Color), 2.0f);
                }
            }
        }

        private void DrawScatterSeries(ImDrawListPtr drawList, Vector2 pos, Vector2 size, DataSeries series, string name)
        {
            for (int i = 0; i < series.Values.Count; i++)
            {
                var x = pos.X + (size.X / _maxDataPoints) * i;
                var normalizedValue = NormalizeValue(series.Values[i], name);
                var y = pos.Y + size.Y - (normalizedValue * (size.Y - 20)) - 10;
                
                // Draw scatter point
                drawList.AddCircleFilled(new Vector2(x, y), 4.0f, ImGui.ColorConvertFloat4ToU32(series.Color));
            }
        }

        private void DrawLegend()
        {
            ImGui.Text("Legend:");
            
            foreach (var (name, series) in _dataSeries)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, series.Color);
                ImGui.Text($"● {name}");
                ImGui.PopStyleColor();
                
                if (series.Values.Count > 0)
                {
                    ImGui.SameLine();
                    ImGui.Text($"(Current: {series.Values.LastOrDefault():F1})");
                }
            }
        }

        private void DrawStatistics()
        {
            ImGui.Text("Statistics:");
            
            if (ImGui.BeginTable("StatsTable", 3, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
            {
                ImGui.TableSetupColumn("Metric");
                ImGui.TableSetupColumn("Current");
                ImGui.TableSetupColumn("Average");
                ImGui.TableHeadersRow();
                
                foreach (var (name, series) in _dataSeries)
                {
                    if (series.Values.Count == 0) continue;
                    
                    ImGui.TableNextRow();
                    
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text(name);
                    
                    ImGui.TableSetColumnIndex(1);
                    ImGui.Text($"{series.Values.LastOrDefault():F2}");
                    
                    ImGui.TableSetColumnIndex(2);
                    ImGui.Text($"{series.Values.Average():F2}");
                }
                
                ImGui.EndTable();
            }
        }

        private void DrawDataSeriesPanel()
        {
            ImGui.Separator();
            ImGui.Text("Data Series Settings:");
            
            // Max data points
            ImGui.Text($"Max Data Points: {_maxDataPoints}");
            ImGui.SameLine();
            if (ImGui.Button("-##maxpoints"))
            {
                _maxDataPoints = Math.Max(10, _maxDataPoints - 10);
            }
            ImGui.SameLine();
            if (ImGui.Button("+##maxpoints"))
            {
                _maxDataPoints = Math.Min(500, _maxDataPoints + 10);
            }
            
            // Display options
            ImGui.Checkbox("Show Grid", ref _showGrid);
            ImGui.SameLine();
            ImGui.Checkbox("Show Legend", ref _showLegend);
            
            // Series toggles
            ImGui.Separator();
            ImGui.Text("Toggle Series:");
            
            var activeSeries = _dataSeries.Where(kvp => kvp.Value.Values.Count > 0).ToList();
            foreach (var (name, series) in activeSeries)
            {
                var isActive = series.Values.Count > 0;
                
                if (ImGui.Checkbox(name, ref isActive))
                {
                    if (!isActive)
                    {
                        series.Values.Clear();
                    }
                }
            }
        }

        private float NormalizeValue(float value, string seriesName)
        {
            // Simple normalization - in real implementation would use actual min/max ranges
            return seriesName switch
            {
                "FPS" => Math.Min(1.0f, value / 120.0f),
                "Memory" => Math.Min(1.0f, value / 1000.0f),
                "CPU" => Math.Min(1.0f, value / 100.0f),
                "Network" => Math.Min(1.0f, value / 100.0f),
                "Response Time" => Math.Min(1.0f, value / 1000.0f),
                _ => 0.5f
            };
        }

        private float GetCpuUsage()
        {
            // Simplified CPU usage - would use actual system metrics
            return 25.0f + (float)(new Random().NextDouble() * 30.0f);
        }

        private float GetNetworkUsage()
        {
            // Simplified network usage - would use actual network metrics
            return 10.0f + (float)(new Random().NextDouble() * 20.0f);
        }

        private float GetResponseTime()
        {
            // Simplified response time - would measure actual response times
            return 50.0f + (float)(new Random().NextDouble() * 100.0f);
        }

        private void ClearAllData()
        {
            foreach (var series in _dataSeries.Values)
            {
                series.Values.Clear();
            }
        }

        private void ExportData()
        {
            var filename = $"data_export_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            
            try
            {
                using var writer = new System.IO.StreamWriter(filename);
                
                // Write header
                var headers = _dataSeries.Keys.ToList();
                writer.WriteLine(string.Join(",", headers));
                
                // Write data
                var maxRows = _dataSeries.Values.Max(series => series.Values.Count);
                for (int row = 0; row < maxRows; row++)
                {
                    var values = new List<string>();
                    foreach (var header in headers)
                    {
                        var series = _dataSeries[header];
                        if (row < series.Values.Count)
                        {
                            values.Add(series.Values[row].ToString("F2"));
                        }
                        else
                        {
                            values.Add("");
                        }
                    }
                    writer.WriteLine(string.Join(",", values));
                }
                
                Service.Logger.Info($"Data exported to {filename}");
            }
            catch (Exception ex)
            {
                Service.Logger.Error(ex, "Failed to export data");
            }
        }

        public void Dispose()
        {
            Service.Logger.Info("DataVisualization disposed");
        }
    }
}