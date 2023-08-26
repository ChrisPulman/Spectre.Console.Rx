// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents something that is alignable.
/// </summary>
public interface IAlignable
{
    /// <summary>
    /// Gets or sets the alignment.
    /// </summary>
    Justify? Alignment { get; set; }
}
