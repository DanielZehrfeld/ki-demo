namespace KiDemo.Components.Pages.Chat;

internal class ServiceStateVm(bool isConnected, bool isProcessing, bool isSubmitEnabled)
{
	public static readonly ServiceStateVm Empty = new(
		isConnected: false,
		isProcessing: false,
		isSubmitEnabled: false);

	public bool IsConnected { get; } = isConnected;
	public bool IsProcessing { get; } = isProcessing;
	public bool IsSubmitEnabled { get; } = isSubmitEnabled;
}