using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using KiDemo.Backend;

namespace KiDemo.Components.Pages.Chat;

public class ChatVm(IBackendService backendService)
{
	private readonly Subject<MessageItemVm[]> _messageItems = new Subject<MessageItemVm[]>();
	private readonly Subject<MessageDetailsVm> _messageDetails = new Subject<MessageDetailsVm>();
	private readonly Subject<ServiceStateVm> _serviceState = new Subject<ServiceStateVm>();
	private readonly Subject<Unit> _stateHasChanged = new Subject<Unit>();

	public IObservable<MessageItemVm[]> MessageItems => _messageItems;
	public IObservable<MessageDetailsVm> MessageDetails => _messageDetails;
	public IObservable<ServiceStateVm> ServiceState=> _serviceState;
	public IObservable<Unit> StateHasChanged => _stateHasChanged;



	private List<(string, Guid)> _messages = new List<(string, Guid)>();

	public void SubmitMessage(string message)
	{

		_messages.Add((message, Guid.NewGuid()));

		var messageItemVms = _messages
			.Select(m => new MessageItemVm(m.Item2, $"Display: {m.Item1}"))
			.ToArray();

		_messageItems.OnNext(messageItemVms);

		TriggerStateChanged();
	}



	public void SelectMessage(Guid messageId)
	{
		_messageDetails.OnNext(new MessageDetailsVm(messageId, $"CONTENT of {messageId}"));

		TriggerStateChanged();
	}

	private void TriggerStateChanged() => _stateHasChanged.OnNext(Unit.Default);

}