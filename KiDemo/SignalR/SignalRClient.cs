using System.Reactive;
using System.Reactive.Linq;
using KiDemo.SignalR.Messages;
using log4net;
using Microsoft.AspNetCore.SignalR.Client;

namespace KiDemo.SignalR;

internal class SignalRClient : ISignalRClient
{
	private const string Failed = "FAILED";

	private static readonly ILog Log = LogManager.GetLogger(typeof(SignalRClient));
	private static readonly TimeSpan ReconnectDelay = TimeSpan.FromSeconds(2);

	private readonly System.Reactive.Subjects.BehaviorSubject<bool> _clientState = new(false);
	private readonly System.Reactive.Subjects.BehaviorSubject<Statistics> _statistics = new(new Statistics());

	private readonly System.Reactive.Subjects.Subject<RootMessage> _rootResults = new();
	private readonly System.Reactive.Subjects.Subject<QueryProcessedMessage> _queriesProcessed = new();
	private readonly System.Reactive.Subjects.Subject<ServiceStateMessage> _serviceState = new();

	public IObservable<QueryProcessedMessage> QueryProcessed => _queriesProcessed.Synchronize();
	public IObservable<RootMessage> RootResults => _rootResults.Synchronize();
	public IObservable<ServiceStateMessage> ServiceState => _serviceState.Synchronize();
	public IObservable<bool> ClientState => _clientState.Synchronize();
	public IObservable<Statistics> StatisticValues => _statistics.Synchronize();

	private HubConnection? _connection;

	public IDisposable Run(string signalRServerUrl)
		=> Observable.Return(Unit.Default)
			.Select(_ => CreateConnection(signalRServerUrl))
			.Select(Connect)
			.Switch()
			.Do(connection =>
			{
				_connection = connection;
				_clientState.OnNext(true);
			})
			.Catch<HubConnection, Exception>(_ =>
			{
				_connection = default;
				_clientState.OnNext(false);

				return Observable.Empty<HubConnection>()
					.Delay(ReconnectDelay);
			})
			.Repeat()
			.Subscribe(
				onNext: _ => { },
				onError: _clientState.OnError,
				onCompleted: _clientState.OnCompleted);

	private HubConnection CreateConnection(string signalRServerUrl)
	{
		if (string.IsNullOrWhiteSpace(signalRServerUrl))
		{
			throw new Exception("SignalR server url is not set");
		}

		var connectionUrl = signalRServerUrl + SignalRConstants.UrlChatHub;

		Log.Debug($"Creating signalr connection to: {connectionUrl}");

		var connection = new HubConnectionBuilder()
			.WithUrl(connectionUrl)
			.Build();

		connection.On<QueryProcessedMessage>(SignalRConstants.OnQueryProcessed, OnQueryProcessed);
		connection.On<RootMessage>(SignalRConstants.OnRootResults, OnRootResult);
		connection.On<ServiceStateMessage>(SignalRConstants.OnServiceState, OnServiceState);
		connection.On<Statistics>(SignalRConstants.OnStatisticValues, OnStatisticValues);

		return connection;
	}

	private static IObservable<HubConnection> Connect(HubConnection connection)
		=> Observable.Create<HubConnection>(
			obs =>
			{
				Log.Debug("Connection signalR");

				connection.Closed += error =>
				{
					if (error != null)
					{
						Log.Debug($"SignalR connection closed: {error.Message}");
						obs.OnError(error);
					}
					else
					{
						Log.Debug("SignalR connection closed");
						obs.OnCompleted();
					}

					return Task.CompletedTask;
				};

				connection.StartAsync()
					.ContinueWith(task =>
					{
						if (task.IsFaulted)
						{
							Log.Debug($"Connection signal task failed: {task.Exception.Message}");
							obs.OnError(task.Exception);
						}
						else
						{
							Log.Debug($"Connection signal task succeeded");
							obs.OnNext(connection);
						}
					});

				return () => { };
			}
		);

	public string CommandRelease(int count)
	{
		try
		{
			Log.Debug($"Sending CommandRelease: {count}");
			return _connection?.InvokeAsync<string>(SignalRConstants.CommandRelease, count).Result ?? Failed;
		}
		catch (Exception ex)
		{
			Log.Error("Exception sending CommandRelease", ex);
			throw;
		}
	}

	public string CommandClientMessage(ClientCommand message)
	{
		try
		{
			Log.Debug($"Sending CommandClientMessage: {message}");
			return _connection?.InvokeAsync<string>(SignalRConstants.CommandClientMessage, message).Result ?? Failed;
		}
		catch (Exception ex)
		{
			Log.Error("Exception sending CommandClientMessage", ex);
			throw;
		}
	}
	private void OnQueryProcessed(QueryProcessedMessage message)
	{
		try
		{
			Log.Debug($"query processed: {message.QueryId}");
			_queriesProcessed.OnNext(message);
		}
		catch (Exception ex)
		{
			Log.Error("Exception executing OnIncomingMessage", ex);
		}
	}

	private void OnRootResult(RootMessage message)
	{
		try
		{
			Log.Debug($"root result: {message}");
			_rootResults.OnNext(message);
		}
		catch (Exception ex)
		{
			Log.Error("Exception executing OnRootResult", ex);
		}
	}

	private void OnServiceState(ServiceStateMessage stateValue)
	{
		try
		{
			Log.Debug($"service state: {stateValue.RunState}, model: {stateValue.Model}");
			_serviceState.OnNext(stateValue);
		}
		catch (Exception ex)
		{
			Log.Error("Exception executing OnServiceState", ex);
		}
	}

	private void OnStatisticValues(Statistics statistics)
	{
		try
		{
			Log.Debug($"statistics received: {statistics}");
			_statistics.OnNext(statistics);
		}
		catch (Exception ex)
		{
			Log.Error("Exception executing OnStatisticValues", ex);
		}
	}
}