namespace KiDemo.Components.Pages.Chat;

public class MessageItemVm(Guid id, string displayName)
{
	public Guid Id { get; } = id;
	public string DisplayName { get; } = displayName;
}