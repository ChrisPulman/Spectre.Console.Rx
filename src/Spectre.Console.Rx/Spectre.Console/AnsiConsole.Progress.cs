// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// A console capable of writing ANSI escape sequences.
/// </summary>
public static partial class AnsiConsole
{
    /// <summary>
    /// Creates a new <see cref="Progress"/> instance.
    /// </summary>
    /// <returns>A <see cref="Progress"/> instance.</returns>
    internal static Progress Progress() => Console.Progress();

    /// <summary>
    /// Creates a new <see cref="Status"/> instance.
    /// </summary>
    /// <returns>A <see cref="Status"/> instance.</returns>
    internal static Status Status() => Console.Status();
}
