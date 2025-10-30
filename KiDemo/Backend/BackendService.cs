using KiChat.Client.SignalR;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Microsoft.AspNetCore.SignalR;

namespace KiDemo.Backend;

internal class BackendService(ISignalRClient signalRClient): IBackendService
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