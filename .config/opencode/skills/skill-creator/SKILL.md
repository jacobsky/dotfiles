---
name: skill-creator
description: Interactive guided creator for building custom OpenCode skills with validation and templates
license: MIT
compatibility: opencode
metadata:
  workflow: interactive-creation
  parallel: false
  output: structured-skill
---

## What I do

- Guide users through interactive skill creation process step-by-step
- Generate proper directory structure and templates based on skill type
- Validate naming conventions and required fields automatically
- Provide best practice recommendations for skill design and organization
- Create working skill files that integrate seamlessly with OpenCode
- Package skills into .skill format when requested

## When to use me

- Creating a new OpenCode skill from scratch
- Needing guidance on proper skill structure and organization
- Wanting to validate existing skill structure for compliance
- Learning OpenCode skill creation patterns and best practices
- Creating template-based skills for common use cases

## How I work

### Phase 1: Skill Discovery & Requirements

I start by understanding your skill requirements through targeted questions:

- **Purpose & Scope**: What problem will your skill solve?
- **Skill Type Classification**: Analysis, automation, generation, or documentation
- **Agent Selection**: Which agents should handle different phases?
- **Resource Requirements**: Scripts, references, or assets needed
- **Output Specification**: What format should the skill produce?

### Phase 2: Structure Generation

Based on your requirements, I create the foundational structure:

- **Directory Creation**: Proper skill directory with subfolders
- **Naming Validation**: Ensure compliance with `^[a-z0-9]+(-[a-z0-9]+)*$` pattern
- **Frontmatter Generation**: Valid YAML with required fields
- **Template Selection**: Appropriate template based on skill type

### Phase 3: Content Creation Guidance

I walk you through writing effective skill content:

- **Description Writing**: Clear, concise skill descriptions
- **Capability Definition**: "What I do" section with action-oriented bullet points
- **Use Case Specification**: "When to use me" with clear scenarios
- **Execution Design**: "How I work" with phased approach and agent selection
- **Output Formatting**: Structured output patterns and examples

### Phase 4: Validation & Packaging

I ensure your skill is production-ready:

- **Structure Validation**: Directory and file compliance checking
- **Field Validation**: Required frontmatter fields presence
- **Best Practices**: Progressive disclosure and context optimization
- **Package Generation**: .skill format creation if requested

## Execution Pattern

### Interactive Creation Workflow

1. **Initial Assessment**: Ask about skill purpose and requirements
2. **Type Classification**: Determine skill category and optimal structure
3. **Template Selection**: Choose appropriate base template
4. **Structure Creation**: Generate directory structure and files
5. **Content Guidance**: Walk through content creation step-by-step
6. **Validation**: Verify compliance with OpenCode standards
7. **Finalization**: Package and provide usage instructions

### Agent Selection Logic

- **Primary Agent**: Handle interactive guidance and file creation
- **Build Agent**: For script creation and validation logic
- **Plan Agent**: For skill structure analysis and recommendations

### Template Library

**Analysis Skills** (like task-review):
- Codebase analysis and pattern detection
- Historical analysis (commits, changes)
- Gap assessment and recommendations

**Automation Skills**:
- Workflow automation and coordination
- Task automation and scheduling
- Process optimization

**Generation Skills**:
- Code generation and boilerplate creation
- File and documentation generation
- Template-based content creation

**Documentation Skills**:
- API documentation generation
- Technical writing assistance
- Knowledge base creation

## Output Format

I create a complete, working skill structure:

```
skill-name/
├── SKILL.md                    # Main skill file with validated content
├── scripts/                    # Optional automation scripts
│   ├── validate-structure.sh   # Structure validation utilities
│   └── task-specific.sh        # Skill-specific automation
├── references/                 # Optional documentation and resources
│   ├── domain-knowledge.md     # Subject matter expertise
│   └── api-documentation.md    # External API references
└── assets/                     # Optional templates and examples
    ├── templates/              # Reusable content templates
    └── examples/               # Usage examples and demos
```

## Validation & Error Handling

### Naming Convention Compliance
- Validate skill name matches `^[a-z0-9]+(-[a-z0-9]+)*$`
- Ensure directory name matches frontmatter `name` field
- Check name uniqueness in skills directory

### Required Field Validation
- Verify `name` field presence and format
- Ensure `description` field exists and is descriptive
- Validate `license` and `compatibility` fields
- Check metadata structure if present

### Structure Compliance
- Confirm SKILL.md exists in root directory
- Validate directory structure follows best practices
- Ensure proper file organization for resource types

### Graceful Error Recovery
- Continue creation process despite individual validation failures
- Provide constructive suggestions for fixing issues
- Offer alternative approaches when constraints prevent ideal solution

## Best Practices I Enforce

### Progressive Disclosure
- Keep SKILL.md lean and focused (<500 lines)
- Move detailed information to references/ directory
- Use agent delegation for complex subtasks
- Structure content for optimal context usage

### Agent Optimization
- Select appropriate agents for different skill phases
- Use parallel execution when beneficial
- Leverage agent-specific capabilities effectively
- Consider model selection for specialized tasks

### Context Management
- Minimize context window usage through smart organization
- Use conditional details for advanced functionality
- Implement selective resource loading
- Follow established patterns for efficiency

## Usage Examples

### Creating an Analysis Skill
```
User: "I want to create a skill that analyzes my code for security issues"
Me: [Guides through security-focused analysis skill creation]
Result: Complete security-audit skill with vulnerability scanning
```

### Creating an Automation Skill
```
User: "I need a skill that automates my daily commit process"
Me: [Creates workflow automation skill with git integration]
Result: Working commit-automation skill with validation
```

### Creating a Generation Skill
```
User: "I want a skill that generates API documentation from my code"
Me: [Builds documentation generation skill with template system]
Result: Functional doc-generator skill with output formatting
```

## Integration Notes

### Tool Usage
- Uses `question` tool for interactive guidance
- Leverages `read`/`write` for file operations
- Integrates `bash` for validation scripts
- Employs `glob` for pattern matching and discovery

### Permission Compliance
- Respects existing skill permission system
- Follows OpenCode tool usage guidelines
- Maintains security best practices
- Supports permission-based feature access

### Ecosystem Integration
- Creates skills that work with existing agents
- Follows established output formatting patterns
- Maintains compatibility with skill discovery system
- Supports progressive disclosure architecture

## Notes

- Focuses on creating practical, working skills rather than theoretical examples
- Emphasizes validation and compliance with OpenCode standards
- Provides educational guidance throughout the creation process
- Adapts to user experience level (beginner to advanced)
- Continuously improves based on skill creation patterns and user feedback