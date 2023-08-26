// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class BreakdownBar(List<IBreakdownChartItem> data) : Renderable
{
    private readonly List<IBreakdownChartItem> _data = data ?? throw new ArgumentNullException(nameof(data));

    public int? Width { get; set; }

    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        var width = Math.Min(Width ?? maxWidth, maxWidth);
        return new Measurement(width, width);
    }

    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var width = Math.Min(Width ?? maxWidth, maxWidth);

        // Chart
        var maxValue = _data.Sum(i => i.Value);
        var items = _data.ToArray();
        var bars = Ratio.Distribute(width, items.Select(i => Math.Max(0, (int)(width * (i.Value / maxValue)))).ToArray());

        for (var index = 0; index < items.Length; index++)
        {
            yield return new Segment(new string('█', bars[index]), new Style(items[index].Color));
        }

        yield return Segment.LineBreak;
    }
}
