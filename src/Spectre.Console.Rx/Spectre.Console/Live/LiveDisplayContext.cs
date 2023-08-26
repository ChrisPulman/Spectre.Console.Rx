// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a context that can be used to interact with a <see cref="LiveDisplay" />.
/// </summary>
/// <seealso cref="Spectre.Console.Rx.IContext" />
public sealed class LiveDisplayContext : IContext
{
    private readonly IAnsiConsole _console;
    private bool _disposedValue;

    internal LiveDisplayContext(IAnsiConsole console, IRenderable target)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));

        Live = new LiveRenderable(_console, target);
        Lock = new object();
    }

    /// <summary>
    /// Gets a value indicating whether this instance is finished.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is finished; otherwise, <c>false</c>.
    /// </value>
    public bool IsFinished { get; internal set; }

    internal object Lock { get; }

    internal LiveRenderable Live { get; }

    /// <summary>
    /// Updates the live display target.
    /// </summary>
    /// <param name="target">The new live display target.</param>
    public void UpdateTarget(IRenderable? target)
    {
        lock (Lock)
        {
            Live.SetRenderable(target);
            Refresh();
        }
    }

    /// <summary>
    /// Refreshes the live display.
    /// </summary>
    public void Refresh()
    {
        lock (Lock)
        {
            _console.Write(new ControlCode(string.Empty));
        }
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    internal void SetOverflow(VerticalOverflow overflow, VerticalOverflowCropping cropping)
    {
        Live.Overflow = overflow;
        Live.OverflowCropping = cropping;
    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _console.Dispose();
            }

            _disposedValue = true;
        }
    }
}
