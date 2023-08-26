// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Contains extension methods for <see cref="TextPath"/>.
/// </summary>
public static class TextPathExtensions
{
    /// <summary>
    /// Sets the separator style.
    /// </summary>
    /// <param name="obj">The path.</param>
    /// <param name="style">The separator style to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPath SeparatorStyle(this TextPath obj, Style style)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.SeparatorStyle = style;
        return obj;
    }

    /// <summary>
    /// Sets the separator color.
    /// </summary>
    /// <param name="obj">The path.</param>
    /// <param name="color">The separator color.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPath SeparatorColor(this TextPath obj, Color color) => SeparatorStyle(obj, new Style(foreground: color));

    /// <summary>
    /// Sets the root style.
    /// </summary>
    /// <param name="obj">The path.</param>
    /// <param name="style">The root style to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPath RootStyle(this TextPath obj, Style style)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.RootStyle = style;
        return obj;
    }

    /// <summary>
    /// Sets the root color.
    /// </summary>
    /// <param name="obj">The path.</param>
    /// <param name="color">The root color.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPath RootColor(this TextPath obj, Color color) => RootStyle(obj, new Style(foreground: color));

    /// <summary>
    /// Sets the stem style.
    /// </summary>
    /// <param name="obj">The path.</param>
    /// <param name="style">The stem style to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPath StemStyle(this TextPath obj, Style style)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.StemStyle = style;
        return obj;
    }

    /// <summary>
    /// Sets the stem color.
    /// </summary>
    /// <param name="obj">The path.</param>
    /// <param name="color">The stem color.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPath StemColor(this TextPath obj, Color color) => StemStyle(obj, new Style(foreground: color));

    /// <summary>
    /// Sets the leaf style.
    /// </summary>
    /// <param name="obj">The path.</param>
    /// <param name="style">The stem leaf to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPath LeafStyle(this TextPath obj, Style style)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.LeafStyle = style;
        return obj;
    }

    /// <summary>
    /// Sets the leaf color.
    /// </summary>
    /// <param name="obj">The path.</param>
    /// <param name="color">The leaf color.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TextPath LeafColor(this TextPath obj, Color color) => LeafStyle(obj, new Style(foreground: color));
}
