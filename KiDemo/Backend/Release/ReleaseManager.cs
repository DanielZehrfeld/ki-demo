using System.Reactive.Linq;
using KiDemo.Backend.State;
using log4net;

namespace KiDemo.Backend.Release;

internal class ReleaseManager(IStateAggregate state) : IReleaseManager, IDisposable
{
	private const int DailyReleaseCount = 1000;
	private static readonly ILog Log = LogManager.GetLogger(typeof(ReleaseManager));

	private DateTimeOffset? _lastRelease;
	private IDisposable? _subscription;
	private Func<int, bool>? _tryReleaseItems;

	public void Start(Func<int, bool> tryReleaseItems)
	{
		Log.Info("Starting release manager");

		_tryReleaseItems = tryReleaseItems;

		_subscription = Observable
			.Interval(TimeSpan.FromMinutes(1))
			.StartWith(0)
			.Subscribe(_ => CheckRelease());
	}

	private void CheckRelease()
	{
		if (state.Current.IsConnected)
		{
			if (_lastRelease == null || DateTimeOffset.Now.Date != _lastRelease?.Date)
			{
				Log.Info($"Need to release items, since: {_lastRelease}");

				var currentCount = state.Current.ReleaseCount;

				var itemsToRelease = unchecked((int)(DailyReleaseCount - currentCount));

				if (itemsToRelease > 0)
				{
					Log.Info($"Releasing: {itemsToRelease}");

					var releaseResult = _tryReleaseItems?.Invoke(itemsToRelease) == true
						? "Successfully released items"
						: "Failed to release, will try again";

					Log.Info(releaseResult);
				}
				else
				{
					Log.Info("Processing done, zero items to release");
					_lastRelease = DateTimeOffset.Now;
				}
			}
		}
	}

	public void Dispose()
	{
		_subscription?.Dispose();
	}
}