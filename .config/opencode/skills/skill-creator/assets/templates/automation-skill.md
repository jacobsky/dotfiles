---
name: automation-skill-template
description: Template for creating automation skills that streamline repetitive tasks and coordinate workflows
license: MIT
compatibility: opencode
metadata:
  workflow: coordinated-execution
  parallel: true
  output: execution-summary
---

## What I do

- Automate [specific process or workflow] with reliable execution
- Coordinate [multiple tools/systems] for end-to-end automation
- Handle [error conditions and edge cases] with graceful recovery
- Execute [sequence of actions] consistently and reproducibly
- Monitor and report on [process status and outcomes]

## When to use me

Use this skill when you need to:
- Streamline repetitive [tasks or processes]
- Coordinate [multiple tools, systems, or steps]
- Ensure [consistency and reliability] in execution
- Implement [complex workflows] with error handling
- Reduce manual effort and improve [process efficiency]

## How I work

### Phase 1: Setup and Validation

- Verify prerequisites and environment requirements
- Validate inputs, configurations, and parameters
- Prepare tools, resources, and working directories
- Establish logging and monitoring capabilities
- Check access permissions and system availability

### Phase 2: Execution and Monitoring

- Execute workflow steps in defined sequence
- Monitor progress and intermediate results
- Handle errors with retry logic and fallback mechanisms
- Maintain execution state and checkpointing
- Collect metrics and performance data

### Phase 3: Completion and Cleanup

- Verify successful completion of all steps
- Generate comprehensive execution summary
- Clean up temporary resources and artifacts
- Archive logs and results for audit purposes
- Update status and notifications as needed

## Execution Pattern

### Agent Selection Logic

- **build**: Primary execution for implementation tasks
- **plan**: Workflow design and task coordination
- **explore**: Resource discovery and environment validation
- **oracle**: Decision making and exception handling

### Coordination Strategy

```yaml
metadata:
  parallel: true
  workflow: multi-step-automation
  retry-policy: exponential-backoff
  timeout-handling: graceful-degradation
```

### Parallel Execution Model

When steps can run independently:
```markdown
### Parallel Phase Setup
1. [Task Group A] - Background execution
2. [Task Group B] - Background execution  
3. [Task Group C] - Background execution
4. **build**: Monitor all tasks and collect results
```

### Sequential Dependencies

When order matters:
```markdown
### Sequential Workflow
1. **explore**: Validate environment and prerequisites
2. **build**: Execute core automation logic
3. **plan**: Analyze results and generate reports
4. **build**: Handle cleanup and finalization
```

## Error Handling and Resilience

### Error Classification

**Recoverable Errors**:
- Network timeouts and transient failures
- Resource temporarily unavailable
- Minor data inconsistencies
- Retryable system failures

**Non-Recoverable Errors**:
- Permission or access denied
- Critical system failures
- Data corruption or loss
- Configuration errors

### Retry and Recovery Strategies

```markdown
### Retry Policy
- **Network operations**: 3 retries with exponential backoff (1s, 2s, 4s)
- **File operations**: 2 retries with 1-second delay
- **External services**: Configurable retry based on service type
- **Critical operations**: Manual intervention required

### Fallback Mechanisms
- Use alternative data sources when primary unavailable
- Implement graceful degradation for non-critical failures
- Provide manual intervention points for complex scenarios
- Maintain partial progress when complete execution fails
```

### Monitoring and Alerting

- Real-time progress tracking with status indicators
- Automatic alerting for critical failures or delays
- Comprehensive logging for debugging and audit
- Performance metrics collection and analysis

## Output Format

```
# Automation Execution Report

## Execution Summary
- **Start Time**: [timestamp]
- **End Time**: [timestamp]
- **Duration**: [elapsed time]
- **Status**: [success/partial/failure]
- **Steps Completed**: [X/Y]

## Step-by-Step Results

### Phase 1: Setup and Validation
✅ Environment Validation - Duration: [time]
✅ Configuration Check - Duration: [time]
✅ Resource Preparation - Duration: [time]

### Phase 2: Execution
✅ Step 1 - [description] - Duration: [time]
⚠️  Step 2 - [description] - Duration: [time] - Warnings: [count]
✅ Step 3 - [description] - Duration: [time]

### Phase 3: Completion
✅ Result Verification - Duration: [time]
✅ Cleanup Operations - Duration: [time]
✅ Report Generation - Duration: [time]

## Metrics and Performance

### Execution Metrics
- **Total Processing Time**: [duration]
- **Average Step Duration**: [duration]
- **Peak Resource Usage**: [memory/CPU]
- **Throughput**: [items processed per time unit]

### Quality Metrics
- **Success Rate**: [percentage]
- **Error Rate**: [percentage]
- **Retry Rate**: [percentage]
- **Manual Interventions**: [count]

## Artifacts and Outputs

### Generated Files
- [file1] - [description] - [size]
- [file2] - [description] - [size]

### Reports and Summaries
- [report1] - [description] - [location]
- [report2] - [description] - [location]

### Logs and Audit Trails
- [execution.log] - Detailed execution log
- [errors.log] - Error and warning messages
- [metrics.json] - Performance metrics

## Issues and Recommendations

### Successful Operations
- [List of successfully completed operations]
- [Notable achievements or improvements]

### Issues Encountered
- [Issue 1]: [description] - [resolution]
- [Issue 2]: [description] - [workaround applied]

### Recommendations for Improvement
1. [Optimization suggestion 1] - Potential improvement: [benefit]
2. [Optimization suggestion 2] - Potential improvement: [benefit]
3. [Process enhancement] - Potential improvement: [benefit]

## Next Scheduled Execution
- **Scheduled Time**: [next run timestamp]
- **Recommended Changes**: [modifications for next run]
- **Monitoring Requirements**: [what to watch for]
```

## Configuration and Customization

### Environment Variables

```bash
# Core Configuration
AUTOMATION_DEBUG=false
AUTOMATION_TIMEOUT=300
AUTOMATION_RETRY_COUNT=3
AUTOMATION_LOG_LEVEL=INFO

# Resource Limits
AUTOMATION_MAX_MEMORY=2G
AUTOMATION_MAX_CPU=2
AUTOMATION_CONCURRENT_TASKS=4

# External Service Configuration
SERVICE_ENDPOINT_URL="https://api.example.com"
SERVICE_TIMEOUT=30
SERVICE_RETRY_POLICY=exponential-backoff
```

### Configuration Files

```yaml
# automation-config.yml
automation:
  name: "[automation-name]"
  version: "1.0.0"
  description: "[automation description]"
  
workflow:
  phases:
    - name: "setup"
      parallel: false
      timeout: 60
    - name: "execution" 
      parallel: true
      timeout: 300
    - name: "cleanup"
      parallel: false
      timeout: 30

error_handling:
  retry_policy:
    max_attempts: 3
    backoff_strategy: "exponential"
    max_delay: 30
    
monitoring:
  log_level: "INFO"
  metrics_collection: true
  alert_on_failure: true
```

## Best Practices and Guidelines

### Automation Design Principles

**Idempotency**: Ensure operations can be safely repeated
**Atomicity**: Design operations that either complete fully or not at all
**Observability**: Include comprehensive logging and monitoring
**Recoverability**: Build in error recovery and manual intervention points

### Resource Management

- Clean up temporary files and resources
- Monitor memory and CPU usage
- Implement rate limiting for external services
- Use connection pooling for database operations

### Security Considerations

- Validate all inputs and configurations
- Use secure credential management
- Implement proper access controls
- Audit all automation actions

## Troubleshooting Guide

### Common Issues

**Timeout Failures**:
- Check system resource availability
- Verify network connectivity
- Review timeout configurations
- Consider breaking down large operations

**Permission Errors**:
- Verify user credentials and access rights
- Check file and directory permissions
- Review service account configurations
- Ensure proper authentication setup

**Configuration Issues**:
- Validate configuration file syntax
- Check environment variable settings
- Verify external service connectivity
- Review dependency versions and compatibility

### Debugging Tools

- Enable debug logging for detailed execution traces
- Use dry-run mode to test workflows without side effects
- Implement checkpoint debugging to isolate issues
- Use performance profiling to identify bottlenecks

## Integration Points

### External Systems

- **CI/CD Platforms**: Jenkins, GitHub Actions, GitLab CI
- **Monitoring Systems**: Prometheus, Grafana, DataDog
- **Notification Services**: Slack, Email, PagerDuty
- **Storage Systems**: S3, Azure Blob, Google Cloud Storage

### API Integration

```python
# Example external service integration
class ServiceIntegration:
    def __init__(self, endpoint, timeout=30):
        self.endpoint = endpoint
        self.timeout = timeout
        
    def execute_with_retry(self, operation, max_retries=3):
        for attempt in range(max_retries):
            try:
                return operation()
            except Exception as e:
                if attempt == max_retries - 1:
                    raise
                time.sleep(2 ** attempt)  # Exponential backoff
```

## Maintenance and Updates

### Version Management

- Semantic versioning for automation releases
- Backward compatibility considerations
- Migration strategies for breaking changes
- Rollback procedures for failed updates

### Performance Optimization

- Regular performance monitoring and analysis
- Identification of bottlenecks and optimization opportunities
- Resource usage optimization
- Caching strategies for frequently accessed data

### Documentation Maintenance

- Keep documentation synchronized with code changes
- Update configuration examples for new features
- Maintain troubleshooting guides with known issues
- Regular review and update of best practices