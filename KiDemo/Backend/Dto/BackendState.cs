namespace KiDemo.Backend.Dto;

internal class BackendState(bool isConnected, bool hasQueueItems, long releaseCount)
{
	public static readonly BackendState Empty = new(false, false, 0);

	public bool IsConnected { get; } = isConnected;
	public bool HasQueueItems { get; } = hasQueueItems;
	public long ReleaseCount { get; } = releaseCount;

	protected bool Equals(BackendState other)
	{
		return IsConnected == other.IsConnected && HasQueueItems == other.HasQueueItems && ReleaseCount == other.ReleaseCount;
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
		return HashCode.Combine(IsConnected, HasQueueItems, ReleaseCount);
	}
}