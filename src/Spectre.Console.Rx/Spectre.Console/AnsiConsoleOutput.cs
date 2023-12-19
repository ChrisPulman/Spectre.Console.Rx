// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents console output.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AnsiConsoleOutput"/> class.
/// </remarks>
/// <param name="writer">The output writer.</param>
public sealed class AnsiConsoleOutput(TextWriter writer) : IAnsiConsoleOutput
{

    /// <inheritdoc/>
    public TextWriter Writer { get; } = writer ?? throw new ArgumentNullException(nameof(writer));

    /// <inheritdoc/>
    public bool IsTerminal
    {
        get
        {
            if (Writer.IsStandardOut())
            {
                return !System.Console.IsOutputRedirected;
            }

            if (Writer.IsStandardError())
            {
                return !System.Console.IsErrorRedirected;
            }

            return false;
        }
    }

    /// <inheritdoc/>
    public int Width => ConsoleHelper.GetSafeWidth();

    /// <inheritdoc/>
    public int Height => ConsoleHelper.GetSafeHeight();

    /// <inheritdoc/>
    public void SetEncoding(Encoding encoding)
    {
        if (Writer.IsStandardOut() || Writer.IsStandardError())
        {
            System.Console.OutputEncoding = encoding;
        }
    }
}
