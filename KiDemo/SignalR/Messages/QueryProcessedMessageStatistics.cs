namespace KiDemo.SignalR.Messages;

public class QueryProcessedMessageStatistics
{
	public long InTokenCount { get; set; }
	public long ProcessingTokenCount { get; set; }
	public double ProcessingDurationMs { get; set; }
	public string ModelVersion { get; set; } = string.Empty;
}