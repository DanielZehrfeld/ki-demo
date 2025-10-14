using KiChat.Client.SignalR;
using System;
using System.Text;

namespace KiDemo.Backend;

public static class BackendService
{

	private static StringBuilder sb = new StringBuilder();
	
	private static SignalRClient? _client;



	public static string ProcessUserInput(string input)
    {
		try
		{
		    if (_client == null || _client?.IsError == true)
		    {
				_client?.Dispose();
				_client = null;

				sb.AppendLine("Client NULL, creating client");
		
				var client = new SignalRClient();
		
		
		        client.RootResults.Subscribe(rr => sb.AppendLine($"{DateTimeOffset.Now:O} - RR: " + rr));
		        //10.0.0.5
		        //client.Run("http://localhost:5249", log => sb.AppendLine($"{DateTimeOffset.Now:O} - LOG: " + log));
		        client.Run("http://10.0.0.5:5249", log => sb.AppendLine($"{DateTimeOffset.Now:O} - LOG: " + log));
		
				_client = client;
		    }
		
		
		    sb.AppendLine("releasing 1");
		
			_client.CommandRelease(1);
		
			sb.AppendLine("releasing DONE, sleeping");
		
			Thread.Sleep(2000);
		
			sb.AppendLine("sleeping DONE");
		}
		catch (Exception ex)
		{
		    sb.AppendLine($"{DateTimeOffset.Now:O} - ERROR: {ex}");
		}

		return $"processed: {sb}";
		
		//return $"processed x: {input}";
    }



}