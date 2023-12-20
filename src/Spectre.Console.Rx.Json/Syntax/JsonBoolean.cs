// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json.Syntax;

/// <summary>
/// Represents a boolean literal in the JSON abstract syntax tree.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="JsonBoolean"/> class.
/// </remarks>
/// <param name="lexeme">The lexeme.</param>
public sealed class JsonBoolean(string lexeme) : JsonSyntax
{
    /// <summary>
    /// Gets the lexeme.
    /// </summary>
    public string Lexeme { get; } = lexeme;

    internal override void Accept<T>(JsonSyntaxVisitor<T> visitor, T context) =>
        visitor.VisitBoolean(this, context);
}
