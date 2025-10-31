namespace KiDemo.SignalR.Messages;

public class QueryProcessedMessage
{
	public DateTimeOffset Timestamp { get; set; }
	public int QueryId { get; set; } = -1;
    public CommandMessage[] Commands { get; set; } = [];
    public string RawContent { get; set; } = string.Empty;
    public string QueryMessageText { get; set; } = string.Empty;
    public QueryProcessedMessageStatistics Statistics { get; set; } = new();
}