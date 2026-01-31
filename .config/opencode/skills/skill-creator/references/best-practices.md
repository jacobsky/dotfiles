# OpenCode Skill Best Practices

This guide covers best practices for creating effective, maintainable OpenCode skills.

## Core Principles

### 1. Progressive Disclosure

Keep your SKILL.md focused and lean. Move detailed information to the references/ directory.

**Good Practice**:
```markdown
## What I do
- Analyze code security patterns and vulnerabilities
- Generate prioritized security recommendations
```

**Move to references/**:
- Detailed vulnerability classification systems
- Comprehensive security checklists
- External tool documentation

### 2. Clear Purpose Definition

Each skill should have a single, well-defined purpose.

**Good Example**:
- Clear scope: "Analyze React components for accessibility issues"
- Not: "Analyze code and fix bugs and optimize performance"

### 3. Appropriate Freedom Levels

Match specificity to task fragility:
- **High Freedom**: Creative, flexible tasks (content generation, design)
- **Medium Freedom**: Structured but adaptable (analysis, planning)
- **Low Freedom**: Precise, deterministic tasks (validation, formatting)

## Structure Best Practices

### Directory Organization

Follow the standard structure consistently:

```
skill-name/
├── SKILL.md              # Main skill file (keep lean)
├── scripts/              # Executable automation
├── references/           # Documentation and knowledge
│   ├── domain-knowledge.md
│   └── best-practices.md
└── assets/               # Templates and examples
    ├── templates/        # Reusable content templates
    └── examples/         # Usage examples
```

### File Naming Conventions

- Use lowercase with hyphens: `security-audit`
- Scripts should be executable: `chmod +x scripts/*.sh`
- Reference files should be descriptive: `api-documentation.md`

## Content Best Practices

### Frontmatter Standards

```yaml
---
name: skill-name                    # Required: 1-64 chars, hyphenated
description: Clear, concise description  # Required: 1-1024 chars
license: MIT                        # Recommended
compatibility: opencode             # Recommended
metadata:                           # Optional but useful
  workflow: multi-analysis          # Workflow type
  parallel: true                    # Use parallel when beneficial
  output: structured-list           # Output format
---
```

### Section Structure

#### What I do
- Use bullet points with action verbs
- Focus on capabilities, not implementation details
- Be specific about scope and limitations

**Good**:
```markdown
## What I do
- Analyze React components for WCAG 2.1 AA compliance
- Identify accessibility issues with line-specific references
- Generate prioritized remediation recommendations
```

**Avoid**:
```markdown
## What I do
- I use grep to search for accessibility issues
- I read files and check them against a list
- I create a report with my findings
```

#### When to use me
- Focus on user scenarios and problems solved
- Include specific use cases and situations
- Explain when this skill is the right choice

**Good**:
```markdown
## When to use me
- Before deploying React applications to production
- When conducting accessibility audits
- During code reviews to catch accessibility issues early
- When creating accessibility remediation plans
```

#### How I work
- Use numbered phases for clear progression
- Specify agent selection logic
- Include tool usage patterns
- Mention error handling strategies

## Agent Selection Best Practices

### Choose the Right Agent

**build**: Implementation and code tasks
```markdown
- **build**: Implement accessibility fixes
- **build**: Create test files for accessibility features
```

**plan**: Analysis and task breakdown
```markdown
- **plan**: Analyze components and generate accessibility audit
- **plan**: Create structured remediation plan
```

**explore**: File system and discovery
```markdown
- **explore**: Scan codebase for React components
- **explore**: Find accessibility-related files and configurations
```

**librarian**: Documentation and research
```markdown
- **librarian**: Research WCAG 2.1 AA guidelines
- **librarian**: Analyze accessibility documentation
```

**oracle**: Strategic recommendations
```markdown
- **oracle**: Prioritize accessibility issues by impact
- **oracle**: Generate strategic accessibility improvement plan
```

### Agent Delegation Patterns

**Sequential Pattern**:
```markdown
1. **explore**: Discover React components in codebase
2. **plan**: Analyze components for accessibility issues
3. **oracle**: Prioritize findings by impact and effort
4. **build**: Generate accessibility report and recommendations
```

**Parallel Pattern**:
```markdown
metadata:
  parallel: true
  workflow: multi-analysis

### Parallel Analysis
- **explore**: Analyze codebase structure (parallel)
- **librarian**: Research accessibility guidelines (parallel)
- **oracle**: Synthesize findings and provide recommendations
```

## Context Optimization

### Efficient Content Organization

1. **SKILL.md**: Keep under 500 lines for context efficiency
2. **Progressive Loading**: Load detailed content only when needed
3. **Conditional Details**: Basic info in SKILL.md, advanced in references/

### Resource Management

**Use scripts/ for**:
- Repetitive automation tasks
- Deterministic operations
- Context-heavy processing

**Use references/ for**:
- Domain knowledge and documentation
- External API references
- Detailed specifications

**Use assets/ for**:
- Templates and boilerplate
- Example outputs
- Reusable content patterns

## Error Handling and Resilience

### Graceful Degradation

Always provide fallback mechanisms:

```markdown
## Error Handling

- Handle missing configuration files gracefully
- Continue analysis despite individual component failures
- Provide constructive alternatives when primary methods fail
- Generate partial results when complete analysis isn't possible
```

### Validation Strategies

Include validation at key points:
- Input validation before processing
- Intermediate result validation
- Output validation before completion

## Output Formatting Best Practices

### Structured Output Format

Use consistent, predictable formatting:

```markdown
# [Report Title]

## [Section 1]
- [Finding with context]
- [Impact assessment]
- [Recommendation]

## [Section 2]
[Additional analysis or findings]

## Recommended Next Tasks
1. [High-priority task with agent suggestion]
2. [Medium-priority task with agent suggestion]
3. [Low-priority task with agent suggestion]
```

### Actionable Recommendations

Make recommendations specific and actionable:
- Include file references with line numbers
- Suggest specific agents for implementation
- Provide priority and effort estimates
- Include dependency information

## Testing and Validation

### Skill Testing

Test your skills thoroughly:
1. **Structure Validation**: Use the validation script
2. **Content Testing**: Test with various input scenarios
3. **Agent Integration**: Verify agent delegation works correctly
4. **Error Handling**: Test failure scenarios and recovery
5. **Context Usage**: Monitor context window efficiency

### Validation Script Usage

Always include and use the validation script:
```bash
# Validate during development
./scripts/validate.sh

# Validate before committing
./scripts/validate.sh && git add .
```

## Security and Permissions

### Security Best Practices

- Never expose secrets or sensitive information
- Validate all inputs before processing
- Use appropriate file permissions
- Follow principle of least privilege
- Handle file paths securely

### Permission Integration

Respect the permission system:
- Check tool availability before use
- Handle permission denials gracefully
- Provide alternative approaches when restricted
- Document permission requirements

## Maintenance and Evolution

### Version Control Practices

- Commit skills with clear, descriptive messages
- Tag stable versions appropriately
- Document breaking changes in references/
- Maintain backward compatibility when possible

### Continuous Improvement

- Monitor skill performance and usage patterns
- Collect feedback on effectiveness and usability
- Update based on new OpenCode features and capabilities
- Refactor for better context optimization and efficiency

## Common Pitfalls to Avoid

### Context Bloat
❌ Don't put everything in SKILL.md
✅ Use progressive disclosure with references/

### Over-Engineering
❌ Don't create complex solutions for simple problems
✌ Match complexity to task requirements

### Agent Misuse
❌ Don't use specialized agents for general tasks
✌ Choose agents based on their core strengths

### Poor Error Handling
❌ Don't fail completely when one step fails
✌ Build resilience with fallback mechanisms

### Inconsistent Output
❌ Don't create unpredictable output formats
✌ Use structured, consistent formatting patterns

## Documentation Standards

### Self-Documenting Code

- Use clear function and variable names
- Include inline comments for complex logic
- Provide usage examples in reference files
- Document edge cases and limitations

### User-Friendly Messages

- Provide clear progress indicators
- Explain what's happening at each step
- Offer helpful error messages
- Include next steps and guidance

By following these best practices, you'll create skills that are effective, maintainable, and integrate seamlessly with the OpenCode ecosystem.