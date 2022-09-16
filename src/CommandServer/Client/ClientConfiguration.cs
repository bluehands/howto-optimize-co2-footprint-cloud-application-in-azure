namespace Client;

public class ClientConfiguration
{
    public string QueueName { get; set; }
    public string ConnectionString { get; set; }
    public int Iterations { get; set; }
    public int AmountOfMessages { get; set; }
    public int PerMillisecond { get; set; }
}