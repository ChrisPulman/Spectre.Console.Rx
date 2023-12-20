// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Testing;

/// <summary>
/// Represents fake capabilities useful in tests.
/// </summary>
public sealed class TestCapabilities : IReadOnlyCapabilities
{
    /// <inheritdoc/>
    public ColorSystem ColorSystem { get; set; } = ColorSystem.TrueColor;

    /// <inheritdoc/>
    public bool Ansi { get; set; }

    /// <inheritdoc/>
    public bool Links { get; set; }

    /// <inheritdoc/>
    public bool Legacy { get; set; }

    /// <inheritdoc/>
    public bool IsTerminal { get; set; }

    /// <inheritdoc/>
    public bool Interactive { get; set; }

    /// <inheritdoc/>
    public bool Unicode { get; set; }

    /// <summary>
    /// Creates a <see cref="RenderOptions"/> with the same capabilities as this instace.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <returns>A <see cref="RenderOptions"/> with the same capabilities as this instace.</returns>
    public RenderOptions CreateRenderContext(IAnsiConsole console)
    {
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        return RenderOptions.Create(console, this);
    }
}
