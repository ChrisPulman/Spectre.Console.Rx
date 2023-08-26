// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Represents an invisible border.
/// </summary>
public sealed class NoBoxBorder : BoxBorder
{
    /// <inheritdoc/>
    public override string GetPart(BoxBorderPart part) => " ";
}
