namespace KiDemo.Backend.Dto;

internal class BackendMessage(int number, string messageContent, string messageReply, BackendMessageStatistics statistics)
{
	public int Number { get; } = number;
	public string MessageContent { get; } = messageContent;
	public string MessageReply { get; } = messageReply;
	public BackendMessageStatistics Statistics { get; } = statistics;
}