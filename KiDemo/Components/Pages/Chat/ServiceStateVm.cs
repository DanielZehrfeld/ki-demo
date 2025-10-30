namespace KiDemo.Components.Pages.Chat;

internal class ServiceStateVm(bool isConnected, bool isProcessing, bool isSubmitEnabled, int messageCount)
{
	public bool IsConnected { get; } = isConnected;
	public bool IsProcessing { get; } = isProcessing;
	public bool IsSubmitEnabled { get; } = isSubmitEnabled;
	public int MessageCount { get; } = messageCount;
}