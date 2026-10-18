using Microsoft.Extensions.DependencyInjection;

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("==================================================");
Console.WriteLine("   Factory Method Pattern (.NET 8) Demonstration  ");
Console.WriteLine("==================================================");
Console.ResetColor();

// ============================================================================
// PART 1: Classic GoF Factory Method Pattern
// ============================================================================
Console.WriteLine("\n--- Part 1: Classic GoF Factory Method Pattern ---");

// Step 5: Use the creator in client code without depending on concrete products directly
PaymentFactory creditFactory = new CreditCardPaymentFactory();
PaymentFactory payPalFactory = new PayPalPaymentFactory();

creditFactory.ExecutePayment(150.00m);
payPalFactory.ExecutePayment(75.50m);

// ============================================================================
// PART 2: Modern .NET 8 Keyed Services as a Factory
// ============================================================================
Console.WriteLine("\n--- Part 2: Modern .NET 8 Keyed Services (Built-in Factory) ---");

var services = new ServiceCollection();

// Register multiple implementations of IPaymentProcessor using unique keys
services.AddKeyedTransient<IPaymentProcessor, CreditCardProcessor>("credit");
services.AddKeyedTransient<IPaymentProcessor, PayPalProcessor>("paypal");

var serviceProvider = services.BuildServiceProvider();

// Simulating runtime decision based on user checkout input
string[] userSelections = ["credit", "paypal"];

foreach (var selection in userSelections)
{
    var processor = serviceProvider.GetRequiredKeyedService<IPaymentProcessor>(selection);
    Console.Write($"[User selected '{selection}'] => ");
    processor.Process(99.99m);
}

// ============================================================================
// PART 3: Factory Delegate in Dependency Injection
// ============================================================================
Console.WriteLine("\n--- Part 3: Factory Delegate via DI (Func<string, IPaymentProcessor>) ---");

services.AddTransient<CheckoutService>();
services.AddTransient<Func<string, IPaymentProcessor>>(sp => key => key switch
{
    "credit" => sp.GetRequiredKeyedService<IPaymentProcessor>("credit"),
    "paypal" => sp.GetRequiredKeyedService<IPaymentProcessor>("paypal"),
    _ => throw new NotSupportedException($"Unknown payment method: {key}")
});

var updatedProvider = services.BuildServiceProvider();
var checkout = updatedProvider.GetRequiredService<CheckoutService>();

checkout.Checkout("credit", 250.00m);
checkout.Checkout("paypal", 49.90m);

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\nDemonstration completed successfully!");
Console.ResetColor();

// ============================================================================
// DOMAIN & FACTORY IMPLEMENTATIONS
// ============================================================================

/// <summary>
/// Step 1: Create the common Product Interface.
/// </summary>
public interface IPaymentProcessor
{
    string ProviderName { get; }
    void Process(decimal amount);
}

/// <summary>
/// Step 2: Create Concrete Products implementing the interface.
/// </summary>
public class CreditCardProcessor : IPaymentProcessor
{
    public string ProviderName => "Credit Card";

    public void Process(decimal amount) =>
        Console.WriteLine($"Charged ${amount:F2} via Credit Card Gateway.");
}

public class PayPalProcessor : IPaymentProcessor
{
    public string ProviderName => "PayPal";

    public void Process(decimal amount) =>
        Console.WriteLine($"Charged ${amount:F2} via PayPal API.");
}

/// <summary>
/// Step 3: Create the Abstract Creator declaring the Factory Method.
/// </summary>
public abstract class PaymentFactory
{
    // The Factory Method
    public abstract IPaymentProcessor CreateProcessor();

    // Core business operation relying on the product created by the factory method
    public void ExecutePayment(decimal amount)
    {
        var processor = CreateProcessor();
        Console.WriteLine($"[Factory: {GetType().Name}] Initiating transaction with {processor.ProviderName}...");
        processor.Process(amount);
    }
}

/// <summary>
/// Step 4: Create Concrete Creators overriding the Factory Method.
/// (Step 6: Easily add new products & creators here without touching existing ones!)
/// </summary>
public class CreditCardPaymentFactory : PaymentFactory
{
    public override IPaymentProcessor CreateProcessor() => new CreditCardProcessor();
}

public class PayPalPaymentFactory : PaymentFactory
{
    public override IPaymentProcessor CreateProcessor() => new PayPalProcessor();
}

/// <summary>
/// High-level application service demonstrating factory delegate injection.
/// </summary>
public class CheckoutService(Func<string, IPaymentProcessor> paymentFactory)
{
    public void Checkout(string paymentType, decimal amount)
    {
        var processor = paymentFactory(paymentType);
        Console.WriteLine($"[CheckoutService] Processing order with {processor.ProviderName}:");
        processor.Process(amount);
    }
}
