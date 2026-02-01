# ImGui Best Practices for Dalamud Plugins

This document covers best practices, performance optimization, and common pitfalls when using ImGui for FFXIV Dalamud plugin development.

## Table of Contents

- [Performance Optimization](#performance-optimization)
- [Memory Management](#memory-management)
- [Thread Safety](#thread-safety)
- [UI Design Principles](#ui-design-principles)
- [Integration Guidelines](#integration-guidelines)
- [Common Pitfalls](#common-pitfalls)
- [Debugging and Troubleshooting](#debugging-and-troubleshooting)

## Performance Optimization

### Frame Rate Impact Minimization

```csharp
// Good: Throttled updates
public class OptimizedUI
{
    private DateTime _lastUpdate = DateTime.MinValue;
    private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(100); // 10 FPS
    
    public void Draw()
    {
        var now = DateTime.Now;
        if ((now - _lastUpdate) < _updateInterval)
        {
            // Only render cached data
            DrawCachedData();
            return;
        }
        
        UpdateExpensiveData();
        _lastUpdate = now;
        DrawData();
    }
}

// Bad: Updating every frame
public class UnoptimizedUI
{
    public void Draw()
    {
        // This runs every frame - expensive!
        var data = FetchExpensiveData(); // API call, file I/O, etc.
        ImGui.Text(data.ToString());
    }
}
```

### Efficient Rendering Patterns

```csharp
// Good: Use DrawLists for complex shapes
public class EfficientDrawing
{
    public void DrawComplexUI()
    {
        var drawList = ImGui.GetWindowDrawList();
        
        // Batch draw calls
        foreach (var shape in shapes)
        {
            drawList.AddRectFilled(shape.Pos, shape.Size, shape.Color);
        }
    }
}

// Bad: Multiple widgets for simple shapes
public class InefficientDrawing
{
    public void DrawComplexUI()
    {
        foreach (var shape in shapes)
        {
            // Each button is a separate draw call
            if (ImGui.Button($"{shape.Name}##{shape.Id}"))
            {
                // Handle click
            }
        }
    }
}
```

### Smart Widget Usage

```csharp
// Good: Reuse widgets with proper IDs
public class SmartWidgetUsage
{
    public void DrawItemList(List<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            ImGui.PushID(i); // Unique ID for each item
            
            if (ImGui.Button(items[i].Name))
            {
                HandleItemClick(items[i]);
            }
            
            ImGui.PopID();
        }
    }
}

// Bad: Conflicting IDs or expensive ID generation
public class BadWidgetUsage
{
    public void DrawItemList(List<Item> items)
    {
        foreach (var item in items)
        {
            // ID conflicts can occur with duplicate item names
            if (ImGui.Button(item.Name))
            {
                HandleItemClick(item);
            }
        }
    }
}
```

## Memory Management

### Resource Disposal

```csharp
// Good: Proper IDisposable implementation
public class ProperResourceManagement : IDisposable
{
    private bool _disposed = false;
    private readonly List<IDisposable> _resources = new();
    
    public ProperResourceManagement()
    {
        // Register for events
        Service.PluginInterface.UiBuilder.Draw += OnDraw;
        
        // Load resources
        LoadTextures();
    }
    
    public void Dispose()
    {
        if (_disposed) return;
        
        // Unregister events first
        Service.PluginInterface.UiBuilder.Draw -= OnDraw;
        
        // Dispose resources
        foreach (var resource in _resources)
        {
            resource.Dispose();
        }
        
        _resources.Clear();
        _disposed = true;
    }
}

// Bad: Memory leaks
public class LeakyResourceManagement
{
    public LeakyResourceManagement()
    {
        Service.PluginInterface.UiBuilder.Draw += OnDraw;
        // Never unregister - memory leak!
    }
    
    // No Dispose method
}
```

### Texture and Font Management

```csharp
// Good: Cache frequently used textures
public class TextureCache
{
    private static readonly Dictionary<string, TextureWrap> _cache = new();
    
    public static TextureWrap GetOrLoad(string path)
    {
        if (_cache.TryGetValue(path, out var cached))
            return cached;
        
        var texture = Service.PluginInterface.UiBuilder.LoadImage(path);
        _cache[path] = texture;
        return texture;
    }
    
    public static void Cleanup()
    {
        foreach (var texture in _cache.Values)
        {
            texture.Dispose();
        }
        _cache.Clear();
    }
}

// Bad: Loading textures repeatedly
public class TextureWaste
{
    public void DrawButton()
    {
        // Loads texture every frame - expensive!
        var texture = Service.PluginInterface.UiBuilder.LoadImage("icon.png");
        ImGui.Image(texture.ImGuiHandle, new Vector2(32, 32));
        // Texture never disposed - memory leak!
    }
}
```

## Thread Safety

### Safe UI Updates

```csharp
// Good: Thread-safe data sharing
public class ThreadSafeUI
{
    private readonly ConcurrentQueue<string> _messageQueue = new();
    private volatile string _currentMessage = "";
    
    public void AddMessage(string message)
    {
        _messageQueue.Enqueue(message);
    }
    
    public void Draw()
    {
        // Process queued messages on UI thread
        while (_messageQueue.TryDequeue(out var message))
        {
            _currentMessage = message;
        }
        
        ImGui.Text(_currentMessage);
    }
}

// Bad: Cross-thread UI operations
public class UnsafeUI
{
    public void UpdateFromBackground()
    {
        Task.Run(() =>
        {
            var data = FetchDataAsync();
            
            // DANGEROUS: UI operations on background thread!
            ImGui.Text(data); // This will crash!
        });
    }
}
```

### Async Operations

```csharp
// Good: Proper async handling
public class AsyncUIHandler
{
    private readonly ConcurrentQueue<Action> _uiActions = new();
    
    public async Task UpdateDataAsync()
    {
        try
        {
            var data = await FetchDataAsync();
            
            // Queue UI update on main thread
            _uiActions.Enqueue(() => UpdateUI(data));
        }
        catch (Exception ex)
        {
            _uiActions.Enqueue(() => ShowError(ex.Message));
        }
    }
    
    public void Draw()
    {
        // Process all pending UI actions
        while (_uiActions.TryDequeue(out var action))
        {
            action();
        }
    }
}
```

## UI Design Principles

### Consistent Styling

```csharp
// Good: Consistent styling system
public class UIStyle
{
    public static readonly Vector4 PrimaryColor = new(0.2f, 0.6f, 1.0f, 1.0f);
    public static readonly Vector4 SuccessColor = new(0.2f, 0.8f, 0.2f, 1.0f);
    public static readonly Vector4 WarningColor = new(1.0f, 0.8f, 0.2f, 1.0f);
    public static readonly Vector4 ErrorColor = new(1.0f, 0.2f, 0.2f, 1.0f);
    
    public static void PushPrimaryButton()
    {
        ImGui.PushStyleColor(ImGuiCol.Button, PrimaryColor);
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, PrimaryColor * 1.2f);
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, PrimaryColor * 0.8f);
    }
    
    public static void PopButton()
    {
        ImGui.PopStyleColor(3);
    }
}

// Usage
public class StyledUI
{
    public void DrawButtons()
    {
        UIStyle.PushPrimaryButton();
        if (ImGui.Button("Primary Action"))
        {
            // Handle primary action
        }
        UIStyle.PopButton();
    }
}
```

### Responsive Layout

```csharp
// Good: Responsive design
public class ResponsiveLayout
{
    public void DrawResponsive()
    {
        var availableWidth = ImGui.GetContentRegionAvail().X;
        
        if (availableWidth > 400)
        {
            // Wide layout - side by side
            ImGui.BeginGroup();
            DrawLeftPanel();
            ImGui.EndGroup();
            
            ImGui.SameLine();
            
            ImGui.BeginGroup();
            DrawRightPanel();
            ImGui.EndGroup();
        }
        else
        {
            // Narrow layout - stacked
            DrawLeftPanel();
            ImGui.Separator();
            DrawRightPanel();
        }
    }
}
```

### Accessible Design

```csharp
// Good: Accessible UI with clear feedback
public class AccessibleUI
{
    public void DrawWithFeedback()
    {
        // Clear labels
        ImGui.Text("Player Status:");
        ImGui.SameLine();
        
        var statusColor = IsPlayerHealthy() ? Green : Red;
        ImGui.TextColored(statusColor, GetPlayerStatus());
        
        // Keyboard navigation
        ImGui.PushStyleVar(ImGuiStyleVar.ButtonTextAlign, new Vector2(0.5f, 0.5f));
        
        if (ImGui.Button("Heal Player", new Vector2(-1, 0)))
        {
            HealPlayer();
            // Provide feedback
            ShowNotification("Player healed!");
        }
        
        ImGui.PopStyleVar();
        
        // Tooltips for additional information
        if (ImGui.IsItemHovered())
        {
            ImGui.SetTooltip("Restores player health to maximum");
        }
    }
}
```

## Integration Guidelines

### Proper Plugin Lifecycle

```csharp
// Good: Correct lifecycle management
public class ProperPlugin : IDalamudPlugin
{
    private readonly UIManager _uiManager;
    
    public void Initialize(IDalamudPluginInterface pluginInterface)
    {
        _uiManager = new UIManager(pluginInterface);
        
        // Register UI events
        pluginInterface.UiBuilder.Draw += _uiManager.Draw;
        pluginInterface.UiBuilder.OpenConfigUi += _uiManager.ToggleConfig;
    }
    
    public void Dispose()
    {
        // Unregister events first
        Service.PluginInterface.UiBuilder.Draw -= _uiManager.Draw;
        Service.PluginInterface.UiBuilder.OpenConfigUi -= _uiManager.ToggleConfig;
        
        // Then dispose UI manager
        _uiManager.Dispose();
    }
}
```

### Game State Awareness

```csharp
// Good: Respect game state
public class GameStateAwareUI
{
    private readonly ICondition _condition;
    
    public void Draw()
    {
        var inCombat = _condition[ConditionFlag.InCombat];
        var inCutscene = _condition[ConditionFlag.WatchingCutscene];
        var betweenAreas = _condition[ConditionFlag.BetweenAreas];
        
        // Don't show intrusive UI in certain states
        if (inCutscene || betweenAreas)
            return;
        
        // Simplified UI during combat
        if (inCombat)
        {
            DrawCombatUI();
        }
        else
        {
            DrawFullUI();
        }
    }
}
```

### Input Handling

```csharp
// Good: Respect game input
public class InputRespectingUI
{
    public bool ShouldCaptureInput()
    {
        // Don't capture input during certain game states
        var inCutscene = Service.Condition[ConditionFlag.WatchingCutscene];
        var inGpose = Service.Condition[ConditionFlag.UsingGPose];
        
        return !inCutscene && !inGpose;
    }
    
    public void Draw()
    {
        if (!ShouldCaptureInput())
            return;
            
        // Safe to handle input here
        DrawInteractiveElements();
    }
}
```

## Common Pitfalls

### ID Conflicts

```csharp
// Bad: ID conflicts
public class IDConflictExample
{
    public void DrawPlayerList(List<Player> players)
    {
        foreach (var player in players)
        {
            // All buttons have same ID if names duplicate!
            if (ImGui.Button(player.Name))
            {
                HandlePlayerClick(player);
            }
        }
    }
}

// Good: Proper ID management
public class ProperIDManagement
{
    public void DrawPlayerList(List<Player> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            ImGui.PushID(i); // Unique ID
            ImGui.PushID(players[i].ObjectId); // Also unique per player
            
            if (ImGui.Button(players[i].Name))
            {
                HandlePlayerClick(players[i]);
            }
            
            ImGui.PopID();
            ImGui.PopID();
        }
    }
}
```

### Memory Leaks

```csharp
// Bad: Event subscription without unsubscription
public class EventLeakExample
{
    public void Initialize()
    {
        Service.Framework.Update += OnFrameworkUpdate;
        // Never unsubscribed - memory leak!
    }
}

// Good: Proper event management
public class ProperEventManagement : IDisposable
{
    private bool _disposed = false;
    
    public void Initialize()
    {
        Service.Framework.Update += OnFrameworkUpdate;
    }
    
    public void Dispose()
    {
        if (_disposed) return;
        
        Service.Framework.Update -= OnFrameworkUpdate;
        _disposed = true;
    }
}
```

### Excessive Updates

```csharp
// Bad: Expensive operations every frame
public class ExcessiveUpdates
{
    public void Draw()
    {
        // File I/O every frame - very slow!
        var lines = File.ReadAllLines("data.txt");
        foreach (var line in lines)
        {
            ImGui.Text(line);
        }
    }
}

// Good: Cached updates
public class CachedUpdates
{
    private string[] _cachedLines;
    private DateTime _lastRead = DateTime.MinValue;
    
    public void Draw()
    {
        var now = DateTime.Now;
        if ((now - _lastRead).TotalSeconds > 5) // Update every 5 seconds
        {
            _cachedLines = File.ReadAllLines("data.txt");
            _lastRead = now;
        }
        
        foreach (var line in _cachedLines ?? Array.Empty<string>())
        {
            ImGui.Text(line);
        }
    }
}
```

## Debugging and Troubleshooting

### Debug Rendering

```csharp
public class DebugUI
{
    private bool _showDebugInfo = false;
    
    public void Draw()
    {
        if (ImGui.Begin("Debug Window"))
        {
            ImGui.Checkbox("Show Debug Info", ref _showDebugInfo);
            
            if (_showDebugInfo)
            {
                DrawDebugInfo();
            }
        }
        
        ImGui.End();
    }
    
    private void DrawDebugInfo()
    {
        ImGui.Text($"FPS: {ImGui.GetIO().Framerate:F1}");
        ImGui.Text($"Frame Time: {1000f / ImGui.GetIO().Framerate:F2}ms");
        ImGui.Text($"Memory: {GC.GetTotalMemory(false) / (1024f * 1024f):F2}MB");
        ImGui.Text($"Mouse Pos: {ImGui.GetMousePos()}");
        ImGui.Text($"Window Pos: {ImGui.GetWindowPos()}");
        ImGui.Text($"Window Size: {ImGui.GetWindowSize()}");
    }
}
```

### Error Handling

```csharp
public class RobustUI
{
    public void Draw()
    {
        try
        {
            DrawUI();
        }
        catch (Exception ex)
        {
            Service.Logger.Error(ex, "UI rendering error");
            DrawErrorState(ex);
        }
    }
    
    private void DrawErrorState(Exception ex)
    {
        ImGui.TextColored(Red, "UI Error:");
        ImGui.Text(ex.Message);
        
        if (ImGui.Button("Reset UI"))
        {
            ResetUI();
        }
    }
}
```

### Performance Monitoring

```csharp
public class PerformanceMonitor
{
    private readonly Queue<float> _frameTimes = new();
    private DateTime _lastFrameTime = DateTime.MinValue;
    
    public void Draw()
    {
        var now = DateTime.Now;
        if (_lastFrameTime != DateTime.MinValue)
        {
            var frameTime = (now - _lastFrameTime).TotalMilliseconds;
            _frameTimes.Enqueue((float)frameTime);
            
            if (_frameTimes.Count > 60)
                _frameTimes.Dequeue();
        }
        _lastFrameTime = now;
        
        if (_frameTimes.Count > 0)
        {
            var avgFrameTime = _frameTimes.Average();
            var fps = 1000f / avgFrameTime;
            
            ImGui.Text($"Frame Time: {avgFrameTime:F2}ms");
            ImGui.Text($"FPS: {fps:F1}");
            
            // Color code performance
            var color = fps < 30 ? Red : fps < 50 ? Yellow : Green;
            ImGui.TextColored(color, GetPerformanceGrade(fps));
        }
    }
    
    private string GetPerformanceGrade(float fps)
    {
        return fps switch
        {
            >= 60 => "Excellent",
            >= 50 => "Good",
            >= 30 => "Fair",
            _ => "Poor"
        };
    }
}
```

Following these best practices will help you create efficient, maintainable, and user-friendly ImGui interfaces for your Dalamud plugins. Remember to always prioritize performance in the game environment and test thoroughly under different conditions.