namespace Spectre.Console.Rx.Rendering;

internal readonly struct SegmentShape(int width, int height)
{
    public int Width { get; } = width;
    public int Height { get; } = height;

    public static SegmentShape Calculate(RenderOptions options, List<SegmentLine> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);

        var height = lines.Count;
        var width = lines.Count > 0 ? lines.Max(l => Segment.CellCount(l)) : 0;

        return new SegmentShape(width, height);
    }

    public SegmentShape Inflate(SegmentShape other) => new SegmentShape(
            Math.Max(Width, other.Width),
            Math.Max(Height, other.Height));

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
                lines.Add([
                    Segment.Padding(Width)
                ]);
            }
        }
    }
}