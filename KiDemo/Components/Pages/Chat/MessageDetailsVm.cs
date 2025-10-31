namespace KiDemo.Components.Pages.Chat;

internal class MessageDetailsVm(string messageContent, string messageReply, string messageMetadata, string utcTimestampString)
{
	public string MessageContent { get; } = messageContent;
	public string MessageReply { get; } = messageReply;
	public string MessageMetadata{ get; } = messageMetadata;
	public string UtcTimestampString { get; } = utcTimestampString;
}