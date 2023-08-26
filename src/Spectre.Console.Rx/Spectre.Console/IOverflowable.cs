// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents something that can overflow.
/// </summary>
public interface IOverflowable
{
    /// <summary>
    /// Gets or sets the text overflow strategy.
    /// </summary>
    Overflow? Overflow { get; set; }
}