// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Represents the different parts of a box border.
/// </summary>
public enum BoxBorderPart
{
    /// <summary>
    /// The top left part of a box.
    /// </summary>
    TopLeft,

    /// <summary>
    /// The top part of a box.
    /// </summary>
    Top,

    /// <summary>
    /// The top right part of a box.
    /// </summary>
    TopRight,

    /// <summary>
    /// The left part of a box.
    /// </summary>
    Left,

    /// <summary>
    /// The right part of a box.
    /// </summary>
    Right,

    /// <summary>
    /// The bottom left part of a box.
    /// </summary>
    BottomLeft,

    /// <summary>
    /// The bottom part of a box.
    /// </summary>
    Bottom,

    /// <summary>
    /// The bottom right part of a box.
    /// </summary>
    BottomRight,
}