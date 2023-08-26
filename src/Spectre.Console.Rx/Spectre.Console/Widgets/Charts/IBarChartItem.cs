// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a bar chart item.
/// </summary>
public interface IBarChartItem
{
    /// <summary>
    /// Gets the item label.
    /// </summary>
    string Label { get; }

    /// <summary>
    /// Gets the item value.
    /// </summary>
    double Value { get; }

    /// <summary>
    /// Gets the item color.
    /// </summary>
    Color? Color { get; }
}