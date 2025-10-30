using KiDemo.Backend.Dto;
using KiDemo.SignalR.Messages;

namespace KiDemo.Backend.Message;

internal interface IMessageBatch
{
	IObservable<BackendMessage> Message { get; }

	void ProcessRootMessage(RootMessage message);
	void ProcessQueryMessage(QueryProcessedMessage message);
	void ProcessSentMessage(string message);
}