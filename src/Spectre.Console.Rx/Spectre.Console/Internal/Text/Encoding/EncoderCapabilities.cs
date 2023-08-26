// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Internal;

internal sealed class EncoderCapabilities(ColorSystem colors) : IReadOnlyCapabilities
{
    public ColorSystem ColorSystem { get; } = colors;

    public bool Ansi => false;

    public bool Links => false;

    public bool Legacy => false;

    public bool IsTerminal => false;

    public bool Interactive => false;

    public bool Unicode => true;
}
