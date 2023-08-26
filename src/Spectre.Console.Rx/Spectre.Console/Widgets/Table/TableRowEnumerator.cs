// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class TableRowEnumerator(TableRow[] items) : IEnumerator<TableRow>
{
    private readonly TableRow[] _items = items ?? throw new ArgumentNullException(nameof(items));
    private int _index = -1;

    public TableRow Current => _items[_index];

    object? IEnumerator.Current => _items[_index];

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        _index++;
        return _index < _items.Length;
    }

    public void Reset() => _index = -1;
}
