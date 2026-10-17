# Repository

## What is a Repository?
A repository mediates between the domain and data mapping layers. It acts as an in-memory collection of domain entities.

> **Crucial Rule:** The repository is for **data access only**. It must **not** contain business logic.

---

## `FindAsync` vs `Where` in EF Core

A very common interview and architectural question is the difference between `FindAsync` and `Where`:

| Method | Behavior |
|---|---|
| `FindAsync(id)` | Checks the `DbContext` **change tracker** first. If the entity is already loaded in the current context, it returns it immediately. If not found in the change tracker, it queries the database. |
| `Where(...).FirstOrDefaultAsync()` | **Always sends a query to the database.** However, if the matching entity is already being tracked, EF Core returns the tracked instance without overwriting its in-memory values. |

### Important Rules About EF Core Behavior

> [!IMPORTANT]
> **The change tracker is NOT a general-purpose cache.**
> The change tracker tracks entity instances and their mutation states (`Added`, `Modified`, `Unchanged`, `Deleted`) within a single `DbContext` lifecycle. Calling it a "cache" in technical discussions is incorrect.

- **`FindAsync(id)`**: Can avoid a database roundtrip if the entity is already tracked by the current `DbContext`.
- **Tracking queries**: Even though `Where()` queries the database, EF Core attaches the returned rows to the change tracker.
- **`AsNoTracking()`**: Skips change tracker registration. Use it for read-only queries to save memory and CPU cycles.
- **`ReloadAsync()`**: Forces EF Core to refresh an already-tracked entity with fresh values from the database.

---

## Why Use `BaseEntity`?

A generic repository needs a way to identify an entity by its primary key. By requiring entities to inherit from a common base class, we can write type-safe queries on `Id`:

```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
}
```

---

## Generic Repository Implementation (.NET 8)

```csharp
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    IQueryable<T> GetAll();
    Task AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);
    void Delete(T entity);
}

// Using C# 12 primary constructors
public class GenericRepository<T>(AppDbContext db) : IGenericRepository<T>
    where T : BaseEntity
{
    public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await db.Set<T>().FirstOrDefaultAsync(e => e.Id == id, ct);

    public IQueryable<T> GetAll() => db.Set<T>();

    public async Task AddAsync(T entity, CancellationToken ct = default) =>
        await db.Set<T>().AddAsync(entity, ct);

    public void Update(T entity) => db.Set<T>().Update(entity);

    public void Delete(T entity) => db.Set<T>().Remove(entity);
}
```

---

## Problem Solved
You write one generic class once instead of writing separate repository boilerplate for 50 different tables.

---

## New Problem Introduced
Business rules are now repeated across controllers and endpoints:

```csharp
// Repeated across 20 different endpoints or controllers:
var activeProducts = await _repo.GetAll()
    .Where(p => p.IsActive)
    .ToListAsync();
```

If the business rule changes (for example, we now need `p.IsActive && !p.IsDeleted`), you have to locate and update 20 different files.

---

## Next Step
Move repeated business rules into a single source of truth: the [Service Layer](./03-service-layer.md).
