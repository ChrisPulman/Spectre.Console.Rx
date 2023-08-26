// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Extensions;

/// <summary>
/// Contains extension methods for <see cref="Align"/>.
/// </summary>
public static class AlignExtensions
{
    /// <summary>
    /// Sets the width.
    /// </summary>
    /// <param name="align">The <see cref="Align"/> object.</param>
    /// <param name="width">The width, or <c>null</c> for no explicit width.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Align Width(this Align align, int? width)
    {
        if (align is null)
        {
            throw new ArgumentNullException(nameof(align));
        }

        align.Width = width;
        return align;
    }

    /// <summary>
    /// Sets the height.
    /// </summary>
    /// <param name="align">The <see cref="Align"/> object.</param>
    /// <param name="height">The height, or <c>null</c> for no explicit height.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Align Height(this Align align, int? height)
    {
        if (align is null)
        {
            throw new ArgumentNullException(nameof(align));
        }

        align.Height = height;
        return align;
    }

    /// <summary>
    /// Sets the vertical alignment.
    /// </summary>
    /// <param name="align">The <see cref="Align"/> object.</param>
    /// <param name="vertical">The vertical alignment, or <c>null</c> for no vertical alignment.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Align VerticalAlignment(this Align align, VerticalAlignment? vertical)
    {
        if (align is null)
        {
            throw new ArgumentNullException(nameof(align));
        }

        align.Vertical = vertical;
        return align;
    }

    /// <summary>
    /// Sets the <see cref="Align"/> object to be top aligned.
    /// </summary>
    /// <param name="align">The <see cref="Align"/> object.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Align TopAligned(this Align align)
    {
        if (align is null)
        {
            throw new ArgumentNullException(nameof(align));
        }

        align.Vertical = Rx.VerticalAlignment.Top;
        return align;
    }

    /// <summary>
    /// Sets the <see cref="Align"/> object to be middle aligned.
    /// </summary>
    /// <param name="align">The <see cref="Align"/> object.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Align MiddleAligned(this Align align)
    {
        if (align is null)
        {
            throw new ArgumentNullException(nameof(align));
        }

        align.Vertical = Rx.VerticalAlignment.Middle;
        return align;
    }

    /// <summary>
    /// Sets the <see cref="Align"/> object to be bottom aligned.
    /// </summary>
    /// <param name="align">The <see cref="Align"/> object.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Align BottomAligned(this Align align)
    {
        if (align is null)
        {
            throw new ArgumentNullException(nameof(align));
        }

        align.Vertical = Rx.VerticalAlignment.Bottom;
        return align;
    }
}
