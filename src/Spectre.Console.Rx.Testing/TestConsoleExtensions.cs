// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Testing;

/// <summary>
/// Contains extensions for <see cref="TestConsole"/>.
/// </summary>
public static class TestConsoleExtensions
{
    /// <summary>
    /// Sets the console's color system.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="colors">The color system to use.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TestConsole Colors(this TestConsole console, ColorSystem colors)
    {
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        console.Profile.Capabilities.ColorSystem = colors;
        return console;
    }

    /// <summary>
    /// Sets whether or not ANSI is supported.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="enable">Whether or not VT/ANSI control codes are supported.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TestConsole SupportsAnsi(this TestConsole console, bool enable)
    {
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        console.Profile.Capabilities.Ansi = enable;
        return console;
    }

    /// <summary>
    /// Makes the console interactive.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TestConsole Interactive(this TestConsole console)
    {
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        console.Profile.Capabilities.Interactive = true;
        return console;
    }

    /// <summary>
    /// Sets the console width.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="width">The console width.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TestConsole Width(this TestConsole console, int width)
    {
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        console.Profile.Width = width;
        return console;
    }

    /// <summary>
    /// Sets the console height.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="width">The console height.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TestConsole Height(this TestConsole console, int width)
    {
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        console.Profile.Height = width;
        return console;
    }

    /// <summary>
    /// Sets the console size.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="size">The console size.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TestConsole Size(this TestConsole console, Size size)
    {
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        console.Profile.Width = size.Width;
        console.Profile.Height = size.Height;
        return console;
    }

    /// <summary>
    /// Turns on emitting of VT/ANSI sequences.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TestConsole EmitAnsiSequences(this TestConsole console)
    {
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        console.SetCursor(null);
        console.EmitAnsiSequences = true;
        return console;
    }
}
