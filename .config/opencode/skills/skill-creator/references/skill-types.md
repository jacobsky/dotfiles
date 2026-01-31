# Skill Types Guide

This guide describes common patterns and structures for different types of OpenCode skills.

## Analysis Skills

Analysis skills examine code, data, or systems to extract insights and information.

### Common Characteristics
- Input-focused: Read and analyze existing content
- Pattern recognition: Identify trends, issues, or opportunities
- Reporting: Generate structured reports with findings
- Often multi-phase: Discovery → Analysis → Synthesis → Reporting

### Structure Template
```markdown
## What I do
- Analyze [specific domain] for [patterns/issues/insights]
- Identify [specific types of findings]
- Generate structured reports with recommendations
- Categorize findings by [priority/type/severity]

## When to use me
- When you need to understand [domain] state and characteristics
- For identifying [specific problems or opportunities]
- Before making [decisions or changes]
- When creating [improvement plans or strategies]

## How I work
### Phase 1: Discovery and Data Collection
- Scan codebase for [relevant files/patterns]
- Collect metrics and metadata
- Establish baseline understanding

### Phase 2: Pattern Analysis
- Apply analytical frameworks to collected data
- Identify trends, anomalies, and opportunities
- Categorize findings by meaningful criteria

### Phase 3: Insight Generation
- Synthesize patterns into actionable insights
- Prioritize findings by impact and feasibility
- Generate specific recommendations

### Phase 4: Report Generation
- Create structured output with clear sections
- Provide context for each finding
- Include next steps and implementation guidance
```

### Examples
- **task-review**: Analyzes codebase agents, commits, TODOs
- **security-audit**: Examines code for security vulnerabilities
- **performance-analyzer**: Identifies performance bottlenecks
- **dependency-audit**: Analyzes package dependencies and risks

## Automation Skills

Automation skills streamline repetitive tasks and coordinate complex workflows.

### Common Characteristics
- Process-oriented: Execute defined workflows
- Multi-step: Coordinate sequence of actions
- Integration: Connect multiple tools and systems
- Error handling: Robust failure recovery

### Structure Template
```markdown
## What I do
- Automate [specific process or workflow]
- Coordinate [multiple tools/systems]
- Handle [error conditions and edge cases]
- Execute [sequence of actions] reliably

## When to use me
- For repetitive [tasks or processes]
- When coordinating [multiple tools or steps]
- To ensure [consistency and reliability]
- When implementing [complex workflows]

## How I work
### Phase 1: Setup and Validation
- Verify prerequisites and environment
- Validate inputs and configuration
- Prepare tools and resources

### Phase 2: Execution
- Execute workflow steps in sequence
- Monitor progress and intermediate results
- Handle errors and retries automatically

### Phase 3: Completion and Cleanup
- Verify successful completion
- Generate execution summary
- Clean up temporary resources

## Execution Pattern
- **Primary Agent**: Usually build or plan for orchestration
- **Parallel Execution**: Often uses parallel: true for efficiency
- **Error Recovery**: Built-in retry and fallback mechanisms
```

### Examples
- **commit-automation**: Streamlines daily commit process
- **deploy-coordinator**: Manages deployment workflows
- **backup-automation**: Coordinates backup processes
- **test-runner**: Executes test suites automatically

## Generation Skills

Generation skills create new content, code, or artifacts based on specifications.

### Common Characteristics
- Creative output: Produce new content from inputs
- Template-based: Use patterns and templates
- Customizable: Adapt output based on parameters
- Validation: Ensure generated content meets standards

### Structure Template
```markdown
## What I do
- Generate [type of content] from [inputs]
- Apply [templates and patterns] consistently
- Customize output based on [parameters and requirements]
- Validate generated content for [quality and compliance]

## When to use me
- When creating [type of content] from scratch
- For consistent [documentation or code] generation
- To automate [content creation] workflows
- When [templates and patterns] should be applied

## How I work
### Phase 1: Input Analysis
- Analyze requirements and parameters
- Identify appropriate templates and patterns
- Validate inputs and prerequisites

### Phase 2: Content Generation
- Apply selected templates to inputs
- Generate base content structure
- Customize based on specific requirements

### Phase 3: Refinement and Validation
- Refine generated content for quality
- Validate against standards and requirements
- Make final adjustments and improvements

## Output Format
- [Describe the structure and format of generated content]
- [Include examples of typical output]
- [Mention customization options]
```

### Examples
- **doc-generator**: Creates API documentation from code
- **boilerplate-generator**: Generates project scaffolding
- **config-generator**: Creates configuration files
- **report-generator**: Produces formatted reports from data

## Documentation Skills

Documentation skills organize and present information in clear, accessible formats.

### Common Characteristics
- Information synthesis: Combine multiple sources
- Structure and organization: Logical content arrangement
- Clarity focus: Present complex information clearly
- Multi-format: Support various output formats

### Structure Template
```markdown
## What I do
- Organize [type of information] into clear documentation
- Synthesize [multiple sources] into coherent content
- Create [structured formats] for different audiences
- Ensure [accuracy and completeness] of documentation

## When to use me
- When creating [technical documentation]
- For organizing [complex information]
- To improve [clarity and accessibility]
- When standardizing [documentation formats]

## How I work
### Phase 1: Information Collection
- Gather relevant information from multiple sources
- Identify target audience and requirements
- Organize information by topic and priority

### Phase 2: Structure and Organization
- Create logical document structure
- Organize content into sections and subsections
- Establish navigation and cross-references

### Phase 3: Content Creation
- Write clear, accessible documentation
- Include examples and illustrations
- Add references and additional resources

## Output Format
- [Describe documentation structure and format]
- [Include examples of typical documentation sections]
- [Mention customization options for different audiences]
```

### Examples
- **api-doc-generator**: Creates API documentation
- **knowledge-base-creator**: Organizes team knowledge
- **tutorial-generator**: Creates step-by-step guides
- **spec-writer**: Generates technical specifications

## Hybrid Skills

Many skills combine elements from multiple types. For example:
- **Analysis + Generation**: Analyze code then generate documentation
- **Automation + Analysis**: Run automated processes then analyze results
- **Generation + Documentation**: Create content then document it

When creating hybrid skills, focus on the primary purpose while incorporating secondary capabilities naturally.

## Skill Type Selection Guide

### Choose Analysis Skills When:
- Primary goal is understanding existing state
- You need to identify patterns or issues
- Output is primarily informational
- Decision-making support is needed

### Choose Automation Skills When:
- Process efficiency is the main goal
- Tasks are repetitive or complex
- Multiple tools/systems need coordination
- Reliability and consistency are critical

### Choose Generation Skills When:
- Creating new content from inputs
- Templates and patterns can be applied
- Consistency in output is important
- Customization based on parameters is needed

### Choose Documentation Skills When:
- Information organization is key
- Multiple sources need synthesis
- Clarity and accessibility are priorities
- Different audiences need different formats

## Best Practices for All Skill Types

1. **Clear Purpose**: Each skill should have a focused, well-defined purpose
2. **Progressive Disclosure**: Keep SKILL.md lean, move details to references/
3. **Agent Selection**: Choose appropriate agents for different phases
4. **Error Handling**: Include robust error handling and recovery
5. **Validation**: Validate inputs, outputs, and intermediate results
6. **Documentation**: Provide clear usage examples and troubleshooting guidance
7. **Context Optimization**: Structure content for efficient context window usage
8. **Extensibility**: Design skills to be adaptable and extensible