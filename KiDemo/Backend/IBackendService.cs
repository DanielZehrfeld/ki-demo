namespace KiDemo.Backend;

internal interface IBackendService
{
	IObservable<string> Messages { get; }
	void AddMessage(string message);
	int Count { get;}
}