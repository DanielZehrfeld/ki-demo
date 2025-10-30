using KiChat.Client.SignalR;
using KiDemo.Backend;
using KiDemo.Components;
using KiDemo.Components.Pages.Chat;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddSingleton<IBackendService, BackendService>();

builder.Services
	.AddScoped<ChatVm>();

builder.Services
	.AddTransient<ISignalRClient, SignalRClient>();


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
