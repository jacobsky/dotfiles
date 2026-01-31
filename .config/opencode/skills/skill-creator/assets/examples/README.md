# Skill Creation Examples

This directory contains examples and demonstrations of different skill types and their implementations.

## Example Structures

### Basic Analysis Skill Structure

```
security-analyzer/
├── SKILL.md
├── scripts/
│   ├── vulnerability-scanner.sh
│   ├── pattern-matcher.py
│   └── report-generator.sh
├── references/
│   ├── vulnerability-taxonomy.md
│   ├── security-best-practices.md
│   └── owasp-top-10.md
└── assets/
    ├── templates/
    │   ├── security-report.md
    │   └── vulnerability-detail.md
    └── examples/
        ├── sample-reports/
        └── test-scenarios/
```

### Automation Skill Structure

```
deploy-coordinator/
├── SKILL.md
├── scripts/
│   ├── deploy.sh
│   ├── rollback.sh
│   ├── health-check.sh
│   └── notify.sh
├── references/
│   ├── deployment-strategies.md
│   ├── rollback-procedures.md
│   └── monitoring-setup.md
└── assets/
    ├── templates/
    │   ├── deployment-config.yml
    │   └── status-report.md
    └── examples/
        ├── sample-configs/
        └── deployment-scenarios/
```

### Generation Skill Structure

```
doc-generator/
├── SKILL.md
├── scripts/
│   ├── parse-code.py
│   ├── generate-docs.js
│   └── format-output.sh
├── references/
│   ├── documentation-standards.md
│   ├── api-specification.md
│   └── style-guide.md
└── assets/
    ├── templates/
    │   ├── api-endpoint.md
    │   ├── class-documentation.md
    │   └── README-template.md
    └── examples/
        ├── generated-docs/
        └── sample-inputs/
```

## Example Implementations

### Example 1: Security Analysis Skill

**Purpose**: Analyze code for security vulnerabilities and generate prioritized reports

**Key Features**:
- Multi-language vulnerability scanning
- OWASP Top 10 compliance checking
- Risk-based prioritization
- Remediation recommendations

**Agent Selection**:
```markdown
- **explore**: Scan codebase and identify security-sensitive files
- **librarian**: Research current security standards and best practices
- **oracle**: Analyze findings and prioritize by risk and impact
- **build**: Generate security reports and remediation plans
```

**Sample Output Structure**:
```
# Security Analysis Report

## Executive Summary
- Critical vulnerabilities: 3
- High-risk issues: 7
- Medium-risk issues: 12
- Overall security posture: Needs Improvement

## Critical Findings
1. SQL Injection in user-authentication.js (Critical)
2. Hardcoded credentials in config/database.yml (Critical)
3. Cross-site scripting in template/comments.html (Critical)

## Remediation Plan
### Immediate Actions (Next 24 hours)
1. Fix SQL injection vulnerability - **Agent**: build - **Effort**: 4 hours
2. Remove hardcoded credentials - **Agent**: build - **Effort**: 2 hours

### Short-term Actions (Next Week)
3. Implement XSS protection - **Agent**: build - **Effort**: 8 hours
```

### Example 2: Deployment Automation Skill

**Purpose**: Coordinate deployment workflows with rollback capabilities

**Key Features**:
- Multi-environment deployment support
- Automated health checks
- Rollback on failure
- Deployment notifications

**Agent Selection**:
```markdown
- **build**: Execute deployment scripts and handle file operations
- **plan**: Coordinate deployment phases and validation
- **explore**: Verify environment readiness and dependencies
- **oracle**: Make rollback decisions and failure analysis
```

**Sample Workflow**:
```markdown
## Deployment Execution
### Phase 1: Pre-deployment
✅ Environment validation - Duration: 2m 15s
✅ Dependency check - Duration: 1m 30s
✅ Health verification - Duration: 45s

### Phase 2: Deployment
✅ Application deployment - Duration: 5m 12s
✅ Database migration - Duration: 2m 8s
✅ Service restart - Duration: 30s

### Phase 3: Post-deployment
✅ Health check - Duration: 1m 45s
✅ Smoke tests - Duration: 3m 20s
✅ Notification sent - Duration: 15s

## Results
- Status: Success
- Total Duration: 16m 45s
- Rollback: Not required
- Notifications: Sent to 3 channels
```

### Example 3: Documentation Generation Skill

**Purpose**: Generate API documentation from code annotations

**Key Features**:
- Multi-language parsing support
- Template-based output generation
- Interactive documentation examples
- Version-controlled documentation

**Agent Selection**:
```markdown
- **explore**: Scan codebase for documentation annotations
- **librarian**: Research documentation standards and best practices
- **build**: Parse code and generate documentation content
- **plan**: Structure documentation and create navigation
```

**Sample Output**:
```
# API Documentation v2.1.0

## User Management API

### Create User
**POST** `/api/users`

Create a new user account with the provided details.

#### Parameters
| Name | Type | Required | Description |
|------|------|----------|-------------|
| email | string | Yes | User email address |
| name | string | Yes | User full name |
| role | string | No | User role (default: 'user') |

#### Request Example
```json
{
  "email": "user@example.com",
  "name": "John Doe",
  "role": "admin"
}
```

#### Response Example
```json
{
  "id": 12345,
  "email": "user@example.com",
  "name": "John Doe",
  "role": "admin",
  "created_at": "2023-12-01T10:30:00Z"
}
```

#### Implementation Notes
- Email validation uses RFC 5322 standard
- Password will be automatically generated and sent via email
- Rate limiting: 5 requests per minute per IP
```

## Common Patterns and Anti-patterns

### Good Patterns

**Clear Agent Delegation**:
```markdown
# Good: Clear agent selection with specific tasks
- **explore**: Scan codebase for React components
- **plan**: Analyze components for accessibility issues
- **oracle**: Prioritize findings by impact and user experience
- **build**: Generate accessibility report and remediation recommendations
```

**Progressive Disclosure**:
```markdown
# Good: Keep SKILL.md focused, move details to references/
## What I do
- Analyze React components for WCAG 2.1 AA compliance
- Generate prioritized accessibility recommendations

# Details moved to references/accessibility-standards.md
- Complete WCAG 2.1 AA checklist
- Success criteria definitions
- Testing methodologies
```

**Error Handling**:
```markdown
# Good: Comprehensive error handling
## Error Handling
- Handle missing components gracefully
- Continue analysis despite individual component failures
- Provide partial reports when complete analysis isn't possible
- Suggest manual verification for edge cases
```

### Anti-patterns to Avoid

**Vague Agent Selection**:
```markdown
# Avoid: Unclear agent delegation
- **agent**: Do some analysis
- **another agent**: Fix issues
- **some agent**: Generate report
```

**Monolithic SKILL.md**:
```markdown
# Avoid: Too much detail in SKILL.md
## What I do
- [Long, detailed explanation of implementation details]
- [Specific algorithm descriptions that should be in scripts/]
- [Complete API documentation that should be in references/]
```

**Poor Error Handling**:
```markdown
# Avoid: No error handling strategies
# If something fails, the skill just stops
# No fallback mechanisms or partial results
```

## Testing Your Skills

### Unit Testing Approach

```bash
# Test individual components
./scripts/test-vulnerability-scanner.sh
./scripts/test-deployment-workflow.sh
./scripts/test-doc-generation.sh

# Test integration
./scripts/integration-test.sh
```

### Scenario Testing

```bash
# Test with sample inputs
./scripts/test-with-examples.sh

# Test edge cases
./scripts/test-edge-cases.sh

# Test error conditions
./scripts/test-error-handling.sh
```

### Validation Testing

```bash
# Run full validation
./scripts/validate.sh

# Check structure compliance
./scripts/validate-structure.sh

# Test content quality
./scripts/validate-content.sh
```

## Real-World Integration Examples

### CI/CD Pipeline Integration

```yaml
# .github/workflows/skill-validation.yml
name: Skill Validation
on: [push, pull_request]

jobs:
  validate-skills:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - name: Validate all skills
        run: find skills/ -name "validate.sh" -exec {} \;
```

### Multi-skill Orchestration

```markdown
# Example: Complete code review workflow
1. **security-analyzer**: Scan for security vulnerabilities
2. **performance-analyzer**: Check performance bottlenecks
3. **accessibility-checker**: Validate WCAG compliance
4. **code-quality-checker**: Analyze code quality metrics
5. **oracle**: Synthesize all findings and prioritize
6. **build**: Generate comprehensive review report
```

### Skill Composition Patterns

**Sequential Skills**:
```markdown
1. **code-generator**: Create base implementation
2. **test-generator**: Generate test suite
3. **doc-generator**: Create documentation
4. **deploy-automation**: Deploy to staging
5. **validation-runner**: Validate deployment
```

**Parallel Skills**:
```markdown
1. **security-audit** (parallel)
2. **performance-analysis** (parallel)
3. **accessibility-review** (parallel)
4. **synthesizer**: Combine all findings
```

These examples demonstrate how to structure and implement different types of skills effectively, following OpenCode best practices and patterns.