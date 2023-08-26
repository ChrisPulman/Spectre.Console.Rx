// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents something that can be used to resolve ratios.
/// </summary>
internal interface IRatioResolvable
{
    /// <summary>
    /// Gets the ratio.
    /// </summary>
    int Ratio { get; }

    /// <summary>
    /// Gets the size.
    /// </summary>
    int? Size { get; }

    /// <summary>
    /// Gets the minimum size.
    /// </summary>
    int MinimumSize { get; }
}
