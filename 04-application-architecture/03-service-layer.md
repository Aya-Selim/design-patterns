# Service Layer

## Purpose
The Service Layer contains the **business logic** of your application. It acts as the boundary between your presentation layer (Controllers / Minimal APIs) and your data persistence layer (Repositories).

```csharp
public interface IProductService
{
    Task<List<ProductDto>> GetActiveProductsAsync(CancellationToken ct = default);
}

// C# 12 primary constructor injecting the repository
public class ProductService(IGenericRepository<Product> repo) : IProductService
{
    public async Task<List<ProductDto>> GetActiveProductsAsync(CancellationToken ct = default) =>
        await repo.GetAll()
            .Where(p => p.IsActive && !p.IsDeleted) // Business rule lives here ONLY
            .Select(p => new ProductDto(p.Id, p.Name, p.Price))
            .ToListAsync(ct);
}
```

Now if the definition of "Active Product" changes, you edit **one place**, not 20 call sites.

---

## The Cross-Domain Golden Rule

Imagine you are writing `CarService`. You need to verify that a car's category is valid.

> **Question:** In `CarService`, should you inject `ICategoryRepository`?  
> **Answer:** ❌ **NO.** Inject `ICategoryService`.

### Why?
The category repository only knows how to talk to the database. It bypasses any domain rules, security checks, or caching defined in `ICategoryService`.

```csharp
// BAD: Injects another domain's repository directly
public class CarService(
    IGenericRepository<Car> carRepo,
    IGenericRepository<Category> categoryRepo) // ❌ Skips Category business logic!
```

```csharp
// GOOD: Injects the domain's service
public class CarService(
    IGenericRepository<Car> carRepo,
    ICategoryService categoryService)         // ✅ Respects Category domain rules
```

> [!IMPORTANT]
> **Rule:** Never inject the repository of another domain. Always inject its service.

---

## Problem Solved
Business rules are centralized in one service instead of leaking into presentation controllers.

---

## New Problem Introduced
1. **One Endpoint, Many Services:** A complex controller endpoint might need to inject 5 or 6 different domain services.
   - **Solution:** Use the [Facade Pattern](../02-structural/03-facade/README.md) to hide multiple services behind a unified, simple interface.
2. **"God Service" Bloat:** Over time, `ProductService` grows into a 2,000-line class with 30 unrelated methods doing reads, updates, imports, and exports.
   - **Solution:** Split operations into single-purpose handlers using [CQRS and MediatR](./05-cqrs-and-mediatr.md).
