namespace KiDemo.Components.Pages.Chat;

internal class MessageItem(Guid id, string displayName, string messageContent, string messageReply, string messageMetadata)
{
	public Guid Id { get; } = id;
	public string DisplayName { get; } = displayName;
	public string MessageContent { get; } = messageContent;
	public string MessageReply { get; } = messageReply;
	public string MessageMetadata { get; } = messageMetadata;
}