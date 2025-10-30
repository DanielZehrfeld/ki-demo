using KiDemo.Backend.Dto;

namespace KiDemo.Backend;

internal interface IBackendService: IDisposable
{
	IObservable<BackendMessage> Message { get; }
	IObservable<BackendState> State { get; }

	void SubmitMessage(string message);
	void Release(int count); //todo: release manager
}