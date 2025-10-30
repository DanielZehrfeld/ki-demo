namespace KiDemo.Components.Pages.Chat;

internal class ServiceStateVm(bool isSubmitEnabled, int totalTokenCount)
{
	public bool IsSubmitEnabled { get; } = isSubmitEnabled;
	public int TotalTokenCount { get; } = totalTokenCount;
}