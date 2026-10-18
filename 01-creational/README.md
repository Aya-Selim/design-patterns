# Creational Design Patterns

Creational design patterns focus on **how objects are created**. 

Directly instantiating objects throughout an application with `new ClassName()` causes tight coupling. Creational patterns encapsulate object creation logic, making your system independent of how its objects are created, composed, and represented.

---

## Patterns in this Category

| # | Pattern | One-Line Summary | Status |
|---|---|---|:---:|
| 01 | [Singleton](./01-singleton/README.md) | Guarantees a class has only one instance and provides a global point of access to it. | ✅ Done |
| 02 | [Factory Method](./02-factory-method/README.md) | Defines an interface for creating an object, letting subclasses decide which class to instantiate. | ✅ Done |
| 03 | [Abstract Factory](./03-abstract-factory/README.md) | Provides an interface for creating families of related or dependent objects without specifying concrete classes. | ✅ Done |
| 04 | [Builder](./04-builder/README.md) | Separates the construction of a complex object from its representation. | ⏳ Todo |
| 05 | [Prototype](./05-prototype/README.md) | Creates new objects by copying an existing instance (prototyping). | ⏳ Todo |

---

## Creational Patterns in Modern ASP.NET Core

In modern ASP.NET Core (.NET 8), many classic creational patterns are handled by the framework:
- **Dependency Injection (DI)** container manages object instantiation and lifetimes (`AddSingleton`, `AddScoped`, `AddTransient`).
- **Options Pattern** (`IOptions<T>`, `IOptionsSnapshot<T>`) creates and binds strongly typed configuration objects.
- **Factory delegates and keyed services** (`AddKeyedSingleton`, `IServiceProvider.GetKeyedService`) replace complex factory classes.
