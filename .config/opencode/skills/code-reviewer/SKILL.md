---
name: code-reviewer
description: Advanced human-like code reviewing skill providing multi-dimensional analysis with security, performance, and maintainability insights
license: MIT
compatibility: opencode
metadata:
  workflow: multi-analysis
  parallel: true
  output: structured-report
---

## What I do

- Perform comprehensive code reviews with human-like contextual feedback
- Analyze security vulnerabilities and potential risks
- Identify performance bottlenecks and optimization opportunities
- Assess code maintainability and architectural consistency
- Provide prioritized, actionable recommendations with explanations
- Generate structured TODOs for specialized agent follow-up

## When to use me

Use this when you need thorough code analysis beyond basic syntax checking, especially for:
- Pre-commit code review of staged changes
- Full repository health assessment
- Security vulnerability scanning
- Performance optimization analysis
- Code quality improvement initiatives

## How I work

### Phase 1: Scope Discovery & Analysis Setup
- Determine analysis scope (staged changes vs full repository)
- Identify project languages, frameworks, and architecture patterns
- Establish baseline code quality metrics and conventions

### Phase 2: Parallel Multi-Dimensional Analysis
- **Security Analysis**: Vulnerability scanning, dependency analysis, data flow examination
- **Performance Analysis**: Bottleneck detection, resource usage patterns, algorithmic complexity
- **Maintainability Analysis**: Code complexity, documentation quality, architectural consistency
- **Integration Analysis**: Dependency impact, API compatibility, breaking changes

### Phase 3: Contextual Synthesis
- Analyze recent commit history for development patterns
- Review existing TODOs and known issues
- Cross-reference findings with project conventions
- Prioritize issues by impact and effort required

### Phase 4: Strategic Recommendations
- Generate prioritized action items with specific agent assignments
- Provide educational explanations for each finding
- Suggest incremental improvements over major refactoring
- Create structured TODOs for follow-up work

## Execution Pattern

### Parallel Analysis Configuration

1. **explore agent**: Codebase structure, git analysis, pattern discovery
2. **security agent**: Vulnerability scanning, risk assessment, security best practices
3. **perf agent**: Performance profiling, bottleneck analysis, optimization opportunities
4. **librarian agent**: Documentation review, best practices research, convention analysis
5. **oracle agent**: Strategic prioritization, impact assessment, overall recommendations

### Agent Selection Logic

- **explore**: File system operations, git history, codebase navigation
- **security**: Security vulnerability detection, dependency analysis, data flow analysis
- **perf**: Performance profiling, algorithmic complexity, resource usage patterns
- **librarian**: Documentation quality, best practices, framework conventions
- **oracle**: Strategic prioritization, impact assessment, recommendation synthesis
- **plan**: Task breakdown, structured TODO creation, action item organization

## Output Format

```markdown
# Code Review Analysis Report

## Executive Summary
**Overall Assessment**: [PASS/NEEDS WORK/FAIL]
- Critical Issues: [count]
- High Priority: [count] 
- Medium Priority: [count]
- Low Priority: [count]
- Estimated Effort: [time estimate]

## Security Analysis
### 🔴 Critical Security Issues
[Critical security findings with file:line references]

### 🟡 Security Concerns  
[Medium security issues and recommendations]

## Performance Analysis
### ⚡ Performance Bottlenecks
[Critical performance issues with impact assessment]

### 🐌 Optimization Opportunities
[Performance improvement suggestions]

## Code Quality Assessment
### 🏗️ Architectural Issues
[Structural and design problems]

### 📚 Documentation & Maintainability
[Documentation gaps and maintainability concerns]

### 🎯 Best Practices
[Convention violations and improvements]

## Integration Impact Analysis
### 🔗 Dependency Changes
[Impact on existing dependencies and integrations]

### 💥 Breaking Changes
[API compatibility issues and migration needs]

## Prioritized Action Items
1. **[CRITICAL]** [Description] - file:line - agent:security
2. **[HIGH]** [Description] - file:line - agent:perf
3. **[HIGH]** [Description] - file:line - agent:plan
4. **[MEDIUM]** [Description] - file:line - agent:explore
5. **[LOW]** [Description] - file:line - agent:librarian

## Strategic Recommendations
### Immediate Actions
[Critical items to address before commit]

### Short-term Improvements  
[High-impact improvements for next iteration]

### Long-term Enhancements
[Strategic improvements for future consideration]

## Learning Opportunities
### Educational Notes
[Explanations of why issues matter and best practices]

### Alternative Approaches
[Multiple solution options when applicable]
```

## Analysis Methodology

### Security Analysis Framework
- Input validation and sanitization review
- Authentication and authorization patterns
- Sensitive data exposure risks
- Dependency vulnerability scanning
- Cryptographic implementation review
- Error handling information leakage

### Performance Analysis Framework
- Algorithmic complexity assessment (Big O analysis)
- Database query optimization opportunities
- Memory usage and resource management
- I/O operation efficiency
- Concurrency and threading issues
- Caching strategy evaluation

### Maintainability Analysis Framework
- Code complexity metrics (cyclomatic complexity)
- Naming conventions and readability
- Function and class size analysis
- Duplication and redundancy detection
- Documentation completeness and accuracy
- Testing coverage and quality

### Integration Analysis Framework
- API compatibility and versioning
- Dependency management health
- Configuration and environment impact
- Migration requirements assessment
- Backward compatibility verification

## Error Handling & Resilience

### Graceful Degradation
- Continue analysis if individual agents encounter issues
- Provide partial results with clear limitations noted
- Fallback to basic analysis when advanced features unavailable
- Maintain usability across diverse codebase types

### Edge Cases
- Handle empty or minimal codebases
- Support multiple programming languages simultaneously
- Work with incomplete git histories
- Adapt to various project structures and conventions

### Quality Assurance
- Validate findings before inclusion in report
- Cross-check recommendations against project context
- Avoid false positives through context verification
- Provide confidence levels for uncertain findings

## Integration Points

### Command Integration
- Enhances existing `/codereview` command with `--skill=code-reviewer`
- Maintains backward compatibility with current workflow
- Supports both staged changes and full repository analysis modes

### TODO System Integration
- Creates structured TODOs with appropriate agent assignments
- Links TODOs to specific files and line numbers
- Provides context and effort estimates for each TODO
- Supports bulk TODO creation for related issues

### Configuration Integration
- Respects existing `opencode.json` settings
- Supports project-specific review configurations
- Adapts analysis depth based on project size and complexity
- Integrates with LSP settings for language-specific analysis

## Success Criteria

### Technical Excellence
- All critical security and performance issues identified
- Maintainability problems detected with specific solutions
- Integration impact accurately assessed
- Recommendations are actionable and prioritized

### User Experience
- Reports are clear, structured, and easy to navigate
- Explanations provide educational value
- Tone is constructive and supportive
- Analysis completes in reasonable time

### Integration Success
- Seamlessly works with existing opencode commands
- Properly integrates with TODO system
- Maintains compatibility with various project types
- Provides consistent, reliable results across sessions