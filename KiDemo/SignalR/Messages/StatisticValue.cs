namespace KiDemo.SignalR.Messages;

public class StatisticValue
{
    public string Key { get; set; } = string.Empty;
    public long Value { get; set; } = -1;

    public override string ToString() => $"{Key} = {Value}";
}