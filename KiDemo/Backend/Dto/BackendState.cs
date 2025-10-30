namespace KiDemo.Backend.Dto;

internal class BackendState(bool isConnected, bool isProcessing, int messageCount)
{
	public static readonly BackendState Empty = new(false, false, 0);

	public bool IsConnected { get; } = isConnected;
	public bool IsProcessing { get; } = isProcessing;
	public int MessageCount { get; } = messageCount;

	protected bool Equals(BackendState other)
	{
		return IsConnected == other.IsConnected &&
		       IsProcessing == other.IsProcessing &&
		       MessageCount == other.MessageCount;
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
		return HashCode.Combine(IsConnected, IsProcessing, MessageCount);
	}
}