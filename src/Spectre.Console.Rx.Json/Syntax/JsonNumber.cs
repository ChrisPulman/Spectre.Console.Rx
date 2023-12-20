// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json.Syntax;

internal sealed class JsonNumber(string lexeme) : JsonSyntax
{
    public string Lexeme { get; } = lexeme;

    internal override void Accept<T>(JsonSyntaxVisitor<T> visitor, T context) =>
        visitor.VisitNumber(this, context);
}
