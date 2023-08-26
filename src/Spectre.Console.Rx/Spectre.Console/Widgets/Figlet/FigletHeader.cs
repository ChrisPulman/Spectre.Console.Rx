// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class FigletHeader
{
    public char Hardblank { get; set; }

    public int Height { get; set; }

    public int Baseline { get; set; }

    public int MaxLength { get; set; }

    public int OldLayout { get; set; }

    public int CommentLines { get; set; }
}