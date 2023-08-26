// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Contains extension methods for <see cref="RemainingTimeColumn"/>.
/// </summary>
public static class RemainingTimeColumnExtensions
{
    /// <summary>
    /// Sets the style of the remaining time text.
    /// </summary>
    /// <param name="column">The column.</param>
    /// <param name="style">The style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static RemainingTimeColumn Style(this RemainingTimeColumn column, Style style)
    {
        if (column is null)
        {
            throw new ArgumentNullException(nameof(column));
        }

        if (style is null)
        {
            throw new ArgumentNullException(nameof(style));
        }

        column.Style = style;
        return column;
    }
}