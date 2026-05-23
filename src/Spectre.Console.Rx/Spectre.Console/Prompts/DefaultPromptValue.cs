namespace Spectre.Console.Rx;

internal sealed class DefaultPromptValue<T>
{
    public T Value { get; }

    public DefaultPromptValue(T value)
    {
        Value = value;
    }
}