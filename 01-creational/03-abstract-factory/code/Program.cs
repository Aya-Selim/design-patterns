using Microsoft.Extensions.DependencyInjection;

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("======================================================");
Console.WriteLine("   Abstract Factory Pattern (.NET 8) Demonstration   ");
Console.WriteLine("======================================================");
Console.ResetColor();

// ============================================================================
// PART 1: Classic GoF Abstract Factory Pattern
// ============================================================================
Console.WriteLine("\n--- Part 1: Classic GoF Abstract Factory Pattern ---");

// Client receives factory family; all products created belong to the same cloud provider
Console.WriteLine("\n[Scenario A: AWS Cloud Infrastructure]");
ICloudResourceFactory awsFactory = new AwsResourceFactory();
var awsBackupService = new CloudBackupService(awsFactory);
awsBackupService.PerformBackup("invoice_2026_01.pdf", [0x25, 0x50, 0x44, 0x46]);

Console.WriteLine("\n[Scenario B: Azure Cloud Infrastructure]");
ICloudResourceFactory azureFactory = new AzureResourceFactory();
var azureBackupService = new CloudBackupService(azureFactory);
azureBackupService.PerformBackup("invoice_2026_01.pdf", [0x25, 0x50, 0x44, 0x46]);

// ============================================================================
// PART 2: ASP.NET Core DI Configuration-Based Resolution
// ============================================================================
Console.WriteLine("\n--- Part 2: Modern ASP.NET Core DI (Config-Driven Factory) ---");

var services = new ServiceCollection();

// Simulate reading "CloudProvider" setting from appsettings.json ("AWS" or "Azure")
string configuredProvider = "Azure";

// Register the appropriate factory family based on configuration
switch (configuredProvider.ToUpperInvariant())
{
    case "AWS":
        services.AddSingleton<ICloudResourceFactory, AwsResourceFactory>();
        break;
    case "AZURE":
        services.AddSingleton<ICloudResourceFactory, AzureResourceFactory>();
        break;
    default:
        throw new InvalidOperationException($"Unsupported provider: {configuredProvider}");
}

services.AddTransient<CloudBackupService>();

var provider = services.BuildServiceProvider();
var backupService = provider.GetRequiredService<CloudBackupService>();

Console.WriteLine($"[DI Configured: {configuredProvider}] Executing automated backup:");
backupService.PerformBackup("daily_db_dump.sql", [0x53, 0x51, 0x4C]);

// ============================================================================
// PART 3: Modern .NET 8 Keyed Services as Multi-Tenant Abstract Factories
// ============================================================================
Console.WriteLine("\n--- Part 3: Modern .NET 8 Keyed Services (Multi-Tenant Cloud Families) ---");

var keyedServices = new ServiceCollection();

// Register each family of services under a specific tenant or environment key
keyedServices.AddKeyedSingleton<ICloudResourceFactory, AwsResourceFactory>("aws");
keyedServices.AddKeyedSingleton<ICloudResourceFactory, AzureResourceFactory>("azure");

var keyedProvider = keyedServices.BuildServiceProvider();

// Simulating multi-tenant routing where Tenant A uses AWS and Tenant B uses Azure
string[] tenants = ["aws", "azure"];

foreach (var tenantKey in tenants)
{
    var tenantFactory = keyedProvider.GetRequiredKeyedService<ICloudResourceFactory>(tenantKey);
    var tenantService = new CloudBackupService(tenantFactory);

    Console.WriteLine($"\n[Tenant Context: '{tenantKey}']");
    tenantService.PerformBackup($"tenant_{tenantKey}_report.csv", [0x43, 0x53, 0x56]);
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\nDemonstration completed successfully!");
Console.ResetColor();

// ============================================================================
// DOMAIN, PRODUCT INTERFACES & ABSTRACT FACTORY
// ============================================================================

/// <summary>
/// Abstract Product A: Storage service contract.
/// </summary>
public interface IStorageService
{
    string ProviderName { get; }
    void UploadFile(string fileName, byte[] content);
}

/// <summary>
/// Abstract Product B: Message queue service contract.
/// </summary>
public interface IMessageQueue
{
    string ProviderName { get; }
    void Enqueue(string queueName, string payload);
}

// ----------------------------------------------------------------------------
// Concrete Products for Family 1: AWS
// ----------------------------------------------------------------------------
public class AwsS3Storage : IStorageService
{
    public string ProviderName => "Amazon S3";

    public void UploadFile(string fileName, byte[] content) =>
        Console.WriteLine($"  [AWS S3] Uploaded '{fileName}' ({content.Length} bytes) to S3 bucket.");
}

public class AwsSqsQueue : IMessageQueue
{
    public string ProviderName => "Amazon SQS";

    public void Enqueue(string queueName, string payload) =>
        Console.WriteLine($"  [AWS SQS] Published message to queue '{queueName}': {payload}");
}

// ----------------------------------------------------------------------------
// Concrete Products for Family 2: Azure
// ----------------------------------------------------------------------------
public class AzureBlobStorage : IStorageService
{
    public string ProviderName => "Azure Blob Storage";

    public void UploadFile(string fileName, byte[] content) =>
        Console.WriteLine($"  [Azure Blob] Stored '{fileName}' ({content.Length} bytes) in Blob container.");
}

public class AzureServiceBusQueue : IMessageQueue
{
    public string ProviderName => "Azure Service Bus";

    public void Enqueue(string queueName, string payload) =>
        Console.WriteLine($"  [Azure Service Bus] Sent message to queue '{queueName}': {payload}");
}

// ============================================================================
// THE ABSTRACT FACTORY INTERFACE
// ============================================================================

/// <summary>
/// Declares creation methods for each product in the family.
/// Products created by the same factory are guaranteed to be compatible.
/// </summary>
public interface ICloudResourceFactory
{
    IStorageService CreateStorage();
    IMessageQueue CreateQueue();
}

// ----------------------------------------------------------------------------
// Concrete Factories
// ----------------------------------------------------------------------------
public class AwsResourceFactory : ICloudResourceFactory
{
    public IStorageService CreateStorage() => new AwsS3Storage();
    public IMessageQueue CreateQueue() => new AwsSqsQueue();
}

public class AzureResourceFactory : ICloudResourceFactory
{
    public IStorageService CreateStorage() => new AzureBlobStorage();
    public IMessageQueue CreateQueue() => new AzureServiceBusQueue();
}

// ============================================================================
// CLIENT CODE
// ============================================================================

/// <summary>
/// Client class that depends ONLY on the abstract factory and abstract products.
/// It has zero knowledge of concrete AWS or Azure types.
/// </summary>
public class CloudBackupService(ICloudResourceFactory factory)
{
    private readonly IStorageService _storage = factory.CreateStorage();
    private readonly IMessageQueue _queue = factory.CreateQueue();

    public void PerformBackup(string fileName, byte[] data)
    {
        Console.WriteLine($"Initiating backup workflow using {_storage.ProviderName} and {_queue.ProviderName}:");
        _storage.UploadFile(fileName, data);
        _queue.Enqueue("backup-notifications", $"File '{fileName}' successfully backed up.");
    }
}
