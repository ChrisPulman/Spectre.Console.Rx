namespace Spectre.Console.Rx;

[DebuggerDisplay("{Region,nq}")]
internal sealed class LayoutRender(Region region, List<SegmentLine> render)
{
    public Region Region { get; } = region;
    public List<SegmentLine> Render { get; } = render ?? throw new ArgumentNullException(nameof(render));
}