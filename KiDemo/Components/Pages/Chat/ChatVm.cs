using System.Reactive;
using System.Reactive.Subjects;
using KiDemo.Backend;
using KiDemo.Backend.Dto;
using KiDemo.Backend.Utils;
using log4net;

namespace KiDemo.Components.Pages.Chat;

// todo: Overlay
// todo impressum
// todo links überprüfen, link in DZI-Webpage
// todo: Beschriftung "demo"
// todo: workflow nachricht nach antwort nachricht
// todo: App start: vorhandene antworten mal da und mal nicht, weg. async Client connect nachricht (z.T. vor client connect)
// todo: sprache beschriftungen engl deutsch
// todo lampen-farben
// todo Überschrift 'nachricht, Ausgabe' - Text; Font / Size unschön,
// todo Font / size navigation pane übernehmen
// todo info-Seite
// todo selected button / item color (message-list)
// todo: ?!? v.o.n.u: 20: Workflow; 22: Ausgabe; 21: Workflow - what???

internal class ChatVm : IDisposable
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ChatVm));

	private readonly IBackendService _backendService;

	private readonly BehaviorSubject<(MessageItemVm[], int)> _messageItems = new(([], 0));
	private readonly BehaviorSubject<ServiceStateVm> _serviceState = new(ServiceStateVm.Empty);

	private readonly Subject<MessageDetailsVm> _messageDetails = new();

	private readonly Subject<Unit> _stateHasChanged = new();

	private readonly MultiDisposable _disposables = new();
	private readonly List<MessageItem> _messages = [];

	public IObservable<(MessageItemVm[], int)> MessageItems => _messageItems; 
	public IObservable<MessageDetailsVm> MessageDetails => _messageDetails;
	public IObservable<ServiceStateVm> ServiceState => _serviceState;
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
			_messageDetails.OnNext(new MessageDetailsVm(
				messageContent: selectedMessage.MessageContent,
				messageReply: selectedMessage.MessageReply,
				messageMetadata: selectedMessage.MessageMetadata,
				utcTimestampString: selectedMessage.Timestamp.ToUniversalTime().ToString("O")));

			TriggerStateChanged();
		}
	}

	private void OnNewMessage(BackendMessage message)
	{
		var id = Guid.NewGuid();

		var messageName = string.Empty;
		var messageMetadata = string.Empty;

		switch (message.MessageType)
		{
			case MessageType.Request:
				messageName = "Nachricht";
				break;
			case MessageType.Answer:
				messageName = "Ausgabe";
				break;
			case MessageType.Workflow:
				messageName = "Workflow";
				messageMetadata = CreateMetadataString(message.Statistics);
				break;
		}

		var displayName = $"{message.Number}: {messageName}";

		var messageItem = new MessageItem(
			id,
			message.Number,
			message.MessageType,
			message.Statistics.Timestamp,
			displayName,
			message.MessageContent,
			message.MessageReply,
			messageMetadata);

		_messages.Add(messageItem);

		if (!_isInit)
		{
			SubmitCurrentMessages();

			TriggerStateChanged();
		}
	}

	private static string CreateMetadataString(BackendMessageStatistics messageStatistics) 
		=> $"""
		    model:           
		    {messageStatistics.ModelVersion}
		    
		    tokens in:  {messageStatistics.InTokens}
		    tokens out: {messageStatistics.ProcessTokens}
		    duration:   {messageStatistics.ProcessingTimeMs / 1000:F2} sec.
		    """;

	private void SubmitCurrentMessages()
	{
		var messages = _messages
			.Select(m => new MessageItemVm(m.Id, m.MessageType, m.DisplayName))
			.ToArray();

		_messageItems.OnNext((messages, _messages.LastOrDefault()?.Number ?? 0));
	}

	private void OnNewState(BackendState state)
	{
		var stateIsConnected = state.IsConnected;
		var stateHasQueueItems = state.HasQueueItems;
		var isSubmitEnabled = state is {IsConnected: true, HasQueueItems: false, ReleaseCount: > 0};

		var stateVm = new ServiceStateVm(
			isConnected: stateIsConnected,
			isProcessing: stateHasQueueItems,
			isSubmitEnabled: isSubmitEnabled);

		_serviceState.OnNext(stateVm);

		TriggerStateChanged();
	}

	private void TriggerStateChanged() => _stateHasChanged.OnNext(Unit.Default);

	public void Dispose()
	{
		_disposables.Dispose();
	}
}