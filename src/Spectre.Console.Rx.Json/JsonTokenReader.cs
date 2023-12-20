// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json;

internal sealed class JsonTokenReader(List<JsonToken> tokens)
{
    private readonly int _length = tokens.Count;

    public int Position { get; private set; }

    public bool Eof => Position >= _length;

    public JsonToken Consume(JsonTokenType type)
    {
        var read = Read() ?? throw new InvalidOperationException("Could not read token");
        if (read.Type != type)
        {
            throw new InvalidOperationException($"Expected '{type}' token, but found '{read.Type}'");
        }

        return read;
    }

    public JsonToken? Peek()
    {
        if (Eof)
        {
            return null;
        }

        return tokens[Position];
    }

    public JsonToken? Read()
    {
        if (Eof)
        {
            return null;
        }

        Position++;
        return tokens[Position - 1];
    }
}
