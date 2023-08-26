// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents something that has tree nodes.
/// </summary>
public interface IHasTreeNodes
{
    /// <summary>
    /// Gets the tree's child nodes.
    /// </summary>
    List<TreeNode> Nodes { get; }
}