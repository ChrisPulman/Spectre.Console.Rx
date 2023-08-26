// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// An enumerator for <see cref="SegmentLine"/> collections.
/// </summary>
public sealed class SegmentLineEnumerator : IEnumerable<Segment>
{
    private readonly List<SegmentLine> _lines;

    /// <summary>
    /// Initializes a new instance of the <see cref="SegmentLineEnumerator"/> class.
    /// </summary>
    /// <param name="lines">The lines to enumerate.</param>
    public SegmentLineEnumerator(IEnumerable<SegmentLine> lines)
    {
        if (lines is null)
        {
            throw new ArgumentNullException(nameof(lines));
        }

        _lines = new List<SegmentLine>(lines);
    }

    /// <inheritdoc/>
    public IEnumerator<Segment> GetEnumerator() => new SegmentLineIterator(_lines);

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}