namespace Spectre.Console.Rx;

internal abstract class TableAccessor(Table table, RenderOptions options)
{
    private readonly Table _table = table ?? throw new ArgumentNullException(nameof(table));

    public RenderOptions Options { get; } = options ?? throw new ArgumentNullException(nameof(options));
    public IReadOnlyList<TableColumn> Columns => _table.Columns;
    public virtual IReadOnlyList<TableRow> Rows => _table.Rows;
    public bool Expand => _table.Expand || _table.Width != null;
}