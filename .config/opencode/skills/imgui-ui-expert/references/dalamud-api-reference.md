# Dalamud API Reference

This document provides comprehensive reference information for Dalamud-specific ImGui utilities and APIs that enhance the standard ImGui.NET experience for FFXIV plugin development.

## Table of Contents

- [ImGuiHelpers](#imguihelpers)
- [ImGuiExtensions](#imguiextensions)
- [ImGuiTable](#imguitable)
- [Common Patterns](#common-patterns)
- [Integration Examples](#integration-examples)

## ImGuiHelpers

ImGuiHelpers provides utility methods specifically designed for Dalamud plugin development, offering shortcuts and enhanced functionality for common UI tasks.

### Key Classes

#### ImGuiHelpers.HorizontalButtonGroup

Helper for creating organized button groups with consistent spacing and alignment.

```csharp
// Basic usage
var buttonGroup = new ImGuiHelpers.HorizontalButtonGroup
{
    IsCentered = true,
    Height = 32f,
    ExtraMargin = 2f
};

// Manual implementation equivalent
ImGuiHelpers.PushButtonWidth(100f);
if (ImGui.Button("Button 1")) { /* action */ }
ImGui.SameLine();
if (ImGui.Button("Button 2")) { /* action */ }
ImGui.SameLine();
if (ImGui.Button("Button 3")) { /* action */ }
ImGui.PopButtonWidth();
```

**Properties:**
- `IsCentered` (bool): Center buttons horizontally
- `Height` (float?): Custom button height
- `ExtraMargin` (float?): Additional margin for each button

### Utility Methods

#### PushButtonWidth(float width) / PopButtonWidth()

Temporarily sets button width for consistent button sizing.

```csharp
ImGuiHelpers.PushButtonWidth(120f);
if (ImGui.Button("Save")) { /* save action */ }
ImGui.SameLine();
if (ImGui.Button("Cancel")) { /* cancel action */ }
ImGui.PopButtonWidth();
```

#### PushFramePadding(Vector2 padding) / PopFramePadding()

Temporarily modifies frame padding for widgets.

```csharp
ImGuiHelpers.PushFramePadding(new Vector2(8, 4));
ImGui.Checkbox("Spaced Checkbox", ref isSpaced);
ImGui.PopFramePadding();
```

#### RightAlign(string text)

Aligns text to the right side of the available space.

```csharp
ImGui.Text("Left aligned text");
ImGui.SameLine();
ImGuiHelpers.RightAlign("Right aligned text");
```

#### CenterText(string text)

Centers text within the available space.

```csharp
ImGuiHelpers.CenterText("This text is centered");
```

#### SeStringIcon(uint iconId)

Creates a SeString with an icon for use in ImGui widgets.

```csharp
var icon = ImGuiHelpers.SeStringIcon(0xE038); // Example icon
ImGui.TextUnformatted(icon.TextValue);
```

#### IconText(FontAwesomeIcon icon, string text)

Combines FontAwesome icons with text.

```csharp
ImGuiHelpers.IconText(FontAwesomeIcon.Save, "Save File");
// Equivalent to: ImGui.Text($"{(char)FontAwesomeIcon.Save} Save File");
```

## ImGuiExtensions

ImGuiExtensions provides extended functionality for custom widget creation and advanced drawing operations.

### Drawing Extensions

#### AddTextClippedEx(ImDrawListPtr, Vector2, Vector2, string, Vector2?, Vector2, Vector4?)

Advanced text drawing with clipping and alignment.

```csharp
var drawList = ImGui.GetWindowDrawList();
var posMin = ImGui.GetCursorScreenPos();
var posMax = posMin + new Vector2(200, 50);
var textSize = ImGui.CalcTextSize("Clipped text", false, 200f);

drawList.AddTextClippedEx(
    drawList,
    posMin,
    posMax,
    "This text will be clipped if too long",
    textSize,
    new Vector2(0.5f, 0.5f), // Center alignment
    new Vector4(1, 1, 1, 1)  // White color
);
```

#### ButtonEx(string label, Vector2 size, ImGuiButtonFlags flags)

Enhanced button creation with additional flags.

```csharp
var clicked = ImGuiExtensions.ButtonEx(
    "Advanced Button",
    new Vector2(150, 40),
    ImGuiButtonFlags.MouseButtonLeft | ImGuiButtonFlags.MouseButtonRight
);

if (clicked)
{
    // Handle button click
}
```

#### ToggleButton(string label, ref bool active, Vector2 size)

Creates a toggle button that changes appearance based on state.

```csharp
bool isActive = false;
if (ImGuiExtensions.ToggleButton("Toggle Me", ref isActive, new Vector2(100, 30)))
{
    // Toggle state changed
    // isActive will be automatically updated
}
```

### Utility Extensions

#### Vector2 ToVector2(this Vector3 v3)

Converts Vector3 to Vector2 (drops Z component).

```csharp
Vector3 position3D = new Vector3(10, 20, 30);
Vector2 position2D = position3D.ToVector2(); // (10, 20)
```

#### Vector3 ToVector3(this Vector2 v2, float z = 0)

Converts Vector2 to Vector3 (adds Z component).

```csharp
Vector2 position2D = new Vector2(10, 20);
Vector3 position3D = position2D.ToVector3(5); // (10, 20, 5)
```

#### Color helpers

```csharp
// Convert ImGui color to Vector4
Vector4 color = ImGui.ColorConvertU32ToFloat4(ImGui.GetColorU32(ImGuiCol.Button));

// Convert Vector4 to ImGui color
uint imguiColor = ImGui.ColorConvertFloat4ToU32(new Vector4(1, 0, 0, 1)); // Red
```

## ImGuiTable

ImGuiTable provides enhanced table functionality specifically designed for Dalamud plugins.

### Basic Table Usage

```csharp
// Basic table with headers
if (ImGui.BeginTable("ExampleTable", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
{
    // Headers
    ImGui.TableSetupColumn("ID", ImGuiTableColumnFlags.WidthFixed, 50f);
    ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthStretch);
    ImGui.TableSetupColumn("Value", ImGuiTableColumnFlags.WidthFixed, 100f);
    ImGui.TableSetupColumn("Actions", ImGuiTableColumnFlags.WidthFixed, 80f);
    ImGui.TableHeadersRow();

    // Rows
    for (int i = 0; i < items.Count; i++)
    {
        ImGui.TableNextRow();
        ImGui.TableSetColumnIndex(0);
        ImGui.Text($"{i}");
        
        ImGui.TableSetColumnIndex(1);
        ImGui.Text(items[i].Name);
        
        ImGui.TableSetColumnIndex(2);
        ImGui.Text($"{items[i].Value:F2}");
        
        ImGui.TableSetColumnIndex(3);
        if (ImGui.Button($"Edit##{i}"))
        {
            // Edit action
        }
    }
    
    ImGui.EndTable();
}
```

### Advanced Table Features

```csharp
// Table with sorting and filtering
var flags = ImGuiTableFlags.Resizable | ImGuiTableFlags.Sortable | 
           ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.BordersOuter;

if (ImGui.BeginTable("SortableTable", 3, flags))
{
    ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.DefaultSort);
    ImGui.TableSetupColumn("Value", ImGuiTableColumnFlags.DefaultSort);
    ImGui.TableSetupColumn("Status", ImGuiTableColumnFlags.NoSort);
    ImGui.TableHeadersRow();

    // Sort indicators
    var sortSpecs = ImGui.TableGetSortSpecs();
    if (sortSpecs.SpecsCount > 0)
    {
        // Apply sorting logic
        SortItems(sortSpecs);
    }

    foreach (var item in sortedItems)
    {
        ImGui.TableNextRow();
        ImGui.TableSetColumnIndex(0);
        ImGui.Text(item.Name);
        
        ImGui.TableSetColumnIndex(1);
        ImGui.Text($"{item.Value}");
        
        ImGui.TableSetColumnIndex(2);
        ImGui.TextColored(item.Active ? Green : Red, item.Status);
    }

    ImGui.EndTable();
}
```

## Common Patterns

### 1. Plugin Integration Pattern

```csharp
public class MyPlugin : IDalamudPlugin
{
    private readonly UIManager _uiManager;
    
    public void Initialize(IDalamudPluginInterface pluginInterface)
    {
        _uiManager = new UIManager(pluginInterface);
        
        // Register UI with Dalamud's UI system
        pluginInterface.UiBuilder.Draw += _uiManager.Draw;
        pluginInterface.UiBuilder.OpenConfigUi += _uiManager.ToggleConfig;
    }
    
    public void Dispose()
    {
        _uiManager.Dispose();
    }
}
```

### 2. Window Management Pattern

```csharp
public class PluginWindow : IDisposable
{
    private bool _isVisible = false;
    private readonly string _windowTitle = "My Plugin Window";
    
    public bool IsVisible
    {
        get => _isVisible;
        set => _isVisible = value;
    }
    
    public void Draw()
    {
        if (!_isVisible) return;
        
        var windowFlags = ImGuiWindowFlags.None;
        
        ImGui.SetNextWindowSize(new Vector2(400, 300), ImGuiCond.FirstUseEver);
        
        if (ImGui.Begin(_windowTitle, ref _isVisible, windowFlags))
        {
            DrawContent();
        }
        
        ImGui.End();
    }
    
    private void DrawContent()
    {
        // Window content here
        ImGui.Text("Hello from my plugin!");
    }
    
    public void Toggle()
    {
        _isVisible = !_isVisible;
    }
    
    public void Dispose()
    {
        // Cleanup
    }
}
```

### 3. Settings Management Pattern

```csharp
public class PluginConfig
{
    public bool EnableFeature { get; set; } = true;
    public float SliderValue { get; set; } = 50f;
    public string CustomText { get; set; } = "";
    public Vector4 AccentColor { get; set; } = new Vector4(0, 1, 0, 1);
}

public class ConfigWindow : IDisposable
{
    private PluginConfig _config;
    private bool _isVisible = false;
    private bool _hasChanges = false;
    
    public void Draw()
    {
        if (!_isVisible) return;
        
        if (ImGui.Begin("Configuration", ref _isVisible))
        {
            DrawConfigOptions();
            
            ImGui.Separator();
            
            if (_hasChanges)
            {
                if (ImGui.Button("Save"))
                {
                    SaveConfig();
                    _hasChanges = false;
                }
                
                ImGui.SameLine();
            }
            
            if (ImGui.Button("Reset"))
            {
                _config = new PluginConfig();
                _hasChanges = true;
            }
        }
        
        ImGui.End();
    }
    
    private void DrawConfigOptions()
    {
        if (ImGui.Checkbox("Enable Feature", ref _config.EnableFeature))
        {
            _hasChanges = true;
        }
        
        if (ImGui.SliderFloat("Value", ref _config.SliderValue, 0f, 100f))
        {
            _hasChanges = true;
        }
        
        // ... other config options
    }
}
```

## Integration Examples

### Example 1: Simple Status Window

```csharp
public class StatusWindow : IDisposable
{
    private readonly IClientState _clientState;
    private bool _isVisible = false;
    
    public StatusWindow(IClientState clientState)
    {
        _clientState = clientState;
    }
    
    public void Draw()
    {
        if (!_isVisible) return;
        
        if (ImGui.Begin("Status", ref _isVisible))
        {
            // Player information
            if (_clientState.IsLoggedIn)
            {
                ImGui.Text($"Player: {_clientState.LocalPlayer?.Name ?? "Unknown"}");
                ImGui.Text($"Level: {_clientState.LocalPlayer?.Level ?? 0}");
                ImGui.Text($"Job: {_clientState.LocalPlayer?.ClassJob.Value?.Name ?? "Unknown"}");
            }
            else
            {
                ImGui.Text("Not logged in");
            }
            
            ImGui.Separator();
            
            // System information
            ImGui.Text($"FPS: {ImGui.GetIO().Framerate:F1}");
            ImGui.Text($"Frame Time: {1000.0f / ImGui.GetIO().Framerate:F2}ms");
        }
        
        ImGui.End();
    }
}
```

### Example 2: Interactive Tool Panel

```csharp
public class ToolPanel : IDisposable
{
    private bool _isVisible = false;
    private List<string> _actions = new();
    private string _newAction = "";
    
    public void Draw()
    {
        if (!_isVisible) return;
        
        if (ImGui.Begin("Tool Panel", ref _isVisible))
        {
            // Action list
            ImGui.Text("Quick Actions:");
            ImGui.SameLine();
            ImGuiHelpers.IconText(FontAwesomeIcon.Plus, "Add");
            
            ImGui.Separator();
            
            // Input for new action
            ImGui.SetNextItemWidth(-1);
            if (ImGui.InputTextWithHint("##newaction", "Enter new action...", ref _newAction, 256, ImGuiInputTextFlags.EnterReturnsTrue))
            {
                if (!string.IsNullOrWhiteSpace(_newAction))
                {
                    _actions.Add(_newAction);
                    _newAction = "";
                }
            }
            
            // Action buttons
            ImGui.PushButtonWidth(-1);
            for (int i = 0; i < _actions.Count; i++)
            {
                if (ImGui.Button($"{_actions[i]}##{i}"))
                {
                    // Execute action
                    ExecuteAction(_actions[i]);
                }
            }
            ImGui.PopButtonWidth();
        }
        
        ImGui.End();
    }
    
    private void ExecuteAction(string action)
    {
        // Implement action execution logic
        Service.Logger.Info($"Executing action: {action}");
    }
}
```

## Best Practices

1. **Always use Dalamud-specific helpers** when available for better integration
2. **Properly dispose** of UI resources to prevent memory leaks
3. **Use ImGui.Push/Pop pairs** for temporary style changes
4. **Implement proper ID handling** for duplicate widgets
5. **Follow Dalamud's UI thread requirements** for all UI operations
6. **Use appropriate window flags** for different UI behaviors
7. **Handle window sizing** gracefully for different screen resolutions
8. **Implement proper state management** for interactive components

This reference provides the foundation for creating professional-quality ImGui interfaces in Dalamud plugins. For more advanced patterns and examples, see the other reference documents in this skill.