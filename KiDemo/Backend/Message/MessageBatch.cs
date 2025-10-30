using System.Reactive.Linq;
using System.Reactive.Subjects;
using KiDemo.Backend.Dto;
using KiDemo.SignalR.Messages;

namespace KiDemo.Backend.Message;

internal class MessageBatch : IMessageBatch
{
	private readonly BehaviorSubject<ReplaySubject<BackendMessage>> _messages = new(new ReplaySubject<BackendMessage>());

	public IObservable<BackendMessage> Message => _messages.Switch();




	private int _messageCount = 0;

	public void ProcessSentMessage(string message)
	{
		_messages.OnNext(new ReplaySubject<BackendMessage>());
		_messageCount = 0;

		var statistics = new BackendMessageStatistics(
			timestamp: DateTimeOffset.Now, 
			inTokens: 0, 
			processTokens: 0, 
			processingTimeMs: 0, 
			modelVersion: string.Empty);

		var backendMessage = new BackendMessage(
			number: _messageCount++, 
			messageType: MessageType.Request, 
			messageContent: message, 
			messageReply: string.Empty, 
			statistics: statistics);

		SubmitMessage(backendMessage);
	}



	public void ProcessQueryMessage(QueryProcessedMessage message)
	{
		//todo lock

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
			_messageCount++, 
			MessageType.Workflow, 
			messageContent, 
			messageReply, 
			statistics);

		SubmitMessage(backendMessage);
	}



	public void ProcessRootMessage(RootMessage message)
	{

		var messageReply = message.Content;

		var statistics = new BackendMessageStatistics(
			timestamp: message.Timestamp,
			inTokens: 0,
			processTokens: 0,
			processingTimeMs: 0,
			modelVersion: string.Empty);

		var backendMessage = new BackendMessage(
			number: _messageCount++, 
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