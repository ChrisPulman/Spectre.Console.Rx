// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json;

/// <summary>
/// A renderable piece of JSON text.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="JsonText"/> class.
/// </remarks>
/// <param name="json">The JSON to render.</param>
public sealed class JsonText(string json) : JustInTimeRenderable
{
    private readonly string _json = json ?? throw new ArgumentNullException(nameof(json));
    private JsonSyntax? _syntax;
    private IJsonParser? _parser;

    /// <summary>
    /// Gets or sets the style used for braces.
    /// </summary>
    public Style? BracesStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for brackets.
    /// </summary>
    public Style? BracketsStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for member names.
    /// </summary>
    public Style? MemberStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for colons.
    /// </summary>
    public Style? ColonStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for commas.
    /// </summary>
    public Style? CommaStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for string literals.
    /// </summary>
    public Style? StringStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for number literals.
    /// </summary>
    public Style? NumberStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for boolean literals.
    /// </summary>
    public Style? BooleanStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for <c>null</c> literals.
    /// </summary>
    public Style? NullStyle { get; set; }

    /// <summary>
    /// Gets or sets the JSON parser.
    /// </summary>
    public IJsonParser? Parser
    {
        get => _parser;

        set
        {
            _syntax = null;
            _parser = value;
        }
    }

    /// <inheritdoc/>
    protected override IRenderable Build()
    {
        _syntax ??= (Parser ?? JsonParser.Shared).Parse(_json);

        var context = new JsonBuilderContext(new JsonTextStyles
        {
            BracesStyle = BracesStyle ?? Color.Grey,
            BracketsStyle = BracketsStyle ?? Color.Grey,
            MemberStyle = MemberStyle ?? Color.Blue,
            ColonStyle = ColonStyle ?? Color.Yellow,
            CommaStyle = CommaStyle ?? Color.Grey,
            StringStyle = StringStyle ?? Color.Red,
            NumberStyle = NumberStyle ?? Color.Green,
            BooleanStyle = BooleanStyle ?? Color.Green,
            NullStyle = NullStyle ?? Color.Grey,
        });

        _syntax.Accept(JsonBuilder.Shared, context);
        return context.Paragraph;
    }
}
