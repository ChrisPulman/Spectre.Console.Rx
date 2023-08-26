// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class LegacyConsoleBackend(IAnsiConsole console) : IAnsiConsoleBackend
{
    private readonly IAnsiConsole _console = console ?? throw new System.ArgumentNullException(nameof(console));
    private Style _lastStyle = Style.Plain;

    public IAnsiConsoleCursor Cursor { get; } = new LegacyConsoleCursor();

    public void Clear(bool home)
    {
        var (x, y) = (System.Console.CursorLeft, System.Console.CursorTop);

        System.Console.Clear();

        if (!home)
        {
            // Set the cursor position
            System.Console.SetCursorPosition(x, y);
        }
    }

    public void Write(IRenderable renderable)
    {
        foreach (var segment in renderable.GetSegments(_console))
        {
            if (segment.IsControlCode)
            {
                continue;
            }

            if (_lastStyle?.Equals(segment.Style) != true)
            {
                SetStyle(segment.Style);
            }

            _console.Profile.Out.Writer.Write(segment.Text.NormalizeNewLines(native: true));
        }
    }

    private void SetStyle(Style style)
    {
        _lastStyle = style;

        System.Console.ResetColor();

        var background = Color.ToConsoleColor(style.Background);
        if (_console.Profile.Capabilities.ColorSystem != ColorSystem.NoColors && (int)background != -1)
        {
            System.Console.BackgroundColor = background;
        }

        var foreground = Color.ToConsoleColor(style.Foreground);
        if (_console.Profile.Capabilities.ColorSystem != ColorSystem.NoColors && (int)foreground != -1)
        {
            System.Console.ForegroundColor = foreground;
        }
    }
}
