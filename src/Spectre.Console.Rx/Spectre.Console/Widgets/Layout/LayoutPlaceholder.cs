// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class LayoutPlaceholder(Layout layout) : Renderable
{
    public Layout Layout { get; } = layout ?? throw new ArgumentNullException(nameof(layout));

    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var width = maxWidth;
        var height = options.Height ?? options.ConsoleSize.Height;
        var title = Layout.Name != null
            ? $"{Layout.Name} ({width} x {height})"
            : $"{width} x {height}";

        var panel = new Panel(
            Align.Center(new Text("Placeholder"), VerticalAlignment.Middle))
        {
            Width = maxWidth,
            Height = options.Height ?? options.ConsoleSize.Height,
            Header = new PanelHeader(title),
            Border = BoxBorder.Rounded,
        };

        return ((IRenderable)panel).Render(options, maxWidth);
    }
}
