using System.Reactive;
using System.Reactive.Subjects;
using KiDemo.Backend;
using KiDemo.Backend.Dto;
using KiDemo.Backend.Utils;
using log4net;

namespace KiDemo.Components.Pages.Chat;

internal class ChatVm : IDisposable
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ChatVm));

	private static readonly ServiceStateVm DefaultStateVm = new ServiceStateVm(
		isConnected: false,
		isProcessing: false,
		isSubmitEnabled: false,
		messageCount: 0);

	private readonly IBackendService _backendService;

	private readonly BehaviorSubject<MessageItemVm[]> _messageItems = new([]);
	private readonly BehaviorSubject<ServiceStateVm> _serviceState = new(DefaultStateVm);

	private readonly Subject<MessageDetailsVm> _messageDetails = new();

	private readonly Subject<Unit> _stateHasChanged = new();

	private readonly MultiDisposable _disposables = new();
	private readonly List<MessageItem> _messages = [];

	public IObservable<MessageItemVm[]> MessageItems => _messageItems;
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
				messageMetadata: selectedMessage.MessageMetadata));

			TriggerStateChanged();
		}
	}

	private void OnNewMessage(BackendMessage message)
	{
		var id = Guid.NewGuid();
		
		var displayName = $"{message.Number}: Workflow";

		var messageMetadata = CreateMetadataString(message.Statistics);

		var messageItem = new MessageItem(
			id,
			message.MessageType,
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
		    timestamp:       {messageStatistics.Timestamp:O}
		    model:           {messageStatistics.ModelVersion}
		    tokens in:       {messageStatistics.InTokens}
		    tokens out:      {messageStatistics.ProcessTokens}
		    processing time: {messageStatistics.ProcessingTimeMs / 1000:F2} sec.
		    """;

	private void SubmitCurrentMessages()
	{
		var messages = _messages
			.Select(m => new MessageItemVm(m.Id, m.MessageType, m.DisplayName))
			.ToArray();

		_messageItems.OnNext(messages);
	}

	private void OnNewState(BackendState state)
	{
		var stateVm = new ServiceStateVm(
			isConnected: state.IsConnected,
			isProcessing: state.IsProcessing,
			isSubmitEnabled: state.IsConnected && !state.IsProcessing,
			messageCount: state.MessageCount);

		_serviceState.OnNext(stateVm);

		TriggerStateChanged();
	}

	private void TriggerStateChanged() => _stateHasChanged.OnNext(Unit.Default);

	public void Dispose()
	{
		_disposables.Dispose();
	}
}