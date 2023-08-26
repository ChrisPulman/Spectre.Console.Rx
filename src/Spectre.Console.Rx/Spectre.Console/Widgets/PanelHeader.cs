// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a panel header.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PanelHeader"/> class.
/// </remarks>
/// <param name="text">The panel header text.</param>
/// <param name="alignment">The panel header alignment.</param>
public sealed class PanelHeader(string text, Justify? alignment = null) : IHasJustification
{
    /// <summary>
    /// Gets the panel header text.
    /// </summary>
    public string Text { get; } = text ?? throw new ArgumentNullException(nameof(text));

    /// <summary>
    /// Gets or sets the panel header alignment.
    /// </summary>
    public Justify? Justification { get; set; } = alignment;

    /// <summary>
    /// Sets the panel header style.
    /// </summary>
    /// <param name="style">The panel header style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    [Obsolete("Use markup instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public PanelHeader SetStyle(Style? style) => this;

    /// <summary>
    /// Sets the panel header style.
    /// </summary>
    /// <param name="style">The panel header style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    [Obsolete("Use markup instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public PanelHeader SetStyle(string style) => this;

    /// <summary>
    /// Sets the panel header alignment.
    /// </summary>
    /// <param name="alignment">The panel header alignment.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public PanelHeader SetAlignment(Justify alignment)
    {
        Justification = alignment;
        return this;
    }
}
