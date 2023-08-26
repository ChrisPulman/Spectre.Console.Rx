// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a console.
/// </summary>
public interface IAnsiConsole
{
    /// <summary>
    /// Gets the console profile.
    /// </summary>
    Profile Profile { get; }

    /// <summary>
    /// Gets the console cursor.
    /// </summary>
    IAnsiConsoleCursor Cursor { get; }

    /// <summary>
    /// Gets the console input.
    /// </summary>
    IAnsiConsoleInput Input { get; }

    /// <summary>
    /// Gets the exclusivity mode.
    /// </summary>
    IExclusivityMode ExclusivityMode { get; }

    /// <summary>
    /// Gets the render pipeline.
    /// </summary>
    RenderPipeline Pipeline { get; }

    /// <summary>
    /// Clears the console.
    /// </summary>
    /// <param name="home">If the cursor should be moved to the home position.</param>
    void Clear(bool home);

    /// <summary>
    /// Writes a <see cref="IRenderable"/> to the console.
    /// </summary>
    /// <param name="renderable">The <see cref="IRenderable"/> to write.</param>
    void Write(IRenderable renderable);
}