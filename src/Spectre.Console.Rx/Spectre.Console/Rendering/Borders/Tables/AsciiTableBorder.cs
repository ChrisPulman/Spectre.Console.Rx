// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Represents an old school ASCII border.
/// </summary>
public sealed class AsciiTableBorder : TableBorder
{
    /// <inheritdoc/>
    public override string GetPart(TableBorderPart part) => part switch
    {
        TableBorderPart.HeaderTopLeft => "+",
        TableBorderPart.HeaderTop => "-",
        TableBorderPart.HeaderTopSeparator => "-",
        TableBorderPart.HeaderTopRight => "+",
        TableBorderPart.HeaderLeft => "|",
        TableBorderPart.HeaderSeparator => "|",
        TableBorderPart.HeaderRight => "|",
        TableBorderPart.HeaderBottomLeft => "|",
        TableBorderPart.HeaderBottom => "-",
        TableBorderPart.HeaderBottomSeparator => "+",
        TableBorderPart.HeaderBottomRight => "|",
        TableBorderPart.CellLeft => "|",
        TableBorderPart.CellSeparator => "|",
        TableBorderPart.CellRight => "|",
        TableBorderPart.FooterTopLeft => "|",
        TableBorderPart.FooterTop => "-",
        TableBorderPart.FooterTopSeparator => "+",
        TableBorderPart.FooterTopRight => "|",
        TableBorderPart.FooterBottomLeft => "+",
        TableBorderPart.FooterBottom => "-",
        TableBorderPart.FooterBottomSeparator => "-",
        TableBorderPart.FooterBottomRight => "+",
        _ => throw new InvalidOperationException("Unknown border part."),
    };
}