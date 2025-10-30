namespace KiDemo.Backend.Release;

internal interface IReleaseManager
{
	void Start(Action<int> releaseItems);
}