# Behavioral Design Patterns

Behavioral design patterns are concerned with **algorithms and the assignment of responsibilities between objects**. They characterize complex control flow that is difficult to follow at runtime.

---

## Patterns in this Category

| # | Pattern | One-Line Summary | Status |
|---|---|---|:---:|
| 01 | Strategy | Defines a family of algorithms, encapsulates each one, and makes them interchangeable. | ⏳ Todo |
| 02 | Observer | Defines a subscription mechanism to notify multiple objects about events happening to the observed object. | ⏳ Todo |
| 03 | Command | Turns a request into a stand-alone object containing all information about the request. | ⏳ Todo |
| 04 | State | Allows an object to alter its behavior when its internal state changes. | ⏳ Todo |
| 05 | Template Method | Defines the skeleton of an algorithm in the superclass but lets subclasses override specific steps. | ⏳ Todo |
| 06 | Chain of Responsibility | Passes requests along a chain of handlers until one handles it. | ⏳ Todo |
| 07 | Mediator | Restricts direct communications between objects and forces them to collaborate through a mediator object. | ⏳ Todo |
| 08 | Iterator | Traverses elements of a collection without exposing its underlying representation. | ⏳ Todo |
| 09 | Memento | Saves and restores the previous state of an object without revealing implementation details. | ⏳ Todo |
| 10 | Visitor | Separates algorithms from the objects on which they operate. | ⏳ Todo |

---

## Behavioral Patterns in Modern ASP.NET Core

- **Chain of Responsibility**: Powers the ASP.NET Core **Middleware Pipeline** (`RequestDelegate next`).
- **Mediator**: Popularized by `MediatR` for CQRS architectures (in-process messaging).
- **Observer**: Built into C# via `event` delegates, `IObservable<T>`, and messaging systems (RabbitMQ/Azure Service Bus).
- **Strategy**: Cleanly implemented by injecting keyed services (`[FromKeyedServices("stripe")] IPaymentStrategy`) or strategy resolver delegates.
- **Iterator**: Language-level feature via `IEnumerable<T>`, `IAsyncEnumerable<T>`, and `yield return`.
