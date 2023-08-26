// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a table row.
/// </summary>
public sealed class TableRow : IEnumerable<IRenderable>
{
    private readonly List<IRenderable> _items;

    /// <summary>
    /// Initializes a new instance of the <see cref="TableRow"/> class.
    /// </summary>
    /// <param name="items">The row items.</param>
    public TableRow(IEnumerable<IRenderable> items)
        : this(items, false, false)
    {
    }

    private TableRow(IEnumerable<IRenderable> items, bool isHeader, bool isFooter)
    {
        _items = new List<IRenderable>(items ?? Array.Empty<IRenderable>());

        IsHeader = isHeader;
        IsFooter = isFooter;
    }

    /// <summary>
    /// Gets the number of columns in the row.
    /// </summary>
    public int Count => _items.Count;

    internal bool IsHeader { get; }

    internal bool IsFooter { get; }

    /// <summary>
    /// Gets a row item at the specified table column index.
    /// </summary>
    /// <param name="index">The table column index.</param>
    /// <returns>The row item at the specified table column index.</returns>
    public IRenderable this[int index] => _items[index];

    /// <inheritdoc/>
    public IEnumerator<IRenderable> GetEnumerator() => _items.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    internal static TableRow Header(IEnumerable<IRenderable> items) => new(items, true, false);

    internal static TableRow Footer(IEnumerable<IRenderable> items) => new(items, false, true);

    internal void Add(IRenderable item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        _items.Add(item);
    }
}
