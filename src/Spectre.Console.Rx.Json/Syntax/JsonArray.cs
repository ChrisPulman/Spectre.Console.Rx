// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json.Syntax;

/// <summary>
/// Represents an array in the JSON abstract syntax tree.
/// </summary>
public sealed class JsonArray : JsonSyntax
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonArray"/> class.
    /// </summary>
    public JsonArray() => Items = new();

    /// <summary>
    /// Gets the array items.
    /// </summary>
    public List<JsonSyntax> Items { get; }

    internal override void Accept<T>(JsonSyntaxVisitor<T> visitor, T context) =>
        visitor.VisitArray(this, context);
}
