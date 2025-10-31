using KiDemo.Backend.Dto;
using KiDemo.SignalR.Messages;

namespace KiDemo.Backend.Message;

internal interface IMessageBatch
{
	IObservable<BackendMessage> Message { get; }

	void ProcessUserMessage(UserMessage message);
	void ProcessQueryMessage(QueryProcessedMessage message);
	void ProcessSentMessage(string message);
	void ProcessClientConnected();
}