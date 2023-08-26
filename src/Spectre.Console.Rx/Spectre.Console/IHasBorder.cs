// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents something that has a border.
/// </summary>
public interface IHasBorder
{
    /// <summary>
    /// Gets or sets a value indicating whether or not to use
    /// a "safe" border on legacy consoles that might not be able
    /// to render non-ASCII characters.
    /// </summary>
    bool UseSafeBorder { get; set; }

    /// <summary>
    /// Gets or sets the box style.
    /// </summary>
    public Style? BorderStyle { get; set; }
}