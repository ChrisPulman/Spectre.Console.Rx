// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// A console recorder used to record output from a console.
/// </summary>
[SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Only used for scoping")]
public class Recorder(IAnsiConsole console) : IAnsiConsole
{
    private readonly IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));
    private readonly List<IRenderable> _recorded = new();

    /// <inheritdoc/>
    public Profile Profile => _console.Profile;

    /// <inheritdoc/>
    public IAnsiConsoleCursor Cursor => _console.Cursor;

    /// <inheritdoc/>
    public IAnsiConsoleInput Input => _console.Input;

    /// <inheritdoc/>
    public IExclusivityMode ExclusivityMode => _console.ExclusivityMode;

    /// <inheritdoc/>
    public RenderPipeline Pipeline => _console.Pipeline;

    /// <inheritdoc/>
    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Only used for scoping")]
    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Only used for scoping")]
    public void Dispose() => _console.Dispose(); // Only used for scoping.

    /// <inheritdoc/>
    public void Clear(bool home) => _console.Clear(home);

    /// <inheritdoc/>
    public void Write(IRenderable renderable)
    {
        if (renderable is null)
        {
            throw new ArgumentNullException(nameof(renderable));
        }

        _recorded.Add(renderable);

        _console.Write(renderable);
    }

    /// <summary>
    /// Exports the recorded data.
    /// </summary>
    /// <param name="encoder">The encoder.</param>
    /// <returns>The recorded data represented as a string.</returns>
    public string Export(IAnsiConsoleEncoder encoder)
    {
        if (encoder is null)
        {
            throw new ArgumentNullException(nameof(encoder));
        }

        return encoder.Encode(_console, _recorded);
    }

    internal Recorder Clone(IAnsiConsole console)
    {
        var recorder = new Recorder(console);
        recorder._recorded.AddRange(_recorded);
        return recorder;
    }
}
