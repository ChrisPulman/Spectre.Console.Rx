// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Represents a Markdown border.
/// </summary>
public sealed class MarkdownTableBorder : TableBorder
{
    /// <inheritdoc/>
    public override string GetPart(TableBorderPart part) => part switch
    {
        TableBorderPart.HeaderTopLeft => " ",
        TableBorderPart.HeaderTop => " ",
        TableBorderPart.HeaderTopSeparator => " ",
        TableBorderPart.HeaderTopRight => " ",
        TableBorderPart.HeaderLeft => "|",
        TableBorderPart.HeaderSeparator => "|",
        TableBorderPart.HeaderRight => "|",
        TableBorderPart.HeaderBottomLeft => "|",
        TableBorderPart.HeaderBottom => "-",
        TableBorderPart.HeaderBottomSeparator => "|",
        TableBorderPart.HeaderBottomRight => "|",
        TableBorderPart.CellLeft => "|",
        TableBorderPart.CellSeparator => "|",
        TableBorderPart.CellRight => "|",
        TableBorderPart.FooterTopLeft => " ",
        TableBorderPart.FooterTop => " ",
        TableBorderPart.FooterTopSeparator => " ",
        TableBorderPart.FooterTopRight => " ",
        TableBorderPart.FooterBottomLeft => " ",
        TableBorderPart.FooterBottom => " ",
        TableBorderPart.FooterBottomSeparator => " ",
        TableBorderPart.FooterBottomRight => " ",
        _ => throw new InvalidOperationException("Unknown border part."),
    };

    /// <inheritdoc/>
    public override string GetColumnRow(TablePart part, IReadOnlyList<int> widths, IReadOnlyList<IColumn> columns)
    {
        if (part == TablePart.FooterSeparator)
        {
            return string.Empty;
        }

        if (part != TablePart.HeaderSeparator)
        {
            return base.GetColumnRow(part, widths, columns);
        }

        var (left, center, separator, right) = GetTableParts(part);

        var builder = new StringBuilder();
        builder.Append(left);

        foreach (var (columnIndex, _, lastColumn, columnWidth) in widths.Enumerate())
        {
            var padding = columns[columnIndex].Padding;

            if (padding != null && padding.Value.Left > 0)
            {
                // Left padding
                builder.Append(" ".Repeat(padding.Value.Left));
            }

            var justification = columns[columnIndex].Alignment;
            if (justification == null)
            {
                // No alignment
                builder.Append(center.Repeat(columnWidth));
            }
            else if (justification.Value == Justify.Left)
            {
                // Left
                builder.Append(':')
                    .Append(center.Repeat(columnWidth - 1));
            }
            else if (justification.Value == Justify.Center)
            {
                // Centered
                builder.Append(':')
                    .Append(center.Repeat(columnWidth - 2))
                    .Append(':');
            }
            else if (justification.Value == Justify.Right)
            {
                // Right
                builder.Append(center.Repeat(columnWidth - 1))
                    .Append(':');
            }

            // Right padding
            if (padding != null && padding.Value.Right > 0)
            {
                builder.Append(" ".Repeat(padding.Value.Right));
            }

            if (!lastColumn)
            {
                builder.Append(separator);
            }
        }

        builder.Append(right);
        return builder.ToString();
    }
}