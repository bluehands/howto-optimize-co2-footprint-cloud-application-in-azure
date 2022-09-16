using System.Collections.Immutable;
using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CosmosDatabase;

public class CosmosReadonlyProductRepository : IReadonlyProductRepository
{
    private readonly ILogger<CosmosReadonlyProductRepository> logger;
    private readonly CosmosDbContext cosmosDbContext;

    public CosmosReadonlyProductRepository(ILogger<CosmosReadonlyProductRepository> logger,CosmosDbContext cosmosDbContext)
    {
        this.logger = logger;
        this.cosmosDbContext = cosmosDbContext;
    }

    public async Task<IReadOnlyCollection<Product>> GetPageOfProducts(int pageSize)
    {
        logger.LogInformation("DB Access");
        return (await cosmosDbContext.Products.Take(pageSize).ToListAsync()).ToImmutableArray();
    }
}

