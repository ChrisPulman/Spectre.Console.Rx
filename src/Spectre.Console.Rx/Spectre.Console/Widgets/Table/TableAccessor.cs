// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal abstract class TableAccessor
{
    private readonly Table _table;

    protected TableAccessor(Table table, RenderOptions options)
    {
        _table = table ?? throw new ArgumentNullException(nameof(table));
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public RenderOptions Options { get; }

    public IReadOnlyList<TableColumn> Columns => _table.Columns;

    public virtual IReadOnlyList<TableRow> Rows => _table.Rows;

    public bool Expand => _table.Expand || _table.Width != null;
}
