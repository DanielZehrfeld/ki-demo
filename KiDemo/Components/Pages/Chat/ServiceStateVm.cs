namespace KiDemo.Components.Pages.Chat;

public class ServiceStateVm(bool isSubmitEnabled, int tokenCount)
{
	public bool IsSubmitEnabled { get; } = isSubmitEnabled;
	public int TokenCount { get; } = tokenCount;
}