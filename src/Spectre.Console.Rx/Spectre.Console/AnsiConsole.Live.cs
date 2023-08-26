// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// A console capable of writing ANSI escape sequences.
/// </summary>
public static partial class AnsiConsole
{
    /// <summary>
    /// Creates a new <see cref="LiveDisplay"/> instance.
    /// </summary>
    /// <param name="target">The target renderable to update.</param>
    /// <returns>A <see cref="LiveDisplay"/> instance.</returns>
    internal static LiveDisplay Live(IRenderable target) => Console.Live(target);
}
