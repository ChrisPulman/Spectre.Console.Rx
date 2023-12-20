// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json;

/// <summary>
/// Represents a JSON parser.
/// </summary>
public interface IJsonParser
{
    /// <summary>
    /// Parses the provided JSON into an abstract syntax tree.
    /// </summary>
    /// <param name="json">The JSON to parse.</param>
    /// <returns>An <see cref="JsonSyntax"/> instance.</returns>
    JsonSyntax Parse(string json);
}
