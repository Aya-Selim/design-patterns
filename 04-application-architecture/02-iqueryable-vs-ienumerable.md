# IQueryable vs IEnumerable

Understanding the difference between `IEnumerable` and `IQueryable` is essential when writing repositories and services with Entity Framework Core.

---

## The Core Difference

| Feature | IEnumerable | IQueryable |
|---|---|---|
| **Namespace** | `System.Collections.Generic` | `System.Linq` |
| **Execution Engine** | LINQ to Objects | LINQ to Entities (Translates Expression Trees to SQL) |
| **Where does filtering run?** | In **memory** (C# runtime) | In the **database** (SQL Server, PostgreSQL, etc.) |
| **Data loaded across network** | Fetches all records first, then filters | Database filters first; only matching rows are fetched |

```csharp
// IEnumerable: loads ALL 100,000 products over network, then filters in RAM (SLOW & DANGEROUS)
IEnumerable<Product> inMemory = db.Products.AsEnumerable();
var expensiveResult = inMemory.Where(p => p.Price > 100).ToList();

// IQueryable: generates "SELECT * FROM Products WHERE Price > 100" in SQL (FAST & OPTIMIZED)
IQueryable<Product> query = db.Products;
var optimizedResult = await query.Where(p => p.Price > 100).ToListAsync();
```

---

## What Should a Repository Return? (3 Approaches)

Architects debate what a Repository should return. Here are the 3 common approaches:

### Approach 1: Expose `IQueryable<T>` (Pragmatic but Leaky)
The repository returns `IQueryable<T>` directly. The Service Layer composes filters (`.Where()`) and calls `.ToListAsync()`.

```csharp
// Repository
public IQueryable<Product> GetAll() => db.Products;

// Service
public async Task<List<ProductDto>> GetActiveProductsAsync()
{
    // Needs Microsoft.EntityFrameworkCore in the Service project!
    return await repo.GetAll()
        .Where(p => p.IsActive)
        .Select(p => new ProductDto(p.Id, p.Name, p.Price))
        .ToListAsync();
}
```
- **Why developers use it:** Maximum flexibility. The caller decides pagination, projection, and sorting.
- **The danger (Leaky Abstraction):** 
  - Leaks EF Core into the Service layer (requires `using Microsoft.EntityFrameworkCore` to call `ToListAsync`).
  - C# LINQ methods may fail at runtime if EF Core cannot translate them into SQL.
  - Deferred execution can lead to disposed `DbContext` errors.

---

### Approach 2: Return Evaluated Collections `Task<IReadOnlyList<T>>` (Strict Encapsulation)
The repository executes the query and returns materialized data.

```csharp
// Repository
public async Task<IReadOnlyList<Product>> GetActiveAsync() =>
    await db.Products.Where(p => p.IsActive).ToListAsync();
```
- **Pros:** Completely hides EF Core. The Service Layer has zero knowledge of the database provider.
- **Cons:** Repository interfaces can explode with specialized methods (`GetActiveAsync()`, `GetActiveByCategoryAsync()`, `GetActivePagedAsync()`).

---

### Approach 3: The Specification Pattern (⭐ Recommended for Clean Architecture)
Instead of exposing `IQueryable` or creating hundreds of repository methods, you encapsulate query logic inside reusable **Specification** objects.

```csharp
// 1. Specification interface
public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
}

// 2. Concrete reusable specification
public class ActiveProductsWithCategorySpec : BaseSpecification<Product>
{
    public ActiveProductsWithCategorySpec() : base(p => p.IsActive)
    {
        AddInclude(p => p.Category);
    }
}

// 3. Repository executes the specification cleanly
public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
{
    return await ApplySpecification(spec).ToListAsync();
}
```

---

## Trade-off Comparison

| Approach | Service Decoupled from EF Core? | Prevents Leaky Queries? | Flexibility | Code Maintainability |
|---|:---:|:---:|:---:|:---:|
| **1. Exposing `IQueryable`** | ❌ No | ❌ No | ⭐⭐⭐ High | ⚠️ Low (queries scattered everywhere) |
| **2. Materialized `IReadOnlyList`** | ✅ Yes | ✅ Yes | ⭐ Low | ⚠️ Medium (repository interface bloat) |
| **3. Specification Pattern (Recommended)** | ✅ Yes | ✅ Yes | ⭐⭐⭐ High | ⭐⭐⭐ High (reusable query objects) |

> **Clean Architecture Recommendation:** 
> Avoid returning `IQueryable` from repositories. Use the **Specification Pattern** to keep your queries testable, reusable, and free from database-specific leaks.