// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a region.
/// </summary>
[DebuggerDisplay("[X={X,nq}, Y={Y,nq}, W={Width,nq}, H={Height,nq}]")]
[SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Intensional")]
public readonly struct Region(int x, int y, int width, int height)
{
    /// <summary>
    /// Gets the x-coordinate.
    /// </summary>
    public int X { get; } = x;

    /// <summary>
    /// Gets the y-coordinate.
    /// </summary>
    public int Y { get; } = y;

    /// <summary>
    /// Gets the width.
    /// </summary>
    public int Width { get; } = width;

    /// <summary>
    /// Gets the height.
    /// </summary>
    public int Height { get; } = height;
}
