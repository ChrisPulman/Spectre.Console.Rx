// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json;

internal sealed class JsonBuilderContext(JsonTextStyles styling)
{
    public Paragraph Paragraph { get; } = new Paragraph();

    public int Indentation { get; set; }

    public JsonTextStyles Styling { get; } = styling;

    public void InsertIndentation() => Paragraph.Append(new string(' ', Indentation * 3));
}
