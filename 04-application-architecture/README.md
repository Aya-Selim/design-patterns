# Application Architecture in Practice

In a modern ASP.NET Core backend, design patterns and clean architecture are not just theory. They solve specific, real-world problems as your codebase grows.

Each step in this section **solves the problem introduced by the step before it**.

---

## The Architecture Journey

| Step | Technique | Problem Solved | Solved In |
|:---:|---|---|---|
| **1** | **Generic Repository** | Repeated, boilerplate CRUD data access code | [01-repository.md](./01-repository.md) |
| **-** | **Query Strategy** | Memory leaks vs database translation | [02-iqueryable-vs-ienumerable.md](./02-iqueryable-vs-ienumerable.md) |
| **2** | **Service Layer** | Business rules scattered across call sites | [03-service-layer.md](./03-service-layer.md) |
| **3** | **DTO & ViewModel** | Database entities leaking to clients and security risks | [04-dto-and-viewmodel.md](./04-dto-and-viewmodel.md) |
| **4** | **Facade** | Endpoints needing multiple services at once | [03-facade](../02-structural/03-facade/README.md) |
| **5** | **CQRS + MediatR** | Bloated service classes with mixed read/write concerns | [05-cqrs-and-mediatr.md](./05-cqrs-and-mediatr.md) |
| **6** | **Orchestrator** | Complex workflows that touch multiple domains | [06-orchestrator.md](./06-orchestrator.md) |

---

## Request Flow

```
HTTP Request
     │
     ▼
Controller / Minimal API
     │
     ▼ (Sends Request DTO)
Service / CQRS Handler
     │
     ▼ (Queries / Persists)
Repository / DbContext
     │
     ▼ (Maps Entity to DTO)
HTTP Response (DTO only)
```

```
Controller -> Service / Handler -> Repository -> Database
     ^               │
     │               ▼
    DTO <────── Entity (never leaves the service/handler)
```

---

## 4 Golden Rules of Backend Architecture

1. **Repository = data access only**: Never put business rules or validation inside a repository.
2. **Controllers never talk to repositories**: Controllers only inject Services or Handlers.
3. **Never return an EF entity from an endpoint**: Always map to a DTO to prevent schema leaks, over-posting, and circular JSON serialization crashes.
4. **Never inject the repository of another domain**: Inject its service. Bypassing a domain's service skips that domain's business rules.
