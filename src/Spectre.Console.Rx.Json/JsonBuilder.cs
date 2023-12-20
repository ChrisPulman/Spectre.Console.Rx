// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json;

internal sealed class JsonBuilder : JsonSyntaxVisitor<JsonBuilderContext>
{
    public static JsonBuilder Shared { get; } = new JsonBuilder();

    public override void VisitObject(JsonObject syntax, JsonBuilderContext context)
    {
        context.Paragraph.Append("{", context.Styling.BracesStyle);
        context.Paragraph.Append("\n");
        context.Indentation++;

        foreach (var (_, _, last, property) in syntax.Members.Enumerate())
        {
            context.InsertIndentation();
            property.Accept(this, context);

            if (!last)
            {
                context.Paragraph.Append(",", context.Styling.CommaStyle);
            }

            context.Paragraph.Append("\n");
        }

        context.Indentation--;
        context.InsertIndentation();
        context.Paragraph.Append("}", context.Styling.BracesStyle);
    }

    public override void VisitArray(JsonArray syntax, JsonBuilderContext context)
    {
        context.Paragraph.Append("[", context.Styling.BracketsStyle);
        context.Paragraph.Append("\n");
        context.Indentation++;

        foreach (var (_, _, last, item) in syntax.Items.Enumerate())
        {
            context.InsertIndentation();
            item.Accept(this, context);

            if (!last)
            {
                context.Paragraph.Append(",", context.Styling.CommaStyle);
            }

            context.Paragraph.Append("\n");
        }

        context.Indentation--;
        context.InsertIndentation();
        context.Paragraph.Append("]", context.Styling.BracketsStyle);
    }

    public override void VisitMember(JsonMember syntax, JsonBuilderContext context)
    {
        context.Paragraph.Append(syntax.Name, context.Styling.MemberStyle);
        context.Paragraph.Append(":", context.Styling.ColonStyle);
        context.Paragraph.Append(" ");

        syntax.Value.Accept(this, context);
    }

    public override void VisitNumber(JsonNumber syntax, JsonBuilderContext context) =>
        context.Paragraph.Append(syntax.Lexeme, context.Styling.NumberStyle);

    public override void VisitString(JsonString syntax, JsonBuilderContext context) =>
        context.Paragraph.Append(syntax.Lexeme, context.Styling.StringStyle);

    public override void VisitBoolean(JsonBoolean syntax, JsonBuilderContext context) =>
        context.Paragraph.Append(syntax.Lexeme, context.Styling.BooleanStyle);

    public override void VisitNull(JsonNull syntax, JsonBuilderContext context) =>
        context.Paragraph.Append(syntax.Lexeme, context.Styling.NullStyle);
}
