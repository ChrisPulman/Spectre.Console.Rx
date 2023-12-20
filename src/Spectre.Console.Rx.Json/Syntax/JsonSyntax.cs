// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json.Syntax;

/// <summary>
/// Represents a syntax node in the JSON abstract syntax tree.
/// </summary>
public abstract class JsonSyntax
{
    internal abstract void Accept<T>(JsonSyntaxVisitor<T> visitor, T context);
}
