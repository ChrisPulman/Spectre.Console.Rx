// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json;

internal sealed class JsonTextStyles
{
    public Style BracesStyle { get; set; } = null!;

    public Style BracketsStyle { get; set; } = null!;

    public Style MemberStyle { get; set; } = null!;

    public Style ColonStyle { get; set; } = null!;

    public Style CommaStyle { get; set; } = null!;

    public Style StringStyle { get; set; } = null!;

    public Style NumberStyle { get; set; } = null!;

    public Style BooleanStyle { get; set; } = null!;

    public Style NullStyle { get; set; } = null!;
}
