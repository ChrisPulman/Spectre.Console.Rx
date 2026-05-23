namespace Spectre.Console.Rx;

internal sealed class AnsiConsoleBackend : IAnsiConsoleBackend
{
    private readonly IAnsiConsole _console;
    private AnsiWriter _writer;
    private IAnsiConsoleOutput _cachedOutput;

    public IAnsiConsoleCursor Cursor { get; }
    public Capabilities Capabilities => _console.Profile.Capabilities;

    public AnsiConsoleBackend(IAnsiConsole console)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
        _cachedOutput = _console.Profile.Out;
        _writer = new AnsiWriter(_console.Profile.Out.Writer, _console.Profile.Capabilities);

        Cursor = new AnsiConsoleCursor(this);
    }

    public void Clear(bool home)
    {
        Write(w =>
        {
            w.EraseInDisplay(2);
            w.ClearScrollback();
        });

        if (home)
        {
            Write(w => w.CursorPosition(1, 1));
        }
    }

    public void Write(IRenderable renderable)
    {
        EnsureWriter();
        _writer.Write(_console, renderable);
    }

    public void Write(Action<AnsiWriter> action)
    {
        EnsureWriter();
        action(_writer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureWriter()
    {
        // Output has not changed?
        if (ReferenceEquals(_cachedOutput, _console.Profile.Out))
        {
            return;
        }

        _cachedOutput = _console.Profile.Out;
        _writer = new AnsiWriter(_console.Profile.Out.Writer, _console.Profile.Capabilities);
    }
}