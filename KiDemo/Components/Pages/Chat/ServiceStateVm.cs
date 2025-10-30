namespace KiDemo.Components.Pages.Chat;

internal class ServiceStateVm(bool isSubmitEnabled, int tokenCount)
{
	public bool IsSubmitEnabled { get; } = isSubmitEnabled;
	public int TokenCount { get; } = tokenCount;
}