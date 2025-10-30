using KiDemo.Log;

namespace KiDemo.Components.Pages.Log;

internal class LogVm(ILogContentProvider logProvider)
{
	public string GetLog() => logProvider.GetLogContent();
}