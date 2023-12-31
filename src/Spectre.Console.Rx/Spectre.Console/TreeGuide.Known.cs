// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents tree guide lines.
/// </summary>
public abstract partial class TreeGuide
{
    /// <summary>
    /// Gets an <see cref="AsciiTreeGuide"/> instance.
    /// </summary>
    public static TreeGuide Ascii { get; } = new AsciiTreeGuide();

    /// <summary>
    /// Gets a <see cref="LineTreeGuide"/> instance.
    /// </summary>
    public static TreeGuide Line { get; } = new LineTreeGuide();

    /// <summary>
    /// Gets a <see cref="DoubleLineTreeGuide"/> instance.
    /// </summary>
    public static TreeGuide DoubleLine { get; } = new DoubleLineTreeGuide();

    /// <summary>
    /// Gets a <see cref="BoldLineTreeGuide"/> instance.
    /// </summary>
    public static TreeGuide BoldLine { get; } = new BoldLineTreeGuide();
}