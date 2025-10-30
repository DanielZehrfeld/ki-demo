using KiDemo.Components;
using KiDemo.Install;
using log4net;
using log4net.Config;
using log4net.Layout;
using KiDemo.Log;

namespace KiDemo;

internal class Program
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

	public static void Main(string[] args)
	{
		try
		{
			var getLogContent = ConfigureLog4Net();

			Log.Info("Application starting...");

			var builder = WebApplication.CreateBuilder(args);

			var serviceCollection = builder.Services;

			serviceCollection.AddSingleton<ILogContentProvider>(new LogContentProvider(getLogContent));

			serviceCollection
				.AddRazorComponents()
				.AddInteractiveServerComponents();

			Installer.InstallServices(serviceCollection);

			var app = builder.Build();

			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error", createScopeForErrors: true);
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();
			app.UseAntiforgery();

			app.MapRazorComponents<App>()
				.AddInteractiveServerRenderMode();

			app.Run();
		}
		catch (Exception ex)
		{
			Log.Error("Exception starting service", ex);
			throw;
		}
	}

	private static Func<string> ConfigureLog4Net()
	{
		var layout = new PatternLayout
		{
			ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
		};
		layout.ActivateOptions();

		var stringBuilderAppender = new StringBuilderAppender()
		{
			Layout = layout
		};
		stringBuilderAppender.ActivateOptions();

		BasicConfigurator.Configure(stringBuilderAppender);

		return stringBuilderAppender.GetLog;
	}
}