# Orchestrator

## What is an Orchestrator?
An **Orchestrator** is a specialized command handler whose only job is to **coordinate a multi-step business workflow** across multiple domains and entities.

For example, when a user clicks **"Place Order"**, four things must happen in sequence:
1. Validate & create the order record.
2. Reduce warehouse inventory.
3. Process credit card payment.
4. Dispatch order confirmation email.

Without an orchestrator, you either write a giant 500-line controller, or make domain handlers depend on each other in a tangled web.

---

## The Golden Rules of an Orchestrator

1. **It is a standard command handler**: It implements `IRequestHandler<TCommand, TResult>`.
2. **It NEVER injects repositories directly**: It coordinates business actions by dispatching commands and queries through `IMediator`.
3. **Each sub-step remains isolated and reusable**: `CreateOrderCommand`, `ReduceStockCommand`, and `ProcessPaymentCommand` can each be tested and used independently.

---

## Implementation Example (.NET 8)

```csharp
public record PlaceOrderCommand(
    int CustomerId,
    List<OrderItemDto> Items,
    PaymentDetailsDto PaymentInfo) : IRequest<OrderResponseDto>;

public class PlaceOrderOrchestrator(IMediator mediator)
    : IRequestHandler<PlaceOrderCommand, OrderResponseDto>
{
    public async Task<OrderResponseDto> Handle(
        PlaceOrderCommand request,
        CancellationToken ct)
    {
        // Step 1: Create the pending order record
        var order = await mediator.Send(
            new CreateOrderCommand(request.CustomerId, request.Items), ct);

        // Step 2: Reserve inventory
        await mediator.Send(
            new ReserveStockCommand(request.Items), ct);

        // Step 3: Process the payment
        await mediator.Send(
            new ProcessPaymentCommand(order.Id, request.PaymentInfo), ct);

        // Step 4: Publish notification event or command
        await mediator.Send(
            new SendOrderConfirmationEmailCommand(order.Id), ct);

        return order;
    }
}
```

---

## Can an Orchestrator Call Another Orchestrator?

**Yes, but strictly follow a unidirectional hierarchy with NO circular dependencies:**

- **Top-Down Flow Only**: A high-level macro orchestrator (`CheckoutOrchestrator`) may invoke a smaller sub-orchestrator (`FulfillmentOrchestrator`). A sub-orchestrator must **never** call back into a parent orchestrator.
- **Avoid Cycles**: `A -> B -> A` causes infinite recursion and crashes your application with a `StackOverflowException`.
- **Extract Shared Steps**: If two orchestrators need identical logic, extract those common steps into a dedicated **Command** rather than having orchestrators call each other sideways.

---

## Complete Architecture Summary

Looking back across our full architecture journey:

```
[Controller / Minimal API]
         │
         ▼ (Dispatches Request)
  [Orchestrator] ──(Dispatches sub-commands)──► [Command Handlers]
         │                                              │
         ▼ (Reads projections)                          ▼ (Mutates domain)
  [Query Handlers]                              [Domain Entities]
         │                                              │
         ▼ (Uses Specification Pattern)                 ▼ (Persists)
  [Generic Repository] ────────────────────────► [DbContext / SQL]
```

By separating concerns at each layer:
- **Repositories** handle database queries without business leaks.
- **Specifications** encapsulate query filters cleanly.
- **Services / Handlers** encapsulate single-domain business rules.
- **DTOs** protect schemas and contracts.
- **Orchestrators** coordinate complex cross-domain workflows cleanly.
