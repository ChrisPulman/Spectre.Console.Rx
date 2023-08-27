// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using static Spectre.Console.Rx.AnsiSequences;

namespace Spectre.Console.Rx.Rendering;

internal sealed class LiveRenderable(IAnsiConsole console) : Renderable
{
    private readonly object _lock = new();
    private readonly IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));
    private SegmentShape? _shape;

    public LiveRenderable(IAnsiConsole console, IRenderable renderable)
        : this(console) => Target = renderable ?? throw new ArgumentNullException(nameof(renderable));

    public IRenderable? Target { get; private set; }

    public bool DidOverflow { get; private set; }

    [MemberNotNullWhen(true, nameof(Target))]
    public bool HasRenderable => Target != null;

    public VerticalOverflow Overflow { get; set; } = VerticalOverflow.Ellipsis;

    public VerticalOverflowCropping OverflowCropping { get; set; } = VerticalOverflowCropping.Top;

    public void SetRenderable(IRenderable? renderable)
    {
        lock (_lock)
        {
            Target = renderable;
        }
    }

    public IRenderable PositionCursor()
    {
        lock (_lock)
        {
            if (_shape == null)
            {
                return new ControlCode(string.Empty);
            }

            var linesToMoveUp = _shape.Value.Height - 1;
            return new ControlCode("\r" + (EL(2) + CUU(1)).Repeat(linesToMoveUp));
        }
    }

    public IRenderable RestoreCursor()
    {
        lock (_lock)
        {
            if (_shape == null)
            {
                return new ControlCode(string.Empty);
            }

            var linesToClear = _shape.Value.Height - 1;
            return new ControlCode("\r" + EL(2) + (CUU(1) + EL(2)).Repeat(linesToClear));
        }
    }

    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        lock (_lock)
        {
            DidOverflow = false;

            if (Target != null)
            {
                var segments = Target.Render(options, maxWidth);
                var lines = Segment.SplitLines(segments);

                var shape = SegmentShape.Calculate(options, lines);
                if (shape.Height > _console.Profile.Height)
                {
                    if (Overflow == VerticalOverflow.Crop)
                    {
                        if (OverflowCropping == VerticalOverflowCropping.Bottom)
                        {
                            // Remove bottom lines
                            var index = Math.Min(_console.Profile.Height, lines.Count);
                            var count = lines.Count - index;
                            lines.RemoveRange(index, count);
                        }
                        else
                        {
                            // Remove top lines
                            var start = lines.Count - _console.Profile.Height;
                            lines.RemoveRange(0, start);
                        }

                        shape = SegmentShape.Calculate(options, lines);
                    }
                    else if (Overflow == VerticalOverflow.Ellipsis)
                    {
                        var ellipsisText = _console.Profile.Capabilities.Unicode ? "…" : "...";
                        var ellipsis = new SegmentLine(((IRenderable)new Markup($"[yellow]{ellipsisText}[/]")).Render(options, maxWidth));

                        if (OverflowCropping == VerticalOverflowCropping.Bottom)
                        {
                            // Remove bottom lines
                            var index = Math.Min(_console.Profile.Height - 1, lines.Count);
                            var count = lines.Count - index;
                            lines.RemoveRange(index, count);
                            lines.Add(ellipsis);
                        }
                        else
                        {
                            // Remove top lines
                            var start = lines.Count - _console.Profile.Height;
                            lines.RemoveRange(0, start + 1);
                            lines.Insert(0, ellipsis);
                        }

                        shape = SegmentShape.Calculate(options, lines);
                    }

                    DidOverflow = true;
                }

                _shape = _shape == null ? shape : _shape.Value.Inflate(shape);
                _shape.Value.Apply(options, ref lines);

                foreach (var (_, _, last, line) in lines.Enumerate())
                {
                    foreach (var item in line)
                    {
                        yield return item;
                    }

                    if (!last)
                    {
                        yield return Segment.LineBreak;
                    }
                }

                yield break;
            }

            _shape = null;
        }
    }
}
