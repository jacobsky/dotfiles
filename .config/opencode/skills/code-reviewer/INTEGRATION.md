# Code Reviewer Skill Integration Guide

This guide explains how to use the advanced code-reviewer skill with the existing opencode workflow.

## Quick Start

### Using the Advanced Code Reviewer Skill

**Option 1: Direct Skill Invocation**
```bash
/skill code-reviewer
```

**Option 2: Enhanced Code Review Command**
```bash
/codereview --skill=code-reviewer
```

**Option 3: Analyze Full Repository**
```bash
/skill code-reviewer --scope=all
```

## Comparison with Basic Code Review

| Feature | Basic `/codereview` | Advanced `code-reviewer` |
|---------|---------------------|--------------------------|
| Analysis Scope | Staged changes only | Staged + full repository |
| Security Analysis | Basic pattern matching | Comprehensive vulnerability scanning |
| Performance Analysis | Manual review | Automated complexity analysis |
| Multi-language Support | Limited | Universal patterns |
| Agent Delegation | Manual | Parallel automated analysis |
| Reporting Format | Simple markdown | Structured comprehensive report |
| TODO Integration | Basic | Smart agent-assigned TODOs |

## Integration Examples

### Example 1: Pre-commit Review
```bash
# Stage your changes
git add .

# Run advanced code review
/skill code-reviewer --scope=staged

# Review the generated report
cat code_review_report.md
```

### Example 2: Repository Health Check
```bash
# Full repository analysis
/skill code-reviewer --scope=all

# Focus on specific file types
/skill code-reviewer --include="*.js,*.ts,*.py"
```

### Example 3: Security-Focused Review
```bash
# Run security analysis
/skill code-reviewer --focus=security

# Generate security TODOs
/skill code-reviewer --create-todos
```

## Output Integration

### Report Generation
The skill creates multiple output files:
- `code_review_report.md` - Main analysis report
- `security_issues.json` - Detailed security findings
- `performance_issues.json` - Performance analysis data
- `analysis_summary.json` - Raw analysis data

### TODO Integration
Automatically creates structured TODOs:
```markdown
TODO(agent: security): Fix SQL injection in src/database/queries.js:42
TODO(agent: perf): Optimize N+1 query in src/models/user.js:67
TODO(agent: plan): Refactor high complexity function in src/controllers/order.js:120
```

## Workflow Integration

### Git Hooks Integration
Add to `.git/hooks/pre-commit`:
```bash
#!/bin/bash
echo "Running advanced code review..."

# Run code reviewer skill
if /skill code-reviewer --scope=staged; then
    echo "Code review passed"
    exit 0
else
    echo "Code review found issues - please review code_review_report.md"
    exit 1
fi
```

### CI/CD Pipeline Integration
```yaml
# Example GitHub Actions
- name: Advanced Code Review
  run: |
    /skill code-reviewer --scope=all
    # Upload reports as artifacts
    upload-code-review-artifacts
```

## Configuration Options

### Custom Analysis Scope
```bash
# Analyze specific directory
/skill code-reviewer --path="src/api/"

# Exclude certain files
/skill code-reviewer --exclude="*.min.js,*.test.js"

# Focus on specific languages
/skill code-reviewer --languages="javascript,python"
```

### Report Customization
```bash
# Generate JSON report
/skill code-reviewer --format=json

# Include code snippets in report
/skill code-reviewer --include-snippets

# Set severity threshold
/skill code-reviewer --min-severity=medium
```

## Best Practices

### When to Use Basic vs Advanced Review

**Use Basic `/codereview` for:**
- Quick daily code reviews
- Small, straightforward changes
- When you need immediate feedback
- Limited analysis scope needed

**Use Advanced `code-reviewer` for:**
- Major feature releases
- Security-sensitive code
- Performance-critical components
- Comprehensive repository health checks
- Multi-service changes

### Integration Tips

1. **Gradual Adoption**: Start with staged changes, expand to full repository
2. **Custom Configuration**: Create project-specific analysis rules
3. **Team Training**: Educate team on interpreting advanced reports
4. **Automation**: Integrate into CI/CD for continuous quality monitoring

## Troubleshooting

### Common Issues

**Skill Not Found**
```bash
# Refresh skill cache
/skill --refresh

# Verify skill installation
ls -la ~/.config/opencode/skills/code-reviewer/
```

**Analysis Too Slow**
```bash
# Limit analysis scope
/skill code-reviewer --scope=staged --max-files=50
```

**Too Many False Positives**
```bash
# Adjust severity thresholds
/skill code-reviewer --min-severity=high
```

### Performance Optimization

- Use `--scope=staged` for daily reviews
- Limit analysis with `--max-files` for large repositories
- Exclude generated files with `--exclude` patterns
- Use `--languages` to focus on relevant codebases

## Migration Guide

### From Basic to Advanced Code Review

1. **Start Small**: Begin with `--scope=staged` reviews
2. **Team Training**: Introduce team to new report format
3. **Process Integration**: Update code review checklists
4. **Gradual Expansion**: Move to full repository analysis
5. **Automation**: Add to CI/CD pipeline

This integration guide ensures smooth adoption of the advanced code-reviewer skill while maintaining compatibility with existing workflows.