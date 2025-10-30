namespace KiDemo.Backend.Dto;

internal class BackendMessageStatistics(DateTimeOffset timestamp, long inTokens, long processTokens, double processingTimeMs, string modelVersion)
{
	public DateTimeOffset Timestamp { get; } = timestamp;
	public long InTokens { get; } = inTokens;
	public long ProcessTokens { get; } = processTokens;
	public double ProcessingTimeMs { get; } = processingTimeMs;
	public string ModelVersion { get; } = modelVersion;
}