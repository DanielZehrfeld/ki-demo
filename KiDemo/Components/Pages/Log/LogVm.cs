using KiDemo.Log;

namespace KiDemo.Components.Pages.Log;

public class LogVm(ILogContentProvider logProvider) //todo make all classes internal
{
	public string GetLog()
	{

		return logProvider.GetLogContent();
	}
}