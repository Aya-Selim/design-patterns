# CQRS and MediatR

## What is CQRS?
**CQRS** stands for **Command Query Responsibility Segregation**.

Instead of having a single monolithic service that handles both reading and writing, CQRS splits the responsibility:
- **Command**: Changes the state of the application (Create, Update, Delete). It performs an action and does not return full query models.
- **Query**: Reads data without modifying state (GetById, List, Search). It has zero side-effects.

```
                  ┌──────────────┐
                  │ HTTP Request │
                  └──────┬───────┘
                         │
             ┌───────────┴───────────┐
             ▼                       ▼
      [Command Model]          [Query Model]
      Writes / State Changes   Fast Reads / Projections
             │                       │
             ▼                       ▼
      Database (Write)       Database / Read Replica
```

---

## CQRS with MediatR (.NET 8)

The `MediatR` library provides a popular in-process implementation of the Mediator pattern to dispatch commands and queries to their respective handlers.

> [!WARNING]
> **Check License Terms:** Always check current MediatR license terms before using it in commercial enterprise projects. Alternative implementations include custom interfaces (`IQueryHandler<TQuery, TResult>`) or lightweight libraries like Wolverine.

### 1. The Query (Request)
```csharp
// Immutable request record
public record GetProductByIdQuery(int Id) : IRequest<ProductResponseDto?>;
```

### 2. The Handler
```csharp
// Single-responsibility handler using C# 12 primary constructor
public class GetProductByIdHandler(IGenericRepository<Product> repo)
    : IRequestHandler<GetProductByIdQuery, ProductResponseDto?>
{
    public async Task<ProductResponseDto?> Handle(
        GetProductByIdQuery request,
        CancellationToken ct)
    {
        return await repo.GetAll()
            .Where(p => p.Id == request.Id && p.IsActive)
            .Select(p => new ProductResponseDto(p.Id, p.Name, p.Price))
            .FirstOrDefaultAsync(ct);
    }
}
```

### 3. The Minimal API Endpoint
```csharp
app.MapGet("/products/{id:int}", async (int id, IMediator mediator) =>
{
    var result = await mediator.Send(new GetProductByIdQuery(id));
    return result is not null ? Results.Ok(result) : Results.NotFound();
});
```

---

## Key Architecture Rules with CQRS Handlers

1. **A Handler replaces a Service method**: Because a handler is dedicated to one single request, it can inject the repository directly.
2. **Handlers return DTOs**: Never return EF Core entities from a query handler.
3. **No God Services**: Each handler does one thing. This adheres to the **Single Responsibility Principle**.

---

## New Problem Introduced
What happens when a single user action touches multiple different domains or entities?  
*(e.g., "Place Order" must: create order record, reduce product stock, charge payment, and email confirmation).*

If handlers start calling each other directly, you risk messy spaghetti code and circular dependencies.

---

## Next Step
Coordinate multi-step business operations safely using an [Orchestrator](./06-orchestrator.md).
