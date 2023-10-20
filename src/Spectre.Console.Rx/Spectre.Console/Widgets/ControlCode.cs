// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// ControlCode.
/// </summary>
/// <seealso cref="Spectre.Console.Rx.Rendering.Renderable" />
/// <remarks>
/// Initializes a new instance of the <see cref="ControlCode"/> class.
/// </remarks>
/// <param name="control">The control.</param>
public sealed class ControlCode(string control) : Renderable
{
    private readonly Segment _segment = Segment.Control(control);

    /// <summary>
    /// Measures the renderable object.
    /// </summary>
    /// <param name="options">The render options.</param>
    /// <param name="maxWidth">The maximum allowed width.</param>
    /// <returns>
    /// The minimum and maximum width of the object.
    /// </returns>
    protected override Measurement Measure(RenderOptions options, int maxWidth) => new(0, 0);

    /// <summary>
    /// Renders the object.
    /// </summary>
    /// <param name="options">The render options.</param>
    /// <param name="maxWidth">The maximum allowed width.</param>
    /// <returns>
    /// A collection of segments.
    /// </returns>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        if (options.Ansi)
        {
            yield return _segment;
        }
    }
}
