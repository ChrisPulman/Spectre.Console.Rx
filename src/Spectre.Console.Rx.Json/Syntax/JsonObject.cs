// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json.Syntax;

/// <summary>
/// Represents an object in the JSON abstract syntax tree.
/// </summary>
public sealed class JsonObject : JsonSyntax
{

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonObject"/> class.
    /// </summary>
    public JsonObject() => Members = new();

    /// <summary>
    /// Gets the object's members.
    /// </summary>
    public List<JsonMember> Members { get; }

    internal override void Accept<T>(JsonSyntaxVisitor<T> visitor, T context) =>
        visitor.VisitObject(this, context);
}
