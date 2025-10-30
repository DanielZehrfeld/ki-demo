namespace KiDemo.Components.Pages.Chat;

internal class ServiceStateVm(bool isSubmitEnabled, long totalTokenCount, bool isProcessing)
{
	public bool IsSubmitEnabled { get; } = isSubmitEnabled;
	public long TotalTokenCount { get; } = totalTokenCount;
	public bool IsProcessing { get; } = isProcessing;
}