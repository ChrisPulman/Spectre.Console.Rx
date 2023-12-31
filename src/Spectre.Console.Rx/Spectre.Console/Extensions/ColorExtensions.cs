// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// ColorExtensions.
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Gets the color of the inverted.
    /// </summary>
    /// <param name="color">The color.</param>
    /// <returns>A color.</returns>
    public static Color GetInvertedColor(this Color color) =>
        GetLuminance(color) < 140 ? Rx.Color.White : Rx.Color.Black;

    private static float GetLuminance(this Color color) =>
        (float)((0.2126 * color.R) + (0.7152 * color.G) + (0.0722 * color.B));
}
