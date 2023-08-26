// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Represents a collection of segments.
/// </summary>
public sealed class SegmentLine : List<Segment>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SegmentLine"/> class.
    /// </summary>
    public SegmentLine()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SegmentLine"/> class.
    /// </summary>
    /// <param name="segments">The segments.</param>
    public SegmentLine(IEnumerable<Segment> segments)
        : base(segments)
    {
    }

    /// <summary>
    /// Gets the width of the line.
    /// </summary>
    public int Length => this.Sum(line => line.Text.Length);

    /// <summary>
    /// Gets the number of cells the segment line occupies.
    /// </summary>
    /// <returns>The cell width of the segment line.</returns>
    public int CellCount() => Segment.CellCount(this);

    /// <summary>
    /// Preprends a segment to the line.
    /// </summary>
    /// <param name="segment">The segment to prepend.</param>
    public void Prepend(Segment segment)
    {
        if (segment is null)
        {
            throw new ArgumentNullException(nameof(segment));
        }

        Insert(0, segment);
    }
}
