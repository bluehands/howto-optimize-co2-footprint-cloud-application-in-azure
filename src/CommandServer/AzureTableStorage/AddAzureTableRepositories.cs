using Application;
using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;

namespace AzureTableStorage;

public static class AddAzureTableRepositories
{
    public static IServiceCollection RegisterAzureTableRepositories(
        this IServiceCollection serviceCollection, string connectionString, string tableName)
    {
        serviceCollection.AddSingleton(new TableServiceClient(connectionString));
        serviceCollection.AddSingleton<IBasketAddedItemRepository, BasketAddedItemRepository>();
        serviceCollection.AddSingleton(new TableConfig() { TableName = tableName });
        return serviceCollection;
    }
}