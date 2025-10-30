namespace KiChat.Client.SignalR;

internal interface ISignalRClient
{
	IObservable<string> RootResults { get; }
	IDisposable Run(string url, Action<string> log);
}