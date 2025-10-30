namespace KiDemo.Log;

internal class LogContentProvider(Func<string> getLogContent) : ILogContentProvider
{
	public string GetLogContent() => getLogContent();
}