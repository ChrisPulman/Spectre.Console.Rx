// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represent a selection item.
/// </summary>
/// <typeparam name="T">The data type.</typeparam>
public interface ISelectionItem<T>
    where T : notnull
{
    /// <summary>
    /// Adds a child to the item.
    /// </summary>
    /// <param name="child">The child to add.</param>
    /// <returns>A new <see cref="ISelectionItem{T}"/> instance representing the child.</returns>
    ISelectionItem<T> AddChild(T child);
}