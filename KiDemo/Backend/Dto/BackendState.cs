namespace KiDemo.Backend.Dto;

internal class BackendState(bool isConnected, bool hasQueueItems)
{
	public static readonly BackendState Empty = new(false, false);

	public bool IsConnected { get; } = isConnected;
	public bool HasQueueItems { get; } = hasQueueItems;

	protected bool Equals(BackendState other)
	{
		return IsConnected == other.IsConnected &&
		       HasQueueItems == other.HasQueueItems;
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
		return HashCode.Combine(IsConnected, HasQueueItems);
	}
}