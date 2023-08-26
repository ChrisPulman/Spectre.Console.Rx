// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Contains extension methods for <see cref="IRenderable"/>.
/// </summary>
public static class RenderableExtensions
{
    /// <summary>
    /// Gets the segments for a renderable using the specified console.
    /// </summary>
    /// <param name="renderable">The renderable.</param>
    /// <param name="console">The console.</param>
    /// <returns>An enumerable containing segments representing the specified <see cref="IRenderable"/>.</returns>
    public static IEnumerable<Segment> GetSegments(this IRenderable renderable, IAnsiConsole console)
    {
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        if (renderable is null)
        {
            throw new ArgumentNullException(nameof(renderable));
        }

        var context = RenderOptions.Create(console, console.Profile.Capabilities);
        var renderables = console.Pipeline.Process(context, new[] { renderable });

        return GetSegments(console, context, renderables);
    }

    private static IEnumerable<Segment> GetSegments(IAnsiConsole console, RenderOptions options, IEnumerable<IRenderable> renderables)
    {
        var result = new List<Segment>();
        foreach (var renderable in renderables)
        {
            result.AddRange(renderable.Render(options, console.Profile.Width));
        }

        return Segment.Merge(result);
    }
}