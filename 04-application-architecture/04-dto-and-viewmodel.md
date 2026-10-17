# DTO and ViewModel

## Why Never Return EF Core Entities from an API?

Returning database entities directly from your API endpoints is a dangerous anti-pattern. Here is why:

1. **Security & Data Exposure**: Entities often have sensitive fields (e.g., `PasswordHash`, `InternalNotes`, `TenantId`, `IsDeleted`). Returning the entity exposes internal data to the client.
2. **Over-Posting Attacks (Mass Assignment)**: If an endpoint accepts an entity directly in `[FromBody]`, malicious users can send JSON payloads containing values for properties they shouldn't edit (e.g., `"IsAdmin": true`).
3. **Tight Coupling**: Any change to your database table schema immediately breaks your public API contracts and breaks frontend clients.
4. **Serialization Loops**: Entity relationships (e.g., `Category` has `Products`, and `Product` has `Category`) cause runtime `JsonException` infinite cycle errors during serialization.

---

## The Solution: Use DTOs (Data Transfer Objects)

A DTO is a simple data container with no business logic. In modern .NET 8, C# `record` types are ideal for DTOs:

```csharp
// Response DTO
public record ProductResponseDto(int Id, string Name, decimal Price);

// Request DTO (immutable and concise)
public record CreateProductRequest(string Name, decimal Price, int CategoryId);
```

---

## Best Practices for DTOs

- **Do not reuse one generic DTO for all endpoints**: A "list products" endpoint needs 3 fields (`Id`, `Name`, `Price`), whereas a "product details" endpoint needs 15 fields. Create focused DTOs tailored to the operation.
- **Keep DTOs flat and simple**: Avoid complex inheritance hierarchies in DTOs.
- **Perform mapping in the service or handler**: Map entities to DTOs in the Service or Handler using manual mapping, extension methods, or mapping libraries.

---

## DTO vs. ViewModel

| Feature | DTO (Data Transfer Object) | ViewModel |
|---|---|---|
| **Primary Purpose** | Transfers plain data across boundaries or over HTTP | Models data shaped specifically for a **UI screen or view** |
| **Typical Usage** | REST Web APIs, gRPC, microservice messaging, handlers | ASP.NET Core MVC, Razor Pages, Blazor, WPF |
| **Contents** | Raw, clean data types (`decimal`, `DateTimeOffset`) | UI-specific data (e.g., pre-formatted `$19.99`, dropdown lists, UI toggle booleans) |

> In modern ASP.NET Core Web APIs, there are no HTML views, so developers generally use the term **DTO** (or Request/Response models) for all payload shapes.