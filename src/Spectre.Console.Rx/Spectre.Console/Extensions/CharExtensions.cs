// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Contains extension methods for <see cref="char"/>.
/// </summary>
public static partial class CharExtensions
{
    /// <summary>
    /// Gets the cell width of a character.
    /// </summary>
    /// <param name="character">The character to get the cell width of.</param>
    /// <returns>The cell width of the character.</returns>
    public static int GetCellWidth(this char character) => Cell.GetCellLength(character);
}