// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a size.
/// </summary>
[DebuggerDisplay("{Width,nq}x{Height,nq}")]
[SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Intensional")]
public readonly struct Size(int width, int height)
{
    /// <summary>
    /// Gets the width.
    /// </summary>
    public int Width { get; } = width;

    /// <summary>
    /// Gets the height.
    /// </summary>
    public int Height { get; } = height;
}
