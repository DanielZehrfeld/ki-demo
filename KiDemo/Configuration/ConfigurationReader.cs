namespace KiDemo.Configuration;

internal class ConfigurationReader(ConfigurationManager manager) : IConfigurationReader
{
	public string SignalRUrl { get; } = manager["SignalRUrl"] ?? string.Empty;
}