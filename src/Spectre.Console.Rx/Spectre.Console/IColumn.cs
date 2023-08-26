// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a column.
/// </summary>
public interface IColumn : IAlignable, IPaddable
{
    /// <summary>
    /// Gets or sets a value indicating whether
    /// or not wrapping should be prevented.
    /// </summary>
    bool NoWrap { get; set; }

    /// <summary>
    /// Gets or sets the width of the column.
    /// </summary>
    int? Width { get; set; }
}