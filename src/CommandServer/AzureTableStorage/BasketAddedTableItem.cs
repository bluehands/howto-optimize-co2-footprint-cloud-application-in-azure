using Application;
using Azure;
using Azure.Data.Tables;

namespace AzureTableStorage;

public class BasketAddedTableItem : ITableEntity

{
    public BasketAddedTableItem(BasketAddedItem basketAddedItem, string partitionKey, string rowKey,
        DateTimeOffset? timestamp, ETag eTag)
    {
        CorrelationId = basketAddedItem.CorrelationId;
        ItemId = basketAddedItem.ItemId;
        BasketId = basketAddedItem.BasketId;
        Amount = basketAddedItem.Amount;
        PartitionKey = partitionKey;
        RowKey = rowKey;
        Timestamp = timestamp;
        ETag = eTag;
    }

    public BasketAddedTableItem()
    {
    }


    public Guid CorrelationId { get; set; }
    public Guid ItemId { get; set; }
    public Guid BasketId { get; set; }
    public int Amount { get; set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public BasketAddedItem ToBasketAddedItem()
    {
        return new BasketAddedItem
        {
            Amount = Amount,
            BasketId = BasketId,
            CorrelationId = CorrelationId,
            ItemId = ItemId
        };
    }
}