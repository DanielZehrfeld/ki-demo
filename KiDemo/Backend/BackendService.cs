using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using KiDemo.Configuration;
using KiDemo.SignalR;
using log4net;
using Microsoft.AspNetCore.SignalR;

namespace KiDemo.Backend;

internal class BackendService : IBackendService
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(BackendService));




	
	private readonly Subject<string> _messages = new Subject<string>();

	private int _count = 0;

	public BackendService(ISignalRClient signalRClient, IConfigurationReader config)
	{
		Log.Info("starting backend service");

		var configSignalRUrl = config.SignalRUrl;

		Log.Info("Connecting signalr to: " + configSignalRUrl);


		signalRClient.Run(configSignalRUrl);


		/*    if (_client == null || _client?.IsError == true)
		       {
		   		_client?.Dispose();
		   		_client = null;
		   
		   		sb.AppendLine("Client NULL, creating client");
		   
		   		var client = new SignalRClient();
		   
		           client.RootResults.Subscribe(rr => sb.AppendLine($"{DateTimeOffset.Now:O} - RR: " + rr));
		   		
		   		// AZ
		           // client.Run("http://10.0.0.4:5249", log => sb.AppendLine($"{DateTimeOffset.Now:O} - LOG: " + log));
		   		
		   		//local
		           client.Run("http://localhost:5249", log => sb.AppendLine($"{DateTimeOffset.Now:O} - LOG: " + log));
		   
		   		_client = client;
		       }*/

	}

	public int Count => _count;


	public IObservable<string> Messages => _messages;

    public void AddMessage(string message)
    {
	    _count++;
	    _messages.OnNext(message);

	}

}