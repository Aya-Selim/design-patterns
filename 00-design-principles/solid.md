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


