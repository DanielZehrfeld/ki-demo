namespace KiDemo.Backend.Dto;

internal class BackendMessage(BackendMessageStatistics statistics)
{
	public BackendMessageStatistics Statistics { get; } = statistics;
}