---
name: analysis-skill-template
description: Template for creating analysis-type skills that examine code, data, or systems
license: MIT
compatibility: opencode
metadata:
  workflow: multi-phase-analysis
  parallel: false
  output: structured-report
---

## What I do

- Analyze [specific domain] for [patterns/issues/insights/opportunities]
- Identify [specific types of findings] with detailed context
- Categorize findings by [priority/severity/type/impact]
- Generate structured reports with actionable recommendations
- Provide [specific metric or measurement] analysis

## When to use me

Use this skill when you need to:
- Understand the current state of [domain]
- Identify [specific problems or opportunities]
- Make informed decisions based on [data/patterns]
- Create [improvement plans or remediation strategies]
- Establish [baseline metrics] for future comparison

## How I work

### Phase 1: Discovery and Data Collection

- Scan codebase for [relevant files, patterns, and configurations]
- Collect metrics and metadata related to [domain]
- Establish baseline understanding of current state
- Identify data sources and analysis scope
- Validate access to required resources

### Phase 2: Pattern Analysis and Classification

- Apply analytical frameworks to collected data
- Identify trends, anomalies, and patterns
- Classify findings by meaningful criteria
- Quantify impact and severity levels
- Correlate findings with business or technical objectives

### Phase 3: Insight Generation and Context

- Synthesize patterns into actionable insights
- Identify root causes and contributing factors
- Assess impact on [relevant systems or processes]
- Estimate effort and complexity for addressing issues
- Prioritize findings by business value and technical feasibility

### Phase 4: Report Generation and Recommendations

- Create structured report with clear sections
- Provide detailed context for each finding
- Generate specific, actionable recommendations
- Include implementation guidance and next steps
- Establish success metrics and measurement approaches

## Execution Pattern

### Agent Selection Logic

- **explore**: File system scanning, pattern discovery, data collection
- **librarian**: Domain research, best practices analysis, benchmarking
- **oracle**: Strategic analysis, prioritization, recommendation synthesis
- **plan**: Task breakdown, implementation planning, roadmap creation

### Analysis Workflow

1. **Initial Assessment**: Use `explore` to understand scope and resources
2. **Data Collection**: Systematic gathering of relevant information
3. **Pattern Recognition**: Apply domain-specific analytical frameworks
4. **Insight Synthesis**: Combine multiple data sources for comprehensive understanding
5. **Recommendation Generation**: Create actionable improvement plans

### Tool Usage Strategy

- **glob**: Find relevant files and patterns
- **grep**: Search for specific content and configurations
- **read**: Analyze file contents and structures
- **bash**: Execute analysis scripts and commands
- **task**: Delegate specialized analysis to appropriate agents

## Output Format

```
# [Domain] Analysis Report

## Executive Summary
- [High-level findings and key insights]
- [Critical issues requiring immediate attention]
- [Overall assessment and strategic recommendations]

## Detailed Findings

### [Category 1] Analysis
- [Finding 1 with context and impact assessment]
- [Finding 2 with technical details and examples]
- [Pattern analysis and trend identification]

### [Category 2] Analysis
- [Finding 3 with severity and priority classification]
- [Finding 4 with root cause analysis]
- [Correlation with other findings]

## Metrics and Measurements

### Current State
- [Metric 1]: [Current value with context]
- [Metric 2]: [Current value with trends]
- [Baseline measurements and comparisons]

### Impact Assessment
- [Impact area 1]: [Assessment with supporting data]
- [Impact area 2]: [Risk evaluation and mitigation]

## Recommendations

### High Priority (Immediate Action Required)
1. [Recommendation 1] - **Agent**: [best agent] - **Effort**: [estimate]
   - [Implementation details and expected outcomes]
   - [Dependencies and prerequisites]

### Medium Priority (Address within 1-2 weeks)
2. [Recommendation 2] - **Agent**: [best agent] - **Effort**: [estimate]
   - [Implementation approach and success criteria]
   - [Resource requirements and timeline]

### Low Priority (Address in next planning cycle)
3. [Recommendation 3] - **Agent**: [best agent] - **Effort**: [estimate]
   - [Long-term improvement strategy]
   - [Benefits and value proposition]

## Next Steps

1. **Immediate**: [Action items for current sprint]
2. **Short-term**: [Tasks for next 2-4 weeks]
3. **Long-term**: [Strategic initiatives for next quarter]

## Success Metrics

- [Metric 1]: [Target value and measurement approach]
- [Metric 2]: [Success criteria and validation method]
- [Monitoring approach and frequency]
```

## Error Handling

### Common Scenarios

**Missing Resources**:
- Gracefully handle missing files or configurations
- Use alternative data sources when primary sources unavailable
- Provide partial analysis with clear limitations documentation

**Access Issues**:
- Continue with available information when access is restricted
- Document limitations and assumptions clearly
- Suggest alternative approaches for complete analysis

**Insufficient Data**:
- Identify data gaps and their impact on analysis
- Provide recommendations for data collection improvements
- Generate qualitative insights when quantitative analysis isn't possible

### Fallback Strategies

- Generate partial reports when complete analysis fails
- Use heuristics and industry best practices when specific data unavailable
- Provide guidance for manual validation and verification

## Notes and Considerations

### Scope Limitations
- [Document specific boundaries and exclusions]
- [Identify areas requiring specialized expertise]
- [Note any constraints on analysis depth]

### Quality Assurance
- Cross-reference findings with multiple data sources
- Validate assumptions through peer review or additional analysis
- Document confidence levels for each recommendation

### Continuous Improvement
- Establish baseline for future comparison
- Identify monitoring opportunities for ongoing assessment
- Schedule periodic review and update of analysis approach

### Dependencies and Prerequisites
- [List any required tools, configurations, or access]
- [Specify any preliminary work needed before analysis]
- [Identify stakeholders or subject matter experts required]