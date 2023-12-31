// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents tree guide lines.
/// </summary>
public abstract partial class TreeGuide
{
    /// <summary>
    /// Gets the safe guide lines or <c>null</c> if none exist.
    /// </summary>
    public virtual TreeGuide? SafeTreeGuide { get; }

    /// <summary>
    /// Get the set of characters used to render the corresponding <see cref="TreeGuidePart"/>.
    /// </summary>
    /// <param name="part">The part of the tree to get rendering string for.</param>
    /// <returns>Rendering string for the tree part.</returns>
    public abstract string GetPart(TreeGuidePart part);
}