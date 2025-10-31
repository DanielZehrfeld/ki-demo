using KiDemo.Backend.Dto;
using KiDemo.Backend.Message;
using KiDemo.Backend.Release;
using KiDemo.Backend.State;
using KiDemo.Backend.Utils;
using KiDemo.Configuration;
using KiDemo.SignalR;
using KiDemo.SignalR.Messages;
using log4net;

namespace KiDemo.Backend;

internal class BackendService : IBackendService
{
	private const int DefaultClientId = 0;
	private const string DefaultChatWorkflowName = "ChatProcess";
	public const int MaxMessageSize = 1000;

	private static readonly ILog Log = LogManager.GetLogger(typeof(BackendService));

	private readonly ISignalRClient _signalRClient;
	private readonly IMessageBatch _messageBatch;
	private readonly IStateAggregate _stateAggregate;
	private readonly MultiDisposable _disposables = new();

	private bool _hasQueueItems;
	private bool _isConnected;
	private bool _pendingQueueItems;

	private readonly object _lock = new();

	public IObservable<BackendMessage> Message => _messageBatch.Message;
	public IObservable<BackendState> State => _stateAggregate.State;

	public BackendService(
		ISignalRClient signalRClient, 
		IMessageBatch messageBatch, 
		IStateAggregate stateAggregate, 
		IReleaseManager releaseManager,
		IConfigurationReader config)
	{
		Log.Info("starting backend service");

		_signalRClient = signalRClient;
		_messageBatch = messageBatch;
		_stateAggregate = stateAggregate;

		_stateAggregate.State.Subscribe(
				OnStateChanged,
				ex => LogError($"ERROR: {ex}", ex),
				() => LogError("ERROR: COMPLETED"))
			.AddTo(_disposables);

		signalRClient.QueryProcessed
			.Subscribe(
				OnQueryProcessed,
				ex => LogError($"ERROR: {ex}", ex),
				() => LogError("ERROR: COMPLETED"))
			.AddTo(_disposables);

		signalRClient.UserMessage
			.Subscribe(
				OnUserMessage,
				ex => LogError($"ERROR: {ex}", ex),
				() => LogError("ERROR: COMPLETED"))
			.AddTo(_disposables);

		signalRClient.ServiceState
			.Subscribe(
				OnServiceState,
				ex => LogError($"ERROR: {ex}", ex),
				() => LogError("ERROR: COMPLETED"))
			.AddTo(_disposables);

		signalRClient.ClientState
			.Subscribe(
				OnClientState,
				ex => LogError($"ERROR: {ex}", ex),
				() => LogError("ERROR: COMPLETED"))
			.AddTo(_disposables);

		signalRClient.StatisticValues
			.Subscribe(
				OnStatisticValues,
				ex => LogError($"ERROR: {ex}", ex),
				() => LogError("ERROR: COMPLETED"))
			.AddTo(_disposables);

		var signalRUrl = config.SignalRUrl;

		Log.Info($"Connecting to: '{signalRUrl}'");

		signalRClient.Run(signalRUrl);

		releaseManager.Start(Release);
	}

	public void SubmitMessage(string message)
	{
		Log.Info($"request to submit message '{message}'");

		lock (_lock)
		{
			var messageSubmitted = false;
			try
			{
				if (message.Length > MaxMessageSize)
				{
					throw new Exception($"Message length needs to be lower than '{MaxMessageSize}'");
				}

				if (_hasQueueItems)
				{
					throw new Exception("queue not empty");
				}

				if (!_isConnected)
				{
					throw new Exception("not connected to service");
				}

				if (_pendingQueueItems)
				{
					throw new Exception("waiting for submitted items");
				}

				var clientCommand = new ClientCommand
				{
					ExecuteType = ExecuteType.Chat,
					ClientId = DefaultClientId,
					Topic = DefaultChatWorkflowName,
					Content = message
				};

				var result = _signalRClient.CommandClientMessage(clientCommand);

				if (!string.IsNullOrEmpty(result))
				{
					throw new Exception($"Received error message from server, processing command: '{result}'");
				}

				Log.Info("Message successfully submit");

				_pendingQueueItems = true;
				messageSubmitted = true;
			}
			catch (Exception ex)
			{
				Log.Warn("Submit message failed", ex);
			}

			if (messageSubmitted)
			{
				_messageBatch.ProcessSentMessage(message);
			}
		}
	}

	private bool Release(int count)
	{
		lock (_lock)
		{
			try
			{
				Log.Info($"Releasing '{count}'");

				var result = _signalRClient.CommandRelease(count);

				if (!string.IsNullOrEmpty(result))
				{
					throw new Exception($"Received error message from server, processing command: '{result}'");
				}
			}
			catch (Exception ex)
			{
				Log.Warn("Release failed: ", ex);
				return false;
			}
		}

		return true;
	}

	private void OnStateChanged(BackendState state)
	{
		lock (_lock)
		{
			try
			{
				_isConnected = state.IsConnected;
				_hasQueueItems = state.HasQueueItems;

				if (_hasQueueItems)
				{
					_pendingQueueItems = false;
				}
			}
			catch (Exception ex)
			{
				LogError($"ERROR: {ex.Message}", ex);
			}
		}
	}

	private void OnUserMessage(UserMessage message)
	{
		try
		{
			_messageBatch.ProcessUserMessage(message);
		}
		catch (Exception ex)
		{
			LogError($"ERROR: {ex.Message}", ex);
		}
	}

	private void OnQueryProcessed(QueryProcessedMessage queryProcessedMessage)
	{
		try
		{
			_messageBatch.ProcessQueryMessage(queryProcessedMessage);
		}
		catch (Exception ex)
		{
			LogError($"ERROR: {ex.Message}", ex);
		}
	}

	private void OnServiceState(ServiceStateMessage serviceState)
	{
		try
		{
			_stateAggregate.ProcessServiceState(serviceState);
		}
		catch (Exception ex)
		{
			LogError($"ERROR: {ex.Message}", ex);
		}
	}

	private void OnClientState(bool clientState)
	{
		try
		{
			_stateAggregate.ProcessClientState(clientState);
		}
		catch (Exception ex)
		{
			LogError($"ERROR: {ex.Message}", ex);
		}
	}

	private void OnStatisticValues(Statistics statistics)
	{
		try
		{
			_stateAggregate.ProcessStatistics(statistics);
		}
		catch (Exception ex)
		{
			LogError($"ERROR: {ex.Message}", ex);
		}
	}

	private static void LogError(string message, Exception? e = null)
	{
		Log.Error(message, e);
	}

	public void Dispose()
	{
		_disposables.Dispose();
	}
}