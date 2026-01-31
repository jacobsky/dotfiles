---
name: generation-skill-template
description: Template for creating generation skills that produce new content, code, or artifacts from specifications
license: MIT
compatibility: opencode
metadata:
  workflow: template-driven-generation
  parallel: false
  output: structured-artifacts
---

## What I do

- Generate [type of content] from [input specifications and parameters]
- Apply [templates and patterns] consistently across outputs
- Customize output based on [requirements and configuration]
- Validate generated content for [quality and compliance]
- Produce [structured formats] suitable for [target systems or audiences]

## When to use me

Use this skill when you need to:
- Create [type of content] from scratch or specifications
- Apply consistent [templates and patterns] across multiple outputs
- Automate [content creation] workflows with customization
- Generate [documentation or code] based on standardized formats
- Produce [artifacts] that meet specific quality standards

## How I work

### Phase 1: Input Analysis and Validation

- Analyze input specifications and requirements
- Identify appropriate templates and generation patterns
- Validate inputs against defined schemas and constraints
- Extract configuration parameters and customization options
- Establish output format and structure requirements

### Phase 2: Template Selection and Customization

- Select appropriate base templates for the content type
- Customize templates based on specific requirements
- Apply parameter substitutions and conditional logic
- Handle edge cases and special formatting requirements
- Validate template structure and completeness

### Phase 3: Content Generation

- Apply selected templates to input specifications
- Generate base content structure with placeholders
- Fill placeholders with actual data and content
- Apply formatting rules and style guidelines
- Include appropriate metadata and documentation

### Phase 4: Refinement and Validation

- Refine generated content for quality and readability
- Validate against established standards and requirements
- Perform consistency checks across related outputs
- Add final formatting and presentation elements
- Generate quality reports and validation summaries

## Template System

### Template Structure

```markdown
# [Template Name]

## Template Variables
{{variable_name}} - [description]
{{optional_variable}} - [description, default: [default_value]]

## Template Content

[Template with variable placeholders]

## Conditional Sections
{{#if condition}}
[Content only included when condition is true]
{{/if}}

## Loops and Iterations
{{#each collection}}
[Repeated content for each item]
- Item: {{this.name}}
- Value: {{this.value}}
{{/each}}
```

### Template Categories

**Code Generation Templates**:
- Class and function scaffolding
- API endpoint templates
- Configuration file templates
- Test file templates

**Documentation Templates**:
- API documentation structure
- Technical report format
- User guide templates
- README file structures

**Configuration Templates**:
- Deployment configurations
- Build system files
- CI/CD pipeline definitions
- Environment configuration files

## Execution Pattern

### Agent Selection Logic

- **build**: Primary content generation and file creation
- **plan**: Template analysis and output structure design
- **librarian**: Template library management and best practices
- **oracle**: Quality validation and optimization recommendations

### Generation Workflow

```markdown
### Template-Driven Generation
1. **plan**: Analyze requirements and select appropriate templates
2. **librarian**: Validate templates against best practices
3. **build**: Generate content using selected templates
4. **oracle**: Validate quality and suggest improvements
```

### Customization Strategies

**Parameter-Based Customization**:
- Use configuration parameters to control output characteristics
- Implement conditional content based on user preferences
- Support multiple output formats from single input

**Style Guide Integration**:
- Apply consistent formatting and style rules
- Support brand-specific customization
- Handle locale and internationalization requirements

## Output Format Specifications

### Standard Output Structure

```
# Generated Content Report

## Generation Summary
- **Template Used**: [template name and version]
- **Input Source**: [input specification or file]
- **Generation Time**: [timestamp]
- **Output Format**: [format type and version]
- **Validation Status**: [passed/warnings/failed]

## Generated Artifacts

### Primary Output
- **File**: [filename]
- **Format**: [file format]
- **Size**: [file size]
- **Location**: [output path]

### Supporting Files
- **File**: [filename] - [purpose]
- **File**: [filename] - [purpose]

### Quality Metrics
- **Completeness**: [percentage]
- **Consistency Score**: [score/max]
- **Standard Compliance**: [yes/no with details]
- **Customization Applied**: [list of customizations]

## Content Preview

[Preview of generated content with key sections highlighted]

## Validation Results

### Structure Validation
✅ Required sections present
✅ Template variables correctly substituted
⚠️ Optional sections missing (if applicable)

### Content Validation
✅ Content meets quality standards
✅ Formatting follows style guidelines
✅ Links and references are valid

### Compliance Validation
✅ Meets [relevant standards]
✅ Follows [established conventions]
⚠️ Custom deviations noted and approved
```

### Content Type Specific Formats

**Code Generation Output**:
```markdown
# Generated Code Module

## Module Information
- **Language**: [programming language]
- **Framework**: [framework if applicable]
- **Version**: [template version]
- **Dependencies**: [list of dependencies]

## Generated Files
### [filename1]
[File content preview]

### [filename2]
[File content preview]

## Integration Notes
- [How to integrate generated code]
- [Required setup or configuration]
- [Testing and validation steps]
```

**Documentation Generation Output**:
```markdown
# Generated Documentation

## Document Information
- **Type**: [document type - API docs, user guide, etc.]
- **Format**: [output format - Markdown, HTML, PDF]
- **Target Audience**: [intended users]
- **Version**: [document version]

## Document Structure
- [Table of contents or section overview]

## Content Sections
[Key sections and their content]

## Quality Assurance
- **Review Checklist**: [items verified]
- **Accessibility**: [compliance status]
- **Localization**: [language and locale information]
```

## Quality Assurance and Validation

### Validation Framework

```yaml
validation_rules:
  structural:
    - required_sections_present
    - template_variables_substituted
    - valid_markup_syntax
    
  content:
    - content_quality_standards
    - style_guide_compliance
    - accessibility_standards
    
  functional:
    - link_validation
    - code_syntax_check
    - integration_compatibility
```

### Quality Metrics

**Completeness**: Percentage of required content successfully generated
**Consistency**: Uniformity across multiple generated items
**Accuracy**: Correctness of template application and data substitution
**Compliance**: Adherence to standards and style guidelines
**Usability**: Quality and accessibility of generated content

### Error Handling

**Template Errors**:
- Missing or invalid template variables
- Malformed template syntax
- Template version incompatibilities

**Content Errors**:
- Invalid input data or specifications
- Content exceeding length or complexity limits
- Formatting or style violations

**System Errors**:
- File system permission issues
- Resource exhaustion or timeouts
- External service unavailability

## Configuration and Customization

### Template Configuration

```yaml
# template-config.yml
templates:
  base_path: "./templates"
  version: "1.0.0"
  
generation:
  output_format: "markdown"
  encoding: "utf-8"
  line_ending: "unix"
  
customization:
  default_style: "corporate"
  accessibility_level: "AA"
  internationalization:
    default_locale: "en-US"
    support_locales: ["en-US", "es-ES", "fr-FR"]
    
validation:
  strict_mode: false
  warn_on_customization: true
  auto_fix_minor_issues: true
```

### Style Guide Integration

```css
/* style-guide.css */
/* Example style definitions for generated content */

.heading {
  font-weight: bold;
  margin-top: 1.5em;
}

.code-block {
  font-family: monospace;
  background-color: #f5f5f5;
  padding: 1em;
}

.warning {
  color: #d63384;
  font-weight: bold;
}
```

### Customization Parameters

```json
{
  "generation_params": {
    "output_format": "markdown",
    "include_toc": true,
    "include_metadata": true,
    "compression_level": "normal"
  },
  "style_params": {
    "heading_style": "atx",
    "code_block_style": "fenced",
    "list_style": "hyphen"
  },
  "content_params": {
    "max_section_depth": 3,
    "include_examples": true,
    "technical_level": "intermediate"
  }
}
```

## Integration and Extension

### Template Development

**Creating New Templates**:
1. Define template structure with variables and conditionals
2. Implement validation rules for template syntax
3. Create test cases with expected outputs
4. Document template purpose and usage patterns
5. Add to template registry with metadata

**Template Versioning**:
- Semantic versioning for template releases
- Backward compatibility considerations
- Migration guides for breaking changes
- Automated testing across template versions

### Plugin Architecture

```python
# Example plugin system for custom generators
class GeneratorPlugin:
    def __init__(self, name, version):
        self.name = name
        self.version = version
        
    def generate(self, template, data, config):
        """Custom generation logic"""
        pass
        
    def validate(self, output):
        """Custom validation logic"""
        pass
        
    def get_metadata(self):
        """Plugin information and capabilities"""
        return {
            "name": self.name,
            "version": self.version,
            "supported_formats": ["markdown", "html"],
            "dependencies": []
        }
```

### External System Integration

**CMS Integration**:
- Direct publishing to content management systems
- Metadata synchronization and taxonomy mapping
- Workflow integration with approval processes

**API Integration**:
- Template storage and versioning systems
- Content distribution and syndication
- Analytics and usage tracking

**Build System Integration**:
- Integration with CI/CD pipelines
- Automated testing and validation
- Deployment automation for generated content

## Performance and Scalability

### Optimization Strategies

**Template Caching**:
- Cache compiled templates for repeated use
- Implement template version management
- Use efficient template rendering engines

**Content Generation Optimization**:
- Batch processing for multiple outputs
- Parallel generation when possible
- Incremental generation for large content sets

**Resource Management**:
- Memory optimization for large templates
- Disk space management for generated content
- Network optimization for distributed generation

### Monitoring and Analytics

**Performance Metrics**:
- Template rendering time
- Content generation throughput
- Resource utilization patterns
- Error rates and types

**Usage Analytics**:
- Template usage patterns
- Popular customization options
- Quality metrics over time
- User satisfaction feedback

## Security Considerations

### Input Validation

- Sanitize all user inputs and parameters
- Validate template syntax and structure
- Prevent code injection in template variables
- Implement content security policies

### Output Security

- Ensure generated content doesn't expose sensitive information
- Validate output against security standards
- Implement proper access controls for generated files
- Audit trail for generation activities

### Template Security

- Validate template sources and integrity
- Implement template signing and verification
- Control access to sensitive templates
- Regular security audits of template system

## Best Practices

### Template Design

- Keep templates simple and maintainable
- Use clear, descriptive variable names
- Document template purpose and usage
- Implement comprehensive testing

### Content Quality

- Follow established style guides consistently
- Ensure accessibility standards are met
- Validate all generated content
- Provide clear documentation for customization

### System Integration

- Design for extensibility and flexibility
- Implement proper error handling and recovery
- Use appropriate caching strategies
- Monitor performance and quality metrics

### User Experience

- Provide clear feedback during generation
- Offer helpful error messages and guidance
- Support incremental and iterative generation
- Include preview and validation capabilities