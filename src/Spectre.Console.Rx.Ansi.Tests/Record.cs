namespace Spectre.Console.Rx.Ansi.Tests;

internal static class Record
{
    public static Exception? Exception(Action action)
    {
        try
        {
            action();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
