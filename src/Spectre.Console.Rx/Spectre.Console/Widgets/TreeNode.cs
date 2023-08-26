// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a tree node.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TreeNode"/> class.
/// </remarks>
public sealed class TreeNode(IRenderable renderable) : IHasTreeNodes
{
    /// <summary>
    /// Gets the tree node's child nodes.
    /// </summary>
    public List<TreeNode> Nodes { get; } = new List<TreeNode>();

    /// <summary>
    /// Gets or sets a value indicating whether or not the tree node is expanded or not.
    /// </summary>
    public bool Expanded { get; set; } = true;

    internal IRenderable Renderable { get; } = renderable;
}
