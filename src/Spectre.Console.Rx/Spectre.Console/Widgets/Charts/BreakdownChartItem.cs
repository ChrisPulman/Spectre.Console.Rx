// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// An item that's shown in a breakdown chart.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BreakdownChartItem"/> class.
/// </remarks>
public sealed class BreakdownChartItem(string label, double value, Color color) : IBreakdownChartItem
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
    public Color Color { get; } = color;
}
