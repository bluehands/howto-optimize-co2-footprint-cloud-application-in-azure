using Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InMemoryDatabase;

public static class RegisterImMemoryDb
{
    public static IServiceCollection AddInMemoryRepositories(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IReadonlyProductRepository, InMemoryReadonlyProductRepository>();
        return serviceCollection;
    }
}