# General Design Principles

## 1. Encapsulate What Varies

Identify the parts of the system that are likely to change and separate them from the parts that stay the same.

This allows changes to one part without affecting the rest of the system.

---

## 2. Program to an Interface, Not an Implementation

Depend on abstractions (interfaces or abstract classes) rather than concrete implementations.

This reduces coupling and makes the code more flexible and easier to change.

> More flexibility often comes with additional complexity, so avoid adding abstractions without a real need.

---

## 3. Favor Composition Over Inheritance

Prefer composing objects from smaller components instead of relying on inheritance when it provides a more flexible design.

### Problems with inheritance

- A child class inherits all accessible behavior from its parent, even if some of it is not appropriate.
- It creates **tight coupling** between the child and parent.
- Changes in the parent can affect child classes.

### When is inheritance useful?

Inheritance is useful when there is a true **"is-a" relationship** and we need:
- **Polymorphism**
- **Generalization**
- Shared behavior that genuinely belongs to the base type.

### Composition

Composition represents a **"has-a" relationship** and allows behavior to be built from smaller, independent components.

It is often preferred when we want:
- Lower coupling
- Greater flexibility
- Easier changes to behavior

> **Inheritance:** IS-A  
> **Composition:** HAS-A