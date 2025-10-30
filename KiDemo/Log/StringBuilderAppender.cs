using System.Text;
using log4net.Appender;

namespace KiDemo;

public class StringBuilderAppender : AppenderSkeleton
{
	private readonly StringBuilder _logContent = new ();

	public string GetLog() => _logContent.ToString();

	protected override void Append(log4net.Core.LoggingEvent loggingEvent)
	{
		var writer = new StringWriter(_logContent);
		Layout?.Format(writer, loggingEvent);
		writer.Flush();
	}
}