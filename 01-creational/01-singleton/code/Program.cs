using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("==================================================");
Console.WriteLine("   Singleton Pattern (.NET 8) Demonstration       ");
Console.WriteLine("==================================================");
Console.ResetColor();

// ============================================================================
// PART 1: Classic Thread-Safe Singleton using Lazy<T>
// ============================================================================
Console.WriteLine("\n--- Part 1: Classic Thread-Safe Singleton (Lazy<T>) ---");

var instanceIds = new ConcurrentBag<int>();

// Simulate 5 threads accessing the Singleton simultaneously
Parallel.For(0, 5, i =>
{
    var config = ConfigurationManager.Instance;
    instanceIds.Add(config.GetHashCode());
    Console.WriteLine($"[Thread {Environment.CurrentManagedThreadId}] Accessing Singleton: HashCode = {config.GetHashCode()}");
});

bool allSameInstance = instanceIds.Distinct().Count() == 1;
Console.WriteLine($"Result: All threads received the exact same instance? => {allSameInstance}");
Console.WriteLine($"App Name from Config: '{ConfigurationManager.Instance.Get("AppName")}'");

// ============================================================================
// PART 2: ASP.NET Core Dependency Injection (AddSingleton)
// ============================================================================
Console.WriteLine("\n--- Part 2: ASP.NET Core DI Container (AddSingleton) ---");

var services = new ServiceCollection();

// Register Cache as Singleton (one instance for entire app)
services.AddSingleton<ICacheService, InMemoryCacheService>();

// Register OrderRepository as Scoped (one instance per request/scope)
services.AddScoped<IOrderRepository, MockOrderRepository>();

// Register a Singleton worker that safely consumes Scoped services via IServiceScopeFactory
services.AddSingleton<BackgroundOrderProcessor>();

var serviceProvider = services.BuildServiceProvider();

// Simulate Request Scope 1
using (var scope1 = serviceProvider.CreateScope())
{
    var cache1 = scope1.ServiceProvider.GetRequiredService<ICacheService>();
    var repo1 = scope1.ServiceProvider.GetRequiredService<IOrderRepository>();
    cache1.Set("user_1", "Alice");

    Console.WriteLine($"[Scope 1] Cache HashCode: {cache1.GetHashCode()} | Repo Id: {repo1.ScopeId}");
}

// Simulate Request Scope 2
using (var scope2 = serviceProvider.CreateScope())
{
    var cache2 = scope2.ServiceProvider.GetRequiredService<ICacheService>();
    var repo2 = scope2.ServiceProvider.GetRequiredService<IOrderRepository>();

    Console.WriteLine($"[Scope 2] Cache HashCode: {cache2.GetHashCode()} | Repo Id: {repo2.ScopeId}");
    Console.WriteLine($"[Scope 2] Read from Singleton Cache: '{cache2.Get("user_1")}'");
}

// ============================================================================
// PART 3: Avoiding the Captive Dependency Trap
// ============================================================================
Console.WriteLine("\n--- Part 3: Safe Scoped Service Resolution in a Singleton ---");
var processor = serviceProvider.GetRequiredService<BackgroundOrderProcessor>();
await processor.ProcessPendingOrdersAsync();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\nDemonstration completed successfully!");
Console.ResetColor();

// ============================================================================
// CLASS DEFINITIONS
// ============================================================================

/// <summary>
/// Classic thread-safe Singleton using Lazy&lt;T&gt;.
/// </summary>
public sealed class ConfigurationManager
{
    private static readonly Lazy<ConfigurationManager> _lazy =
        new(() => new ConfigurationManager());

    public static ConfigurationManager Instance => _lazy.Value;

    private readonly Dictionary<string, string> _settings;

    private ConfigurationManager()
    {
        _settings = new Dictionary<string, string>
        {
            ["AppName"] = "CleanArchitecture.Api",
            ["Version"] = "1.0.0",
            ["Environment"] = "Development"
        };
    }

    public string Get(string key) => _settings.GetValueOrDefault(key, "Not Found");
}

/// <summary>
/// Cache service interface.
/// </summary>
public interface ICacheService
{
    void Set(string key, string value);
    string? Get(string key);
}

/// <summary>
/// Singleton cache implementation using thread-safe ConcurrentDictionary.
/// </summary>
public class InMemoryCacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, string> _cache = new();

    public void Set(string key, string value) => _cache[key] = value;
    public string? Get(string key) => _cache.GetValueOrDefault(key);
}

/// <summary>
/// Simulated scoped repository interface.
/// </summary>
public interface IOrderRepository
{
    Guid ScopeId { get; }
    void SaveOrder(string orderName);
}

public class MockOrderRepository : IOrderRepository
{
    public Guid ScopeId { get; } = Guid.NewGuid();

    public void SaveOrder(string orderName) =>
        Console.WriteLine($"   [MockOrderRepository-{ScopeId.ToString()[..8]}] Saved: {orderName}");
}

/// <summary>
/// Demonstrates how a Singleton safely resolves a Scoped service without
/// falling into the "Captive Dependency" trap, by injecting IServiceScopeFactory.
/// </summary>
public class BackgroundOrderProcessor(IServiceScopeFactory scopeFactory)
{
    public async Task ProcessPendingOrdersAsync()
    {
        Console.WriteLine("BackgroundOrderProcessor (Singleton) creating a scope to use Scoped IOrderRepository...");

        // Create a short-lived scope:
        using var scope = scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
        repo.SaveOrder("Order #1001");

        await Task.CompletedTask;
    }
}
