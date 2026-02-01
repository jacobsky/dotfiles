# Performance Analysis Patterns

This document outlines performance anti-patterns and optimization opportunities for code review.

## Algorithmic Complexity Issues

### 1. Nested Loops (O(n²) and higher)

#### Problem Detection
**Pattern**: Nested loops over large datasets
```python
# O(n²) - Performance nightmare
for user in users:
  for order in orders:
    if order.user_id == user.id:
      process(user, order)

# O(n) - Optimized
orders_by_user = {}
for order in orders:
  orders_by_user.setdefault(order.user_id, []).append(order)

for user in users:
  for order in orders_by_user.get(user.id, []):
    process(user, order)
```

**Detection Regexes**:
- `for.*for`
- `for.*while`
- `while.*while`
- `while.*for`

#### Common Scenarios
- User-permission checking in loops
- Data filtering without proper indexing
- Inefficient search algorithms

### 2. Inefficient Data Structures

#### Array Operations
```javascript
// Bad: O(n) search in array
const user = users.find(u => u.id === targetId);

// Good: O(1) lookup in Map/Set
const userMap = new Map(users.map(u => [u.id, u]));
const user = userMap.get(targetId);
```

#### String Concatenation
```python
# Bad: O(n²) due to string immutability
result = ""
for item in items:
    result += str(item)  # Creates new string each time

# Good: O(n) using join
result = "".join(str(item) for item in items)
```

## Database Performance Issues

### 1. N+1 Query Problem

#### Pattern Detection
```ruby
# Bad: N+1 queries
User.all.each do |user|
  puts user.profile.bio  # Separate query for each user
end

# Good: Eager loading
User.includes(:profile).each do |user|
  puts user.profile.bio  # Data already loaded
end
```

**Detection Patterns**:
- Database queries inside loops
- Lazy loading in iterations
- Missing foreign key indexes

### 2. Inefficient Queries

#### SELECT * Anti-pattern
```sql
-- Bad: Transfers unnecessary data
SELECT * FROM users WHERE active = 1;

-- Good: Specific columns only
SELECT id, name, email FROM users WHERE active = 1;
```

#### Missing Indexes
```sql
-- Bad: Full table scan
SELECT * FROM orders WHERE user_id = 12345;

-- Good: Indexed lookup
CREATE INDEX idx_orders_user_id ON orders(user_id);
```

## Memory Management Issues

### 1. Memory Leaks

#### Resource Leaks
```c
// Bad: File handle leak
FILE* file = fopen("data.txt", "r");
// Missing fclose(file)

// Good: Proper cleanup
FILE* file = fopen("data.txt", "r");
if (file) {
    // Use file
    fclose(file);
}
```

#### Event Listener Leaks
```javascript
// Bad: Never remove event listeners
function setup() {
  window.addEventListener('resize', handleResize);
}

// Good: Cleanup when needed
function setup() {
  const handleResize = () => console.log('resized');
  window.addEventListener('resize', handleResize);
  return () => window.removeEventListener('resize', handleResize);
}
```

### 2. Excessive Object Creation

#### Object Creation in Loops
```java
// Bad: Creates thousands of objects
List<String> names = new ArrayList<>();
for (User user : users) {
    String name = user.getFirstName() + " " + user.getLastName(); // New string each iteration
    names.add(name);
}

// Good: StringBuilder for concatenation
StringBuilder sb = new StringBuilder();
List<String> names = new ArrayList<>();
for (User user : users) {
    sb.setLength(0);
    sb.append(user.getFirstName()).append(" ").append(user.getLastName());
    names.add(sb.toString());
}
```

## I/O Performance Issues

### 1. Synchronous I/O

#### Blocking Operations
```python
# Bad: Blocking I/O prevents concurrency
def fetch_data():
    response = requests.get('http://api.example.com/data')  # Blocks
    return response.json()

# Good: Asynchronous I/O
async def fetch_data():
    async with aiohttp.ClientSession() as session:
        async with session.get('http://api.example.com/data') as response:
            return await response.json()
```

### 2. Inefficient File Operations

#### File Reading Patterns
```javascript
// Bad: Read file line by line (multiple I/O operations)
const lines = [];
for await (const line of fileStream) {
  lines.push(process(line));
}

// Good: Read in chunks
const buffer = Buffer.alloc(8192);
let bytesRead;
while ((bytesRead = await fs.read(fd, buffer, 0, 8192)) > 0) {
  process(buffer.slice(0, bytesRead));
}
```

## Caching and Optimization Issues

### 1. Missing Caching

#### Expensive Computations
```python
# Bad: Recalculates Fibonacci numbers
def fib(n):
    if n <= 1:
        return n
    return fib(n-1) + fib(n-2)  # Exponential time complexity

# Good: Memoization
from functools import lru_cache

@lru_cache(maxsize=None)
def fib(n):
    if n <= 1:
        return n
    return fib(n-1) + fib(n-2)  # Linear time complexity
```

### 2. Caching Strategy Problems

#### Cache Invalidation
```javascript
// Bad: No cache invalidation
let cache = {};
function getUser(id) {
  if (!cache[id]) {
    cache[id] = fetchUser(id); // Never updates
  }
  return cache[id];
}

// Good: TTL-based invalidation
const cache = new Map();
const TTL = 5 * 60 * 1000; // 5 minutes

function getUser(id) {
  const cached = cache.get(id);
  if (cached && Date.now() - cached.timestamp < TTL) {
    return cached.data;
  }
  
  const data = fetchUser(id);
  cache.set(id, { data, timestamp: Date.now() });
  return data;
}
```

## Frontend Performance Issues

### 1. DOM Manipulation

#### Frequent DOM Updates
```javascript
// Bad: Multiple DOM operations
function updateList(items) {
  const list = document.getElementById('list');
  list.innerHTML = ''; // Expensive reflow
  items.forEach(item => {
    const li = document.createElement('li');
    li.textContent = item;
    list.appendChild(li); // Multiple reflows
  });
}

// Good: Batch DOM operations
function updateList(items) {
  const list = document.getElementById('list');
  const fragment = document.createDocumentFragment();
  
  items.forEach(item => {
    const li = document.createElement('li');
    li.textContent = item;
    fragment.appendChild(li);
  });
  
  list.innerHTML = '';
  list.appendChild(fragment); // Single reflow
}
```

### 2. Event Handler Optimization

#### Event Delegation
```javascript
// Bad: Event listener per item
items.forEach(item => {
  item.addEventListener('click', handleClick); // Hundreds of listeners
});

// Good: Event delegation
document.getElementById('list').addEventListener('click', (e) => {
  if (e.target.matches('li')) {
    handleClick(e);
  }
});
```

## Performance Monitoring Patterns

### 1. Metrics Collection

#### Key Performance Indicators
```javascript
// Performance timing
const start = performance.now();
await expensiveOperation();
const duration = performance.now() - start;

if (duration > 1000) { // 1 second threshold
  console.warn(`Slow operation: ${duration}ms`);
  sendMetric('operation_duration', duration);
}
```

### 2. Profiling Integration

#### Memory Usage Tracking
```python
import tracemalloc
import time

def profile_function(func):
    def wrapper(*args, **kwargs):
        tracemalloc.start()
        start_time = time.time()
        
        result = func(*args, **kwargs)
        
        current, peak = tracemalloc.get_traced_memory()
        tracemalloc.stop()
        
        duration = time.time() - start_time
        print(f"{func.__name__}: {duration:.3f}s, Memory: {current / 1024:.1f}KB")
        
        return result
    return wrapper
```

## Optimization Prioritization

### 1. Impact Assessment
- **Critical**: O(n²+) algorithms on large datasets
- **High**: Database performance issues affecting many users
- **Medium**: Frontend performance affecting user experience
- **Low**: Minor optimizations with minimal impact

### 2. Optimization Strategy
1. **Measure First**: Profile before optimizing
2. **Focus on Hot Paths**: Optimize code that runs frequently
3. **Consider Scale**: Impact depends on usage patterns
4. **Balance Complexity**: Don't over-optimize rarely used code

This guide provides a comprehensive framework for identifying and addressing performance issues in code review, with specific patterns, detection methods, and optimization strategies.