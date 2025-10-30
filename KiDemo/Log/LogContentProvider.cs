namespace KiDemo.Log;

public class LogContentProvider(Func<string> getLogContent) : ILogContentProvider
{
	public string GetLogContent() => getLogContent();
}