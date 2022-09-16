namespace Contract;

public class BasketAddedItemMessage
{
    public Guid CorrelationId { get; set; }
    public Guid ItemId { get; set; }
    public Guid BasketId { get; set; }
    public int Amount { get; set; }
    public DateTime SentAt { get; set; }
}