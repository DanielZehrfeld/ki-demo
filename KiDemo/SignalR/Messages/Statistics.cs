namespace KiDemo.SignalR.Messages;

public class Statistics
{
    public StatisticValue[] Values { get; set; } = [];

    public override string ToString() => $"{string.Join(", ", (IEnumerable<StatisticValue>)Values)}";
}