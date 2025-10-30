namespace KiDemo.SignalR.Messages;

public class ServiceStateMessage
{
    public ServiceStateRunValue RunState { get; set; } = ServiceStateRunValue.Stopped;
    public AiModelType Model { get; set; } = AiModelType.NONE;
    public int WorkerCount { get; set; } = -1;
}