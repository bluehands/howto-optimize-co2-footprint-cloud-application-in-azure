namespace MessagesContract;

public record ProductItem(
    int AmountAvailable,
    decimal EuroPrice,
    string? ImageSource,
    Guid ProductId,
    string ProductName);