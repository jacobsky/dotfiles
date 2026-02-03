---
name: tech-doc-reviewer
description: Comprehensive technical documentation quality assessment and review skill for analyzing documentation completeness, accuracy, and effectiveness across all documentation types
license: MIT
compatibility: opencode
metadata:
  workflow: analysis
  parallel: true
  output: structured-report
---

## What I do

- **Comprehensive Documentation Analysis**: Review all documentation types including README files, API docs, code comments, guides, and tutorials
- **Quality Assessment**: Evaluate content accuracy, structural organization, technical correctness, and maintainability
- **Multi-Format Support**: Analyze Markdown, reStructuredText, plain text, and code documentation formats
- **Automated Validation**: Perform link checking, formatting validation, readability analysis, and standards compliance checking
- **Structured Reporting**: Generate detailed review reports with prioritized improvement recommendations
- **Integration Coordination**: Work with document.md agent and technical-writer for comprehensive documentation lifecycle management

## When to use me

- When you need to assess the quality and completeness of project documentation
- Before releases to ensure documentation meets quality standards
- When documentation appears outdated, incomplete, or inconsistent
- For documentation health checks and quality monitoring
- When onboarding new team members and need to evaluate documentation adequacy
- Before documentation restructuring or improvement initiatives
- When integrating with existing documentation workflows and agent coordination

## How I work

### Phase 1: Documentation Discovery
I use the `explore` agent to comprehensively scan your repository for all documentation types:
- **File Pattern Detection**: README*, *.md, *.rst, CHANGELOG*, CONTRIBUTING*, LICENSE*
- **Code Documentation**: Identify docstrings, inline comments, and code documentation patterns
- **API Documentation**: Locate OpenAPI specs, API docs, and interface documentation
- **Structure Mapping**: Analyze documentation organization and cross-reference relationships

### Phase 2: Quality Analysis
I leverage the `librarian` agent to perform detailed quality assessments:
- **Content Quality**: Verify technical accuracy, completeness, clarity, and audience appropriateness
- **Structure Evaluation**: Assess organization, formatting consistency, navigation, and cross-references
- **Technical Validation**: Check code examples, API documentation completeness, and dependency information
- **Standards Compliance**: Validate against OpenCode documentation conventions and best practices

### Phase 3: Issue Prioritization
I use the `oracle` agent to analyze findings and prioritize recommendations:
- **Severity Classification**: Critical, High, Medium, Low severity categorization
- **Impact Assessment**: Evaluate user experience and development workflow impact
- **Effort Estimation**: Provide time and resource estimates for improvements
- **Strategic Planning**: Offer long-term documentation quality strategy recommendations

### Phase 4: Action Planning
I employ the `plan` agent to generate structured outputs:
- **Comprehensive Review Report**: Detailed analysis with quality scores and metrics
- **Prioritized Action Items**: Specific improvement tasks with agent assignments
- **Quality Metrics**: Benchmark data and trend tracking recommendations
- **Integration Recommendations**: Workflow integration with existing OpenCode tools

### Validation Integration
I combine custom OpenCode-specific checks with external tool integration:
- **Custom Checks**: Documentation completeness, agent workflow integration, skill system consistency
- **External Tools**: Markdown linting, link validation, readability analysis, spell checking
- **Hybrid Approach**: Best-of-both-worlds validation for comprehensive quality assurance

### Output Format
I generate structured reports following the established code-reviewer pattern:
- **Executive Summary**: Overall quality assessment and critical issues
- **Content Analysis**: Technical accuracy and completeness evaluation
- **Structure Review**: Organization and formatting assessment
- **Technical Documentation**: Code docs and API documentation verification
- **Maintainability**: Currency and version control analysis
- **Action Items**: Prioritized improvements with assignments

## Key Features

### Multi-Dimensional Analysis
- Content quality, structural integrity, technical accuracy, maintainability assessment
- Audience-appropriate evaluation for developers, users, and contributors
- Integration with existing OpenCode agent workflows and coordination patterns

### Automated Validation
- Comprehensive link checking (internal and external references)
- Markdown formatting and style consistency validation
- Readability scoring and accessibility assessment
- Code example verification and testing recommendations

### Quality Metrics
- Documentation coverage scoring against project complexity
- Consistency measurement across documentation types
- Currency tracking and maintenance recommendation
- Benchmark comparison with OpenCode standards

### Workflow Integration
- Seamless coordination with document.md agent
- Complementary to technical-writer creation capabilities
- Support for staged documentation reviews and repository-wide health checks
- Integration potential with existing `/docs` commands

## Usage Examples

**Basic Documentation Review**:
`/skill tech-doc-reviewer --scope=full --path=./`

**Targeted Analysis**:
`/skill tech-doc-reviewer --include="*.md" --exclude=node_modules`

**API Documentation Focus**:
`/skill tech-doc-reviewer --focus=api-docs --severity=high,critical`

**Integration with Workflow**:
The skill automatically coordinates with document.md agent for comprehensive documentation lifecycle management, providing feedback loops to technical-writer for continuous improvement.

## Expected Outputs

- **Structured Review Report**: Comprehensive quality assessment with scores and recommendations
- **Actionable TODOs**: Prioritized improvement tasks with specific agent assignments
- **Quality Metrics Dashboard**: Benchmark data and trend analysis
- **Integration Plan**: Recommendations for workflow improvements and tool integration

The skill follows OpenCode skill validation standards and integrates seamlessly with the existing skill ecosystem, providing valuable documentation quality assurance capabilities for projects of all sizes and complexity levels.