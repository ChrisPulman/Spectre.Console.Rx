// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Contains extension methods for <see cref="FigletText"/>.
/// </summary>
public static class FigletTextExtensions
{
    /// <summary>
    /// Sets the color of the FIGlet text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="color">The color.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static FigletText Color(this FigletText text, Color? color)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        text.Color = color ?? Rx.Color.Default;
        return text;
    }
}
