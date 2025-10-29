using KiChat.Client.SignalR;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace KiDemo.Backend;


public interface IBackendService
{
    IObservable<string> Messages { get; }
    void AddMessage(string message);
}



public class BackendService: IBackendService
{
	
	private readonly Subject<string> _messages = new Subject<string>();

	public IObservable<string> Messages => _messages;

    public void AddMessage(string message)
    {
	    _messages.OnNext(message);

	}

}