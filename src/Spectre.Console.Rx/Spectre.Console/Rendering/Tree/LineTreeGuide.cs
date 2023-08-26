// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// A tree guide made up of lines.
/// </summary>
public sealed class LineTreeGuide : TreeGuide
{
    /// <inheritdoc/>
    public override string GetPart(TreeGuidePart part) => part switch
    {
        TreeGuidePart.Space => "    ",
        TreeGuidePart.Continue => "│   ",
        TreeGuidePart.Fork => "├── ",
        TreeGuidePart.End => "└── ",
        _ => throw new ArgumentOutOfRangeException(nameof(part), part, "Unknown tree part."),
    };
}