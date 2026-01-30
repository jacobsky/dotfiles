---
description: Analyze the codebase and create a TODO list based on that.
agent: plan
---

Analyze the codebase for `TODO` tags `Warnings` or other feature gaps, bugs or
other inconsistencies in the code that can be addressed.

Run subagents where possible to gain broadest understanding of the codebase.

Create a task list with the following

```md
## Component TODO
- [ ] Task 1: This is a short description of the task and the module to work on
    - [ ] Subtask A: This is a short description of the task
    - [ ] Subtask B: This is a short description of the task
- [ ] Task 2: This is a short description of the task
- [ ] Task 3: This is a short description of the task
    - [ ] Subtask A: This is a short description of the task
    - [ ] Subtask B: This is a short description of the task
- [ ] Task 4: This is a short description of the task
```

In addition, review what has been completed thus far and include those tasks, if completed in the todo list as completed entries.

When you are completed output this list for the user and ASK FOR CONFIRMATION if they would like to save it as todo.md
