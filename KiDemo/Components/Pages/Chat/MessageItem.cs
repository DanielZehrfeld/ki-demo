using KiDemo.Backend;

namespace KiDemo.Components.Pages.Chat;

internal class MessageItem(Guid id, int number, MessageType messageType, string displayName, string messageContent, string messageReply, string messageMetadata)
{
	public Guid Id { get; } = id;
	public int Number { get; } = number;
	public MessageType MessageType { get; } = messageType;
	public string DisplayName { get; } = displayName;
	public string MessageContent { get; } = messageContent;
	public string MessageReply { get; } = messageReply;
	public string MessageMetadata { get; } = messageMetadata;
}