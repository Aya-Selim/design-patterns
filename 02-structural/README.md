# Structural Design Patterns

Structural design patterns explain **how to assemble objects and classes into larger structures** while keeping these structures flexible and efficient.

---

## Patterns in this Category

| # | Pattern | One-Line Summary | Status |
|---|---|---|:---:|
| 01 | Adapter | Allows objects with incompatible interfaces to collaborate. | ⏳ Todo |
| 02 | Decorator | Attaches new behaviors to objects dynamically by placing them inside wrapper objects. | ⏳ Todo |
| 03 | Facade | Provides a simplified, high-level interface to a complex library or subsystem. | ⏳ Todo |
| 04 | Proxy | Provides a substitute or placeholder for another object to control access to it. | ⏳ Todo |
| 05 | Composite | Composes objects into tree structures to represent part-whole hierarchies. | ⏳ Todo |
| 06 | Bridge | Splits a large class or set of closely related classes into two separate hierarchies (abstraction and implementation). | ⏳ Todo |
| 07 | Flyweight | Fits more objects into RAM by sharing common parts of state among multiple objects. | ⏳ Todo |

---

## Structural Patterns in Modern ASP.NET Core

- **Decorator**: Easily implemented using the popular library `Scrutor` (`services.Decorate<IService, CachedService>()`) or manual DI composition.
- **Facade**: Commonly used to aggregate multiple domain services behind one high-level service for an endpoint.
- **Adapter**: Used when integrating third-party SDKs, payment gateways, or legacy libraries with clean application interfaces.
- **Proxy**: ASP.NET Core and EF Core use proxies for lazy loading and dynamic method interception.
