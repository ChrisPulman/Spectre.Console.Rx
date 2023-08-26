// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents something that has a box border.
/// </summary>
public interface IHasBoxBorder
{
    /// <summary>
    /// Gets or sets the box.
    /// </summary>
    public BoxBorder Border { get; set; }
}