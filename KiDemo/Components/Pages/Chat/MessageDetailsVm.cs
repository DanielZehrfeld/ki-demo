namespace KiDemo.Components.Pages.Chat;

public class MessageDetailsVm(Guid id, string messageContent, string messageMetadata)
{
	public Guid Id { get; } = id;
	public string MessageContent { get; } = messageContent;
	public string MessageMetadata{ get; } = messageMetadata;
}