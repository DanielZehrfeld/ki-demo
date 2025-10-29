using System.Reactive.Subjects;
using KiDemo.Backend;

namespace KiDemo.Components.Pages.Chat
{
	public class ChatVm(IBackendService backendService)
	{

		private readonly Subject<string> _messages = new Subject<string>();

		public IObservable<string> Messages => _messages;

		public void AddMessage(string message)
		{
			backendService.AddMessage("egal");
			_messages.OnNext(message + $" c-> {backendService.Count}");


		}

	}
}
