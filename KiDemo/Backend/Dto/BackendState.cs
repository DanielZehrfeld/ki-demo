namespace KiDemo.Backend.Dto;

internal class BackendState(bool isConnected, bool isProcessing)
{
	public static readonly BackendState Empty = new(false, false);

	public bool IsConnected { get; } = isConnected;
	public bool IsProcessing { get; } = isProcessing;

	protected bool Equals(BackendState other)
	{
		return IsConnected == other.IsConnected &&
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
		return HashCode.Combine(IsConnected, IsProcessing);
	}
}