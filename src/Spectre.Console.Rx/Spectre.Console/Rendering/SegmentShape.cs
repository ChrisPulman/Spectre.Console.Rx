// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

internal readonly struct SegmentShape(int width, int height)
{
    public int Width { get; } = width;

    public int Height { get; } = height;

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter.", Justification = "Deliberate")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Deliberate")]
    public static SegmentShape Calculate(RenderOptions options, List<SegmentLine> lines)
    {
        if (lines is null)
        {
            throw new ArgumentNullException(nameof(lines));
        }

        var height = lines.Count;
        var width = lines.Max(l => Segment.CellCount(l));

        return new SegmentShape(width, height);
    }

    public SegmentShape Inflate(in SegmentShape other) => new(
            Math.Max(Width, other.Width),
            Math.Max(Height, other.Height));

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter.", Justification = "Deliberate")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Deliberate")]
    public void Apply(RenderOptions options, ref List<SegmentLine> lines)
    {
        foreach (var line in lines)
        {
            var length = Segment.CellCount(line);
            var missing = Width - length;
            if (missing > 0)
            {
                line.Add(Segment.Padding(missing));
            }
        }

        if (lines.Count < Height && Width > 0)
        {
            var missing = Height - lines.Count;
            for (var i = 0; i < missing; i++)
            {
                lines.Add(new SegmentLine
                    {
                        Segment.Padding(Width),
                    });
            }
        }
    }
}
