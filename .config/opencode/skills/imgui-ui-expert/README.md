# ImGui UI Expert Skill for Dalamud

A comprehensive OpenCode skill for creating rich, customizable ImGui interfaces specifically designed for FFXIV Dalamud plugins. This skill provides progressive learning from beginner to expert levels with a focus on interactive utilities and real-time game integration.

## Features

### 🎨 Progressive Learning System
- **Level 1**: Basic windows, simple widgets, fundamental layouts
- **Level 2**: Advanced widgets, event handling, responsive layouts  
- **Level 3**: Custom drawing, animations, performance optimization
- **Level 4**: Dalamud-specific optimizations, real-time visualization, theming

### 🛠️ Interactive Utility Focus
- Quick action panels and toolbars
- Status monitoring and data displays
- Configuration and settings interfaces
- Workflow automation tools
- Real-time data visualization

### 🔧 Dalamud Integration
- Dalamud-specific ImGui helpers and extensions
- Game-aware UI behaviors and state management
- Performance optimized for game environments
- Thread-safe UI updates and background processing

## Usage

### Basic Usage

```bash
# Use the skill to generate ImGui components
skill({name: "imgui-ui-expert"})
```

### Template Generation

```bash
# Generate a basic window template
./scripts/generate-ui-template.sh basic-window --level 1

# Generate an interactive utility
./scripts/generate-ui-template.sh interactive-tool --level 3 --namespace MyPlugin.UI

# Generate a data visualization dashboard
./scripts/generate-ui-template.sh data-viz --level 4
```

### Validation and Testing

```bash
# Validate Dalamud project structure
./scripts/validate-project.sh

# Test ImGui components
./scripts/test-imgui-components.sh
```

## Skill Structure

```
imgui-ui-expert/
├── SKILL.md                      # Main skill file
├── scripts/
│   ├── validate-project.sh         # Dalamud project validation
│   ├── generate-ui-template.sh     # Template generation
│   └── test-imgui-components.sh   # Component testing
├── references/
│   ├── dalamud-api-reference.md   # Dalamud API documentation
│   ├── imgui-patterns.md         # Game development patterns
│   ├── interactive-components.md   # Interactive component guide
│   └── best-practices.md        # Development best practices
└── assets/
    ├── templates/                 # Code templates
    │   ├── basic-window.cs       # Basic window template
    │   ├── config-panel.cs       # Configuration panel
    │   ├── interactive-tool.cs   # Interactive utility
    │   └── data-viz.cs          # Data visualization
    └── examples/
        ├── quick-action-panel.cs  # Quick action panel example
        ├── status-monitor.cs      # Real-time status monitor
        └── workflow-utility.cs   # Workflow automation
```

## Template Examples

### Basic Window Template

```csharp
// Simple ImGui window with Dalamud integration
var window = new BasicWindow(dataManager);
window.Visible = true;
```

### Interactive Tool Template

```csharp
// Interactive utility with buttons and actions
var tool = new InteractiveTool();
tool.Visible = true;
```

### Data Visualization Template

```csharp
// Real-time data visualization dashboard
var dashboard = new DataVisualization();
dashboard.Visible = true;
```

## Key Patterns

### Performance Optimization

```csharp
// Throttled updates for game performance
private DateTime _lastUpdate = DateTime.MinValue;
private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(100);

public void Draw()
{
    var now = DateTime.Now;
    if ((now - _lastUpdate) < _updateInterval)
        return;
    
    UpdateExpensiveData();
    _lastUpdate = now;
}
```

### Thread-Safe Updates

```csharp
// Queue UI updates from background threads
private readonly ConcurrentQueue<Action> _uiActions = new();

public void AddMessage(string message)
{
    _uiActions.Enqueue(() => ShowMessage(message));
}

public void Draw()
{
    while (_uiActions.TryDequeue(out var action))
        action();
}
```

### Dalamud Integration

```csharp
// Game state-aware UI
private readonly ICondition _condition;

public void Draw()
{
    var inCombat = _condition[ConditionFlag.InCombat];
    
    if (inCombat)
        DrawCombatUI();
    else
        DrawNormalUI();
}
```

## Advanced Features

### Real-time Monitoring

```csharp
// Performance and status monitoring
public class StatusMonitor : IDisposable
{
    public void Draw()
    {
        DrawPlayerStats();
        DrawPerformanceGraph();
        DrawSystemInfo();
    }
}
```

### Workflow Automation

```csharp
// Complex workflow management
public class WorkflowUtility : IDisposable
{
    public void ExecuteWorkflow(Workflow workflow)
    {
        // Execute steps with error handling
        // Support pause/resume functionality
        // Provide detailed logging
    }
}
```

### Custom Drawing

```csharp
// Advanced ImGui drawing
private void DrawCustomUI()
{
    var drawList = ImGui.GetWindowDrawList();
    var pos = ImGui.GetCursorScreenPos();
    
    // Custom shapes, animations, effects
    drawList.AddCircleFilled(pos, radius, color);
    drawList.AddText(pos, color, "Custom Text");
}
```

## Best Practices

### Performance
- Use update throttling for expensive operations
- Cache frequently used textures and data
- Batch drawing operations with DrawList
- Monitor frame rate impact

### Memory Management
- Implement IDisposable for all UI components
- Use weak references for large data structures
- Clean up event subscriptions and timers
- Monitor garbage collection impact

### Game Integration
- Respect game state (combat, cutscenes, etc.)
- Use Dalamud helpers for consistent styling
- Handle input conflicts gracefully
- Provide non-intrusive user interfaces

## Integration Examples

### Plugin Setup

```csharp
public class MyPlugin : IDalamudPlugin
{
    private readonly UIManager _uiManager;
    
    public void Initialize(IDalamudPluginInterface pluginInterface)
    {
        _uiManager = new UIManager(pluginInterface);
        pluginInterface.UiBuilder.Draw += _uiManager.Draw;
    }
    
    public void Dispose()
    {
        _uiManager.Dispose();
    }
}
```

### UI Management

```csharp
public class UIManager : IDisposable
{
    private readonly List<ImGuiWindow> _windows = new();
    
    public void Draw()
    {
        foreach (var window in _windows)
        {
            window.Draw();
        }
    }
}
```

## Troubleshooting

### Common Issues

1. **Performance Problems**
   - Check for expensive operations in Draw methods
   - Ensure proper update throttling
   - Monitor memory usage and garbage collection

2. **UI Conflicts**
   - Use proper ImGui IDs for duplicate widgets
   - Handle input conflicts with game
   - Manage window focus and layering

3. **Memory Leaks**
   - Ensure proper Dispose implementation
   - Check for event subscription cleanup
   - Monitor long-lived object references

### Debug Tools

```bash
# Validate your project structure
./scripts/validate-project.sh

# Test component implementations
./scripts/test-imgui-components.sh
```

## Learning Path

### 1. Start with Basics
- Use basic-window template
- Learn fundamental ImGui widgets
- Understand window management

### 2. Add Interactivity
- Explore interactive-tool template
- Implement event handling
- Add user feedback mechanisms

### 3. Advanced Features
- Study data-viz template
- Implement custom drawing
- Optimize for performance

### 4. Expert Patterns
- Review workflow-utility example
- Implement complex state machines
- Master Dalamud integration

## Contributing

This skill follows OpenCode best practices:

- Progressive disclosure with lean SKILL.md
- Comprehensive reference documentation
- Working, validated templates and examples
- Performance-optimized implementations

### Adding New Components

1. Create template in `assets/templates/`
2. Add example in `assets/examples/`
3. Update documentation in `references/`
4. Test with validation scripts
5. Update this README

## License

MIT License - Feel free to use, modify, and distribute for your Dalamud plugin development needs.

---

This skill provides everything needed to create professional-quality ImGui interfaces for FFXIV Dalamud plugins, from basic windows to complex interactive utilities, with full attention to performance and best practices in game development environments.