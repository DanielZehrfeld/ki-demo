namespace KiDemo.Backend.Dto;

internal class BackendState(long totalTokenCount, bool isSubmitEnabled, bool isProcessing)
{
	public long TotalTokenCount { get; } = totalTokenCount;
	public bool IsSubmitEnabled { get; } = isSubmitEnabled;
	public bool IsProcessing { get; } = isProcessing;


	protected bool Equals(BackendState other)
	{
		return TotalTokenCount == other.TotalTokenCount &&
		       IsSubmitEnabled == other.IsSubmitEnabled &&
		       IsProcessing == other.IsProcessing;
	}

	public override bool Equals(object? obj)
	{
		if (obj is null) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != GetType()) return false;
		return Equals((BackendState) obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(TotalTokenCount, IsSubmitEnabled, IsProcessing);
	}
}