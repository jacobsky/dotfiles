# Agent Capabilities Guide

This guide describes the capabilities and optimal use cases for different OpenCode agents.

## Agent Overview

OpenCode agents are specialized assistants with different strengths and focuses. Understanding their capabilities helps you design skills that leverage the right agent for each task.

## Core Agents

### build

**Primary Focus**: Implementation and code-focused tasks

**Strengths**:
- Code implementation and modification
- File operations and content creation
- Commit generation and version control
- Code review and validation
- Build processes and automation

**Ideal For**:
- Writing new code or modifying existing code
- Creating and managing files
- Running build processes
- Code quality assurance
- Git operations and commits

**Tool Preferences**:
- `read`, `write`, `edit` for file operations
- `bash` for build commands and scripts
- `glob`, `grep` for code analysis
- `todoread`, `todowrite` for task management

**Example Usage**:
```markdown
- **build**: Implement code changes and handle file operations
- **build**: Review and commit modifications
- **build**: Execute build scripts and validation
```

### plan / planner

**Primary Focus**: Analysis, planning, and task generation

**Strengths**:
- Task breakdown and structuring
- Analysis and assessment
- TODO generation and management
- Strategic planning and recommendations
- Workflow design

**Ideal For**:
- Analyzing complex situations
- Breaking down large tasks
- Generating TODO lists and action items
- Creating structured plans
- Assessment and evaluation

**Tool Preferences**:
- `todoread`, `todowrite` for task management
- `read`, `grep` for information gathering
- `task` for delegating specialized work
- `question` for requirements gathering

**Example Usage**:
```markdown
- **plan**: Analyze requirements and generate structured TODO list
- **planner**: Break down complex tasks into manageable steps
- **plan**: Create strategic recommendations and action plans
```

### explore

**Primary Focus**: File system operations and discovery

**Strengths**:
- File system navigation and analysis
- Pattern searching and discovery
- Git analysis and history
- Codebase structure understanding
- Resource location and organization

**Ideal For**:
- Finding files and patterns
- Analyzing git history
- Understanding codebase structure
- Resource discovery
- File system operations

**Tool Preferences**:
- `glob` for file pattern matching
- `grep` for content searching
- `read` for file examination
- `bash` for file system commands
- `task` for comprehensive exploration

**Example Usage**:
```markdown
- **explore**: Search codebase for patterns and analyze structure
- **explore**: Examine git history and commit patterns
- **explore**: Locate resources and understand organization
```

### librarian

**Primary Focus**: Documentation research and knowledge synthesis

**Strengths**:
- Documentation analysis and synthesis
- Research and information gathering
- Knowledge organization
- Reference material processing
- Educational content creation

**Ideal For**:
- Analyzing documentation
- Research and information gathering
- Creating educational content
- Knowledge base organization
- Reference material processing

**Tool Preferences**:
- `read` for documentation analysis
- `webfetch`, `websearch` for external research
- `task` for specialized research tasks
- `write` for creating organized content

**Example Usage**:
```markdown
- **librarian**: Analyze documentation and extract key information
- **librarian**: Research best practices and compile findings
- **librarian**: Create organized knowledge summaries
```

### oracle

**Primary Focus**: Strategic recommendations and decision support

**Strengths**:
- Strategic analysis and recommendations
- Decision support and prioritization
- High-level problem solving
- Trend analysis and prediction
- Optimization suggestions

**Ideal For**:
- Making strategic decisions
- Prioritizing tasks and recommendations
- High-level analysis and insights
- Optimization and improvement suggestions
- Complex problem solving

**Tool Preferences**:
- `task` for comprehensive analysis
- `question` for requirement clarification
- `todoread`, `todowrite` for recommendations
- `write` for structured output

**Example Usage**:
```markdown
- **oracle**: Analyze findings and provide strategic recommendations
- **oracle**: Prioritize tasks based on impact and feasibility
- **oracle**: Generate optimization suggestions and improvement plans
```

## Specialized Agents

### security

**Primary Focus**: Security analysis and vulnerability assessment

**Strengths**:
- Security vulnerability identification
- Code security analysis
- Best practices enforcement
- Threat assessment
- Security recommendations

**Ideal For**:
- Security audits and assessments
- Vulnerability scanning and analysis
- Security best practices implementation
- Threat modeling
- Security policy compliance

### perf (Performance)

**Primary Focus**: Performance analysis and optimization

**Strengths**:
- Performance bottleneck identification
- Optimization recommendations
- Benchmarking and analysis
- Resource usage analysis
- Performance testing

**Ideal For**:
- Performance profiling and analysis
- Optimization strategies
- Bottleneck identification
- Resource usage optimization
- Performance testing and benchmarking

### test

**Primary Focus**: Testing strategy and implementation

**Strengths**:
- Test strategy development
- Test case generation
- Coverage analysis
- Quality assurance
- Testing automation

**Ideal For**:
- Creating comprehensive test strategies
- Generating test cases and scenarios
- Analyzing test coverage
- Quality assurance processes
- Test automation implementation

## Agent Selection Guidelines

### Task Type Matching

**Implementation Tasks** → `build`
- Code writing and modification
- File operations
- Build processes
- Version control

**Analysis Tasks** → `plan` or `explore`
- Requirements analysis → `plan`
- Codebase exploration → `explore`
- Task breakdown → `plan`
- Pattern discovery → `explore`

**Research Tasks** → `librarian`
- Documentation analysis
- Information gathering
- Knowledge synthesis
- Educational content

**Strategic Tasks** → `oracle`
- Decision making
- Prioritization
- High-level recommendations
- Complex problem solving

**Specialized Domains** → Specialized Agents
- Security → `security`
- Performance → `perf`
- Testing → `test`

### Agent Collaboration Patterns

**Sequential Delegation**:
```markdown
1. **explore**: Discover and analyze existing codebase
2. **plan**: Break down findings into actionable tasks
3. **build**: Implement recommended changes
```

**Parallel Analysis**:
```markdown
1. **explore**: Analyze codebase structure (parallel)
2. **librarian**: Research documentation (parallel)
3. **oracle**: Synthesize findings and provide recommendations
```

**Specialist Coordination**:
```markdown
1. **security**: Perform security audit
2. **perf**: Analyze performance characteristics
3. **oracle**: Prioritize findings across domains
4. **build**: Implement highest-priority fixes
```

### Agent Configuration Options

**Model Selection**:
Some tasks benefit from specific models:
```yaml
metadata:
  model: opencode/gpt-5-nano  # For specific tasks requiring particular capabilities
```

**Parallel Execution**:
For skills that benefit from parallel processing:
```yaml
metadata:
  parallel: true
  workflow: multi-analysis
```

**Subtask Delegation**:
When complex tasks should be broken down:
```markdown
Create `TODO(agent: specialized-agent)` for specific subtasks
```

## Best Practices for Agent Selection

### 1. Match Agent to Primary Task Type
Choose the agent whose primary focus aligns with your main task objective.

### 2. Consider Agent Strengths
Leverage each agent's unique strengths and specializations.

### 3. Use Appropriate Delegation
Don't over-delegate simple tasks; don't under-delegate complex specialized work.

### 4. Plan for Parallel Execution
When phases can run independently, use parallel execution for efficiency.

### 5. Validate Agent Capabilities
Ensure the selected agent has access to the tools needed for the task.

### 6. Consider Model Requirements
Some tasks may benefit from specific model capabilities.

### 7. Plan Error Handling
Include fallback strategies when agent delegation fails.

## Common Agent Selection Patterns

### Pattern 1: Discovery → Analysis → Implementation
```markdown
- **explore**: Discover and analyze current state
- **plan**: Analyze findings and create action plan  
- **build**: Implement recommended changes
```

### Pattern 2: Research → Synthesis → Recommendation
```markdown
- **librarian**: Research documentation and best practices
- **oracle**: Synthesize findings and provide strategic recommendations
- **plan**: Create actionable implementation plan
```

### Pattern 3: Audit → Prioritization → Action
```markdown
- **security**: Conduct security audit
- **perf**: Analyze performance characteristics
- **oracle**: Prioritize findings across domains
- **build**: Implement high-priority fixes
```

### Pattern 4: Exploration → Planning → Automation
```markdown
- **explore**: Explore codebase and identify patterns
- **plan**: Create structured automation plan
- **build**: Implement automation scripts and workflows
```

## Agent Limitations and Considerations

### Tool Access
Not all agents have access to all tools. Check agent capabilities when designing skills.

### Context Window
Consider context usage when delegating to multiple agents.

### Coordination Overhead
Multiple agent coordination adds complexity; use judiciously.

### Error Propagation
Plan for handling failures when one agent in a chain encounters issues.

### Performance
Multiple agent delegation may increase execution time; balance quality vs. speed.

This guide helps you make informed decisions about agent selection when creating skills, ensuring your skills leverage the right capabilities for each task.