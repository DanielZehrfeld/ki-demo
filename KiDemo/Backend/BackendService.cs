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
	int Count { get;}
}



public class BackendService: IBackendService
{
	
	private readonly Subject<string> _messages = new Subject<string>();

	private int _count = 0;

	public int Count => _count;


	public IObservable<string> Messages => _messages;

    public void AddMessage(string message)
    {
	    _count++;
	    _messages.OnNext(message);

	}

}