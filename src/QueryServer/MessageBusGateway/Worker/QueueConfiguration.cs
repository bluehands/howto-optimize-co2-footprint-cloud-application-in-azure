namespace MessageBusGateway.Worker;

public class QueueConfiguration
{
    public string GatewaySendQueue { get; set; }
    public string WorkerSendQueue { get; set; }
}