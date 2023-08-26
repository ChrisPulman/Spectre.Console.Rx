// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class StringBuffer : IDisposable
{
    private readonly StringReader _reader;
    private readonly int _length;

    public StringBuffer(string text)
    {
        text ??= string.Empty;

        _reader = new StringReader(text);
        _length = text.Length;

        Position = 0;
    }

    public int Position { get; private set; }

    public bool Eof => Position >= _length;

    public void Dispose() => _reader.Dispose();

    public char Expect(char character)
    {
        var read = Read();
        if (read != character)
        {
            throw new InvalidOperationException($"Expected '{character}', but found '{read}'");
        }

        return read;
    }

    public char Peek()
    {
        if (Eof)
        {
            return '\0';
        }

        return (char)_reader.Peek();
    }

    public char Read()
    {
        if (Eof)
        {
            return '\0';
        }

        Position++;
        return (char)_reader.Read();
    }
}
