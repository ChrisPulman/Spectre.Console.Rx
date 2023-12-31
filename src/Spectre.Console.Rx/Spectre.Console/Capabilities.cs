// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents console capabilities.
/// </summary>
public sealed class Capabilities : IReadOnlyCapabilities
{
    private readonly IAnsiConsoleOutput _out;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="Capabilities" /> class.
    /// </summary>
    /// <param name="out">The out.</param>
    /// <exception cref="ArgumentNullException">out.</exception>
    internal Capabilities(IAnsiConsoleOutput @out) => _out = @out ?? throw new ArgumentNullException(nameof(@out));

    /// <summary>
    /// Gets or sets the color system.
    /// </summary>
    public ColorSystem ColorSystem { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// the console supports VT/ANSI control codes.
    /// </summary>
    public bool Ansi { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// the console support links.
    /// </summary>
    public bool Links { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// this is a legacy console (cmd.exe) on an OS
    /// prior to Windows 10.
    /// </summary>
    /// <remarks>
    /// Only relevant when running on Microsoft Windows.
    /// </remarks>
    public bool Legacy { get; set; }

    /// <summary>
    /// Gets a value indicating whether or not
    /// the output is a terminal.
    /// </summary>
    [Obsolete("Use Profile.Out.IsTerminal instead")]
    public bool IsTerminal => _out.IsTerminal;

    /// <summary>
    /// Gets or sets a value indicating whether
    /// or not the console supports interaction.
    /// </summary>
    public bool Interactive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether
    /// or not the console supports Unicode.
    /// </summary>
    public bool Unicode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether
    /// or not the console supports alternate buffers.
    /// </summary>
    public bool AlternateBuffer { get; set; }
}
