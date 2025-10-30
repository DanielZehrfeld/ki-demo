using System.Reactive.Linq;
using System.Reactive.Subjects;
using KiDemo.Backend.Dto;
using KiDemo.Backend.Extensions;
using KiDemo.SignalR.Messages;

namespace KiDemo.Backend.State;

internal class StateAggregate : IStateAggregate
{
	private static readonly BackendState InitialState = new(
		totalTokenCount: 0, 
		isSubmitEnabled: false, 
		isProcessing: false);

	private readonly BehaviorSubject<BackendState> _state = new(InitialState);

	private bool _serviceStarted;
	private bool _workerCount;
	private bool _clientState;
	private long _tokens;


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
		_tokens = statistics.GetTotalTokens() ?? 0;

		SubmitUpdateState();
	}

	private void SubmitUpdateState()
	{
		long totalTokenCount = 0;
		bool isSubmitEnabled = false;
		bool isProcessing = false;

		_state.OnNext(new BackendState(
			totalTokenCount: totalTokenCount, 
			isSubmitEnabled: isSubmitEnabled, 
			isProcessing: isProcessing));
	}
}

