namespace KiDemo.Components.Pages.Chat
{
	internal class MessageItem(Guid id, string displayName)
	{
		public Guid Id { get; } = id;
		public string DisplayName { get; } = displayName;
	}
}
