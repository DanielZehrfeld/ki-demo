using System.Reactive.Linq;
using System.Reactive.Subjects;
using KiDemo.Backend.Dto;
using KiDemo.SignalR.Messages;

namespace KiDemo.Backend.Message;

internal class MessageBatch : IMessageBatch
{
	private readonly BehaviorSubject<ReplaySubject<BackendMessage>> _messages = new(new ReplaySubject<BackendMessage>());
	private readonly BehaviorSubject<int> _messageCount = new(0);

	public IObservable<BackendMessage> Message => _messages.Switch();
	public IObservable<int> MessageCount => _messageCount;


	//todo timestamp: local tíme display

	public void ProcessSentMessage(string message)
	{
		_messages.OnNext(new ReplaySubject<BackendMessage>());

		var number = _messageCount.Value + 1;

		var statistics = new BackendMessageStatistics(
			timestamp: DateTimeOffset.Now, 
			inTokens: 0, 
			processTokens: 0, 
			processingTimeMs: 0, 
			modelVersion: string.Empty);

		var backendMessage = new BackendMessage(
			number: number, 
			messageType: MessageType.Request, 
			messageContent: message, 
			messageReply: string.Empty, 
			statistics: statistics);

		SubmitMessage(backendMessage);
	}



	public void ProcessQueryMessage(QueryProcessedMessage message)
	{
		//todo lock

		var number = _messageCount.Value + 1;

		var messageContent = message.QueryMessageText;
		var messageReply = message.RawContent;

		var timestamp = DateTimeOffset.Now; //todo timestamp



		var statistics = new BackendMessageStatistics(
			timestamp: timestamp, 
			inTokens: message.Statistics.InTokenCount, 
			processTokens: message.Statistics.ProcessingTokenCount, 
			processingTimeMs: message.Statistics.ProcessingDurationMs,  
			modelVersion: message.Statistics.ModelVersion);

		var backendMessage = new BackendMessage(
			number, 
			MessageType.Workflow, 
			messageContent, 
			messageReply, 
			statistics);

		SubmitMessage(backendMessage);
	}



	public void ProcessRootMessage(RootMessage message)
	{
		var number = _messageCount.Value + 1;

		var messageReply = message.Content;

		var statistics = new BackendMessageStatistics(
			timestamp: message.Timestamp,
			inTokens: 0,
			processTokens: 0,
			processingTimeMs: 0,
			modelVersion: string.Empty);

		var backendMessage = new BackendMessage(
			number: number, 
			messageType: MessageType.Answer, 
			messageContent: string.Empty, 
			messageReply: messageReply, 
			statistics: statistics);

		SubmitMessage(backendMessage);
	}

	private void SubmitMessage(BackendMessage backendMessage)
	{
		_messages.Value.OnNext(backendMessage);

		//todo submit new message count?
	}

}