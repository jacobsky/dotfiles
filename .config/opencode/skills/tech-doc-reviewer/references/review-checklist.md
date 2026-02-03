# Documentation Review Checklist

## Overview

This comprehensive checklist is used by the `tech-doc-reviewer` skill to systematically evaluate technical documentation quality. It covers all aspects of documentation from content accuracy to user experience.

## Content Quality Checklist

### ✅ Technical Accuracy
- [ ] **Code Examples Compile and Run**
  - [ ] All code examples are syntactically correct
  - [ ] Examples have been tested and verified to work
  - [ ] Dependencies and prerequisites are clearly stated
  - [ ] Version-specific compatibility is indicated

- [ ] **Technical Specifications Are Current**
  - [ ] API documentation matches actual implementation
  - [ ] Configuration options are up-to-date
  - [ ] System requirements are accurate
  - [ ] Version information is current

- [ ] **Data and References Are Correct**
  - [ ] URLs and links point to current resources
  - [ ] Data examples reflect realistic scenarios
  - [ ] References to external resources are valid
  - [ ] Citations and attributions are accurate

### ✅ Completeness
- [ ] **Essential Information Present**
  - [ ] Installation and setup instructions included
  - [ ] Basic usage examples provided
  - [ ] Prerequisites clearly listed
  - [ ] System requirements specified

- [ ] **Comprehensive Coverage**
  - [ ] All major features documented
  - [ ] Edge cases and limitations addressed
  - [ ] Error scenarios and troubleshooting included
  - [ ] Advanced usage patterns explained

- [ ] **User Journey Support**
  - [ ] Getting started guide available
  - [ ] Progressive complexity from basic to advanced
  - [ ] Task-based organization present
  - [ ] Learning resources referenced

### ✅ Clarity and Accessibility
- [ ] **Appropriate Language Level**
  - [ ] Technical terms defined on first use
  - [ ] Language matches target audience expertise
  - [ ] Acronyms and abbreviations explained
  - [ ] Consistent terminology used throughout

- [ ] **Clear Explanations**
  - [ ] Concepts explained before implementation
  - [ ] Visual aids support text descriptions
  - [ ] Step-by-step instructions are unambiguous
  - [ ] Context provided for technical decisions

- [ ] **Readability**
  - [ ] Sentence length is reasonable (15-25 words average)
  - [ ] Paragraphs focused on single ideas
  - [ ] Active voice predominantly used
  - [ ] Clear logical flow maintained

## Structure and Organization Checklist

### ✅ Information Architecture
- [ ] **Logical Organization**
  - [ ] Content flows from basic to advanced
  - [ ] Related concepts grouped together
  - [ ] Clear separation of different topics
  - [ ] Consistent organizational patterns used

- [ ] **Navigation Support**
  - [ ] Table of contents present and accurate
  - [ ] Internal links function correctly
  - [ ] Cross-references help users find related content
  - [ ] Back-navigation options available

- [ ] **Hierarchy and Structure**
  - [ ] Proper heading hierarchy (H1 → H2 → H3)
  - [ ] Consistent section organization
  - [ ] Appropriate use of subsections
  - [ ] Clear distinction between main and supporting content

### ✅ Formatting and Presentation
- [ ] **Visual Consistency**
  - [ ] Consistent heading styles and formatting
  - [ ] Uniform list formatting
  - [ ] Consistent code block styling
  - [ ] Standard emphasis and highlighting usage

- [ ] **Code and Technical Elements**
  - [ ] Code blocks properly formatted with syntax highlighting
  - [ ] Inline code uses backticks consistently
  - [ ] Technical terms highlighted appropriately
  - [ ] Mathematical notation rendered correctly

- [ ] **Media and Visuals**
  - [ ] Images are high quality and relevant
  - [ ] Diagrams enhance understanding
  - [ ] Alt text provided for accessibility
  - [ ] Visual elements load correctly

### ✅ Link and Reference Management
- [ ] **Internal Links**
  - [ ] All internal links resolve to correct content
  - [ ] Anchor links point to existing sections
  - [ ] Link text describes target content accurately
  - [ ] No orphaned pages or sections

- [ ] **External Links**
  - [ ] External resources are relevant and current
  - [ ] Links open to accessible content
  - [ ] External dependency risks assessed
  - [ ] Alternative resources provided when appropriate

## OpenCode-Specific Standards Checklist

### ✅ Skill Documentation
- [ ] **SKILL.md Compliance**
  - [ ] Required frontmatter present and valid
  - [ ] Name follows naming conventions
  - [ ] Description within length limits
  - [ ] Compatibility set to "opencode"

- [ ] **Required Sections**
  - [ ] "What I do" section with bullet points
  - [ ] "When to use me" section with clear scenarios
  - [ ] "How I work" section with execution phases
  - [ ] Usage examples provided

- [ ] **Agent Integration**
  - [ ] Agent flow clearly documented
  - [ ] Tool access specified (bash, read, etc.)
  - [ ] Integration with other agents explained
  - [ ] Subagent dispatch patterns described

### ✅ Agent Documentation
- [ ] **Agent Definitions**
  - [ ] Agent purpose clearly stated
  - [ ] Mode (primary/subagent) correctly identified
  - [ ] Tool permissions appropriately set
  - [ ] Role and responsibilities defined

- [ ] **Workflow Documentation**
  - [ ] Agent interaction patterns documented
  - [ ] Data flow between agents explained
  - [ ] Error handling and recovery described
  - [ ] Performance considerations addressed

### ✅ API Documentation
- [ ] **Endpoint Documentation**
  - [ ] All public endpoints documented
  - [ ] HTTP methods specified
  - [ ] Request parameters fully described
  - [ ] Response schemas provided

- [ ] **Integration Information**
  - [ ] Authentication methods explained
  - [ ] Rate limiting information included
  - [ ] Error codes and handling documented
  - [ ] SDK and client library information provided

## User Experience Checklist

### ✅ Discoverability
- [ ] **Search and Findability**
  - [ ] Content is searchable with relevant keywords
  - [ ] Important concepts are easily discoverable
  - [ ] Index or glossary provided for large documentation
  - [ ] Tagging or categorization helps content discovery

- [ ] **First Impressions**
  - [ ] README provides clear project overview
  - [ ] Getting started is obvious and accessible
  - [ ] Value proposition immediately apparent
  - [ ] Prerequisites clearly communicated

### ✅ Learning Curve
- [ ] **Progressive Learning**
  - [ ] Basic concepts introduced first
  - [ ] Complexity increases gradually
  - [ ] Multiple learning paths available
  - [ ] Skip-forward options for experienced users

- [ ] **Hands-On Support**
  - [ ] Interactive examples available
  - [ ] Tutorial content is practical
  - [ ] Real-world scenarios included
  - [ ] Practice exercises provided

### ✅ Task Completion
- [ ] **Goal-Oriented Design**
  - [ ] Documentation organized around user goals
  - [ ] Step-by-step guides for common tasks
  [ ] Quick reference for frequent operations
  [ ] Decision trees for complex choices

- [ ] **Error Recovery**
  - [ ] Common errors documented with solutions
  [ ] Debugging guidance provided
  [ ] Troubleshooting sections comprehensive
  [ ] Community support resources referenced

## Maintainability Checklist

### ✅ Version Management
- [ ] **Version Information**
  - [ ] Current version clearly indicated
  - [ ] Version-specific sections properly marked
  - [ ] Migration paths documented
  - [ ] Deprecation warnings provided

- [ ] **Change Documentation**
  - [ ] Changelog maintained and up-to-date
  - [ ] Breaking changes highlighted
  - [ ] New features documented promptly
  - [ ] Bug fix references included

### ✅ Update Processes
- [ ] **Regular Review Schedule**
  - [ ] Documentation review calendar established
  - [ ] Content freshness indicators present
  - [ ] Automated validation in place
  - [ ] Peer review process defined

- [ ] **Quality Assurance**
  - [ ] Automated testing for code examples
  - [ ] Link checking regularly performed
  [ ] Readability analysis conducted
  [ ] User feedback collected and analyzed

## Issue Severity Classification

### 🔴 Critical Issues
Documentation critical issues that block user understanding or prevent successful implementation:

- Missing essential installation or setup instructions
- Broken code examples that don't compile or run
- Incorrect technical information that misleads users
- Missing files or sections referenced elsewhere

### 🟠 High Priority Issues
Issues that significantly impact usability but have workarounds:

- Missing important usage examples
- Incomplete API documentation
- Broken internal or external links
- Poor organization that hinders navigation

### 🟡 Medium Priority Issues
Issues that moderately affect user experience:

- Unclear explanations or technical terms
- Inconsistent formatting or styling
- Long, complex sentences or paragraphs
- Missing error handling information

### 🔵 Low Priority Issues
Minor improvements that enhance quality but don't impact core functionality:

- Minor formatting inconsistencies
- Opportunities for additional examples
- Could benefit from more visual aids
- Minor grammatical or stylistic improvements

## Review Process

### 1. Automated Validation
Run all validation scripts:
```bash
./scripts/validate-docs.sh
./scripts/check-links.sh  
./scripts/analyze-readability.sh
```

### 2. Manual Review
Use this checklist to manually review:
- Content accuracy and completeness
- User experience and flow
- Technical correctness
- Integration with OpenCode ecosystem

### 3. Score Calculation
Apply quality scoring system:
- Start with base score of 100
- Deduct points based on issue severity
- Consider weighting factors for documentation type
- Generate final quality level

### 4. Report Generation
Create comprehensive review report:
```bash
./scripts/generate-report.sh
```

### 5. Action Planning
Develop improvement plans:
- Prioritize critical and high issues
- Assign responsibilities and timelines
- Establish metrics for success
- Schedule follow-up reviews

---

*This checklist is designed to be used systematically to ensure comprehensive evaluation of technical documentation quality across the OpenCode ecosystem.*