// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Represents a render hook.
/// </summary>
public interface IRenderHook
{
    /// <summary>
    /// Processes the specified renderables.
    /// </summary>
    /// <param name="options">The render options.</param>
    /// <param name="renderables">The renderables to process.</param>
    /// <returns>The processed renderables.</returns>
    IEnumerable<IRenderable> Process(RenderOptions options, IEnumerable<IRenderable> renderables);
}