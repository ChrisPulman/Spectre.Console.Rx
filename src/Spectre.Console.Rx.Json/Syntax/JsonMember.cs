// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json.Syntax;

/// <summary>
/// Represents a member in the JSON abstract syntax tree.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="JsonMember"/> class.
/// </remarks>
/// <param name="name">The name.</param>
/// <param name="value">The value.</param>
public sealed class JsonMember(string name, JsonSyntax value) : JsonSyntax
{
    /// <summary>
    /// Gets the member name.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the member value.
    /// </summary>
    public JsonSyntax Value { get; } = value;

    internal override void Accept<T>(JsonSyntaxVisitor<T> visitor, T context) =>
        visitor.VisitMember(this, context);
}
