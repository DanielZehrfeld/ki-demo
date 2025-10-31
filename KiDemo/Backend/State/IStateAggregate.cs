using KiDemo.Backend.Dto;
using KiDemo.SignalR.Messages;

namespace KiDemo.Backend.State;

internal interface IStateAggregate
{
	IObservable<BackendState> State { get; }
	BackendState Current { get; }

	void ProcessServiceState(ServiceStateMessage state);
	void ProcessClientState(bool state);
	void ProcessStatistics(Statistics statistics);
}