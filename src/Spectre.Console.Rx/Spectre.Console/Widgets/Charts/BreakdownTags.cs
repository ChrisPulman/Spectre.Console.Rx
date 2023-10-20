// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class BreakdownTags(List<IBreakdownChartItem> data) : Renderable
{
    private readonly List<IBreakdownChartItem> _data = data ?? throw new ArgumentNullException(nameof(data));

    public int? Width { get; set; }

    public Color ValueColor { get; set; } = Color.Grey;

    public CultureInfo? Culture { get; set; }

    public bool ShowTagValues { get; set; } = true;

    public Func<double, CultureInfo, string>? ValueFormatter { get; set; }

    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        var width = Math.Min(Width ?? maxWidth, maxWidth);
        return new Measurement(width, width);
    }

    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var culture = Culture ?? CultureInfo.InvariantCulture;

        var panels = new List<Panel>();
        foreach (var item in _data)
        {
            var panel = new Panel(GetTag(item, culture))
            {
                Inline = true,
                Padding = new Padding(0, 0, 2, 0)
            };
            panel.NoBorder();

            panels.Add(panel);
        }

        foreach (var segment in ((IRenderable)new Columns(panels).Padding(0, 0)).Render(options, maxWidth))
        {
            yield return segment;
        }
    }

    private static string DefaultFormatter(double value, CultureInfo culture) => value.ToString(culture);

    private string GetTag(IBreakdownChartItem item, CultureInfo culture) =>
        string.Format(culture, "[{0}]■[/] {1}", item.Color.ToMarkup() ?? "default", FormatValue(item, culture)).Trim();

    private string FormatValue(IBreakdownChartItem item, CultureInfo culture)
    {
        var formatter = ValueFormatter ?? DefaultFormatter;

        if (ShowTagValues)
        {
            return string.Format(
                culture,
                "{0} [{1}]{2}[/]",
                item.Label.EscapeMarkup(),
                ValueColor.ToMarkup(),
                formatter(item.Value, culture));
        }

        return item.Label.EscapeMarkup();
    }
}
