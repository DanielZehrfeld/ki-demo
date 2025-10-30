namespace KiDemo.Components.Pages.Chat;

internal class MessageItemVm(Guid id, string displayName)
{
	public Guid Id { get; } = id;
	public string DisplayName { get; } = displayName;
}