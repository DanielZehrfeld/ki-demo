namespace KiDemo.SignalR.Messages;

public class ClientCommand
{
    public ExecuteType ExecuteType { get; set; }
    public int ClientId { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public override string ToString() => $"Client: {ClientId:X}, Topic: {Topic}, Content: {Content}";
}