// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Contains extension methods for <see cref="BoxBorder"/>.
/// </summary>
public static class BoxExtensions
{
    /// <summary>
    /// Gets the safe border for a border.
    /// </summary>
    /// <param name="border">The border to get the safe border for.</param>
    /// <param name="safe">Whether or not to return the safe border.</param>
    /// <returns>The safe border if one exist, otherwise the original border.</returns>
    public static BoxBorder GetSafeBorder(this BoxBorder border, bool safe)
    {
        if (border is null)
        {
            throw new ArgumentNullException(nameof(border));
        }

        if (safe && border.SafeBorder != null)
        {
            border = border.SafeBorder;
        }

        return border;
    }
}
