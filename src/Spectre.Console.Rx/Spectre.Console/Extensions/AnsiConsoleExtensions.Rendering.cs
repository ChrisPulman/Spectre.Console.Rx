// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static partial class AnsiConsoleExtensions
{
    /// <summary>
    /// Renders the specified object to the console.
    /// </summary>
    /// <param name="console">The console to render to.</param>
    /// <param name="renderable">The object to render.</param>
    [Obsolete("Consider using IAnsiConsole.Write instead.")]
    public static void Render(this IAnsiConsole console, IRenderable renderable)
    {
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        if (renderable is null)
        {
            throw new ArgumentNullException(nameof(renderable));
        }

        console.Write(renderable);
    }
}