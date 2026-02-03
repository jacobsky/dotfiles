# Documentation Quality Standards

## Overview

This document defines the quality standards and criteria used by the `tech-doc-reviewer` skill to assess technical documentation quality across OpenCode projects and skills.

## Quality Dimensions

### 1. Content Quality

#### Technical Accuracy
- **Critical**: Code examples must compile and run correctly
- **Critical**: Technical specifications must be accurate and current
- **High**: API documentation must match actual implementation
- **Medium**: Version compatibility information must be correct

#### Completeness
- **Critical**: Essential installation and setup instructions present
- **High**: Basic usage examples and getting started guide included
- **Medium**: Error handling and troubleshooting information provided
- **Low**: Advanced usage scenarios and edge cases covered

#### Clarity and Accessibility
- **High**: Language appropriate for target audience
- **Medium**: Technical terms defined on first use
- **Low**: Consistent terminology throughout documentation

### 2. Structural Quality

#### Organization
- **High**: Logical flow from basic to advanced concepts
- **Medium**: Clear navigation and table of contents
- **Low**: Proper section hierarchy and cross-references

#### Formatting
- **Medium**: Consistent heading styles and formatting
- **Low**: Proper use of code blocks, lists, and emphasis
- **Low**: Consistent link formatting and styling

#### Navigation
- **Medium**: Internal links function correctly
- **Low**: External references are relevant and accessible
- **Low**: Anchor links point to correct sections

### 3. Technical Documentation Standards

#### OpenCode Skill Documentation
- **Critical**: SKILL.md follows required format with proper frontmatter
- **High**: Required sections present ("What I do", "When to use me", "How I work")
- **Medium**: Agent flow and integration details documented
- **Low**: Examples and usage patterns included

#### API Documentation
- **Critical**: All public endpoints documented with parameters
- **High**: Request/response examples provided
- **Medium**: Error codes and status explained
- **Low**: Rate limiting and authentication details included

#### Code Documentation
- **High**: Public functions and classes documented
- **Medium**: Complex algorithms explained
- **Low**: Comment coverage for critical code paths

### 4. Maintained Quality

#### Currency
- **Critical**: Documentation matches current codebase version
- **High**: Version-specific information clearly marked
- **Medium**: Last updated date present and recent
- **Low**: Future deprecation warnings when appropriate

#### Version Control Integration
- **Medium**: Documentation changes tracked with code changes
- **Low**: Release notes linked to documentation updates
- **Low**: Branch-specific documentation maintained

## Quality Scoring System

### Score Calculation

**Base Score**: 100 points

**Deductions by Severity**:
- Critical: -10 points per issue
- High: -5 points per issue
- Medium: -2 points per issue
- Low: -1 point per issue

### Quality Levels

| Score Range | Quality Level | Description |
|-------------|---------------|-------------|
| 90-100 | Excellent | Outstanding documentation quality |
| 80-89 | Good | High quality with minor improvements possible |
| 70-79 | Fair | Acceptable quality with some notable issues |
| 60-69 | Needs Improvement | Significant issues require attention |
| Below 60 | Poor | Critical issues need immediate resolution |

### Weighting Factors

Different documentation types have varying importance weights:

- **README Files**: Weight 1.5x (critical for first impressions)
- **API Documentation**: Weight 1.3x (essential for developers)
- **Skill Documentation**: Weight 1.4x (core to OpenCode ecosystem)
- **General Guides**: Weight 1.0x (standard importance)
- **Code Comments**: Weight 0.8x (supporting documentation)

## OpenCode-Specific Standards

### Skill Documentation Requirements

Every OpenCode skill must include:

1. **Required Frontmatter**:
   ```yaml
   ---
   name: skill-name
   description: Brief description (1-1024 chars)
   license: MIT
   compatibility: opencode
   metadata:
     workflow: analysis|automation|generation|documentation
     parallel: true|false
     output: output-format
   ---
   ```

2. **Required Sections**:
   - `## What I do` - Capabilities list with bullet points
   - `## When to use me` - Clear use cases and scenarios
   - `## How I work` - Detailed execution phases

3. **Naming Convention**:
   - Pattern: `^[a-z0-9]+(-[a-z0-9]+)*$`
   - 1-64 characters, lowercase alphanumeric with hyphens

### Agent Integration Standards

Documentation should reference agent workflows:

- **Primary Agents**: Can dispatch subagents (`document.md`)
- **Subagents**: Specialized execution (`technical-writer.md`)
- **Agent Flow**: `explore → librarian → oracle → plan` pattern
- **Tool Access**: Clearly specify which tools are available

### Integration Patterns

#### With document.md Agent
- Coordinate documentation workflows
- Track review tasks and progress
- Dispatch appropriate specialized agents
- Maintain awareness of documentation state

#### With technical-writer Agent
- Provide review feedback for created content
- Complement creation with quality assessment
- Establish improvement feedback loops

## Validation Criteria

### Automated Checks

**Content Validation**:
- File existence and accessibility
- Link validation (internal and external)
- Code example syntax checking
- Format compliance

**Structure Validation**:
- Heading hierarchy consistency
- Section completeness
- Navigation functionality
- Cross-reference accuracy

**Quality Validation**:
- Readability scoring
- Jargon density assessment
- Sentence variety analysis
- Technical term usage

### Manual Review Criteria

**User Experience**:
- Information findability
- Learning curve assessment
- Task completion efficiency
- Error recovery guidance

**Technical Accuracy**:
- Code example verification
- API specification matching
- Version compatibility checking
- Dependency accuracy

**Maintainability**:
- Update frequency assessment
- Version control integration
- Change impact analysis
- Maintenance planning

## Best Practices

### Writing Style

1. **Active Voice**: Use active voice for clarity and directness
2. **Consistent Terminology**: Use the same terms throughout documentation
3. **Progressive Disclosure**: Start simple, add complexity gradually
4. **Concrete Examples**: Provide specific, testable examples
5. **Error Scenarios**: Include troubleshooting and error handling

### Structure Guidelines

1. **Logical Flow**: Organize from basic to advanced concepts
2. **Clear Navigation**: Use consistent heading hierarchy
3. **Cross-References**: Link related concepts appropriately
4. **Visual Hierarchy**: Use formatting to guide attention
5. **Search Optimization**: Include relevant keywords and phrases

### Technical Documentation

1. **Code Examples**: All examples must be tested and functional
2. **API Documentation**: Complete with all parameters and responses
3. **Version Information**: Clearly indicate compatibility and requirements
4. **Prerequisites**: List all dependencies and setup requirements
5. **Troubleshooting**: Include common issues and solutions

## Continuous Improvement

### Quality Metrics

Track the following metrics over time:

- **Quality Score Trend**: Monitor overall score changes
- **Issue Resolution Time**: Track how quickly issues are addressed
- **User Feedback**: Collect and analyze user experiences
- **Coverage Metrics**: Monitor documentation completeness

### Review Processes

1. **Automated Testing**: Regular validation script execution
2. **Peer Review**: Manual review by team members
3. **User Testing**: Feedback from actual users
4. **Periodic Audits**: Comprehensive quality assessments

### Improvement Planning

1. **Immediate Actions**: Address critical and high-severity issues
2. **Quality Enhancements**: Implement medium-priority improvements
3. **Strategic Initiatives**: Long-term documentation strategy
4. **Process Optimization**: Improve review and update workflows

---

*These standards are maintained and updated by the OpenCode documentation team to ensure consistent, high-quality technical documentation across the ecosystem.*