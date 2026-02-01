# ImGui Patterns for Game Development

This document covers common ImGui patterns and techniques specifically adapted for game development and FFXIV plugin environments.

## Table of Contents

- [Performance Considerations](#performance-considerations)
- [Real-time Data Display](#real-time-data-display)
- [Non-blocking UI](#non-blocking-ui)
- [Context-aware Interfaces](#context-aware-interfaces)
- [Input Management](#input-management)
- [State Synchronization](#state-synchronization)
- [Memory Management](#memory-management)

## Performance Considerations

### Frame Rate Impact Mitigation

Game environments are sensitive to UI performance. Follow these patterns to minimize impact:

```csharp
public class PerformanceOptimizedUI
{
    private DateTime _lastUpdate = DateTime.MinValue;
    private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(100); // 10 FPS for UI
    
    public void Draw()
    {
        // Throttle expensive operations
        var now = DateTime.Now;
        var shouldUpdate = (now - _lastUpdate) > _updateInterval;
        
        if (shouldUpdate)
        {
            UpdateExpensiveData();
            _lastUpdate = now;
        }
        
        // Always render, but only update data when needed
        RenderUI();
    }
    
    private void UpdateExpensiveData()
    {
        // Expensive calculations, data fetching, etc.
    }
}
```

### Efficient Rendering Patterns

```csharp
public class EfficientRendering
{
    private bool _shouldRedraw = true;
    private List<DisplayItem> _cachedItems = new();
    
    public void Draw()
    {
        // Only rebuild lists when data changes
        if (_shouldRedraw)
        {
            RebuildDisplayList();
            _shouldRedraw = false;
        }
        
        // Render cached data
        foreach (var item in _cachedItems)
        {
            ImGui.Text(item.Name);
            ImGui.SameLine();
            ImGui.Text($"{item.Value:F2}");
        }
    }
    
    public void InvalidateCache()
    {
        _shouldRedraw = true;
    }
}
```

### Draw List Optimization

```csharp
public class DrawListOptimization
{
    private uint _lastColor = 0;
    private Vector2 _lastPos = Vector2.Zero;
    
    public void DrawOptimizedShapes(ImDrawListPtr drawList)
    {
        // Batch similar draw calls
        foreach (var shape in shapes)
        {
            if (shape.Color != _lastColor)
            {
                _lastColor = shape.Color;
                // Color change happens here
            }
            
            drawList.AddRectFilled(shape.Pos, shape.Size, shape.Color);
        }
    }
}
```

## Real-time Data Display

### Data Throttling and Caching

```csharp
public class RealTimeDataDisplay
{
    private readonly ConcurrentDictionary<string, CachedData> _dataCache = new();
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMilliseconds(50);
    
    public struct CachedData
    {
        public string Value;
        public DateTime LastUpdate;
    }
    
    public string GetCachedData(string key, Func<string> dataProvider)
    {
        var now = DateTime.Now;
        
        if (_dataCache.TryGetValue(key, out var cached) && 
            (now - cached.LastUpdate) < _cacheDuration)
        {
            return cached.Value;
        }
        
        var newValue = dataProvider();
        _dataCache[key] = new CachedData
        {
            Value = newValue,
            LastUpdate = now
        };
        
        return newValue;
    }
    
    public void DrawPlayerStats()
    {
        ImGui.Text($"HP: {GetCachedData("hp", () => GetCurrentHP())}");
        ImGui.Text($"MP: {GetCachedData("mp", () => GetCurrentMP())}");
        ImGui.Text($"TP: {GetCachedData("tp", () => GetCurrentTP())}");
    }
}
```

### Status Indicators

```csharp
public class StatusIndicators
{
    public void DrawStatusIndicator(string label, bool status, Vector4 activeColor, Vector4 inactiveColor)
    {
        var color = status ? activeColor : inactiveColor;
        
        // Draw colored circle indicator
        var drawList = ImGui.GetWindowDrawList();
        var pos = ImGui.GetCursorScreenPos();
        var radius = ImGui.GetFontSize() * 0.4f;
        
        drawList.AddCircleFilled(pos + new Vector2(radius, radius), radius, ImGui.ColorConvertFloat4ToU32(color));
        
        // Add text
        ImGui.Dummy(new Vector2(radius * 2.5f, 0));
        ImGui.SameLine();
        ImGui.Text(label);
    }
    
    public void DrawStatusPanel()
    {
        ImGui.Text("System Status:");
        ImGui.Separator();
        
        DrawStatusIndicator("Plugin Active", IsPluginActive(), Green, Red);
        DrawStatusIndicator("Game Connected", IsGameConnected(), Green, Red);
        DrawStatusIndicator("Data Sync", IsDataSyncing(), Yellow, Gray);
    }
}
```

### Progress Bars and Meters

```csharp
public class ProgressDisplay
{
    public void DrawProgressBar(string label, float value, float max, Vector4 color)
    {
        var percentage = value / max;
        
        ImGui.Text($"{label}: {value:F0}/{max:F0}");
        
        // Custom progress bar with color
        var drawList = ImGui.GetWindowDrawList();
        var pos = ImGui.GetCursorScreenPos();
        var size = new Vector2(ImGui.GetContentRegionAvail().X, ImGui.GetFontSize() * 1.5f);
        var filledWidth = size.X * percentage;
        
        // Background
        drawList.AddRectFilled(pos, pos + size, ImGui.ColorConvertFloat4ToU32(Gray));
        
        // Filled portion
        drawList.AddRectFilled(pos, pos + new Vector2(filledWidth, size.Y), ImGui.ColorConvertFloat4ToU32(color));
        
        // Border
        drawList.AddRect(pos, pos + size, ImGui.ColorConvertFloat4ToU32(White));
        
        ImGui.Dummy(size);
    }
    
    public void DrawPlayerHealth()
    {
        var maxHP = GetMaxHP();
        var currentHP = GetCurrentHP();
        
        DrawProgressBar("Health", currentHP, maxHP, Red);
        DrawProgressBar("Mana", GetCurrentMP(), GetMaxMP(), Blue);
        DrawProgressBar("TP", GetCurrentTP(), GetMaxTP(), Yellow);
    }
}
```

## Non-blocking UI

### Async Operations

```csharp
public class AsyncOperations
{
    private readonly ConcurrentQueue<Action> _uiActions = new();
    private CancellationTokenSource _cts = new();
    
    public void StartAsyncOperation()
    {
        Task.Run(async () =>
        {
            try
            {
                // Perform expensive operation
                var result = await ExpensiveCalculationAsync(_cts.Token);
                
                // Queue UI update on main thread
                _uiActions.Enqueue(() => UpdateUIWithResult(result));
            }
            catch (OperationCanceledException)
            {
                _uiActions.Enqueue(() => ShowError("Operation cancelled"));
            }
        });
    }
    
    public void Draw()
    {
        // Process queued UI actions
        while (_uiActions.TryDequeue(out var action))
        {
            action();
        }
        
        // UI rendering here
        ImGui.Text("Async Operations Demo");
        
        if (ImGui.Button("Start Operation"))
        {
            StartAsyncOperation();
        }
    }
    
    private async Task<string> ExpensiveCalculationAsync(CancellationToken token)
    {
        await Task.Delay(2000, token); // Simulate work
        return "Operation complete!";
    }
}
```

### Background Updates

```csharp
public class BackgroundUpdates
{
    private readonly Timer _updateTimer;
    private volatile string _lastData = "";
    private volatile bool _hasNewData = false;
    
    public BackgroundUpdates()
    {
        _updateTimer = new Timer(UpdateData, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }
    
    private void UpdateData(object state)
    {
        // Update data in background
        var newData = FetchDataFromGame();
        _lastData = newData;
        _hasNewData = true;
    }
    
    public void Draw()
    {
        ImGui.Text("Background Update Example");
        
        if (_hasNewData)
        {
            ImGui.Text($"Last Update: {_lastData}");
            _hasNewData = false;
        }
        else
        {
            ImGui.Text("No new data");
        }
    }
}
```

## Context-aware Interfaces

### Game State Awareness

```csharp
public class ContextAwareUI
{
    private readonly IClientState _clientState;
    private readonly ICondition _condition;
    
    public ContextAwareUI(IClientState clientState, ICondition condition)
    {
        _clientState = clientState;
        _condition = condition;
    }
    
    public void Draw()
    {
        var inCombat = _condition[ConditionFlag.InCombat];
        var boundByDuty = _condition[ConditionFlag.BoundByDuty];
        var betweenAreas = _condition[ConditionFlag.BetweenAreas];
        
        if (inCombat)
        {
            DrawCombatInterface();
        }
        else if (boundByDuty)
        {
            DrawDutyInterface();
        }
        else if (betweenAreas)
        {
            DrawLoadingInterface();
        }
        else
        {
            DrawNormalInterface();
        }
    }
    
    private void DrawCombatInterface()
    {
        ImGui.Text("Combat Mode");
        // Show combat-relevant information
        DrawCombatStats();
        DrawQuickActions();
    }
    
    private void DrawNormalInterface()
    {
        ImGui.Text("Normal Mode");
        // Show general-purpose tools
        DrawGeneralTools();
        DrawConfiguration();
    }
}
```

### Job-specific Interfaces

```csharp
public class JobAwareUI
{
    private readonly IClientState _clientState;
    
    public void DrawJobSpecificUI()
    {
        if (!_clientState.IsLoggedIn || _clientState.LocalPlayer == null)
            return;
        
        var job = _clientState.LocalPlayer.ClassJob.Value;
        
        switch (job.Abbreviation)
        {
            case "PLD":
                DrawPaladinUI();
                break;
            case "WAR":
                DrawWarriorUI();
                break;
            case "WHM":
                DrawWhiteMageUI();
                break;
            case "BLM":
                DrawBlackMageUI();
                break;
            default:
                DrawGenericUI();
                break;
        }
    }
    
    private void DrawPaladinUI()
    {
        ImGui.Text("Paladin Tools");
        // Paladin-specific features
        DrawCooldownTracker(new[] { "Shield Lob", "Holy Spirit", "Requiescat" });
        DrawOathGauge();
    }
    
    private void DrawWarriorUI()
    {
        ImGui.Text("Warrior Tools");
        // Warrior-specific features
        DrawCooldownTracker(new[] { "Fell Cleave", "Inner Release", "Upheaval" });
        DrawBeastGauge();
    }
}
```

## Input Management

### Input Capture and Blocking

```csharp
public class InputManagement
{
    private bool _captureInput = false;
    
    public void Draw()
    {
        ImGui.Text("Input Management");
        
        if (ImGui.Checkbox("Capture Input", ref _captureInput))
        {
            if (_captureInput)
            {
                // Start capturing input
                Service.PluginInterface.UiBuilder.DisableUserUiHide = true;
            }
            else
            {
                // Stop capturing input
                Service.PluginInterface.UiBuilder.DisableUserUiHide = false;
            }
        }
        
        if (_captureInput)
        {
            DrawInputCapture();
        }
    }
    
    private void DrawInputCapture()
    {
        ImGui.Text("Input is being captured");
        
        // Handle specific key inputs
        var io = ImGui.GetIO();
        
        if (ImGui.IsKeyPressed(ImGuiKey.Escape))
        {
            _captureInput = false;
            Service.PluginInterface.UiBuilder.DisableUserUiHide = false;
        }
        
        ImGui.Text($"Last key pressed: {(io.KeysDown.Count > 0 ? "Yes" : "No")}");
    }
}
```

### Hotkey Integration

```csharp
public class HotkeyIntegration
{
    private readonly Dictionary<string, KeyBind> _hotkeys = new();
    
    public void RegisterHotkeys()
    {
        _hotkeys["ToggleUI"] = new KeyBind(VirtualKey.F1);
        _hotkeys["QuickAction"] = new KeyBind(VirtualKey.F2, modifiers: KeyBindModifier.Control);
        _hotkeys["Emergency"] = new KeyBind(VirtualKey.F12);
    }
    
    public void ProcessInput()
    {
        foreach (var (name, hotkey) in _hotkeys)
        {
            if (hotkey.IsPressed())
            {
                HandleHotkey(name);
            }
        }
    }
    
    private void HandleHotkey(string hotkeyName)
    {
        switch (hotkeyName)
        {
            case "ToggleUI":
                IsVisible = !IsVisible;
                break;
            case "QuickAction":
                ExecuteQuickAction();
                break;
            case "Emergency":
                TriggerEmergencyAction();
                break;
        }
    }
    
    public void DrawHotkeyList()
    {
        ImGui.Text("Registered Hotkeys:");
        ImGui.Separator();
        
        foreach (var (name, hotkey) in _hotkeys)
        {
            ImGui.Text($"{name}: {hotkey}");
        }
    }
}
```

## State Synchronization

### Game Data Synchronization

```csharp
public class GameStateSync
{
    private readonly ConcurrentDictionary<uint, ActorData> _actorCache = new();
    private volatile DateTime _lastUpdate = DateTime.MinValue;
    
    public struct ActorData
    {
        public string Name;
        public uint ObjectId;
        public Vector3 Position;
        public byte Level;
        public bool IsValid;
    }
    
    public void UpdateGameState()
    {
        var now = DateTime.Now;
        if ((now - _lastUpdate).TotalMilliseconds < 50) // 20 FPS sync
            return;
        
        var actors = Service.ObjectTable.Where(obj => obj != null);
        
        foreach (var actor in actors)
        {
            var data = new ActorData
            {
                Name = actor.Name.ToString(),
                ObjectId = actor.ObjectId,
                Position = actor.Position,
                Level = actor.Level,
                IsValid = true
            };
            
            _actorCache[actor.ObjectId] = data;
        }
        
        // Clean up old actors
        var currentIds = actors.Select(a => a.ObjectId).ToHashSet();
        var keysToRemove = _actorCache.Keys.Where(id => !currentIds.Contains(id)).ToList();
        
        foreach (var key in keysToRemove)
        {
            _actorCache.TryRemove(key, out _);
        }
        
        _lastUpdate = now;
    }
    
    public void DrawActorList()
    {
        ImGui.Text("Actor List:");
        ImGui.Separator();
        
        foreach (var actor in _actorCache.Values)
        {
            if (!actor.IsValid) continue;
            
            ImGui.Text($"{actor.Name} (Lvl {actor.Level})");
            ImGui.SameLine();
            ImGui.TextColored(Gray, $"[{actor.ObjectId:X8}]");
        }
    }
}
```

### Multi-thread Safe Updates

```csharp
public class ThreadSafeUI
{
    private readonly object _lock = new();
    private volatile List<string> _messages = new();
    private volatile List<string> _renderBuffer = new();
    
    public void AddMessage(string message)
    {
        lock (_lock)
        {
            _messages.Add(message);
            
            // Keep only last 100 messages
            if (_messages.Count > 100)
            {
                _messages.RemoveAt(0);
            }
        }
    }
    
    public void Draw()
    {
        // Swap buffers for thread-safe rendering
        lock (_lock)
        {
            (_renderBuffer, _messages) = (_messages, _renderBuffer);
            _messages.Clear();
        }
        
        ImGui.Text("Message Log:");
        ImGui.Separator();
        
        var childHeight = ImGui.GetContentRegionAvail().Y;
        if (ImGui.BeginChild("Messages", new Vector2(0, childHeight)))
        {
            foreach (var message in _renderBuffer)
            {
                ImGui.Text(message);
            }
            
            // Auto-scroll to bottom
            ImGui.SetScrollHereY(1.0f);
            ImGui.EndChild();
        }
    }
}
```

## Memory Management

### Resource Disposal

```csharp
public class ResourceManagement : IDisposable
{
    private bool _disposed = false;
    private readonly List<IDisposable> _disposables = new();
    private TextureWrap? _customTexture;
    
    public ResourceManagement()
    {
        // Load resources
        LoadResources();
    }
    
    private void LoadResources()
    {
        try
        {
            // Load custom textures
            _customTexture = Service.PluginInterface.UiBuilder.LoadImage("custom_icon.png");
            _disposables.Add(_customTexture);
            
            // Register disposables
            Service.PluginInterface.UiBuilder.Draw += OnDraw;
            Service.PluginInterface.UiBuilder.OpenConfigUi += ToggleConfig;
        }
        catch (Exception ex)
        {
            Service.Logger.Error(ex, "Failed to load resources");
        }
    }
    
    private void OnDraw()
    {
        if (_disposed) return;
        
        // Draw UI with resources
        DrawUI();
    }
    
    public void Dispose()
    {
        if (_disposed) return;
        
        // Unregister events
        Service.PluginInterface.UiBuilder.Draw -= OnDraw;
        Service.PluginInterface.UiBuilder.OpenConfigUi -= ToggleConfig;
        
        // Dispose resources
        foreach (var disposable in _disposables)
        {
            try
            {
                disposable.Dispose();
            }
            catch (Exception ex)
            {
                Service.Logger.Error(ex, "Failed to dispose resource");
            }
        }
        
        _disposables.Clear();
        _disposed = true;
    }
}
```

### Memory Leak Prevention

```csharp
public class MemoryEfficientUI
{
    private readonly LRUCache<string, TextureWrap> _textureCache;
    private readonly ConcurrentDictionary<string, WeakReference> _weakRefs = new();
    
    public MemoryEfficientUI()
    {
        _textureCache = new LRUCache<string, TextureWrap>(50); // Cache 50 textures
    }
    
    public TextureWrap GetCachedTexture(string path)
    {
        if (_textureCache.TryGet(path, out var cached))
            return cached;
        
        try
        {
            var texture = Service.PluginInterface.UiBuilder.LoadImage(path);
            _textureCache.Add(path, texture);
            return texture;
        }
        catch (Exception ex)
        {
            Service.Logger.Error(ex, $"Failed to load texture: {path}");
            return null;
        }
    }
    
    public void DrawWithTexture(string texturePath, string label)
    {
        var texture = GetCachedTexture(texturePath);
        if (texture?.ImGuiHandle != null)
        {
            ImGui.Image(texture.ImGuiHandle, new Vector2(32, 32));
            ImGui.SameLine();
        }
        
        ImGui.Text(label);
    }
    
    public void Cleanup()
    {
        // Clear cache on plugin unload
        _textureCache.Clear();
        
        // Clear weak references
        var keysToRemove = new List<string>();
        foreach (var (key, weakRef) in _weakRefs)
        {
            if (!weakRef.IsAlive)
            {
                keysToRemove.Add(key);
            }
        }
        
        foreach (var key in keysToRemove)
        {
            _weakRefs.TryRemove(key, out _);
        }
    }
}
```

These patterns provide a foundation for creating efficient, responsive ImGui interfaces that work well within the FFXIV game environment. Always prioritize performance and thread safety when implementing game UI.