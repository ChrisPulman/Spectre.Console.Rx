// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class ControlCode : Renderable
{
    private readonly Segment _segment;

    public ControlCode(string control) => _segment = Segment.Control(control);

    protected override Measurement Measure(RenderOptions options, int maxWidth) => new(0, 0);

    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        if (options.Ansi)
        {
            yield return _segment;
        }
    }
}