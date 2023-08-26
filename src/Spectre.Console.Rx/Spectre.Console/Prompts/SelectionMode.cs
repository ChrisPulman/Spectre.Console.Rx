// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents how selections are made in a hierarchical prompt.
/// </summary>
public enum SelectionMode
{
    /// <summary>
    /// Will only return lead nodes in results.
    /// </summary>
    Leaf = 0,

    /// <summary>
    /// Allows selection of parent nodes, but each node
    /// is independent of its parent and children.
    /// </summary>
    Independent = 1,
}
