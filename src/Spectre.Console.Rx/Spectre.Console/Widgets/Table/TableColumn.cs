// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a table column.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TableColumn"/> class.
/// </remarks>
/// <param name="header">The <see cref="IRenderable"/> instance to use as the table column header.</param>
public sealed class TableColumn(IRenderable header) : IColumn
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TableColumn"/> class.
    /// </summary>
    /// <param name="header">The table column header.</param>
    public TableColumn(string header)
        : this(new Markup(header).Overflow(Overflow.Ellipsis))
    {
    }

    /// <summary>
    /// Gets or sets the column header.
    /// </summary>
    public IRenderable Header { get; set; } = header ?? throw new ArgumentNullException(nameof(header));

    /// <summary>
    /// Gets or sets the column footer.
    /// </summary>
    public IRenderable? Footer { get; set; }

    /// <summary>
    /// Gets or sets the width of the column.
    /// If <c>null</c>, the column will adapt to its contents.
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the padding of the column.
    /// Vertical padding (top and bottom) is ignored.
    /// </summary>
    public Padding? Padding { get; set; } = new Padding(1, 0, 1, 0);

    /// <summary>
    /// Gets or sets a value indicating whether wrapping of
    /// text within the column should be prevented.
    /// </summary>
    public bool NoWrap { get; set; }

    /// <summary>
    /// Gets or sets the alignment of the column.
    /// </summary>
    public Justify? Alignment { get; set; }
}
