// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Indicates that the tree being rendered includes a cycle, and cannot be rendered.
/// </summary>
public sealed class CircularTreeException : Exception
{
    internal CircularTreeException(string message)
        : base(message)
    {
    }

    internal CircularTreeException()
    {
    }

    internal CircularTreeException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
