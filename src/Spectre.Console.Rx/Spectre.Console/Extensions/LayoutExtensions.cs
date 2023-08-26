// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Contains extension methods for <see cref="Layout"/>.
/// </summary>
public static class LayoutExtensions
{
    /// <summary>
    /// Sets the ratio of the layout.
    /// </summary>
    /// <param name="layout">The layout.</param>
    /// <param name="ratio">The ratio.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Layout Ratio(this Layout layout, int ratio)
    {
        if (layout is null)
        {
            throw new ArgumentNullException(nameof(layout));
        }

        layout.Ratio = ratio;
        return layout;
    }

    /// <summary>
    /// Sets the size of the layout.
    /// </summary>
    /// <param name="layout">The layout.</param>
    /// <param name="size">The size.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Layout Size(this Layout layout, int size)
    {
        if (layout is null)
        {
            throw new ArgumentNullException(nameof(layout));
        }

        layout.Size = size;
        return layout;
    }

    /// <summary>
    /// Sets the minimum width of the layout.
    /// </summary>
    /// <param name="layout">The layout.</param>
    /// <param name="size">The size.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Layout MinimumSize(this Layout layout, int size)
    {
        if (layout is null)
        {
            throw new ArgumentNullException(nameof(layout));
        }

        layout.MinimumSize = size;
        return layout;
    }
}
