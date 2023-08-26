// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// A console capable of writing ANSI escape sequences.
/// </summary>
public static partial class AnsiConsole
{
    /// <summary>
    /// Gets or sets the foreground color.
    /// </summary>
    public static Color Foreground
    {
        get => CurrentStyle.Foreground;
        set => CurrentStyle = CurrentStyle.Foreground(value);
    }

    /// <summary>
    /// Gets or sets the background color.
    /// </summary>
    public static Color Background
    {
        get => CurrentStyle.Background;
        set => CurrentStyle = CurrentStyle.Background(value);
    }

    /// <summary>
    /// Gets or sets the text decoration.
    /// </summary>
    public static Decoration Decoration
    {
        get => CurrentStyle.Decoration;
        set => CurrentStyle = CurrentStyle.Decoration(value);
    }

    internal static Style CurrentStyle { get; private set; } = Style.Plain;

    internal static bool Created { get; private set; }

    /// <summary>
    /// Resets colors and text decorations.
    /// </summary>
    public static void Reset()
    {
        ResetColors();
        ResetDecoration();
    }

    /// <summary>
    /// Resets the current applied text decorations.
    /// </summary>
    public static void ResetDecoration() => Decoration = Decoration.None;

    /// <summary>
    /// Resets the current applied foreground and background colors.
    /// </summary>
    public static void ResetColors() => CurrentStyle = Style.Plain;
}
