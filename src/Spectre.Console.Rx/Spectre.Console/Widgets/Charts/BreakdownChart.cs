// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// A renderable breakdown chart.
/// </summary>
public sealed class BreakdownChart : Renderable, IHasCulture, IExpandable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BreakdownChart"/> class.
    /// </summary>
    public BreakdownChart()
    {
        Data = new List<IBreakdownChartItem>();
        Culture = CultureInfo.InvariantCulture;
    }

    /// <summary>
    /// Gets the breakdown chart data.
    /// </summary>
    public List<IBreakdownChartItem> Data { get; }

    /// <summary>
    /// Gets or sets the Color in which the values will be shown.
    /// </summary>
    public Color ValueColor { get; set; } = Color.Grey;

    /// <summary>
    /// Gets or sets the width of the breakdown chart.
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not to show tags.
    /// </summary>
    public bool ShowTags { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether or not to show tag values.
    /// </summary>
    public bool ShowTagValues { get; set; } = true;

    /// <summary>
    /// Gets or sets the tag value formatter.
    /// </summary>
    public Func<double, CultureInfo, string>? ValueFormatter { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not the
    /// chart and tags should be rendered in compact mode.
    /// </summary>
    public bool Compact { get; set; } = true;

    /// <summary>
    /// Gets or sets the <see cref="CultureInfo"/> to use
    /// when rendering values.
    /// </summary>
    /// <remarks>Defaults to invariant culture.</remarks>
    public CultureInfo? Culture { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not the object should
    /// expand to the available space. If <c>false</c>, the object's
    /// width will be auto calculated.
    /// </summary>
    public bool Expand { get; set; } = true;

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        var width = Math.Min(Width ?? maxWidth, maxWidth);
        return new Measurement(width, width);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var width = Math.Min(Width ?? maxWidth, maxWidth);

        var grid = new Grid().Width(width);
        grid.AddColumn(new GridColumn().NoWrap())
            .AddRow(new BreakdownBar(Data)
            {
                Width = width,
            });

        //// Bar

        if (ShowTags)
        {
            if (!Compact)
            {
                grid.AddEmptyRow();
            }

            // Tags
            grid.AddRow(new BreakdownTags(Data)
            {
                Width = width,
                Culture = Culture,
                ShowTagValues = ShowTagValues,
                ValueFormatter = ValueFormatter,
                ValueColor = ValueColor,
            });
        }

        if (!Expand)
        {
            grid.Collapse();
        }

        return ((IRenderable)grid).Render(options, width);
    }
}
