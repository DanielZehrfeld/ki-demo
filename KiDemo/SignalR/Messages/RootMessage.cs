namespace KiDemo.SignalR.Messages;

public class RootMessage
{
    public DateTimeOffset Timestamp { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public override string ToString() => $"Time: {Timestamp}, Topic: {Topic}, Content: {Content}";
}