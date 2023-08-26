// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a console backend.
/// </summary>
internal interface IAnsiConsoleBackend
{
    /// <summary>
    /// Gets the console cursor for the backend.
    /// </summary>
    IAnsiConsoleCursor Cursor { get; }

    /// <summary>
    /// Clears the console.
    /// </summary>
    /// <param name="home">If the cursor should be moved to the home position.</param>
    void Clear(bool home);

    /// <summary>
    /// Writes a <see cref="IRenderable"/> to the console backend.
    /// </summary>
    /// <param name="renderable">The <see cref="IRenderable"/> to write.</param>
    void Write(IRenderable renderable);
}