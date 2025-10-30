namespace KiDemo.Backend.Dto;

internal class BackendState(int totalTokenCount, bool isSubmitEnabled)
{
	public int TotalTokenCount { get; } = totalTokenCount;
	public bool IsSubmitEnabled { get; } = isSubmitEnabled;
}