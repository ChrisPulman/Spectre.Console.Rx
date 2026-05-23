namespace Spectre.Console.Rx;

internal readonly struct ProgressSample(DateTime timestamp, double value)
{
    public double Value { get; } = value;
    public DateTime Timestamp { get; } = timestamp;
}