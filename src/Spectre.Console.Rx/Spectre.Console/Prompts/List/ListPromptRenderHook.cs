namespace Spectre.Console.Rx;

internal sealed class ListPromptRenderHook<T>(
    IAnsiConsole console,
    Func<IRenderable> builder) : IRenderHook
    where T : notnull
{
    private readonly IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));
    private readonly Func<IRenderable> _builder = builder ?? throw new ArgumentNullException(nameof(builder));
    private readonly LiveRenderable _live = new LiveRenderable(console);
    private readonly object _lock = new();
    private bool _dirty = true;

    public void Clear() => _console.Write(_live.RestoreCursor());

    public void Refresh()
    {
        _dirty = true;
        _console.Write(ControlCode.Empty);
    }

    public IEnumerable<IRenderable> Process(RenderOptions options, IEnumerable<IRenderable> renderables)
    {
        lock (_lock)
        {
            if (!_live.HasRenderable || _dirty)
            {
                _live.SetRenderable(_builder());
                _dirty = false;
            }

            yield return _live.PositionCursor(options);

            foreach (var renderable in renderables)
            {
                yield return renderable;
            }

            yield return _live;
        }
    }
}