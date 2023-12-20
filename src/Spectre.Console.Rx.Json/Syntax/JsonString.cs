// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json.Syntax;

/// <summary>
/// Represents a string literal in the JSON abstract syntax tree.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="JsonString"/> class.
/// </remarks>
/// <param name="lexeme">The lexeme.</param>
public sealed class JsonString(string lexeme) : JsonSyntax
{
    /// <summary>
    /// Gets the lexeme.
    /// </summary>
    public string Lexeme { get; } = lexeme;

    internal override void Accept<T>(JsonSyntaxVisitor<T> visitor, T context) =>
        visitor.VisitString(this, context);
}
