// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents something that has a border.
/// </summary>
public interface IHasTableBorder : IHasBorder
{
    /// <summary>
    /// Gets or sets the border.
    /// </summary>
    public TableBorder Border { get; set; }
}