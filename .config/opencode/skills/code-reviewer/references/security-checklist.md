# Security Review Checklist

This document provides comprehensive security patterns and vulnerability detection guidelines for code review.

## Critical Security Vulnerabilities

### 1. Injection Attacks

#### SQL Injection
**Pattern**: String concatenation in database queries
```javascript
// Vulnerable
const query = `SELECT * FROM users WHERE id = ${userId}`;

// Safe
const query = 'SELECT * FROM users WHERE id = ?';
```

**Detection Regexes**:
- `SELECT.*FROM.*WHERE.*\+`
- `INSERT.*INTO.*\+`
- `UPDATE.*SET.*\+`
- `DELETE.*FROM.*\+`

#### Command Injection
**Pattern**: User input passed to system commands
```python
# Vulnerable
os.system(f"rm {user_path}")

# Safe
subprocess.run(['rm', validated_path], check=True)
```

**Detection Regexes**:
- `system.*\$`
- `exec.*\$`
- `shell_exec.*\$`
- `popen.*\$`

#### Code Injection
**Pattern**: Dynamic code execution with user input
```php
// Vulnerable
eval($user_code);

// Safe
// Never eval user input
```

**Detection Regexes**:
- `eval.*\$`
- `Function.*\$`
- `setTimeout.*string`

### 2. Authentication & Authorization Issues

#### Hardcoded Credentials
**Pattern**: Passwords, API keys, tokens in source code
```javascript
// Very bad
const API_KEY = "sk-1234567890abcdef";
const DB_PASSWORD = "admin123";
```

**Detection Regexes**:
- `password.*=.*["'].*["']`
- `secret.*=.*["'].*["']`
- `api_key.*=.*["'].*["']`
- `token.*=.*["'].*["']`

#### Insecure Authentication
**Pattern**: Weak password handling, insecure session management
```python
# Weak
password = request.POST['password']  # No hashing
session['user_id'] = user.id  # No secure flags
```

### 3. Data Exposure

#### Sensitive Data in Logs
**Pattern**: Logging sensitive information
```javascript
// Bad
console.log('User login:', { email, password, ssn });

// Good
console.log('User login attempt:', { email: maskEmail(email) });
```

#### Error Information Disclosure
**Pattern**: Detailed error messages to users
```python
# Bad
except Exception as e:
    return jsonify({'error': str(e), 'stack': traceback.format_exc()})

# Good
except Exception as e:
    logger.error('Database error', exc_info=True)
    return jsonify({'error': 'Internal server error'})
```

## Medium Security Issues

### 1. Input Validation Problems

#### Missing Input Validation
**Pattern**: Using user input without validation
```javascript
// Vulnerable
app.get('/user/:id', (req, res) => {
  const userId = req.params.id; // No validation
  return getUser(userId);
});

// Safe
app.get('/user/:id', (req, res) => {
  const userId = req.params.id;
  if (!/^\d+$/.test(userId)) {
    return res.status(400).json({ error: 'Invalid user ID' });
  }
  return getUser(userId);
});
```

### 2. Cryptographic Issues

#### Weak Encryption
**Pattern**: Using outdated or weak cryptographic algorithms
```javascript
// Bad
const crypto = require('crypto');
const cipher = crypto.createCipher('des-ecb', key); // DES is weak

// Good
const cipher = crypto.createCipher('aes-256-gcm', key); // AES-256-GCM is strong
```

#### Random Number Generation
**Pattern**: Using predictable random values
```python
# Bad - predictable
import random
token = random.random()

# Good - cryptographically secure
import secrets
token = secrets.token_urlsafe(32)
```

### 3. Session Management Issues

#### Insecure Cookies
**Pattern**: Cookies without security flags
```javascript
// Bad
res.cookie('sessionId', sessionId);

// Good
res.cookie('sessionId', sessionId, {
  httpOnly: true,
  secure: true,
  sameSite: 'strict'
});
```

## Low Security Issues

### 1. Information Disclosure

#### Verbose Error Messages
**Pattern**: Detailed error messages in production
```javascript
// Bad
app.use((err, req, res, next) => {
  res.status(500).json({
    error: err.message,
    stack: err.stack,
    details: err.details
  });
});

// Good
app.use((err, req, res, next) => {
  if (process.env.NODE_ENV === 'development') {
    return res.status(500).json({
      error: err.message,
      stack: err.stack
    });
  }
  
  res.status(500).json({ error: 'Internal server error' });
});
```

### 2. Configuration Security

#### Debug Mode in Production
**Pattern**: Debugging features enabled in production
```python
# Very bad
app.run(debug=True)  # In production!

# Good
if __name__ == '__main__':
    app.run(debug=False)
```

#### Directory Listing
**Pattern**: Exposed directory contents
```nginx
# Bad - enables directory listing
location / {
    autoindex on;
}

# Good - secure directory listing
location / {
    autoindex off;
    try_files $uri $uri/ =404;
}
```

## Security Review Process

### 1. Automated Detection
- Use pattern matching for known vulnerability signatures
- Scan for hardcoded secrets and sensitive data
- Check for unsafe function usage
- Analyze input validation patterns

### 2. Contextual Analysis
- Consider the data sensitivity
- Evaluate the attack surface
- Assess the impact of potential exploitation
- Review authentication and authorization flows

### 3. Risk Assessment
- **Critical**: Direct exploit paths, data breach potential
- **High**: Security bypasses, privilege escalation
- **Medium**: Information disclosure, weak protections
- **Low**: Configuration issues, minor exposures

## Remediation Guidelines

### Immediate Actions (Critical)
1. Remove hardcoded secrets immediately
2. Implement proper input validation
3. Use parameterized queries for database access
4. Secure authentication mechanisms

### Short-term Improvements (High)
1. Implement proper error handling
2. Add security headers
3. Enhance logging and monitoring
4. Review and update dependencies

### Long-term Enhancements (Medium/Low)
1. Implement comprehensive security testing
2. Add security monitoring and alerting
3. Conduct regular security audits
4. Establish security training programs

## Framework-Specific Considerations

### Web Applications
- CSRF protection
- XSS prevention
- Clickjacking protection
- Secure headers implementation

### API Security
- Rate limiting
- Authentication token validation
- API versioning security
- Input schema validation

### Mobile Applications
- Secure data storage
- Certificate pinning
- Root/jailbreak detection
- Secure network communication

This checklist should be used as a comprehensive guide for security code review, focusing on the most critical vulnerabilities first while also addressing medium and low priority security issues.