using Application;
using Azure.Data.Tables;
using FunicularSwitch;
using FunicularSwitch.Extensions;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;
using TableEntity = Azure.Data.Tables.TableEntity;

namespace AzureTableStorage;

public class BasketAddedItemRepository : IBasketAddedItemRepository
{
    private readonly TableServiceClient tableServiceClient;
    private readonly TableConfig tableConfig;
    private static string partitionKey = "BasketAddedItem";
    private readonly TableClient tableClient;

    public BasketAddedItemRepository(
        TableServiceClient tableServiceClient,
        TableConfig tableConfig)
    {
        this.tableServiceClient = tableServiceClient;
        this.tableConfig = tableConfig;
        tableServiceClient.CreateTableIfNotExists(tableConfig.TableName);
        this.tableClient = tableServiceClient.GetTableClient(tableConfig.TableName);
    }

    public async Task<Result<Unit>> Add(BasketAddedItem basketAddedItem)
    {
        var response = await tableClient.UpsertEntityAsync(
            new BasketAddedTableItem(
                basketAddedItem,
                partitionKey,
                basketAddedItem.CorrelationId.ToString(),
                DateTimeOffset.Now,
                default!
            )
        );
        return response.IsError ? Result.Error<Unit>(response.ReasonPhrase) : No.Thing;
    }

    public async Task<Result<IReadOnlyCollection<BasketAddedItem>>> GetByBasketId(Guid basketId)
    {
        try
        {
            return Result.Ok<IReadOnlyCollection<BasketAddedItem>>(
                await tableClient
                    .QueryAsync<BasketAddedTableItem>(tableItem =>
                        tableItem.BasketId.Equals(basketId))
                    .Select(tableItem => tableItem.ToBasketAddedItem())
                    .ToArrayAsync());
        }
        catch (Exception e)
        {
            return Result.Error<IReadOnlyCollection<BasketAddedItem>>(e.ToString());
        }
    }
}