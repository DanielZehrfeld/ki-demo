using System.Reactive.Subjects;
using KiDemo.Backend.Dto;
using KiDemo.SignalR.Messages;

namespace KiDemo.Backend.Message;

internal class MessageBatch : IMessageBatch
{

	private readonly Subject<BackendMessage> _message = new();

	public IObservable<BackendMessage> Message => _message;
	
	public void ProcessRootMessage(RootMessage message)
	{
		

	}

	public void ProcessQueryMessage(QueryProcessedMessage message)
	{
	
	}

	public void ProcessStatistics(Statistics statistics)
	{
	

	}

	public void ProcessSentMessage(string message)
	{
		

	}
}