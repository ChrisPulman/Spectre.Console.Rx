// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Testing;

/// <summary>
/// Contains extensions for <see cref="Style"/>.
/// </summary>
public static class StyleExtensions
{
    /// <summary>
    /// Sets the foreground or background color of the specified style.
    /// </summary>
    /// <param name="style">The style.</param>
    /// <param name="color">The color.</param>
    /// <param name="foreground">Whether or not to set the foreground color.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Style SetColor(this Style style, Color color, bool foreground)
    {
        if (foreground)
        {
            return style.Foreground(color);
        }

        return style.Background(color);
    }
}
