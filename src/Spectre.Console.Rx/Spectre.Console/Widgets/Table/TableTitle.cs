// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a table title such as a heading or footnote.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TableTitle"/> class.
/// </remarks>
/// <param name="text">The title text.</param>
/// <param name="style">The title style.</param>
public sealed class TableTitle(string text, Style? style = null)
{
    /// <summary>
    /// Gets the title text.
    /// </summary>
    public string Text { get; } = text ?? throw new ArgumentNullException(nameof(text));

    /// <summary>
    /// Gets or sets the title style.
    /// </summary>
    public Style? Style { get; set; } = style;

    /// <summary>
    /// Sets the title style.
    /// </summary>
    /// <param name="style">The title style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public TableTitle SetStyle(Style? style)
    {
        Style = style ?? Style.Plain;
        return this;
    }

    /// <summary>
    /// Sets the title style.
    /// </summary>
    /// <param name="style">The title style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public TableTitle SetStyle(string style)
    {
        if (style is null)
        {
            throw new ArgumentNullException(nameof(style));
        }

        Style = Style.Parse(style);
        return this;
    }
}
