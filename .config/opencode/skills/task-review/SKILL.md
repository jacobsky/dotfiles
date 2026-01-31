---
name: task-review
description: Analyze codebase agents, commits, TODOs, and provide task recommendations
license: MIT
compatibility: opencode
metadata:
  workflow: multi-analysis
  parallel: true
  output: structured-list
---

## What I do

- Review available agents and their capabilities from configuration files
- Analyze recent git commit history to understand development patterns  
- Search codebase for TODO, FIX, FIXME tags
- Generate prioritized list of recommended next tasks
- Select appropriate agents automatically for each analysis phase

## When to use me

Use this when you want a comprehensive overview of your codebase status and need intelligent task recommendations.

## How I work

### Phase 1: Agent Ecosystem Analysis

- Parse agent configuration files (typically AGENTS.md)
- Extract agent capabilities, models, and purposes
- Identify potential optimization opportunities

### Phase 2: Commit History Intelligence

- Analyze last 10 commits with full context using `git log --pretty=format:"%h %s%n%b"`
- Categorize commits by type (feat, fix, docs, config, etc.)
- Identify development patterns and activity levels

### Phase 3: Development Tag Scanning

- Search codebase for TODO, FIX, FIXME patterns using grep and ast-grep
- Categorize findings by file and priority
- Include context for each discovered item

### Phase 4: Task Recommendations  

- Synthesize all findings into actionable list
- Prioritize based on impact and current development patterns
- Match tasks to optimal agent combinations

## Execution Pattern

### Parallel Analysis Setup

1. Launch parallel background tasks for each analysis phase
2. Use `explore` agent for codebase and git analysis
3. Use `librarian` for agent documentation research
4. Synthesize results with `oracle` for recommendations

### Agent Selection Logic

- **explore**: File system operations, git analysis, pattern searching
- **librarian**: Agent capability documentation analysis
- **oracle**: Strategic task prioritization and recommendations

## Output Format

Basic structured list with clear task prioritization:

```
# Task Review Report

## Agent Analysis
- [Summary of agent ecosystem findings]
- [Optimization recommendations]

## Commit Patterns  
- [Analysis of recent development activity]
- [Pattern recognition and insights]

## Development Items Found
- [List of TODO/FIX/FIXME items with context]

## Recommended Next Tasks
1. [High-priority task with agent suggestion]
2. [Medium-priority task with agent suggestion]  
3. [Low-priority task with agent suggestion]
```

## Error Handling

- Gracefully handle missing agents.md (use config files as fallback)
- Work with empty TODO lists (provide constructive suggestions)
- Handle shallow git histories (adjust analysis depth)
- Continue analysis if individual phases encounter issues

## Notes

- Only searches for TODO, FIX, FIXME patterns
- Runs in parallel for efficiency
- Stores no historical data or archives
- Provides immediate, actionable recommendations

