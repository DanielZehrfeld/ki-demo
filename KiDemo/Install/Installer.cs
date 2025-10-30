using KiDemo.Backend;
using KiDemo.Components.Pages.Chat;
using KiDemo.Components.Pages.Log;
using KiDemo.SignalR;

namespace KiDemo.Install;

internal static class Installer
{
	public static void InstallServices(IServiceCollection serviceCollection)
	{
		serviceCollection
			.AddSingleton<IBackendService, BackendService>();

		serviceCollection
			.AddScoped<ChatVm>();

		serviceCollection
			.AddTransient<ISignalRClient, SignalRClient>();

		serviceCollection
			.AddTransient<LogVm>();
	}
}