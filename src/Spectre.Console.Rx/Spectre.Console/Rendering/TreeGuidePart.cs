// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Defines the different rendering parts of a <see cref="Tree"/>.
/// </summary>
public enum TreeGuidePart
{
    /// <summary>
    /// Represents a space.
    /// </summary>
    Space,

    /// <summary>
    /// Connection between siblings.
    /// </summary>
    Continue,

    /// <summary>
    /// Branch from parent to child.
    /// </summary>
    Fork,

    /// <summary>
    /// Branch from parent to child for the last child in a set.
    /// </summary>
    End,
}