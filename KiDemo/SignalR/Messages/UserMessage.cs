namespace KiDemo.SignalR.Messages;

public class UserMessage
{
    public string Topic { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsRequest { get; set; } = false;
    public DateTimeOffset Timestamp { get; set; } = default;
}