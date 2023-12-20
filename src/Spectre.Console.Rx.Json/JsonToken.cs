// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json;

internal sealed class JsonToken(JsonTokenType type, string lexeme)
{
    public JsonTokenType Type { get; } = type;

    public string Lexeme { get; } = lexeme ?? throw new ArgumentNullException(nameof(lexeme));
}
