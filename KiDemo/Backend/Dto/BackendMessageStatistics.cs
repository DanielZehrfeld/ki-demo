namespace KiDemo.Backend.Dto;

internal class BackendMessageStatistics(DateTimeOffset timestamp, int inTokens, int processTokens, double processingTimeMs, string modelVersion)
{
	public DateTimeOffset Timestamp { get; } = timestamp;
	public int InTokens { get; } = inTokens;
	public int ProcessTokens { get; } = processTokens;
	public double ProcessingTimeMs { get; } = processingTimeMs;
	public string ModelVersion { get; } = modelVersion;
}