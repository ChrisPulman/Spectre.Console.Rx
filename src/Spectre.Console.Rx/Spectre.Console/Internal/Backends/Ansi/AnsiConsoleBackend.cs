// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using static Spectre.Console.Rx.AnsiSequences;

namespace Spectre.Console.Rx;

internal sealed class AnsiConsoleBackend : IAnsiConsoleBackend
{
    private readonly IAnsiConsole _console;

    public AnsiConsoleBackend(IAnsiConsole console)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
        Cursor = new AnsiConsoleCursor(this);
    }

    public IAnsiConsoleCursor Cursor { get; }

    public void Clear(bool home)
    {
        Write(new ControlCode(ED(2)));
        Write(new ControlCode(ED(3)));

        if (home)
        {
            Write(new ControlCode(CUP(1, 1)));
        }
    }

    public void Write(IRenderable renderable)
    {
        var result = AnsiBuilder.Build(_console, renderable);
        if (result?.Length > 0)
        {
            _console.Profile.Out.Writer.Write(result);
            _console.Profile.Out.Writer.Flush();
        }
    }
}
