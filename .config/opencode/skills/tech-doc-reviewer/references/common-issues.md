# Common Documentation Issues

## Overview

This document catalogs the most frequently encountered documentation issues found during technical documentation reviews. It serves as a reference for the `tech-doc-reviewer` skill and helps maintain consistent issue identification and resolution patterns.

## Critical Issues

### 1. Missing Essential Files

#### README Files Absent or Incomplete
**Frequency**: High  
**Impact**: Critical  
**Examples**:
```
❌ No README.md in repository root
❌ README contains only project name
❌ README lacks installation instructions
❌ Missing basic usage examples
```

**Resolution**:
- Create comprehensive README with project overview
- Include installation/setup instructions
- Add basic usage examples
- Provide getting started guide

#### Missing API Documentation
**Frequency**: High  
**Impact**: Critical  
**Examples**:
```
❌ No API documentation for public endpoints
❌ Missing request/response schemas
❌ No authentication method documentation
❌ Error codes not documented
```

**Resolution**:
- Document all public API endpoints
- Include request/response examples
- Explain authentication and authorization
- Document error codes and handling

### 2. Broken Code Examples

#### Non-Functional Code Snippets
**Frequency**: High  
**Impact**: Critical  
**Examples**:
```
❌ Python code with syntax errors
❌ JavaScript using undefined functions
❌ Missing import statements
❌ Incorrect API endpoint URLs
❌ Outdated library function calls
```

**Resolution**:
- Test all code examples before documentation
- Include import statements and dependencies
- Use current library versions and syntax
- Validate API endpoints and parameters

#### Incomplete Implementation Examples
**Frequency**: Medium  
**Impact**: Critical  
**Examples**:
```
❌ Missing error handling in code
❌ Incomplete class definitions
❌ Missing configuration steps
❌ Unclear data flow in examples
```

**Resolution**:
- Include complete, working examples
- Add proper error handling
- Show full context and setup
- Explain data flow and dependencies

### 3. Technical Inaccuracies

#### Version Compatibility Issues
**Frequency**: High  
**Impact**: Critical  
**Examples**:
```
❌ Documentation for deprecated API version
❌ Outdated dependency versions
❌ Incorrect system requirements
❌ Version-specific features not marked
```

**Resolution**:
- Clearly indicate supported versions
- Update documentation with current versions
- Mark version-specific features
- Document migration paths

#### Incorrect Configuration Instructions
**Frequency**: Medium  
**Impact**: Critical  
**Examples**:
```
❌ Wrong configuration file paths
❌ Invalid parameter names
❌ Missing required configuration options
❌ Incorrect default values
```

**Resolution**:
- Verify all configuration instructions
- Test configuration examples
- Include all required options
- Provide valid default values

## High Priority Issues

### 1. Navigation and Structure Problems

#### Poor Organization
**Frequency**: High  
**Impact**: High  
**Examples**:
```
❌ No logical flow from basic to advanced
❌ Important information buried deep in docs
❌ Inconsistent section organization
❌ Missing table of contents
```

**Resolution**:
- Reorganize content with logical progression
- Place important information prominently
- Use consistent organizational patterns
- Add comprehensive navigation

#### Broken Internal Links
**Frequency**: High  
**Impact**: High  
**Examples**:
```
❌ Links to non-existent sections
❌ Anchor links to moved content
❌ Incorrect relative paths
❌ Case-sensitive link issues
```

**Resolution**:
- Validate all internal links
- Use consistent link formats
- Update links when content moves
- Test link functionality regularly

### 2. Missing Key Information

#### Insufficient Usage Examples
**Frequency**: High  
**Impact**: High  
**Examples**:
```
❌ Only basic "hello world" examples
❌ No real-world use case demonstrations
❌ Missing edge case examples
❌ No configuration examples
```

**Resolution**:
- Provide comprehensive usage examples
- Include real-world scenarios
- Add edge case and error handling examples
- Show various configuration options

#### No Troubleshooting Information
**Frequency**: Medium  
**Impact**: High  
**Examples**:
```
❌ No common error solutions
❌ Missing debugging guidance
❌ No troubleshooting section
❌ No support resource references
```

**Resolution**:
- Add troubleshooting section
- Document common errors and solutions
- Provide debugging guidance
- Reference support resources

### 3. External Link Issues

#### Broken External References
**Frequency**: Medium  
**Impact**: High  
**Examples**:
```
❌ Dead links to external resources
❌ Moved or updated URLs not updated
❌ Paywall content without warning
❌ Link rot over time
```

**Resolution**:
- Regularly validate external links
- Provide alternative resources
- Warn about access restrictions
- Keep external references current

## Medium Priority Issues

### 1. Readability and Clarity Problems

#### Complex Technical Language
**Frequency**: High  
**Impact**: Medium  
**Examples**:
```
❌ Undefined technical jargon
❌ Complex sentences with multiple clauses
❌ Inconsistent terminology
❌ Missing explanations for acronyms
```

**Resolution**:
- Define technical terms on first use
- Simplify complex sentences
- Use consistent terminology
- Explain acronyms and abbreviations

#### Poor Readability Score
**Frequency**: Medium  
**Impact**: Medium  
**Examples**:
```
❌ Long paragraphs (>10 sentences)
❌ Average sentence length >25 words
❌ Low variety in sentence structure
❌ Dense text without white space
```

**Resolution**:
- Break up long paragraphs
- Reduce average sentence length
- Vary sentence structure and length
- Use white space effectively

### 2. Formatting and Style Issues

#### Inconsistent Formatting
**Frequency**: High  
**Impact**: Medium  
**Examples**:
```
❌ Inconsistent heading styles
❌ Mixed list formats
❌ Inconsistent code block formatting
❌ Random emphasis usage
```

**Resolution**:
- Apply consistent style guide
- Use standardized formatting patterns
- Maintain consistent code block styling
- Use emphasis judiciously and consistently

#### Missing Visual Elements
**Frequency**: Medium  
**Impact**: Medium  
**Examples**:
```
❌ No diagrams for complex concepts
❌ Missing screenshots for UI documentation
❌ No architectural diagrams
❌ Text-only explanations for visual topics
```

**Resolution**:
- Add diagrams for complex concepts
- Include relevant screenshots
- Create architectural diagrams
- Use visual elements to enhance understanding

### 3. Coverage Gaps

#### Incomplete Feature Documentation
**Frequency**: Medium  
**Impact**: Medium  
**Examples**:
```
❌ Advanced features not documented
❌ Missing configuration options
❌ Undocumented API parameters
❌ No integration examples
```

**Resolution**:
- Document all features comprehensively
- Include all configuration options
- Document complete API surface
- Provide integration examples

#### Missing Context and Background
**Frequency**: Medium  
**Impact**: Medium  
**Examples**:
```
❌ No problem context provided
❌ Missing design rationale
❌ No comparison with alternatives
❌ No historical context for changes
```

**Resolution**:
- Provide problem context and motivation
- Explain design decisions and rationale
- Compare with alternatives when relevant
- Include historical context for changes

## Low Priority Issues

### 1. Style and Polish

#### Minor Language Issues
**Frequency**: Medium  
**Impact**: Low  
**Examples**:
```
❌ Occasional grammatical errors
❌ Inconsistent punctuation
❌ Minor spelling mistakes
❌ Awkward phrasing
```

**Resolution**:
- Perform thorough proofreading
- Use grammar checking tools
- Apply consistent punctuation rules
- Improve sentence flow and clarity

#### Formatting Inconsistencies
**Frequency**: Medium  
**Impact**: Low  
**Examples**:
```
❌ Inconsistent spacing around headings
❌ Mixed use of bold/italic for emphasis
❌ Inconsistent list indentation
❌ Variable code block styling
```

**Resolution**:
- Apply consistent formatting rules
- Use style guides for formatting
- Standardize list and code block presentation
- Maintain visual consistency

### 2. Enhancement Opportunities

#### Limited Examples
**Frequency**: Low  
**Impact**: Low  
**Examples**:
```
❌ Only one usage example provided
❌ No industry-specific examples
❌ Missing integration scenarios
❌ No performance optimization examples
```

**Resolution**:
- Add diverse usage examples
- Include industry-specific scenarios
- Provide integration examples
- Show optimization techniques

#### Missing Advanced Content
**Frequency**: Low  
**Impact**: Low  
**Examples**:
```
❌ No advanced configuration options
❌ Missing performance tuning guide
❌ No advanced troubleshooting
❌ No best practices section
```

**Resolution**:
- Add advanced configuration documentation
- Include performance optimization guides
- Provide advanced troubleshooting
- Add best practices and patterns

## OpenCode-Specific Issues

### 1. Skill Documentation Problems

#### SKILL.md Compliance Issues
**Frequency**: High  
**Impact**: High  
**Examples**:
```
❌ Missing required frontmatter fields
❌ Incorrect skill naming convention
❌ Missing required sections
❌ Invalid agent flow documentation
```

**Resolution**:
- Follow SKILL.md template exactly
- Use proper naming conventions
- Include all required sections
- Document agent workflows correctly

#### Agent Integration Issues
**Frequency**: Medium  
**Impact**: High  
**Examples**:
```
❌ Incorrect tool permissions
❌ Missing agent coordination patterns
❌ No integration with document.md
❌ Unclear agent responsibilities
```

**Resolution**:
- Specify correct tool permissions
- Document agent coordination
- Integrate with document.md agent
- Clearly define agent responsibilities

### 2. OpenCode Ecosystem Issues

#### Missing OpenCode Integration
**Frequency**: Medium  
**Impact**: Medium  
**Examples**:
```
❌ No integration with existing tools
❌ Missing skill-creator references
❌ No coordination with technical-writer
❌ Unclear command-line interface
```

**Resolution**:
- Integrate with existing OpenCode tools
- Reference skill-creator patterns
- Coordinate with technical-writer agent
- Document CLI usage clearly

#### Validation Failures
**Frequency**: Medium  
**Impact**: Medium  
**Examples**:
```
❌ Skill validation script failures
❌ Naming convention violations
❌ Directory structure issues
❌ File permission problems
```

**Resolution**:
- Run skill validation scripts
- Follow naming conventions exactly
- Maintain proper directory structure
- Set correct file permissions

## Issue Detection Patterns

### Automated Detection

#### Script-Based Checks
```bash
# Validation script detection
./scripts/validate-docs.sh --severity high
./scripts/check-links.sh --timeout 5
./scripts/analyze-readability.sh --flesch
```

#### Pattern Matching
- Regex patterns for common formatting errors
- Link validation using curl or similar tools
- Syntax checking for code examples
- Readability scoring algorithms

### Manual Detection

#### Review Checklists
- Use comprehensive review checklist
- Follow systematic review process
- Apply severity classification consistently
- Document findings with specific examples

#### User Experience Testing
- Walk through user journeys
- Test all examples and instructions
- Verify navigation paths
- Assess learning curve effectiveness

## Resolution Strategies

### Immediate Actions
1. **Fix Critical Issues First**: Address broken links, missing files, non-functional code
2. **Update Dependencies**: Ensure all versions and references are current
3. **Verify Examples**: Test all code examples and configurations
4. **Improve Navigation**: Fix broken internal links and structure

### Quality Improvements
1. **Enhance Readability**: Simplify language, improve formatting
2. **Add Examples**: Provide comprehensive usage examples
3. **Complete Coverage**: Document missing features and options
4. **Add Visual Elements**: Include diagrams and screenshots

### Long-term Maintenance
1. **Establish Processes**: Set up regular review schedules
2. **Automate Validation**: Implement CI/CD for documentation testing
3. **User Feedback**: Collect and act on user suggestions
4. **Continuous Improvement**: Regularly update and enhance content

---

*This catalog is regularly updated based on review findings and evolving best practices in technical documentation.*