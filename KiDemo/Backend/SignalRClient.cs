using System.Reactive;
using System.Reactive.Linq;
using KiChat.Service;
using Microsoft.AspNetCore.SignalR.Client;

namespace KiChat.Client.SignalR;

internal class SignalRClient: IDisposable 
{
    private const string Failed = "FAILED";

    private static readonly TimeSpan ReconnectDelay = TimeSpan.FromSeconds(2);

    private readonly System.Reactive.Subjects.Subject<string> _rootResults = new();

 
    public IObservable<string> RootResults => _rootResults.Synchronize();
 
    private HubConnection? _connection;

    public bool IsError { get; private set; } = false;

    public IDisposable Run(string url, Action<string> log)
        => Observable.Return(Unit.Default)
            .Select(_ => CreateConnection(url, log))
            .Select(hub => Connect(hub, log))
            .Switch()
            .Do(connection =>
            {
                _connection = connection;
            })
            .Catch<HubConnection, Exception>(_ =>
            {
                _connection = default;
                IsError = true;
                
                log("Setting client IsError TRUE");

				return Observable.Empty<HubConnection>()
                    .Delay(ReconnectDelay);
            })
            //.Repeat()
            .Subscribe(
                onNext: _ => { },
                onError: _ =>
                {
                },
                onCompleted: () =>
                {
                });

    private HubConnection CreateConnection(string signalRServerUrl, Action<string> log)
    {
        if (string.IsNullOrWhiteSpace(signalRServerUrl))
        {
            throw new Exception("SignalR server url is not set");
        }

        var connectionUrl = signalRServerUrl + SignalRConstants.UrlChatHub;

        log($"Creating signalr connection to: {connectionUrl}");

        var connection = new HubConnectionBuilder()
            .WithUrl(connectionUrl)
            .Build();


        connection.On<string>(SignalRConstants.OnRootResults, OnRootResult);

        return connection;
    }
    private void OnRootResult(string message)
    {
	    _rootResults.OnNext(message);
    }

    private static IObservable<HubConnection> Connect(HubConnection connection, Action<string> log)
        => Observable.Create<HubConnection>(
            obs =>
            {
                log("Connection signalR");

                connection.Closed += error =>
                {
                    if (error != null)
                    {
                        log($"SignalR connection closed: {error.Message}");
                        obs.OnError(error);
                    }
                    else
                    {
                        log("SignalR connection closed");
                        obs.OnCompleted();
                    }

                    return Task.CompletedTask;
                };

                connection.StartAsync()
                    .ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            log($"Connection signal task failed: {task.Exception.Message}");
                            obs.OnError(task.Exception);
                        }
                        else
                        {
                            log($"Connection signal task succeeded");
                            obs.OnNext(connection);
                        }
                    });

                return () => { };
            }
        );

  
    public void CommandRelease(int count)
    {
            _connection?.SendAsync(SignalRConstants.CommandRelease, count).Wait();
    }

    public void Dispose()
    {
	    _connection?.DisposeAsync();
    }
}