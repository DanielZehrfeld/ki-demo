using KiDemo.Backend;

namespace KiDemo.Components.Pages.Chat;

internal class MessageItemVm(Guid id, MessageType messageType, string displayName)
{
	public Guid Id { get; } = id;
	public MessageType MessageType { get; } = messageType;
	public string DisplayName { get; } = displayName;
}