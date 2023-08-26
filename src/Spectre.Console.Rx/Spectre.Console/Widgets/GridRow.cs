// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a grid row.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GridRow"/> class.
/// </remarks>
/// <param name="items">The row items.</param>
public sealed class GridRow(IEnumerable<IRenderable> items) : IEnumerable<IRenderable>
{
    private readonly List<IRenderable> _items = new(items ?? Array.Empty<IRenderable>());

    /// <summary>
    /// Gets a row item at the specified grid column index.
    /// </summary>
    /// <param name="index">The grid column index.</param>
    /// <returns>The row item at the specified grid column index.</returns>
    public IRenderable this[int index] => _items[index];

    /// <inheritdoc/>
    public IEnumerator<IRenderable> GetEnumerator() => _items.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    internal void Add(IRenderable item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        _items.Add(item);
    }
}
