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
    private readonly List<IRenderable> _items = new List<IRenderable>(items ?? []);

    /// <summary>
    /// Gets a row item at the specified grid column index.
    /// </summary>
    /// <param name="index">The grid column index.</param>
    /// <returns>The row item at the specified grid column index.</returns>
    public IRenderable this[int index]
    {
        get => _items[index];
    }

    internal void Add(IRenderable item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _items.Add(item);
    }

    /// <inheritdoc/>
    public IEnumerator<IRenderable> GetEnumerator() => _items.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}