namespace KiDemo.Backend.Release;

internal interface IReleaseManager
{
	void Start(Func<int, bool> releaseItems);
}