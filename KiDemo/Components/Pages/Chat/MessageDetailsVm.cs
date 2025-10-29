namespace KiDemo.Components.Pages.Chat;

public class MessageDetailsVm(Guid id, string messageContent)
{
	public Guid Id { get; } = id;
	public string MessageContent { get; } = messageContent;
}