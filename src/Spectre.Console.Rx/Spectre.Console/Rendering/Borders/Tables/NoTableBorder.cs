// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Represents an invisible border.
/// </summary>
public sealed class NoTableBorder : TableBorder
{
    /// <inheritdoc/>
    public override bool Visible => false;

    /// <inheritdoc />
    public override bool SupportsRowSeparator => false;

    /// <inheritdoc/>
    public override string GetPart(TableBorderPart part) => " ";
}
