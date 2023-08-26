// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents something that has justification.
/// </summary>
public interface IHasJustification
{
    /// <summary>
    /// Gets or sets the justification.
    /// </summary>
    Justify? Justification { get; set; }
}