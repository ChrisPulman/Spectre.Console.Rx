// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// A console capable of writing ANSI escape sequences.
/// </summary>
public static partial class AnsiConsole
{
    /// <summary>
    /// Writes an exception to the console.
    /// </summary>
    /// <param name="exception">The exception to write to the console.</param>
    /// <param name="format">The exception format options.</param>
    public static void WriteException(Exception exception, ExceptionFormats format = ExceptionFormats.Default) => Console.WriteException(exception, format);

    /// <summary>
    /// Writes an exception to the console.
    /// </summary>
    /// <param name="exception">The exception to write to the console.</param>
    /// <param name="settings">The exception settings.</param>
    public static void WriteException(Exception exception, ExceptionSettings settings) => Console.WriteException(exception, settings);
}