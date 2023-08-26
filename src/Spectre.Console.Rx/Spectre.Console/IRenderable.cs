// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Represents something that can be rendered to the console.
/// </summary>
public interface IRenderable
{
    /// <summary>
    /// Measures the renderable object.
    /// </summary>
    /// <param name="options">The render options.</param>
    /// <param name="maxWidth">The maximum allowed width.</param>
    /// <returns>The minimum and maximum width of the object.</returns>
    Measurement Measure(RenderOptions options, int maxWidth);

    /// <summary>
    /// Renders the object.
    /// </summary>
    /// <param name="options">The render options.</param>
    /// <param name="maxWidth">The maximum allowed width.</param>
    /// <returns>A collection of segments.</returns>
    IEnumerable<Segment> Render(RenderOptions options, int maxWidth);
}