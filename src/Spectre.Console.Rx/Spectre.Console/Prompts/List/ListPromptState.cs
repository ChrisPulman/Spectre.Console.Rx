// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class ListPromptState<T>(IReadOnlyList<ListPromptItem<T>> items, int pageSize, bool wrapAround)
    where T : notnull
{
    public int Index { get; private set; }

    public int ItemCount => Items.Count;

    public int PageSize { get; } = pageSize;

    public bool WrapAround { get; } = wrapAround;

    public IReadOnlyList<ListPromptItem<T>> Items { get; } = items;

    public ListPromptItem<T> Current => Items[Index];

    public bool Update(ConsoleKey key)
    {
        var index = key switch
        {
            ConsoleKey.UpArrow => Index - 1,
            ConsoleKey.DownArrow => Index + 1,
            ConsoleKey.Home => 0,
            ConsoleKey.End => ItemCount - 1,
            ConsoleKey.PageUp => Index - PageSize,
            ConsoleKey.PageDown => Index + PageSize,
            _ => Index,
        };

        index = WrapAround
            ? (ItemCount + (index % ItemCount)) % ItemCount
            : index.Clamp(0, ItemCount - 1);
        if (index != Index)
        {
            Index = index;
            return true;
        }

        return false;
    }
}
