# SOLID Principles

| Letter | Principle | One-line meaning |
|--------|-----------|------------------|
| S | Single Responsibility | A class should have only one reason to change |
| O | Open/Closed | Open for extension, closed for modification |
| L | Liskov Substitution | Subtypes should be substitutable for their base types without breaking expected behavior |
| I | Interface Segregation | Prefer small, focused interfaces over large, fat interfaces |
| D | Dependency Inversion | High-level and low-level modules should depend on abstractions |

## S: Single Responsibility

A class should have **one reason to change**.

It does not necessarily mean that a class should do only one thing or have only one method.

If a class changes for multiple unrelated reasons, its responsibilities may need to be separated.

---

## O: Open/Closed

Software entities should be **open for extension but closed for modification**.

Add new behavior by extending the existing design rather than repeatedly modifying stable, existing code.

---

## L: Liskov Substitution

Subtypes should be usable wherever their base types are expected **without breaking the program's expected behavior**.

- A child class should honor the **contract** of its parent.
- Changing the implementation is fine, but the expected behavior and contract should not be violated.

---

## I: Interface Segregation

Clients should not be forced to depend on methods they do not use.

Prefer **small, focused interfaces** over large, "fat" interfaces.

---

## D: Dependency Inversion

- High-level modules should not depend directly on low-level modules.
- Both should depend on **abstractions**.
- Abstractions should not depend on implementation details; implementation details should depend on abstractions.

### DIP vs. IoC vs. DI (Crucial Interview Distinction)

These three terms sound similar, but they operate at different conceptual levels:

| Concept | What is it? | Simple Explanation |
|---|---|---|
| **DIP (Dependency Inversion Principle)** | High-level **Principle** | The architectural rule: Depend on abstractions (interfaces), not concrete classes. |
| **IoC (Inversion of Control)** | Design **Approach / Philosophy** | Inverting the control flow: Instead of your code creating objects and controlling everything, a framework controls lifecycle and flow ("Don't call us, we'll call you"). |
| **DI (Dependency Injection)** | Specific **Design Pattern / Technique** | A concrete way to implement IoC: Providing an object with its dependencies from the outside (usually via constructor) instead of calling `new`. |

> **In ASP.NET Core:**
> - You follow **DIP** when your service depends on an interface: `public class OrderService(IPaymentProcessor payment)`.
> - You use **DI** when ASP.NET Core supplies the `IPaymentProcessor` instance into the constructor.
> - The ASP.NET Core host container is the **IoC Container** that manages object creation and lifetimes (`AddScoped`, `AddSingleton`, `AddTransient`).



