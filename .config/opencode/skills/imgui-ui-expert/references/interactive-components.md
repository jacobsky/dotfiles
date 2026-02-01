# Interactive Components for Dalamud Plugins

This document covers interactive utility components specifically designed for FFXIV Dalamud plugins, focusing on actionable interfaces and real-time interactions.

## Table of Contents

- [Quick Action Panels](#quick-action-panels)
- [Status Monitors](#status-monitors)
- [Interactive Tools](#interactive-tools)
- [Configuration Interfaces](#configuration-interfaces)
- [Workflow Utilities](#workflow-utilities)
- [Real-time Controls](#real-time-controls)

## Quick Action Panels

### Floating Action Bar

```csharp
public class QuickActionBar : IDisposable
{
    private readonly List<QuickAction> _actions = new();
    private bool _isVisible = true;
    private Vector2 _position = new(100, 100);
    private bool _isDragging = false;
    private Vector2 _dragOffset = Vector2.Zero;
    
    public record QuickAction(string Name, Action OnClick, FontAwesomeIcon Icon, Vector4 Color);
    
    public void Draw()
    {
        if (!_isVisible) return;
        
        // Set window position
        ImGui.SetNextWindowPos(_position, ImGuiCond.Always);
        
        // Minimal window style
        var flags = ImGuiWindowFlags.NoTitleBar | 
                   ImGuiWindowFlags.NoResize | 
                   ImGuiWindowFlags.AlwaysAutoResize |
                   ImGuiWindowFlags.NoScrollbar;
        
        if (ImGui.Begin("QuickActionBar", ref _isVisible, flags))
        {
            DrawActions();
            HandleDragging();
        }
        
        ImGui.End();
    }
    
    private void DrawActions()
    {
        var buttonSize = new Vector2(40, 40);
        var spacing = 5f;
        
        // Layout buttons in grid
        for (int i = 0; i < _actions.Count; i++)
        {
            var action = _actions[i];
            
            if (i > 0 && i % 4 != 0)
                ImGui.SameLine(0, spacing);
            
            // Colored button with icon
            ImGui.PushStyleColor(ImGuiCol.Button, action.Color);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, action.Color * 1.2f);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, action.Color * 0.8f);
            
            if (ImGui.Button($"{(char)action.Icon}##{i}", buttonSize))
            {
                action.OnClick.Invoke();
            }
            
            ImGui.PopStyleColor(3);
            
            // Tooltip
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip(action.Name);
            }
        }
    }
    
    private void HandleDragging()
    {
        if (ImGui.IsWindowFocused() && ImGui.IsMouseDragging(ImGuiMouseButton.Left))
        {
            if (!_isDragging)
            {
                _isDragging = true;
                _dragOffset = ImGui.GetMousePos() - _position;
            }
            
            _position = ImGui.GetMousePos() - _dragOffset;
        }
        else
        {
            _isDragging = false;
        }
    }
    
    public void AddAction(QuickAction action) => _actions.Add(action);
    public void RemoveAction(string name) => _actions.RemoveAll(a => a.Name == name);
}
```

### Context Menu Integration

```csharp
public class ContextMenuManager : IDisposable
{
    private readonly List<ContextMenuItem> _menuItems = new();
    private bool _showContextMenu = false;
    private Vector2 _contextMenuPos = Vector2.Zero;
    
    public record ContextMenuItem(string Label, Action OnClick, string Hotkey = "");
    
    public void Draw()
    {
        if (_showContextMenu)
        {
            ImGui.SetNextWindowPos(_contextMenuPos);
            
            if (ImGui.Begin("ContextMenu", ref _showContextMenu, 
                           ImGuiWindowFlags.NoTitleBar | 
                           ImGuiWindowFlags.NoMove | 
                           ImGuiWindowFlags.NoResize |
                           ImGuiWindowFlags.AlwaysAutoResize))
            {
                foreach (var item in _menuItems)
                {
                    if (ImGui.Selectable($"{item.Label}\t{item.Hotkey}"))
                    {
                        item.OnClick.Invoke();
                        _showContextMenu = false;
                    }
                }
            }
            
            // Close on click outside
            if (ImGui.IsMouseClicked(ImGuiMouseButton.Left) && !ImGui.IsWindowHovered())
            {
                _showContextMenu = false;
            }
            
            ImGui.End();
        }
    }
    
    public void ShowContextMenu(Vector2 position)
    {
        _contextMenuPos = position;
        _showContextMenu = true;
    }
    
    public void AddMenuItem(ContextMenuItem item) => _menuItems.Add(item);
}
```

## Status Monitors

### Real-time Performance Monitor

```csharp
public class PerformanceMonitor : IDisposable
{
    private readonly Queue<float> _fpsHistory = new();
    private readonly Queue<float> _memoryHistory = new();
    private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(100);
    private DateTime _lastUpdate = DateTime.MinValue;
    private const int MaxHistorySize = 60; // 6 seconds at 10 FPS
    
    public void Draw()
    {
        UpdateData();
        
        if (ImGui.Begin("Performance Monitor"))
        {
            DrawPerformanceStats();
            DrawPerformanceGraphs();
        }
        
        ImGui.End();
    }
    
    private void UpdateData()
    {
        var now = DateTime.Now;
        if ((now - _lastUpdate) < _updateInterval)
            return;
        
        // FPS
        var currentFPS = ImGui.GetIO().Framerate;
        _fpsHistory.Enqueue(currentFPS);
        if (_fpsHistory.Count > MaxHistorySize)
            _fpsHistory.Dequeue();
        
        // Memory (in MB)
        var memoryMB = GC.GetTotalMemory(false) / (1024f * 1024f);
        _memoryHistory.Enqueue(memoryMB);
        if (_memoryHistory.Count > MaxHistorySize)
            _memoryHistory.Dequeue();
        
        _lastUpdate = now;
    }
    
    private void DrawPerformanceStats()
    {
        ImGui.Text("Current Performance:");
        ImGui.Separator();
        
        var currentFPS = _fpsHistory.Count > 0 ? _fpsHistory.Last() : 0;
        var avgFPS = _fpsHistory.Count > 0 ? _fpsHistory.Average() : 0;
        var currentMemory = _memoryHistory.Count > 0 ? _memoryHistory.Last() : 0;
        var avgMemory = _memoryHistory.Count > 0 ? _memoryHistory.Average() : 0;
        
        ImGui.Text($"FPS: {currentFPS:F1} (Avg: {avgFPS:F1})");
        ImGui.Text($"Memory: {currentMemory:F2}MB (Avg: {avgMemory:F2}MB)");
        
        // Color code performance
        var fpsColor = currentFPS < 30 ? Red : currentFPS < 50 ? Yellow : Green;
        ImGui.SameLine();
        ImGui.TextColored(fpsColor, $"[{GetPerformanceGrade(currentFPS)}]");
    }
    
    private void DrawPerformanceGraphs()
    {
        ImGui.Separator();
        ImGui.Text("Performance History:");
        
        // FPS Graph
        if (_fpsHistory.Count > 1)
        {
            ImGui.Text("FPS:");
            ImGui.PlotLines("##fps", _fpsHistory.ToArray(), _fpsHistory.Count, 0, "", 0, 120, new Vector2(0, 80));
        }
        
        // Memory Graph
        if (_memoryHistory.Count > 1)
        {
            ImGui.Text("Memory:");
            ImGui.PlotLines("##memory", _memoryHistory.ToArray(), _memoryHistory.Count, 0, "", 0, 100, new Vector2(0, 80));
        }
    }
    
    private string GetPerformanceGrade(float fps)
    {
        return fps switch
        {
            >= 60 => "A+",
            >= 50 => "A",
            >= 40 => "B",
            >= 30 => "C",
            >= 20 => "D",
            _ => "F"
        };
    }
}
```

### Game Status Monitor

```csharp
public class GameStatusMonitor : IDisposable
{
    private readonly IClientState _clientState;
    private readonly ICondition _condition;
    private readonly IFramework _framework;
    
    public GameStatusMonitor(IClientState clientState, ICondition condition, IFramework framework)
    {
        _clientState = clientState;
        _condition = condition;
        _framework = framework;
    }
    
    public void Draw()
    {
        if (ImGui.Begin("Game Status"))
        {
            DrawPlayerStatus();
            DrawGameConditions();
            DrawSystemInfo();
        }
        
        ImGui.End();
    }
    
    private void DrawPlayerStatus()
    {
        ImGui.Text("Player Status:");
        ImGui.Separator();
        
        if (_clientState.IsLoggedIn && _clientState.LocalPlayer != null)
        {
            var player = _clientState.LocalPlayer;
            
            ImGui.Text($"Name: {player.Name}");
            ImGui.Text($"Level: {player.Level}");
            ImGui.Text($"Job: {player.ClassJob.Value?.Name}");
            ImGui.Text($"HP: {player.CurrentHp}/{player.MaxHp}");
            ImGui.Text($"MP: {player.CurrentMp}/{player.MaxMp}");
        }
        else
        {
            ImGui.Text("Not logged in");
        }
    }
    
    private void DrawGameConditions()
    {
        ImGui.Separator();
        ImGui.Text("Game Conditions:");
        
        var conditions = new[]
        {
            ("InCombat", ConditionFlag.InCombat),
            ("BindingDuty", ConditionFlag.BoundByDuty),
            ("BetweenAreas", ConditionFlag.BetweenAreas),
            ("WatchingCutscene", ConditionFlag.WatchingCutscene),
            ("OccupiedInEvent", ConditionFlag.OccupiedInEvent),
            ("OccupiedInQuest", ConditionFlag.OccupiedInQuestEvent)
        };
        
        foreach (var (name, flag) in conditions)
        {
            var isActive = _condition[flag];
            var color = isActive ? Green : Gray;
            ImGui.TextColored(color, $"{name}: {(isActive ? "Active" : "Inactive")}");
        }
    }
    
    private void DrawSystemInfo()
    {
        ImGui.Separator();
        ImGui.Text("System Information:");
        
        var frameTime = 1000.0f / ImGui.GetIO().Framerate;
        ImGui.Text($"Frame Time: {frameTime:F2}ms");
        ImGui.Text($"UI Scale: {ImGui.GetIO().DisplayFramebufferScale.X:F2}");
        
        var gameTime = DateTime.Now - _framework.StartTime;
        ImGui.Text($"Game Uptime: {gameTime:hh\\:mm\\:ss}");
    }
}
```

## Interactive Tools

### Loot Manager

```csharp
public class LootManager : IDisposable
{
    private readonly IGameGui _gameGui;
    private readonly IAddonLifecycle _addonLifecycle;
    private List<LootItem> _lootHistory = new();
    private bool _autoLootEnabled = false;
    private readonly Dictionary<string, bool> _itemFilters = new();
    
    public record LootItem(string Name, uint ItemId, uint Quantity, DateTime Timestamp);
    
    public LootManager(IGameGui gameGui, IAddonLifecycle addonLifecycle)
    {
        _gameGui = gameGui;
        _addonLifecycle = addonLifecycle;
        
        // Initialize default filters
        _itemFilters["All Items"] = true;
        _itemFilters["Gear"] = true;
        _itemFilters["Materials"] = true;
        _itemFilters["Consumables"] = true;
    }
    
    public void Draw()
    {
        if (ImGui.Begin("Loot Manager"))
        {
            DrawLootControls();
            DrawLootHistory();
            DrawLootFilters();
        }
        
        ImGui.End();
    }
    
    private void DrawLootControls()
    {
        ImGui.Text("Loot Controls:");
        ImGui.Separator();
        
        if (ImGui.Checkbox("Auto Loot", ref _autoLootEnabled))
        {
            // Toggle auto loot functionality
        }
        
        ImGui.SameLine();
        if (ImGui.Button("Clear History"))
        {
            _lootHistory.Clear();
        }
        
        ImGui.SameLine();
        if (ImGui.Button("Export Log"))
        {
            ExportLootLog();
        }
    }
    
    private void DrawLootHistory()
    {
        ImGui.Separator();
        ImGui.Text("Loot History:");
        
        if (_lootHistory.Count == 0)
        {
            ImGui.Text("No loot history available");
            return;
        }
        
        // Table for loot items
        if (ImGui.BeginTable("LootTable", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
        {
            ImGui.TableSetupColumn("Item");
            ImGui.TableSetupColumn("Quantity");
            ImGui.TableSetupColumn("Time");
            ImGui.TableSetupColumn("Actions");
            ImGui.TableHeadersRow();
            
            foreach (var loot in _lootHistory.TakeLast(20)) // Show last 20 items
            {
                ImGui.TableNextRow();
                
                ImGui.TableSetColumnIndex(0);
                ImGui.Text(loot.Name);
                
                ImGui.TableSetColumnIndex(1);
                ImGui.Text($"{loot.Quantity}");
                
                ImGui.TableSetColumnIndex(2);
                ImGui.Text(loot.Timestamp.ToString("HH:mm:ss"));
                
                ImGui.TableSetColumnIndex(3);
                if (ImGui.Button($"Link##{loot.ItemId}_{loot.Timestamp.Ticks}"))
                {
                    LinkItem(loot.ItemId);
                }
            }
            
            ImGui.EndTable();
        }
    }
    
    private void DrawLootFilters()
    {
        ImGui.Separator();
        ImGui.Text("Item Filters:");
        
        foreach (var (category, enabled) in _itemFilters)
        {
            if (ImGui.Checkbox(category, ref _itemFilters[category]))
            {
                ApplyFilters();
            }
        }
    }
    
    private void LinkItem(uint itemId)
    {
        // Link item in chat
        var itemLink = GetItemLink(itemId);
        if (!string.IsNullOrEmpty(itemLink))
        {
            _gameGui.Chat.Print(itemLink);
        }
    }
    
    private void ExportLootLog()
    {
        var filename = $"loot_log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
        var lines = _lootHistory.Select(item => 
            $"{item.Timestamp:yyyy-MM-dd HH:mm:ss},{item.Name},{item.Quantity},{item.ItemId}");
        
        File.WriteAllLines(filename, lines);
        _gameGui.Chat.Print($"Loot log exported to {filename}");
    }
}
```

### Market Board Watcher

```csharp
public class MarketBoardWatcher : IDisposable
{
    private readonly Dictionary<uint, MarketData> _watchedItems = new();
    private bool _alertsEnabled = true;
    private float _priceThreshold = 10000f;
    private int _maxHistory = 50;
    
    public record MarketData(string ItemName, List<float> PriceHistory, float CurrentPrice);
    
    public void Draw()
    {
        if (ImGui.Begin("Market Board Watcher"))
        {
            DrawWatcherControls();
            DrawWatchedItems();
        }
        
        ImGui.End();
    }
    
    private void DrawWatcherControls()
    {
        ImGui.Text("Watcher Controls:");
        ImGui.Separator();
        
        ImGui.Checkbox("Enable Price Alerts", ref _alertsEnabled);
        
        ImGui.SameLine();
        if (ImGui.Button("Add Item"))
        {
            ImGui.OpenPopup("AddWatchItem");
        }
        
        ImGui.SliderFloat("Alert Threshold", ref _priceThreshold, 1000f, 1000000f, "%.0f gil");
        
        // Add item popup
        if (ImGui.BeginPopup("AddWatchItem"))
        {
            DrawAddItemPopup();
            ImGui.EndPopup();
        }
    }
    
    private void DrawWatchedItems()
    {
        ImGui.Separator();
        ImGui.Text("Watched Items:");
        
        if (_watchedItems.Count == 0)
        {
            ImGui.Text("No items being watched");
            return;
        }
        
        foreach (var (itemId, data) in _watchedItems)
        {
            ImGui.PushID(itemId.ToString());
            
            // Item name and current price
            ImGui.Text(data.ItemName);
            ImGui.SameLine();
            ImGui.TextColored(Yellow, $"{data.CurrentPrice:N0} gil");
            
            // Price trend indicator
            if (data.PriceHistory.Count >= 2)
            {
                var trend = data.PriceHistory[^1] - data.PriceHistory[^2];
                var trendColor = trend > 0 ? Red : trend < 0 ? Green : Gray;
                var trendSymbol = trend > 0 ? "▲" : trend < 0 ? "▼" : "→";
                ImGui.SameLine();
                ImGui.TextColored(trendColor, trendSymbol);
            }
            
            // Mini price chart
            if (data.PriceHistory.Count > 1)
            {
                var prices = data.PriceHistory.TakeLast(10).ToArray();
                ImGui.PlotLines("##chart", prices, prices.Length, 0, "", 0, data.PriceHistory.Max() * 1.1f, new Vector2(100, 30));
                ImGui.SameLine();
            }
            
            // Remove button
            if (ImGui.Button("Remove"))
            {
                _watchedItems.Remove(itemId);
            }
            
            ImGui.PopID();
        }
    }
    
    private void DrawAddItemPopup()
    {
        ImGui.Text("Add Item to Watch List");
        ImGui.Separator();
        
        // Item search (simplified)
        static string searchText = "";
        ImGui.InputText("Item Name", ref searchText, 256);
        
        // Item list would be populated from market board search
        ImGui.Text("Item selection interface would go here");
        
        if (ImGui.Button("Add"))
        {
            // Add selected item to watch list
        }
    }
    
    public void UpdatePrice(uint itemId, float newPrice)
    {
        if (_watchedItems.TryGetValue(itemId, out var data))
        {
            data.PriceHistory.Add(newPrice);
            if (data.PriceHistory.Count > _maxHistory)
            {
                data.PriceHistory.RemoveAt(0);
            }
            
            // Check alert condition
            if (_alertsEnabled && newPrice <= _priceThreshold)
            {
                TriggerPriceAlert(data.ItemName, newPrice);
            }
        }
    }
    
    private void TriggerPriceAlert(string itemName, float price)
    {
        // Show notification and/or play sound
        Service.ChatGui.Print($"[Market Alert] {itemName} dropped to {price:N0} gil!");
        // Could also play a sound or show ImGui notification
    }
}
```

## Configuration Interfaces

### Feature Toggle Panel

```csharp
public class FeatureTogglePanel : IDisposable
{
    private readonly Dictionary<string, bool> _features = new();
    private readonly Dictionary<string, string> _featureDescriptions = new();
    private readonly Dictionary<string, KeyCode> _featureHotkeys = new();
    
    public void Draw()
    {
        if (ImGui.Begin("Feature Toggles"))
        {
            DrawFeatureList();
            DrawGlobalControls();
        }
        
        ImGui.End();
    }
    
    private void DrawFeatureList()
    {
        ImGui.Text("Plugin Features:");
        ImGui.Separator();
        
        foreach (var (featureName, enabled) in _features)
        {
            ImGui.PushID(featureName);
            
            // Feature name and toggle
            if (ImGui.Checkbox(featureName, ref _features[featureName]))
            {
                OnFeatureToggled(featureName, enabled);
            }
            
            // Description on hover
            if (ImGui.IsItemHovered() && _featureDescriptions.TryGetValue(featureName, out var desc))
            {
                ImGui.SetTooltip(desc);
            }
            
            // Hotkey display
            if (_featureHotkeys.TryGetValue(featureName, out var hotkey) && hotkey != KeyCode.None)
            {
                ImGui.SameLine();
                ImGui.TextColored(Gray, $"[{hotkey}]");
            }
            
            ImGui.PopID();
        }
    }
    
    private void DrawGlobalControls()
    {
        ImGui.Separator();
        ImGui.Text("Global Controls:");
        
        if (ImGui.Button("Enable All"))
        {
            foreach (var key in _features.Keys.ToList())
            {
                _features[key] = true;
            }
        }
        
        ImGui.SameLine();
        if (ImGui.Button("Disable All"))
        {
            foreach (var key in _features.Keys.ToList())
            {
                _features[key] = false;
            }
        }
        
        ImGui.SameLine();
        if (ImGui.Button("Reset to Default"))
        {
            ResetToDefaults();
        }
    }
    
    public void RegisterFeature(string name, bool defaultValue, string description = "", KeyCode hotkey = KeyCode.None)
    {
        _features[name] = defaultValue;
        _featureDescriptions[name] = description;
        _featureHotkeys[name] = hotkey;
    }
    
    private void OnFeatureToggled(string featureName, bool newEnabled)
    {
        // Handle feature toggle logic
        Service.Logger.Info($"Feature {featureName} {(newEnabled ? "enabled" : "disabled")}");
    }
}
```

## Workflow Utilities

### Multi-step Action Wizard

```csharp
public class ActionWizard : IDisposable
{
    private readonly List<WizardStep> _steps = new();
    private int _currentStep = 0;
    private bool _isActive = false;
    private readonly Dictionary<string, object> _stepData = new();
    
    public abstract class WizardStep
    {
        public abstract string Title { get; }
        public abstract bool DrawStep(Dictionary<string, object> data);
        public abstract bool CanProceed(Dictionary<string, object> data);
    }
    
    public void Draw()
    {
        if (!_isActive) return;
        
        if (ImGui.Begin("Action Wizard", ref _isActive))
        {
            DrawWizardHeader();
            DrawCurrentStep();
            DrawNavigationButtons();
        }
        
        ImGui.End();
    }
    
    private void DrawWizardHeader()
    {
        ImGui.Text("Configuration Wizard");
        ImGui.Separator();
        
        // Progress bar
        var progress = (float)_currentStep / _steps.Count;
        ImGui.ProgressBar(progress, new Vector2(-1, 0), $"Step {_currentStep + 1} of {_steps.Count}");
        
        // Step indicators
        for (int i = 0; i < _steps.Count; i++)
        {
            var color = i == _currentStep ? White : 
                       i < _currentStep ? Green : Gray;
            
            ImGui.TextColored(color, $"{i + 1}. {_steps[i].Title}");
            if (i < _steps.Count - 1)
            {
                ImGui.SameLine();
                ImGui.TextColored(Gray, "→");
                ImGui.SameLine();
            }
        }
        
        ImGui.Separator();
    }
    
    private void DrawCurrentStep()
    {
        if (_currentStep >= 0 && _currentStep < _steps.Count)
        {
            _steps[_currentStep].DrawStep(_stepData);
        }
    }
    
    private void DrawNavigationButtons()
    {
        ImGui.Separator();
        
        // Navigation buttons
        var canGoBack = _currentStep > 0;
        var canProceed = _currentStep < _steps.Count - 1 && 
                       _steps[_currentStep].CanProceed(_stepData);
        var canFinish = _currentStep == _steps.Count - 1;
        
        if (canGoBack)
        {
            if (ImGui.Button("Previous"))
            {
                _currentStep--;
            }
            
            if (canProceed || canFinish)
                ImGui.SameLine();
        }
        
        if (canProceed)
        {
            if (ImGui.Button("Next"))
            {
                _currentStep++;
            }
        }
        
        if (canFinish)
        {
            if (ImGui.Button("Finish"))
            {
                CompleteWizard();
            }
        }
        
        ImGui.SameLine();
        if (ImGui.Button("Cancel"))
        {
            _isActive = false;
            _currentStep = 0;
            _stepData.Clear();
        }
    }
    
    private void CompleteWizard()
    {
        // Process all step data
        ProcessWizardData();
        _isActive = false;
        _currentStep = 0;
        _stepData.Clear();
    }
    
    public void AddStep(WizardStep step) => _steps.Add(step);
    public void Start() => _isActive = true;
}
```

## Real-time Controls

### Interactive Timeline

```csharp
public class InteractiveTimeline : IDisposable
{
    private readonly List<TimelineEvent> _events = new();
    private float _currentTime = 0f;
    private float _maxTime = 300f; // 5 minutes
    private bool _isPlaying = false;
    private float _playbackSpeed = 1f;
    private DateTime _lastUpdateTime = DateTime.MinValue;
    
    public record TimelineEvent(string Name, float Time, Color Color, bool Triggered = false);
    
    public void Draw()
    {
        UpdatePlayback();
        
        if (ImGui.Begin("Interactive Timeline"))
        {
            DrawPlaybackControls();
            DrawTimeline();
            DrawEventList();
        }
        
        ImGui.End();
    }
    
    private void DrawPlaybackControls()
    {
        ImGui.Text("Playback Controls:");
        
        // Play/Pause button
        var buttonText = _isPlaying ? "Pause" : "Play";
        if (ImGui.Button(buttonText))
        {
            _isPlaying = !_isPlaying;
            _lastUpdateTime = DateTime.Now;
        }
        
        ImGui.SameLine();
        if (ImGui.Button("Stop"))
        {
            _isPlaying = false;
            _currentTime = 0f;
            ResetEventTriggers();
        }
        
        // Speed control
        ImGui.SameLine();
        ImGui.Text("Speed:");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(80f);
        ImGui.SliderFloat("##speed", ref _playbackSpeed, 0.1f, 3f, "%.1fx");
        
        // Time display
        ImGui.SameLine();
        ImGui.Text($"Time: {FormatTime(_currentTime)}");
    }
    
    private void DrawTimeline()
    {
        ImGui.Separator();
        ImGui.Text("Timeline:");
        
        var timelineSize = new Vector2(ImGui.GetContentRegionAvail().X, 50f);
        var drawList = ImGui.GetWindowDrawList();
        var pos = ImGui.GetCursorScreenPos();
        
        // Draw timeline background
        drawList.AddRectFilled(pos, pos + timelineSize, ImGui.ColorConvertFloat4ToU32(Gray * 0.3f));
        drawList.AddRect(pos, pos + timelineSize, ImGui.ColorConvertFloat4ToU32(White));
        
        // Draw events
        foreach (var evt in _events)
        {
            var eventX = pos.X + (evt.Time / _maxTime) * timelineSize.X;
            var eventY = pos.Y + timelineSize.Y / 2f;
            
            var color = evt.Triggered ? Green : evt.Color;
            drawList.AddCircleFilled(new Vector2(eventX, eventY), 8f, ImGui.ColorConvertFloat4ToU32(color));
            
            // Event label
            var labelPos = new Vector2(eventX, pos.Y + timelineSize.Y + 5f);
            drawList.AddText(labelPos, ImGui.ColorConvertFloat4ToU32(White), evt.Name);
        }
        
        // Draw current time indicator
        var currentX = pos.X + (_currentTime / _maxTime) * timelineSize.X;
        drawList.AddLine(new Vector2(currentX, pos.Y), new Vector2(currentX, pos.Y + timelineSize.Y), 
                       ImGui.ColorConvertFloat4ToU32(Red), 2f);
        
        ImGui.Dummy(timelineSize);
        
        // Handle timeline interaction
        if (ImGui.IsMouseHoveringRect(pos, pos + timelineSize) && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
        {
            var mousePos = ImGui.GetMousePos();
            var relativeX = mousePos.X - pos.X;
            _currentTime = (relativeX / timelineSize.X) * _maxTime;
        }
    }
    
    private void DrawEventList()
    {
        ImGui.Separator();
        ImGui.Text("Events:");
        
        foreach (var evt in _events)
        {
            var color = evt.Triggered ? Green : evt.Color;
            
            ImGui.PushStyleColor(ImGuiCol.Text, color);
            ImGui.Text($"[{FormatTime(evt.Time)}] {evt.Name}");
            ImGui.PopStyleColor();
            
            if (!evt.Triggered && _currentTime >= evt.Time)
            {
                TriggerEvent(evt);
            }
        }
    }
    
    private void UpdatePlayback()
    {
        if (!_isPlaying) return;
        
        var now = DateTime.Now;
        if (_lastUpdateTime == DateTime.MinValue)
        {
            _lastUpdateTime = now;
            return;
        }
        
        var deltaTime = (now - _lastUpdateTime).TotalSeconds;
        _currentTime += (float)deltaTime * _playbackSpeed;
        
        if (_currentTime >= _maxTime)
        {
            _currentTime = _maxTime;
            _isPlaying = false;
        }
        
        _lastUpdateTime = now;
    }
    
    private void TriggerEvent(TimelineEvent evt)
    {
        evt.Triggered = true;
        // Trigger event logic here
        Service.Logger.Info($"Timeline event triggered: {evt.Name}");
    }
    
    private void ResetEventTriggers()
    {
        foreach (var evt in _events)
        {
            evt.Triggered = false;
        }
    }
    
    private string FormatTime(float time)
    {
        var minutes = (int)(time / 60f);
        var seconds = (int)(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}
```

These interactive components provide a solid foundation for creating engaging, actionable interfaces in Dalamud plugins. Each component is designed with performance, usability, and game-specific considerations in mind.