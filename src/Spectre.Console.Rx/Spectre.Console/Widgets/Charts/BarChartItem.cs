// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// An item that's shown in a bar chart.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BarChartItem"/> class.
/// </remarks>
/// <param name="label">The item label.</param>
/// <param name="value">The item value.</param>
/// <param name="color">The item color.</param>
public sealed class BarChartItem(string label, double value, Color? color = null) : IBarChartItem
{
    /// <summary>
    /// Gets the item label.
    /// </summary>
    public string Label { get; } = label ?? throw new ArgumentNullException(nameof(label));

    /// <summary>
    /// Gets the item value.
    /// </summary>
    public double Value { get; } = value;

    /// <summary>
    /// Gets the item color.
    /// </summary>
    public Color? Color { get; } = color;
}
