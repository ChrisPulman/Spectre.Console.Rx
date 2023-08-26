// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents something that has a culture.
/// </summary>
public interface IHasCulture
{
    /// <summary>
    /// Gets or sets the culture.
    /// </summary>
    CultureInfo? Culture { get; set; }
}