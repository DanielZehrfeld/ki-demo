using System.Reactive.Linq;
using System.Reactive.Subjects;
using KiDemo.Backend.Dto;
using KiDemo.SignalR.Messages;
using log4net;

namespace KiDemo.Backend.Message;

internal class MessageBatch : IMessageBatch
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(MessageBatch));

	private readonly BehaviorSubject<ReplaySubject<BackendMessage>> _messages = new(new ReplaySubject<BackendMessage>());
	private readonly object _lock = new();
	private int _messageCount;

	public IObservable<BackendMessage> Message => _messages.Switch().Synchronize();
	
	public void ProcessSentMessage(string message)
	{
		lock (_lock)
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
	}

	public void ProcessQueryMessage(QueryProcessedMessage message)
	{
		lock (_lock)
		{
			var statistics = new BackendMessageStatistics(
				message.Timestamp,
				message.Statistics.InTokenCount,
				message.Statistics.ProcessingTokenCount,
				message.Statistics.ProcessingDurationMs,
				message.Statistics.ModelVersion);

			var backendMessage = new BackendMessage(
				_messageCount++,
				MessageType.Workflow,
				message.QueryMessageText,
				message.RawContent,
				statistics);

			SubmitMessage(backendMessage);
		}
	}

	public void ProcessRootMessage(RootMessage message)
	{
		lock (_lock)
		{
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
				messageReply: message.Content,
				statistics: statistics);

			SubmitMessage(backendMessage);
		}
	}

	private void SubmitMessage(BackendMessage backendMessage)
	{
		try
		{
			var messageSubject = _messages.Value;

			Task.Run(() => messageSubject.OnNext(backendMessage))
				.ContinueWith(e =>
				{
					if (e.IsFaulted)
					{
						Log.Error("Exception task submitting backend message", e.Exception);
					}
				});
		}
		catch (Exception ex)
		{
			Log.Error("Exception submitting backend message", ex);
		}
	}



}