namespace KiDemo.Backend;

public interface IBackendService
{
	IObservable<string> Messages { get; }
	void AddMessage(string message);
	int Count { get;}
}