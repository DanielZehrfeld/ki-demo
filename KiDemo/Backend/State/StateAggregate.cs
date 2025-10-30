using System.Reactive.Linq;
using System.Reactive.Subjects;
using KiDemo.Backend.Dto;
using KiDemo.Backend.Extensions;
using KiDemo.SignalR.Messages;
using log4net;

namespace KiDemo.Backend.State;

internal class StateAggregate : IStateAggregate
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(StateAggregate));

	private readonly BehaviorSubject<BackendState> _state = new(BackendState.Empty);

	private bool _serviceStarted;
	private bool _workerCount;
	private bool _clientState;
	private long _queueCount;

	public IObservable<BackendState> State => _state.DistinctUntilChanged();

	public void ProcessServiceState(ServiceStateMessage state)
	{
		_workerCount = state.WorkerCount > 0;
		_serviceStarted = state.RunState == ServiceStateRunValue.Started;

		SubmitUpdateState();
	}

	public void ProcessClientState(bool state)
	{
		_clientState = state;

		SubmitUpdateState();
	}

	public void ProcessStatistics(Statistics statistics)
	{
		_queueCount = statistics.GetQueueCount() ?? 0;

		SubmitUpdateState();
	}

	private void SubmitUpdateState()
	{
		var isConnected = _clientState && _serviceStarted && _workerCount;
		var isProcessing = _queueCount > 0;

		try
		{
			_state.OnNext(new BackendState(
				isConnected: isConnected,
				isProcessing: isProcessing));
		}
		catch (Exception ex)
		{
			Log.Error("Exception submitting state", ex);
		}
	}
}