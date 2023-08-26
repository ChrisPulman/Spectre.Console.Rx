// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Represents different parts of a table.
/// </summary>
public enum TablePart
{
    /// <summary>
    /// The top of a table.
    /// </summary>
    Top,

    /// <summary>
    /// The separator between the header and the cells.
    /// </summary>
    HeaderSeparator,

    /// <summary>
    /// The separator between the footer and the cells.
    /// </summary>
    FooterSeparator,

    /// <summary>
    /// The bottom of a table.
    /// </summary>
    Bottom,
}