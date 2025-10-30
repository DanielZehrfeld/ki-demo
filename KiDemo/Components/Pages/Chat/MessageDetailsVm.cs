namespace KiDemo.Components.Pages.Chat;

internal class MessageDetailsVm(Guid id, string messageContent, string messageReply, string messageMetadata)
{
	public Guid Id { get; } = id;
	public string MessageContent { get; } = messageContent;
	public string MessageReply { get; } = messageReply;
	public string MessageMetadata{ get; } = messageMetadata;
}