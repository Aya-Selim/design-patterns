# Design Patterns & Architecture Reference (.NET 8)

> A personal, practical reference for Gang of Four (GoF) design patterns and clean backend application architecture in **C# and ASP.NET Core (.NET 8)**.

---

## 🎯 Purpose

This repository documents software design patterns and architectural concepts with a focus on real-world backend engineering:
- **Simple, clear explanations** without unnecessary jargon.
- **Problem-first approach**: Each pattern starts with a bad example before showing the solution.
- **ASP.NET Core integration**: How patterns map to Dependency Injection, Middleware, EF Core, and modern C# 12 / .NET 8 features.
- **Runnable code samples**: Every pattern includes a small, runnable .NET 8 console project inside its `code/` folder.

---

## 🗺️ Roadmap & Progress Tracker

### 00. Design Principles & SOLID
| Topic | Description | Status |
|---|---|:---:|
| [Introduction](./00-design-principles/README.md) | What patterns are and why we learn them | ✅ Done |
| [General Principles](./00-design-principles/general-principles.md) | Encapsulate what varies, interfaces, composition over inheritance | ✅ Done |
| [SOLID Principles](./00-design-principles/solid.md) | SRP, OCP, LSP, ISP, DIP (and DIP vs IoC vs DI) | ✅ Done |

---

### 01. Creational Patterns
*How objects are created, keeping creation flexible and decoupled from consumption.*

| # | Pattern | Key Concept | Status |
|---|---|---|:---:|
| 01 | [Singleton](./01-creational/01-singleton/README.md) | Ensure one instance with global access point; `AddSingleton` in DI | ✅ Done |
| 02 | [Factory Method](./01-creational/02-factory-method/README.md) | Subclasses decide which class to instantiate | ✅ Done |
| 03 | [Abstract Factory](./01-creational/03-abstract-factory/README.md) | Create families of related or dependent objects | ✅ Done |
| 04 | Builder | Step-by-step construction of complex objects | ⏳ Todo |
| 05 | Prototype | Clone existing objects without coupling to concrete classes | ⏳ Todo |

---

### 02. Structural Patterns
*How objects and classes are composed to form larger, flexible structures.*

| # | Pattern | Key Concept | Status |
|---|---|---|:---:|
| 01 | Adapter | Convert interface of a class into another interface clients expect | ⏳ Todo |
| 02 | Decorator | Attach new behaviors dynamically to objects | ⏳ Todo |
| 03 | Facade | Provide a unified, simple interface to a complex subsystem | ⏳ Todo |
| 04 | Proxy | Provide a surrogate or placeholder to control access to an object | ⏳ Todo |
| 05 | Composite | Compose objects into tree structures to represent part-whole hierarchies | ⏳ Todo |
| 06 | Bridge | Decouple an abstraction from its implementation so both can vary | ⏳ Todo |
| 07 | Flyweight | Share common state among multiple objects to save memory | ⏳ Todo |

---

### 03. Behavioral Patterns
*How objects communicate, distribute responsibility, and handle algorithms.*

| # | Pattern | Key Concept | Status |
|---|---|---|:---:|
| 01 | Strategy | Define a family of algorithms and make them interchangeable | ⏳ Todo |
| 02 | Observer | One-to-many dependency notification when state changes | ⏳ Todo |
| 03 | Command | Encapsulate a request as an object | ⏳ Todo |
| 04 | State | Alter behavior when internal state changes | ⏳ Todo |
| 05 | Template Method | Define skeleton of an algorithm in a base method | ⏳ Todo |
| 06 | Chain of Responsibility | Pass request along a chain of handlers (ASP.NET Core middleware) | ⏳ Todo |
| 07 | Mediator | Reduce chaotic dependencies with a central communication hub | ⏳ Todo |
| 08 | Iterator | Access elements of a collection sequentially without exposing representation | ⏳ Todo |
| 09 | Memento | Capture and restore an object's internal state without violating encapsulation | ⏳ Todo |
| 10 | Visitor | Add new operations to existing object structures without modifying them | ⏳ Todo |

---

### 04. Application Architecture in Practice
*How design patterns and clean architecture principles solve real backend problems.*

| # | Topic | What Problem It Solves | Status |
|---|---|---|:---:|
| - | [Overview](./04-application-architecture/README.md) | Request flow, layer responsibilities, and architecture golden rules | ✅ Done |
| 01 | [Generic Repository](./04-application-architecture/01-repository.md) | Avoid duplicate CRUD data access code | ✅ Done |
| 02 | [IQueryable vs IEnumerable](./04-application-architecture/02-iqueryable-vs-ienumerable.md) | In-memory vs database execution & Specification pattern | ✅ Done |
| 03 | [Service Layer](./04-application-architecture/03-service-layer.md) | Single source of truth for business rules | ✅ Done |
| 04 | [DTO and ViewModel](./04-application-architecture/04-dto-and-viewmodel.md) | Prevent entity leaks, secure APIs, decouple schema | ✅ Done |
| 05 | [CQRS and MediatR](./04-application-architecture/05-cqrs-and-mediatr.md) | Separate reads from writes and split bloated services | ✅ Done |
| 06 | [Orchestrator](./04-application-architecture/06-orchestrator.md) | Multi-entity business workflows without cyclic dependencies | ✅ Done |

---

## 🛠️ Repository Standards

1. Every pattern adheres strictly to [_template.md](./_template.md).
2. All runnable projects use **.NET 8** and follow modern C# idioms (primary constructors, file-scoped namespaces, records).
3. Automated CI builds every sample project in the repository on push.
