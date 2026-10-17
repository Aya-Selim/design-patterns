# IQueryable vs IEnumerable

| | IEnumerable | IQueryable |
|---|-------------|------------|
| Where does filtering run? | In **memory** (C#) | In the **database** (SQL) |
| Data loaded | Everything first | Only what you need |


```csharp
// IEnumerable: loads ALL products, then filters in memory (slow)
IEnumerable<Product> a = db.Products.AsEnumerable();
var r1 = a.Where(p => p.Price > 100).ToList();

// IQueryable: SQL has WHERE Price > 100 (fast)
IQueryable<Product> q = db.Products;
var r2 = await q.Where(p => p.Price > 100).ToListAsync();
```

## Which one to return from the repository?
- Controllers cannot reach the repository (Service Layer blocks them).
- So the repository can return `IQueryable`, and the **service** adds filters.
- The service must run `ToListAsync()` **before** returning to the controller.

> Some teams prefer the **Specification Pattern** instead of exposing
> `IQueryable`. Both are valid.