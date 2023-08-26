// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a border.
/// </summary>
public abstract partial class BoxBorder
{
    /// <summary>
    /// Gets the safe border for this border or <c>null</c> if none exist.
    /// </summary>
    public virtual BoxBorder? SafeBorder { get; }

    /// <summary>
    /// Gets the string representation of the specified border part.
    /// </summary>
    /// <param name="part">The part to get the character representation for.</param>
    /// <returns>A character representation of the specified border part.</returns>
    public abstract string GetPart(BoxBorderPart part);
}