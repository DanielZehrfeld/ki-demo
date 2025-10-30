using System.Reactive.Subjects;
using KiDemo.Backend.Dto;
using KiDemo.SignalR.Messages;

namespace KiDemo.Backend.State;

internal class StateAggregate : IStateAggregate
{
	private readonly Subject<BackendState> _state = new();

	public IObservable<BackendState> State => _state;
	public void ProcessServiceState(ServiceStateMessage state)
	{


	}

	public void ProcessClientState(bool state)
	{


	}

	public void ProcessStatistics(Statistics statistics)
	{


	}
}