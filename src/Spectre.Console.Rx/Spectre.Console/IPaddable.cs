// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents something that is paddable.
/// </summary>
public interface IPaddable
{
    /// <summary>
    /// Gets or sets the padding.
    /// </summary>
    public Padding? Padding { get; set; }
}