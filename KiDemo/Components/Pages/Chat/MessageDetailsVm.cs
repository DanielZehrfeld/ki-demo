namespace KiDemo.Components.Pages.Chat;

internal class MessageDetailsVm(string messageContent, string messageReply, string messageMetadata)
{
	public string MessageContent { get; } = messageContent;
	public string MessageReply { get; } = messageReply;
	public string MessageMetadata{ get; } = messageMetadata;
}