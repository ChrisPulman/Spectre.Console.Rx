namespace Spectre.Console.Rx;

internal sealed class LegacyConsoleBackend : IAnsiConsoleBackend
{
    private readonly IAnsiConsole _console;
    private Style _lastStyle;

    public IAnsiConsoleCursor Cursor { get; }

    public LegacyConsoleBackend(IAnsiConsole console)
    {
        _console = console ?? throw new System.ArgumentNullException(nameof(console));
        _lastStyle = Style.Plain;

        Cursor = new LegacyConsoleCursor();
    }

    public void Clear(bool home)
    {
        TryConsoleOperation(() =>
        {
            var (x, y) = (System.Console.CursorLeft, System.Console.CursorTop);

            System.Console.Clear();

            if (!home)
            {
                // Set the cursor position
                System.Console.SetCursorPosition(x, y);
            }
        });
    }

    public void Write(IRenderable renderable)
    {
        foreach (var segment in renderable.GetSegments(_console))
        {
            if (segment.IsControlCode)
            {
                continue;
            }

            if (!_lastStyle.Equals(segment.Style))
            {
                SetStyle(segment.Style);
            }

            _console.Profile.Out.Writer.Write(segment.Text.NormalizeNewLines(native: true));
        }
    }

    public void Write(Action<AnsiWriter> action)
    {
        // Do nothing. The backend is not capable of emitting ANSI/VT escape sequences.
    }

    private void SetStyle(Style style)
    {
        _lastStyle = style;

        TryConsoleOperation(System.Console.ResetColor);

        var background = Color.ToConsoleColor(style.Background);
        if (_console.Profile.Capabilities.ColorSystem != ColorSystem.NoColors && (int)background != -1)
        {
            TryConsoleOperation(() => System.Console.BackgroundColor = background);
        }

        var foreground = Color.ToConsoleColor(style.Foreground);
        if (_console.Profile.Capabilities.ColorSystem != ColorSystem.NoColors && (int)foreground != -1)
        {
            TryConsoleOperation(() => System.Console.ForegroundColor = foreground);
        }
    }

    private static void TryConsoleOperation(Action action)
    {
        try
        {
            action();
        }
        catch (IOException)
        {
        }
        catch (InvalidOperationException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}
