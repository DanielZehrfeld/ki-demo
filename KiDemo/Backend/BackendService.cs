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

	private static readonly ILog Log = LogManager.GetLogger(typeof(BackendService));

	private readonly ISignalRClient _signalRClient;
	private readonly IMessageBatch _messageBatch;
	private readonly IStateAggregate _stateAggregate;
	private readonly MultiDisposable _disposables = new();

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
		_signalRClient = signalRClient;
		_messageBatch = messageBatch;
		_stateAggregate = stateAggregate;

		var signalRUrl = config.SignalRUrl;

		Log.Info($"starting backend service connection to: '{signalRUrl}'");

		signalRClient.RootResults
			.Subscribe(
				OnRootMessage,
				ex => LogError($"ERROR: {ex}", ex),
				() => LogError("ERROR: COMPLETED"))
			.AddTo(_disposables);

		signalRClient.QueryProcessed
			.Subscribe(
				OnQueryProcessed,
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

		signalRClient.Run(signalRUrl);

		releaseManager.Start(Release);
	}

	public void SubmitMessage(string message)
	{
		//todo limit entry length, restrict non printable characters

		lock (_lock)
		{
			//Todo ensure single processing

			var clientCommand = new ClientCommand
			{
				ExecuteType = ExecuteType.Chat,
				ClientId = DefaultClientId,
				Topic = DefaultChatWorkflowName,
				Content = message
			};

			// todo verbindungsfehler abfangen / behandeln

			var result = _signalRClient.CommandClientMessage(clientCommand);

			if (!string.IsNullOrEmpty(result))
			{
				throw new Exception($"Received error message from server, processing command: '{result}'");
			}

			//todo ensure message correctly submitted

			_messageBatch.ProcessSentMessage(message);
		}
	}

	private void Release(int count)
	{
		lock (_lock)
		{
			// todo verbindungsfehler abfangen / behandeln


			var result = _signalRClient.CommandRelease(count);

			if (!string.IsNullOrEmpty(result))
			{
				throw new Exception($"Received error message from server, processing command: '{result}'");
			}
		}
	}

	private void OnRootMessage(RootMessage message)
	{
		try
		{
			_messageBatch.ProcessRootMessage(message);
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