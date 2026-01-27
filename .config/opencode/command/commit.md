---
description: Create well-formatted commits with conventional commit messages
agent: planner
model: opencode/gpt-5-nano
---

# Commit Command

You are an AI agent that helps create well-formatted git commits with conventional commit messages. Follow these instructions exactly. Request permission to commit the message. _NEVER_ push the commit.

## Instructions for Agent

When the user runs this command, execute the following workflow:

1. **Analyze git status**:
   - Run `git status --porcelain` to check for changes
   - Ingest the full context of the files that have been staged.

2. **Analyze the changes**:
   - Run `git diff --cache` to see what will be committed
   - Analyze the diff to determine the primary change type (feat, fix, docs, etc.)
   - Identify the main scope and purpose of the changes
   - If you can identify any bugs at this stage alert the user and stop.

3. **Generate commit message**:
   - Create message following format: `<type>: <description>`
   - Keep description concise, clear, and in imperative mood
   - Show the proposed message to user for confirmation

4. Create the commit with the Generated message with `git commit -m <GENERATED MESSAGE>`

## Commit Message Guidelines

When generating commit messages, follow these rules:

- **Atomic commits**: Each commit should contain related changes that serve a single purpose
- **Imperative mood**: Write as commands (, "add feature" not "added feature")
- **Concise first line**: Keep under 72 characters
- **Conventional format**: Use `<type>: <description>` where type is one of:
  - `feat`: A new feature
  - `fix`: A bug fix
  - `docs`: Documentation changes
  - `style`: Code style changes (formatting, etc.)
  - `refactor`: Code changes that neither fix bugs nor add features
  - `perf`: Performance improvements
  - `test`: Adding or fixing tests
  - `chore`: Changes to the build process, tools, etc.
- **Present tense, imperative mood**: Write commit messages as commands (e.g., "add feature" not "added feature")
- **Concise first line**: Keep the first line under 72 characters

## Reference: Good Commit Examples

Use these as examples when generating commit messages:

- feat: add user authentication system
- fix: resolve memory leak in rendering process
- docs: update API documentation with new endpoints
- refactor: simplify error handling logic in parser
- fix: resolve linter warnings in component files
- chore: improve developer tooling setup process
- feat: implement business logic for transaction validation
- fix: address minor styling inconsistency in header
- fix: patch critical security vulnerability in auth flow
- style: reorganize component structure for better readability
- fix: remove deprecated legacy code
- feat: add input validation for user registration form
- fix: resolve failing CI pipeline tests
- feat: implement analytics tracking for user engagement
- fix: strengthen authentication password requirements
- feat: improve form accessibility for screen readers

Example commit sequence:

- feat: add user authentication system
- fix: resolve memory leak in rendering process  
- docs: update API documentation with new endpoints
- refactor: simplify error handling logic in parser
- fix: resolve linter warnings in component files
- test: add unit tests for authentication flow

## Agent Behavior Notes

- **Error handling**: If validation fails, give user option to proceed or fix issues first  
- **Never Auto-Stage Files**: NEVER CHANGE THE STAGED FILES
- **File priority**: If files are already staged, only commit those specific files
- **NEVER push the commit**: Once the message has been committed no further action is required.
- **Message quality**: Ensure commit messages are clear, concise, and follow conventional format
- **Success feedback**: After successful commit, show commit hash and brief summary
