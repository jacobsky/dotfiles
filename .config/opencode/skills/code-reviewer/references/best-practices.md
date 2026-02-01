# Code Review Best Practices

This document outlines language-agnostic best practices for code review, serving as a reference for the code-reviewer skill.

## Universal Principles

### 1. Security First
- **Input Validation**: Always validate and sanitize user input
- **Output Encoding**: Encode output to prevent injection attacks
- **Authentication & Authorization**: Implement proper access controls
- **Error Handling**: Don't expose sensitive information in error messages
- **Cryptographic Practices**: Use established libraries, never roll your own crypto

### 2. Performance Awareness
- **Algorithm Selection**: Choose appropriate time/space complexity
- **Database Efficiency**: Avoid N+1 queries, use proper indexing
- **Memory Management**: Prevent memory leaks and excessive allocations
- **Caching Strategy**: Cache expensive operations appropriately
- **I/O Operations**: Minimize blocking I/O operations

### 3. Maintainability & Readability
- **Clear Naming**: Use descriptive variable and function names
- **Single Responsibility**: Each function/class should have one clear purpose
- **Consistent Style**: Follow established coding conventions
- **Documentation**: Document complex logic, APIs, and business rules
- **Testing**: Write tests for critical paths and edge cases

## Code Quality Patterns

### Positive Patterns ✅

#### Error Handling
```javascript
// Good: Specific error handling with logging
try {
  const result = await riskyOperation();
  return processResult(result);
} catch (error) {
  logger.error('Operation failed', { error: error.message, context });
  throw new OperationError('Failed to process operation', { cause: error });
}
```

#### Input Validation
```python
# Good: Comprehensive input validation
def process_user_data(data):
    if not isinstance(data, dict):
        raise TypeError("Expected dictionary")
    
    required_fields = ['name', 'email', 'age']
    for field in required_fields:
        if field not in data:
            raise ValueError(f"Missing required field: {field}")
    
    if not re.match(r'^[^@]+@[^@]+\.[^@]+$', data['email']):
        raise ValueError("Invalid email format")
    
    # Process validated data
    return transform_data(data)
```

#### Resource Management
```java
// Good: Proper resource cleanup with try-with-resources
try (Connection conn = dataSource.getConnection();
     PreparedStatement stmt = conn.prepareStatement(sql)) {
    
    stmt.setString(1, userId);
    ResultSet rs = stmt.executeQuery();
    return mapResultSetToUser(rs);
}
```

### Anti-Patterns ❌

#### Hardcoded Secrets
```javascript
// Bad: Hardcoded credentials
const dbPassword = "admin123";  // SECURITY RISK!
```

#### SQL Injection
```php
// Bad: SQL injection vulnerability
$query = "SELECT * FROM users WHERE id = " . $_GET['id'];  // DANGEROUS!
```

#### Inefficient Loops
```python
# Bad: O(n²) nested loops
for user in users:
  for permission in permissions:
    if user.has_permission(permission):
      process(user, permission)  # Inefficient!
```

## Language-Specific Guidelines

### JavaScript/TypeScript
- Use `const` and `let` instead of `var`
- Prefer async/await over Promise chains
- Implement proper error boundaries in React
- Use TypeScript for type safety
- Avoid `eval()` and `Function()` constructor

### Python
- Follow PEP 8 style guidelines
- Use context managers (`with` statements) for resources
- Implement proper exception handling
- Avoid mutable default arguments
- Use f-strings for string formatting

### Java
- Follow Java naming conventions
- Use try-with-resources for automatic resource management
- Implement proper equals/hashCode contracts
- Prefer streams over manual iteration
- Use interfaces for abstraction

### C++/C
- Follow RAII principles
- Use smart pointers instead of raw pointers
- Initialize all variables
- Avoid using `using namespace std`
- Implement proper copy semantics

## Review Checklist

### Security Review
- [ ] Input validation implemented
- [ ] Output encoding used
- [ ] Authentication/authorization checks present
- [ ] No hardcoded secrets
- [ ] Error messages don't expose sensitive data
- [ ] SQL injection protection in place
- [ ] XSS prevention measures

### Performance Review
- [ ] Appropriate algorithm complexity
- [ ] Database queries optimized
- [ ] No memory leaks
- [ ] Efficient I/O operations
- [ ] Proper caching strategy
- [ ] Resource cleanup implemented

### Code Quality Review
- [ ] Clear, descriptive naming
- [ ] Single responsibility principle followed
- [ ] Consistent code style
- [ ] Adequate documentation
- [ ] Proper error handling
- [ ] Test coverage for critical paths

### Integration Review
- [ ] API compatibility maintained
- [ ] Dependencies properly managed
- [ ] Configuration handled correctly
- [ ] Logging implemented appropriately
- [ ] Monitoring/metrics included

## Common Issue Categories

### 1. Security Vulnerabilities
- **Severity**: Critical
- **Examples**: SQL injection, XSS, authentication bypass
- **Action**: Must fix before deployment

### 2. Performance Regressions
- **Severity**: High
- **Examples**: O(n²) algorithms, N+1 queries, memory leaks
- **Action**: Should fix before merge

### 3. Code Quality Issues
- **Severity**: Medium
- **Examples**: Complex functions, poor naming, missing documentation
- **Action**: Should address in near future

### 4. Style and Convention Violations
- **Severity**: Low
- **Examples**: Inconsistent formatting, naming convention violations
- **Action**: Nice to fix, can be addressed later

## Review Communication Guidelines

### Providing Feedback
- **Be Specific**: Reference exact file and line numbers
- **Be Constructive**: Focus on improvement, not criticism
- **Be Educational**: Explain why changes matter
- **Be Respectful**: Assume good intentions

### Prioritization
- **Critical**: Security vulnerabilities, breaking changes
- **High**: Performance issues, major bugs
- **Medium**: Code quality, maintainability
- **Low**: Style issues, minor optimizations

### Action Items
- Use TODOs with appropriate agent assignments
- Provide clear steps for resolution
- Include context and impact assessment
- Offer alternative approaches when appropriate