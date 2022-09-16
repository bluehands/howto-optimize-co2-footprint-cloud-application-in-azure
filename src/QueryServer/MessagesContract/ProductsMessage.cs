namespace MessagesContract;

public record ProductsMessage(
    Guid CorrelationId,
    List<ProductItem> ProductItems);