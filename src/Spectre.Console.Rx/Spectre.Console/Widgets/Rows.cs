// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Renders things in rows.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Rows"/> class.
/// </remarks>
/// <param name="children">The items to render as rows.</param>
public sealed class Rows(IEnumerable<IRenderable> children) : Renderable, IExpandable
{
    private readonly List<IRenderable> _children = new(children ?? throw new ArgumentNullException(nameof(children)));

    /// <summary>
    /// Initializes a new instance of the <see cref="Rows"/> class.
    /// </summary>
    /// <param name="items">The items to render as rows.</param>
    public Rows(params IRenderable[] items)
        : this((IEnumerable<IRenderable>)items)
    {
    }

    /// <inheritdoc/>
    public bool Expand { get; set; }

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        if (Expand)
        {
            return new Measurement(maxWidth, maxWidth);
        }

        var measurements = _children.Select(c => c.Measure(options, maxWidth)).ToArray();
        if (measurements.Length > 0)
        {
            return new Measurement(
                measurements.Min(c => c.Min),
                measurements.Min(c => c.Max));
        }

        return new Measurement(0, 0);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var result = new List<Segment>();

        foreach (var child in _children)
        {
            var segments = child.Render(options, maxWidth);
            foreach (var (_, _, last, segment) in segments.Enumerate())
            {
                result.Add(segment);

                if (last)
                {
                    if (!segment.IsLineBreak)
                    {
                        result.Add(Segment.LineBreak);
                    }
                }
            }
        }

        return result;
    }
}
