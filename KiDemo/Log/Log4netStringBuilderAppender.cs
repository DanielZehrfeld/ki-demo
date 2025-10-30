using System.Text;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace KiDemo.Log;

internal class Log4NetStringBuilderAppender : AppenderSkeleton
{
	private readonly StringBuilder _logContent = new ();

	public string GetLog() => _logContent.ToString();

	protected override void Append(log4net.Core.LoggingEvent loggingEvent)
	{
		var writer = new StringWriter(_logContent);
		Layout?.Format(writer, loggingEvent);
		writer.Flush();
	}

	public static Func<string> Configure()
	{
		var layout = new PatternLayout
		{
			ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
		};
		layout.ActivateOptions();

		var stringBuilderAppender = new Log4NetStringBuilderAppender
		{
			Layout = layout
		};
		stringBuilderAppender.ActivateOptions();

		BasicConfigurator.Configure(stringBuilderAppender);

		return stringBuilderAppender.GetLog;
	}
}