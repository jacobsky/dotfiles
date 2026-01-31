# Skill Creator

A comprehensive OpenCode skill that guides users through creating their own custom skills with validation, templates, and best practices.

## Purpose

The skill-creator helps you:
- Design and structure new OpenCode skills step-by-step
- Choose appropriate skill types and agent configurations
- Generate proper directory structures and templates
- Validate skills against OpenCode standards
- Apply best practices for progressive disclosure and context optimization

## When to Use

Use this skill when you want to:
- Create a new OpenCode skill from scratch
- Understand proper skill structure and organization
- Validate existing skill compliance
- Learn OpenCode skill creation patterns
- Generate template-based skills for common use cases

## How It Works

### Phase 1: Discovery & Requirements
I'll ask targeted questions about your skill's purpose, scope, and requirements to understand exactly what you want to build.

### Phase 2: Type Classification & Structure
Based on your requirements, I'll classify your skill type (analysis, automation, generation, or documentation) and recommend the optimal structure.

### Phase 3: Template Selection & Customization
I'll help you select appropriate templates and customize them based on your specific needs and preferences.

### Phase 4: Content Creation Guidance
I'll walk you through writing effective skill descriptions, designing execution phases, and defining agent selection logic.

### Phase 5: Validation & Packaging
I'll validate your skill against OpenCode standards and package it for immediate use.

## Included Resources

### Scripts
- `validate-structure.sh`: Comprehensive skill validation and compliance checking
- `create-skeleton.sh`: Automated skill directory structure creation

### Templates
- `analysis-skill.md`: Template for analysis-type skills (like task-review)
- `automation-skill.md`: Template for workflow automation skills
- `generation-skill.md`: Template for content generation skills

### References
- `skill-types.md`: Detailed guide to different skill patterns and structures
- `agent-guide.md`: Comprehensive agent capabilities and selection guide
- `best-practices.md`: OpenCode skill development best practices

### Examples
- Sample skill structures for different types
- Implementation examples with agent selection patterns
- Common patterns and anti-patterns to avoid

## Quick Start

To create a new skill immediately:

```bash
# Use the skeleton creation script
./scripts/create-skeleton.sh my-skill "Brief description of what it does"

# Or use the interactive skill creator
skill({name: "skill-creator"})
```

## Skill Types

### Analysis Skills
Examine code, data, or systems to extract insights and information.
- **Examples**: Security analysis, performance analysis, code review
- **Agents**: explore → plan → oracle → build
- **Output**: Structured reports with findings and recommendations

### Automation Skills
Streamline repetitive tasks and coordinate complex workflows.
- **Examples**: Deployment coordination, CI/CD automation, backup processes
- **Agents**: build → plan → explore (with parallel execution)
- **Output**: Execution summaries with status and artifacts

### Generation Skills
Create new content, code, or artifacts based on specifications.
- **Examples**: Documentation generation, code scaffolding, report generation
- **Agents**: plan → librarian → build → oracle
- **Output**: Generated files with validation and quality reports

### Documentation Skills
Organize and present information in clear, accessible formats.
- **Examples**: API documentation, knowledge base creation, tutorial generation
- **Agents**: librarian → explore → plan → build
- **Output**: Structured documentation with examples and guides

## Validation Features

The included validation script checks:
- ✅ Naming convention compliance (`^[a-z0-9]+(-[a-z0-9]+)*$`)
- ✅ Required frontmatter fields presence
- ✅ YAML syntax validation
- ✅ Directory structure compliance
- ✅ Content section validation
- ✅ File permission checks
- ✅ Size and context optimization recommendations

## Best Practices Enforced

### Progressive Disclosure
- Keep SKILL.md lean (<500 lines)
- Move detailed information to references/ directory
- Use conditional details for advanced functionality

### Agent Selection
- Choose appropriate agents for different phases
- Use parallel execution when beneficial
- Implement proper error handling and recovery

### Context Optimization
- Structure content for efficient context window usage
- Use scripts/ for repetitive operations
- Implement selective resource loading

## Usage Examples

### Creating an Analysis Skill
```
User: "I want to create a skill that analyzes my React components for accessibility issues"

Skill Creator: "I'll help you create an accessibility analysis skill. Let me gather some information..."
[Interactive questioning and guidance]

Result: Complete accessibility-analyzer skill with:
- React component scanning capabilities
- WCAG 2.1 AA compliance checking
- Prioritized remediation recommendations
- Appropriate agent selection (explore → plan → oracle → build)
```

### Creating an Automation Skill
```
User: "I need a skill that automates my deployment process with rollback capabilities"

Skill Creator: "I'll create a deployment automation skill for you..."
[Requirements gathering and template selection]

Result: Working deploy-automation skill with:
- Multi-environment deployment support
- Automated health checks
- Rollback on failure
- Deployment notifications
```

## Contributing

To improve the skill-creator:

1. Create new templates for additional skill types
2. Enhance validation rules and checks
3. Add more example implementations
4. Improve documentation and guides
5. Test with various skill creation scenarios

## License

MIT License - feel free to use and modify for your OpenCode skill development needs.

## Support

For questions or issues with skill creation:
1. Check the `references/` directory for detailed guides
2. Review `assets/examples/` for implementation patterns
3. Use the validation script to check compliance
4. Create issues with specific scenarios you'd like supported

---

This skill helps you create skills that are effective, maintainable, and integrate seamlessly with the OpenCode ecosystem. Happy skill building! 🚀