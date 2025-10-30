using KiDemo.Components;
using KiDemo.Install;
using log4net;
using KiDemo.Log;

namespace KiDemo;

internal class Program
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

	public static void Main(string[] args)
	{
		try
		{
			var getLogContent = Log4NetStringBuilderAppender.Configure();

			Log.Info("Application starting");

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

			Log.Info("Application starting DONE. Running");

			app.Run();

			Log.Info("Application running DONE.");
		}
		catch (Exception ex)
		{
			Log.Error("Exception starting service", ex);
			throw;
		}
	}
}