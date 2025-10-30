using System.Reactive;
using System.Reactive.Subjects;
using KiDemo.Backend;
using KiDemo.Backend.Dto;
using KiDemo.Backend.Utils;
using log4net;

namespace KiDemo.Components.Pages.Chat;

internal class ChatVm: IDisposable
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ChatVm));

	private readonly IBackendService _backendService;

	
	private readonly BehaviorSubject<MessageItemVm[]> _messageItems = new([]);
	private readonly BehaviorSubject<ServiceStateVm> _serviceState = new(new ServiceStateVm(false, 0));
	private readonly Subject<MessageDetailsVm> _messageDetails = new();

	private readonly Subject<Unit> _stateHasChanged = new();

	private readonly MultiDisposable _disposables = new();
	private readonly List<MessageItem> _messages = [];

	public IObservable<MessageItemVm[]> MessageItems => _messageItems;
	public IObservable<MessageDetailsVm> MessageDetails => _messageDetails;
	public IObservable<ServiceStateVm> ServiceState=> _serviceState;
	public IObservable<Unit> StateHasChanged => _stateHasChanged;

	private bool _isInit;

	public ChatVm(IBackendService backendService)
	{
		Log.Info("Creating VM");
		_isInit = true;

		_backendService = backendService;

		_backendService.Message
			.Subscribe(
				OnNewMessage,
				ex => Log.Error($"ERROR: {ex}", ex),
				() => Log.Error("ERROR: COMPLETED"))
			.AddTo(_disposables);

		_backendService.State
			.Subscribe(
				OnNewState,
				ex => Log.Error($"ERROR: {ex}", ex),
				() => Log.Error("ERROR: COMPLETED"))
			.AddTo(_disposables);

		SubmitCurrentMessages();

		Log.Info("Creating VM DONE");
		_isInit = false;
	}

	public void SubmitMessage(string message) => _backendService.SubmitMessage(message);

	public void SelectMessage(Guid messageId)
	{
		var selectedMessage = _messages
				.FirstOrDefault(message => message.Id == messageId);

		if (selectedMessage != null)
		{
			//todo
			string messageContent = "";
			string messageReply = "";
			string messageMetadata = "";

			_messageDetails.OnNext(new MessageDetailsVm(
				id: messageId,
				messageContent: messageContent,
				messageReply: messageReply,
				messageMetadata: messageMetadata));

			TriggerStateChanged();
		}
	}

	private void OnNewMessage(BackendMessage message)
	{
		var displayName = ""; //todo

		_messages.Add(new MessageItem(Guid.NewGuid(), displayName));

		if (!_isInit)
		{
			SubmitCurrentMessages();

			TriggerStateChanged();
		}
	}

	private void SubmitCurrentMessages()
	{
		var messages = _messages
			.Select(m => new MessageItemVm(m.Id, m.DisplayName))
			.ToArray();

		_messageItems.OnNext(messages);
	}

	private void OnNewState(BackendState state)
	{
		_serviceState.OnNext(
			new ServiceStateVm(
				state.IsSubmitEnabled,
				state.TotalTokenCount));

		TriggerStateChanged();
	}

	private void TriggerStateChanged() => _stateHasChanged.OnNext(Unit.Default);
	
	public void Dispose()
	{
		_disposables.Dispose();
	}
}