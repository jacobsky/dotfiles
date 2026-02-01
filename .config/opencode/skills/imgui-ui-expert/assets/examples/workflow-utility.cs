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
    /// Workflow Utility - Automation and workflow management tools
    /// Expert-level example with state machines and complex interactions
    /// </summary>
    public class WorkflowUtility : IDisposable
    {
        private bool _isVisible = true;
        private readonly List<Workflow> _workflows = new();
        private readonly List<WorkflowStep> _currentSteps = new();
        private int _selectedWorkflow = -1;
        private WorkflowState _currentState = WorkflowState.Idle;
        private DateTime _stepStartTime = DateTime.Now;
        
        // Workflow execution
        private readonly Queue<string> _executionLog = new();
        private const int MaxLogEntries = 50;
        private bool _isExecuting = false;
        private float _executionProgress = 0f;
        
        // Settings
        private bool _autoAdvance = true;
        private bool _showNotifications = true;
        private int _stepDelayMs = 500;
        
        public enum WorkflowState
        {
            Idle,
            Running,
            Paused,
            Completed,
            Error
        }
        
        public record Workflow(string Name, string Description, List<WorkflowStep> Steps);
        public record WorkflowStep(string Name, string Description, Action<WorkflowStep> Execute, Func<WorkflowStep, bool> CanExecute, int DelayMs = 0);
        
        public WorkflowUtility()
        {
            InitializeWorkflows();
        }

        private void InitializeWorkflows()
        {
            // Example workflows
            _workflows.Add(new Workflow(
                "Daily Preparation",
                "Automated daily routine for game preparation",
                new List<WorkflowStep>
                {
                    new WorkflowStep("Check Inventory", "Verify inventory space and items", 
                        (step) => CheckInventory(), 
                        (step) => true, 1000),
                    new WorkflowStep("Repair Gear", "Repair damaged equipment", 
                        (step) => RepairGear(), 
                        (step) => HasDamagedGear(), 2000),
                    new WorkflowStep("Sort Inventory", "Organize inventory items", 
                        (step) => SortInventory(), 
                        (step) => true, 1500),
                    new WorkflowStep("Check Market Board", "Review market prices", 
                        (step) => CheckMarketBoard(), 
                        (step) => true, 3000)
                }
            ));
            
            _workflows.Add(new Workflow(
                "Dungeon Preparation",
                "Prepare for dungeon runs",
                new List<WorkflowStep>
                {
                    new WorkflowStep("Select Job", "Choose appropriate job", 
                        (step) => SelectJob(), 
                        (step) => true, 500),
                    new WorkflowStep("Check Gear", "Verify gear condition", 
                        (step) => CheckGear(), 
                        (step) => true, 1000),
                    new WorkflowStep("Stock Consumables", "Ensure adequate supplies", 
                        (step) => StockConsumables(), 
                        (step) => true, 2000),
                    new WorkflowStep("Set Skills", "Configure skill bars", 
                        (step) => SetSkills(), 
                        (step) => true, 1500)
                }
            ));
            
            _workflows.Add(new Workflow(
                "Crafting Session",
                "Automated crafting workflow",
                new List<WorkflowStep>
                {
                    new WorkflowStep("Gather Materials", "Collect required materials", 
                        (step) => GatherMaterials(), 
                        (step) => true, 3000),
                    new WorkflowStep("Select Recipe", "Choose crafting recipe", 
                        (step) => SelectRecipe(), 
                        (step) => true, 500),
                    new WorkflowStep("Craft Items", "Execute crafting process", 
                        (step) => CraftItems(), 
                        (step) => true, 5000),
                    new WorkflowStep("Quality Check", "Inspect crafted items", 
                        (step) => QualityCheck(), 
                        (step) => true, 1000)
                }
            ));
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => _isVisible = value;
        }

        public void Draw()
        {
            if (!_isVisible) return;

            UpdateExecution();
            
            var windowFlags = ImGuiWindowFlags.None;
            ImGui.SetNextWindowSize(new Vector2(700, 550), ImGuiCond.FirstUseEver);
            
            if (ImGui.Begin("Workflow Utility", ref _isVisible, windowFlags))
            {
                DrawHeader();
                DrawWorkflowSelector();
                DrawWorkflowInfo();
                DrawCurrentSteps();
                DrawExecutionControls();
                DrawExecutionLog();
            }
            
            ImGui.End();
        }

        private void DrawHeader()
        {
            // Title and status
            ImGui.Text("Workflow Utility - Automation Manager");
            
            ImGui.SameLine();
            
            var statusColor = GetStateColor(_currentState);
            var statusText = _currentState.ToString();
            ImGui.TextColored(statusColor, $"● {statusText}");
            
            // Execution progress
            if (_isExecuting)
            {
                ImGui.SameLine();
                ImGui.Text($"({_executionProgress:P0} complete)");
            }
            
            ImGui.Separator();
        }

        private void DrawWorkflowSelector()
        {
            ImGui.Text("Available Workflows:");
            
            // Workflow selection
            if (ImGui.BeginCombo("##workflow", _selectedWorkflow >= 0 ? _workflows[_selectedWorkflow].Name : "Select workflow..."))
            {
                for (int i = 0; i < _workflows.Count; i++)
                {
                    var isSelected = i == _selectedWorkflow;
                    var workflow = _workflows[i];
                    
                    if (ImGui.Selectable(workflow.Name, isSelected))
                    {
                        _selectedWorkflow = i;
                        ResetCurrentSteps();
                    }
                    
                    if (isSelected)
                    {
                        ImGui.SameLine();
                        ImGui.Text($"- {workflow.Description}");
                    }
                }
                ImGui.EndCombo();
            }
            
            // Create new workflow button
            ImGui.SameLine();
            if (ImGui.Button("Create New"))
            {
                // Open workflow creation dialog
                ImGui.OpenPopup("CreateWorkflow");
            }
            
            // Create workflow popup
            if (ImGui.BeginPopup("CreateWorkflow"))
            {
                static string newName = "";
                static string newDescription = "";
                
                ImGui.SetNextItemWidth(200);
                ImGui.InputText("Name", ref newName, 256);
                ImGui.SetNextItemWidth(300);
                ImGui.InputText("Description", ref newDescription, 512);
                
                if (ImGui.Button("Create"))
                {
                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        _workflows.Add(new Workflow(newName, newDescription, new List<WorkflowStep>()));
                        _selectedWorkflow = _workflows.Count - 1;
                        newName = "";
                        newDescription = "";
                        ImGui.CloseCurrentPopup();
                    }
                }
                
                ImGui.SameLine();
                if (ImGui.Button("Cancel"))
                {
                    ImGui.CloseCurrentPopup();
                }
                
                ImGui.EndPopup();
            }
            
            ImGui.Separator();
        }

        private void DrawWorkflowInfo()
        {
            if (_selectedWorkflow < 0 || _selectedWorkflow >= _workflows.Count)
                return;
            
            var workflow = _workflows[_selectedWorkflow];
            
            ImGui.Text($"Workflow: {workflow.Name}");
            ImGui.SameLine();
            ImGui.TextColored(new Vector4(0.5f, 0.5f, 0.5f, 1), workflow.Description);
            
            // Workflow statistics
            ImGui.Text($"Steps: {workflow.Steps.Count}");
            ImGui.SameLine();
            ImGui.Text($"Est. Duration: {workflow.Steps.Sum(s => s.DelayMs) / 1000}s");
            
            // Step list preview
            if (ImGui.TreeNode("Step List"))
            {
                for (int i = 0; i < workflow.Steps.Count; i++)
                {
                    var step = workflow.Steps[i];
                    var canExecute = step.CanExecute(step);
                    var icon = canExecute ? "✓" : "✗";
                    var color = canExecute ? new Vector4(0, 1, 0, 1) : new Vector4(1, 0, 0, 1);
                    
                    ImGui.TextColored(color, $"{icon} {i + 1}. {step.Name}");
                    
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip(step.Description);
                    }
                }
                
                ImGui.TreePop();
            }
            
            ImGui.Separator();
        }

        private void DrawCurrentSteps()
        {
            ImGui.Text("Current Execution:");
            
            if (_currentSteps.Count == 0)
            {
                ImGui.TextColored(new Vector4(0.5f, 0.5f, 0.5f, 1), "No workflow selected or started");
                return;
            }
            
            // Progress bar
            var completedSteps = _currentSteps.Count(s => s.Name.StartsWith("[COMPLETED]"));
            var progress = (float)completedSteps / _currentSteps.Count;
            
            ImGui.ProgressBar(progress, new Vector2(-1, 0), $"{completedSteps}/{_currentSteps.Count} steps completed");
            
            // Step list with status
            if (ImGui.BeginTable("CurrentSteps", 3, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
            {
                ImGui.TableSetupColumn("Status");
                ImGui.TableSetupColumn("Step");
                ImGui.TableSetupColumn("Time");
                ImGui.TableHeadersRow();
                
                for (int i = 0; i < _currentSteps.Count; i++)
                {
                    var step = _currentSteps[i];
                    var isCompleted = step.Name.StartsWith("[COMPLETED]");
                    var isCurrent = i == GetCurrentStepIndex();
                    
                    ImGui.TableNextRow();
                    
                    ImGui.TableSetColumnIndex(0);
                    var statusColor = isCompleted ? new Vector4(0, 1, 0, 1) : 
                                       isCurrent ? new Vector4(1, 1, 0, 1) : 
                                       new Vector4(0.5f, 0.5f, 0.5f, 1);
                    var statusIcon = isCompleted ? "✓" : isCurrent ? "→" : "○";
                    ImGui.TextColored(statusColor, statusIcon);
                    
                    ImGui.TableSetColumnIndex(1);
                    var displayName = isCompleted ? step.Name.Replace("[COMPLETED] ", "") : step.Name;
                    if (isCurrent)
                    {
                        ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(1, 1, 0, 1));
                        ImGui.Text(displayName);
                        ImGui.PopStyleColor();
                    }
                    else
                    {
                        ImGui.Text(displayName);
                    }
                    
                    ImGui.TableSetColumnIndex(2);
                    // Time display would be calculated from actual execution times
                    ImGui.Text(isCompleted ? "Completed" : "Pending");
                }
                
                ImGui.EndTable();
            }
        }

        private void DrawExecutionControls()
        {
            ImGui.Separator();
            ImGui.Text("Execution Controls:");
            
            // Control buttons
            if (_currentState == WorkflowState.Idle || _currentState == WorkflowState.Completed)
            {
                if (ImGui.Button("Start Workflow"))
                {
                    StartWorkflow();
                }
            }
            else if (_currentState == WorkflowState.Running)
            {
                if (ImGui.Button("Pause"))
                {
                    PauseWorkflow();
                }
                
                ImGui.SameLine();
                
                if (ImGui.Button("Stop"))
                {
                    StopWorkflow();
                }
            }
            else if (_currentState == WorkflowState.Paused)
            {
                if (ImGui.Button("Resume"))
                {
                    ResumeWorkflow();
                }
                
                ImGui.SameLine();
                
                if (ImGui.Button("Stop"))
                {
                    StopWorkflow();
                }
            }
            else if (_currentState == WorkflowState.Error)
            {
                if (ImGui.Button("Retry"))
                {
                    RetryWorkflow();
                }
                
                ImGui.SameLine();
                
                if (ImGui.Button("Reset"))
                {
                    ResetWorkflow();
                }
            }
            
            ImGui.SameLine();
            
            if (ImGui.Button("Clear Log"))
            {
                ClearExecutionLog();
            }
            
            // Settings
            ImGui.Separator();
            ImGui.Text("Settings:");
            
            ImGui.Checkbox("Auto Advance", ref _autoAdvance);
            ImGui.SameLine();
            ImGui.Checkbox("Show Notifications", ref _showNotifications);
            
            ImGui.Text("Step Delay (ms):");
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100);
            ImGui.SliderInt("##stepdelay", ref _stepDelayMs, 0, 5000);
        }

        private void DrawExecutionLog()
        {
            ImGui.Separator();
            ImGui.Text("Execution Log:");
            
            var logHeight = ImGui.GetContentRegionAvail().Y;
            if (ImGui.BeginChild("ExecutionLog", new Vector2(0, logHeight)))
            {
                foreach (var entry in _executionLog.Reverse().Take(20))
                {
                    ImGui.Text(entry);
                }
            }
            
            ImGui.EndChild();
        }

        private void UpdateExecution()
        {
            if (!_isExecuting || _currentState != WorkflowState.Running)
                return;
            
            var currentStepIndex = GetCurrentStepIndex();
            if (currentStepIndex < 0 || currentStepIndex >= _currentSteps.Count)
            {
                CompleteWorkflow();
                return;
            }
            
            var currentStep = _currentSteps[currentStepIndex];
            var elapsedMs = (DateTime.Now - _stepStartTime).TotalMilliseconds;
            
            if (elapsedMs >= currentStep.DelayMs)
            {
                ExecuteStep(currentStep);
            }
            
            // Update progress
            _executionProgress = (float)currentStepIndex / _currentSteps.Count;
        }

        private void StartWorkflow()
        {
            if (_selectedWorkflow < 0 || _selectedWorkflow >= _workflows.Count)
            {
                AddLogEntry("Error: No workflow selected");
                return;
            }
            
            var workflow = _workflows[_selectedWorkflow];
            _currentSteps = workflow.Steps.ToList();
            _currentState = WorkflowState.Running;
            _isExecuting = true;
            _stepStartTime = DateTime.Now;
            _executionProgress = 0f;
            
            AddLogEntry($"Started workflow: {workflow.Name}");
            
            if (_showNotifications)
            {
                Service.ChatGui.Print($"[Workflow] Started: {workflow.Name}");
            }
        }

        private void PauseWorkflow()
        {
            _currentState = WorkflowState.Paused;
            AddLogEntry("Workflow paused");
        }

        private void ResumeWorkflow()
        {
            _currentState = WorkflowState.Running;
            _stepStartTime = DateTime.Now; // Reset step time on resume
            AddLogEntry("Workflow resumed");
        }

        private void StopWorkflow()
        {
            _currentState = WorkflowState.Idle;
            _isExecuting = false;
            _executionProgress = 0f;
            AddLogEntry("Workflow stopped");
            
            if (_showNotifications)
            {
                Service.ChatGui.Print("[Workflow] Stopped");
            }
        }

        private void CompleteWorkflow()
        {
            _currentState = WorkflowState.Completed;
            _isExecuting = false;
            _executionProgress = 1.0f;
            AddLogEntry("Workflow completed successfully");
            
            if (_showNotifications)
            {
                Service.ChatGui.Print("[Workflow] ✅ Completed successfully");
            }
        }

        private void RetryWorkflow()
        {
            ResetCurrentSteps();
            StartWorkflow();
            AddLogEntry("Retrying workflow");
        }

        private void ResetWorkflow()
        {
            _currentState = WorkflowState.Idle;
            _isExecuting = false;
            _executionProgress = 0f;
            ResetCurrentSteps();
            AddLogEntry("Workflow reset");
        }

        private void ExecuteStep(WorkflowStep step)
        {
            try
            {
                AddLogEntry($"Executing: {step.Name}");
                
                if (step.CanExecute(step))
                {
                    step.Execute(step);
                    
                    // Mark step as completed
                    var index = _currentSteps.IndexOf(step);
                    if (index >= 0)
                    {
                        _currentSteps[index] = step with { Name = $"[COMPLETED] {step.Name}" };
                    }
                    
                    AddLogEntry($"Completed: {step.Name}");
                }
                else
                {
                    AddLogEntry($"Skipped: {step.Name} (conditions not met)");
                }
                
                _stepStartTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                _currentState = WorkflowState.Error;
                AddLogEntry($"Error in {step.Name}: {ex.Message}");
                Service.Logger.Error(ex, $"Workflow step error: {step.Name}");
            }
        }

        private int GetCurrentStepIndex()
        {
            for (int i = 0; i < _currentSteps.Count; i++)
            {
                if (!_currentSteps[i].Name.StartsWith("[COMPLETED]"))
                {
                    return i;
                }
            }
            return -1; // All completed
        }

        private void ResetCurrentSteps()
        {
            if (_selectedWorkflow >= 0 && _selectedWorkflow < _workflows.Count)
            {
                var workflow = _workflows[_selectedWorkflow];
                _currentSteps = workflow.Steps.ToList();
            }
            else
            {
                _currentSteps.Clear();
            }
        }

        private void AddLogEntry(string entry)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            _executionLog.Enqueue($"[{timestamp}] {entry}");
            
            while (_executionLog.Count > MaxLogEntries)
            {
                _executionLog.Dequeue();
            }
        }

        private void ClearExecutionLog()
        {
            _executionLog.Clear();
            AddLogEntry("Log cleared");
        }

        private Vector4 GetStateColor(WorkflowState state)
        {
            return state switch
            {
                WorkflowState.Running => new Vector4(0, 1, 0, 1),    // Green
                WorkflowState.Paused => new Vector4(1, 1, 0, 1),   // Yellow
                WorkflowState.Completed => new Vector4(0, 0.5f, 1, 1), // Blue
                WorkflowState.Error => new Vector4(1, 0, 0, 1),     // Red
                _ => new Vector4(0.5f, 0.5f, 0.5f, 1)      // Gray
            };
        }

        // Step implementation methods (simplified for example)
        private void CheckInventory()
        {
            Service.Logger.Info("Checking inventory...");
            // Actual inventory check implementation
        }

        private bool HasDamagedGear()
        {
            // Check if any gear is damaged
            return false; // Simplified
        }

        private void RepairGear()
        {
            Service.Logger.Info("Repairing gear...");
            // Actual repair implementation
        }

        private void SortInventory()
        {
            Service.Logger.Info("Sorting inventory...");
            // Actual sorting implementation
        }

        private void CheckMarketBoard()
        {
            Service.Logger.Info("Checking market board...");
            // Actual market check implementation
        }

        private void SelectJob()
        {
            Service.Logger.Info("Selecting job...");
            // Job selection implementation
        }

        private void CheckGear()
        {
            Service.Logger.Info("Checking gear...");
            // Gear check implementation
        }

        private void StockConsumables()
        {
            Service.Logger.Info("Stocking consumables...");
            // Consumable stocking implementation
        }

        private void SetSkills()
        {
            Service.Logger.Info("Setting skills...");
            // Skill setup implementation
        }

        private void GatherMaterials()
        {
            Service.Logger.Info("Gathering materials...");
            // Material gathering implementation
        }

        private void SelectRecipe()
        {
            Service.Logger.Info("Selecting recipe...");
            // Recipe selection implementation
        }

        private void CraftItems()
        {
            Service.Logger.Info("Crafting items...");
            // Crafting implementation
        }

        private void QualityCheck()
        {
            Service.Logger.Info("Checking quality...");
            // Quality check implementation
        }

        public void Dispose()
        {
            Service.Logger.Info("WorkflowUtility disposed");
        }
    }
}