namespace Spectre.Console.Rx;

internal sealed class DefaultPromptValue<T>(T value)
{
    public T Value { get; } = value;
}