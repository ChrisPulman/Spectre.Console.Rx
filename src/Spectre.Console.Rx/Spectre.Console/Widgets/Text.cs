// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// A renderable piece of text.
/// </summary>
[DebuggerDisplay("{_text,nq}")]
[SuppressMessage("Naming", "CA1724:Type names should not match namespaces", Justification = "Deliberate")]
public sealed class Text(string text, Style? style = null) : Renderable, IHasJustification, IOverflowable
{
    private readonly Paragraph _paragraph = new(text, style);

    /// <summary>
    /// Gets an empty <see cref="Text"/> instance.
    /// </summary>
    public static Text Empty { get; } = new Text(string.Empty);

    /// <summary>
    /// Gets an instance of <see cref="Text"/> containing a new line.
    /// </summary>
    public static Text NewLine { get; } = new Text(Environment.NewLine, Style.Plain);

    /// <summary>
    /// Gets or sets the text alignment.
    /// </summary>
    public Justify? Justification
    {
        get => _paragraph.Justification;
        set => _paragraph.Justification = value;
    }

    /// <summary>
    /// Gets or sets the text overflow strategy.
    /// </summary>
    public Overflow? Overflow
    {
        get => _paragraph.Overflow;
        set => _paragraph.Overflow = value;
    }

    /// <summary>
    /// Gets the character count.
    /// </summary>
    public int Length => _paragraph.Length;

    /// <summary>
    /// Gets the number of lines in the text.
    /// </summary>
    public int Lines => _paragraph.Lines;

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth) => ((IRenderable)_paragraph).Measure(options, maxWidth);

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth) => ((IRenderable)_paragraph).Render(options, maxWidth);
}
