// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Base class for a renderable object implementing <see cref="IRenderable"/>.
/// </summary>
public abstract class Renderable : IRenderable
{
    /// <inheritdoc/>
    [DebuggerStepThrough]
    Measurement IRenderable.Measure(RenderOptions options, int maxWidth) => Measure(options, maxWidth);

    /// <inheritdoc/>
    [DebuggerStepThrough]
    IEnumerable<Segment> IRenderable.Render(RenderOptions options, int maxWidth) => Render(options, maxWidth);

    /// <summary>
    /// Measures the renderable object.
    /// </summary>
    /// <param name="options">The render options.</param>
    /// <param name="maxWidth">The maximum allowed width.</param>
    /// <returns>The minimum and maximum width of the object.</returns>
    protected virtual Measurement Measure(RenderOptions options, int maxWidth) => new(maxWidth, maxWidth);

    /// <summary>
    /// Renders the object.
    /// </summary>
    /// <param name="options">The render options.</param>
    /// <param name="maxWidth">The maximum allowed width.</param>
    /// <returns>A collection of segments.</returns>
    protected abstract IEnumerable<Segment> Render(RenderOptions options, int maxWidth);
}