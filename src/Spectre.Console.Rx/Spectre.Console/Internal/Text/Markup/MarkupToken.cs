// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class MarkupToken(MarkupTokenKind kind, string value, int position)
{
    public MarkupTokenKind Kind { get; } = kind;

    public string Value { get; } = value ?? throw new ArgumentNullException(nameof(value));

    public int Position { get; set; } = position;
}
