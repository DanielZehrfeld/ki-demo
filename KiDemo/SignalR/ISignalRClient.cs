using KiDemo.SignalR.Messages;

namespace KiDemo.SignalR;

internal interface ISignalRClient
{
	IObservable<UserMessage> UserMessage { get; }
	IObservable<QueryProcessedMessage> QueryProcessed { get; }
	IObservable<ServiceStateMessage> ServiceState { get; }
	IObservable<bool> ClientState { get; }
	IObservable<Statistics> StatisticValues { get; }

	string CommandClientMessage(ClientCommand message);
	string CommandRelease(int count);

	IDisposable Run(string url);
}