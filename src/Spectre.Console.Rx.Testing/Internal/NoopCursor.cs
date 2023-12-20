// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Testing;

internal sealed class NoopCursor : IAnsiConsoleCursor
{
    public void Move(CursorDirection direction, int steps)
    {
    }

    public void SetPosition(int column, int line)
    {
    }

    public void Show(bool show)
    {
    }
}
