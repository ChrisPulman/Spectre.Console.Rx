namespace Spectre.Console.Rx;

/// <summary>
/// A control code.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ControlCode"/> class.
/// </remarks>
/// <param name="control">The control code.</param>
public sealed class ControlCode(string control) : Renderable
{
    private readonly Segment _segment = Segment.Control(control);

    internal static ControlCode Empty { get; } = new(string.Empty);

    /// <summary>
    /// Creates a new <see cref="ControlCode"/> using a <see cref="AnsiWriter"/>.
    /// </summary>
    /// <param name="capabilities">The capabilities.</param>
    /// <param name="action">The <see cref="AnsiWriter"/> action.</param>
    /// <returns>A new <see cref="ControlCode"/> instance.</returns>
    public static ControlCode Create(
        IReadOnlyCapabilities capabilities,
        Action<AnsiWriter> action)
    {
        ArgumentNullException.ThrowIfNull(capabilities);
        ArgumentNullException.ThrowIfNull(action);

        return new ControlCode(
            AnsiStringWriter.Shared.Write(
                capabilities, action));
    }

    /// <summary>
    /// Creates a new <see cref="ControlCode"/> using a <see cref="AnsiWriter"/>.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="action">The <see cref="AnsiWriter"/> action.</param>
    /// <returns>A new <see cref="ControlCode"/> instance.</returns>
    public static ControlCode Create(
        IAnsiConsole console,
        Action<AnsiWriter> action)
    {
        ArgumentNullException.ThrowIfNull(console);
        ArgumentNullException.ThrowIfNull(action);

        return new ControlCode(
            AnsiStringWriter.Shared.Write(
                console.Profile.Capabilities, action));
    }

    /// <inheritdoc />
    protected override Measurement Measure(RenderOptions options, int maxWidth) => new Measurement(0, 0);

    /// <inheritdoc />
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        if (options.Ansi)
        {
            yield return _segment;
        }
    }
}