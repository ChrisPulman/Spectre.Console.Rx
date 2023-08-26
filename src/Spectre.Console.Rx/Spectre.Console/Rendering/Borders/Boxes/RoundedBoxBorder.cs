// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Represents a rounded border.
/// </summary>
public sealed class RoundedBoxBorder : BoxBorder
{
    /// <inheritdoc/>
    public override BoxBorder? SafeBorder => BoxBorder.Square;

    /// <inheritdoc/>
    public override string GetPart(BoxBorderPart part) => part switch
    {
        BoxBorderPart.TopLeft => "╭",
        BoxBorderPart.Top => "─",
        BoxBorderPart.TopRight => "╮",
        BoxBorderPart.Left => "│",
        BoxBorderPart.Right => "│",
        BoxBorderPart.BottomLeft => "╰",
        BoxBorderPart.Bottom => "─",
        BoxBorderPart.BottomRight => "╯",
        _ => throw new InvalidOperationException("Unknown border part."),
    };
}
