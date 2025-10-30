using KiDemo.SignalR.Messages;

namespace KiDemo.Backend.Extensions;

internal static class StatisticsExt
{
	private const string QueueSizeTotalKey = "a) Queue size total";
	private const string ItemReleaseCountKey = "b) Items release count";

	private const string ItemProcessedKey = "c) Items processed";

	// private const string ItemProcessFailedKey = "d) Processing failures";
	// private const string SuspiciousContentKey = "e) Suspicious content, empty result";
	private const string WorkerCount = "f) Active workers";

	// private const string MessageLengthLastKey = "g) Message length last";
	// private const string MessageLengthAvgKey = "h) Message length avg";
	// private const string MaxPromptTokensKey = "i) Max prompt tokens";
	// private const string MaxCompletionTokensKey = "j) Max result tokens";
	private const string UsagePromptTokensTotalKey = "k) Tokens total (prompt)";

	private const string UsageCompletionTokensTotalKey = "l) Tokens total (result)";
	// private const string UsagePromptTokensLastKey = "m) Tokens last (prompt)";
	// private const string UsageCompletionTokensLastKey = "n) Tokens last (result)";
	// private const string LogClientCountKey = "o) Clients count total";
	// private const string ReturnCommand = "p) Command count of: ";

	public const string BreakTime = "f) Break until: ";

	public static long? GetQueueCount(this Statistics statistics) 
		=> statistics.Values.Get(QueueSizeTotalKey);

	private static long? Get(this StatisticValue[] statisticsValues, string key) 
		=> statisticsValues.FirstOrDefault(s => s.Key == key)?.Value;
}
