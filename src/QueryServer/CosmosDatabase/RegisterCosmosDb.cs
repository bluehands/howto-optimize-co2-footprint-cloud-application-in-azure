using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CosmosDatabase;

public static class RegisterCosmosDb
{
    public static IServiceCollection AddCosmosRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<CosmosDbContext>(options =>
            options.UseCosmos(
                "Secret HERE", 
                "WebShop1")
        );
        serviceCollection.AddScoped<IReadonlyProductRepository, CosmosReadonlyProductRepository>();
        return serviceCollection;
    }
}