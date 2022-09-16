using CosmosDatabase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SeedCosmosDb;

public class SeedWorker : BackgroundService
{
    private readonly IServiceScopeFactory scopeFactory;

    public SeedWorker(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();
        var cosmosDbContext = scope.ServiceProvider.GetRequiredService<CosmosDbContext>();
        await cosmosDbContext.Products.AddRangeAsync(Enumerable.Range(0, 1000).Select(e => Fake.FakeProduct()), stoppingToken);
        await cosmosDbContext.SaveChangesAsync(stoppingToken);
    }
}