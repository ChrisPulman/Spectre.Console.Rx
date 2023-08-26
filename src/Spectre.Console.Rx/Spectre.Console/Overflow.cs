// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents text overflow.
/// </summary>
public enum Overflow
{
    /// <summary>
    /// Put any excess characters on the next line.
    /// </summary>
    Fold = 0,

    /// <summary>
    /// Truncates the text at the end of the line.
    /// </summary>
    Crop = 1,

    /// <summary>
    /// Truncates the text at the end of the line and
    /// also inserts an ellipsis character.
    /// </summary>
    Ellipsis = 2,
}