// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represent a multi selection prompt item.
/// </summary>
/// <typeparam name="T">The data type.</typeparam>
public interface IMultiSelectionItem<T> : ISelectionItem<T>
    where T : notnull
{
    /// <summary>
    /// Gets a value indicating whether or not this item is selected.
    /// </summary>
    bool IsSelected { get; }

    /// <summary>
    /// Selects the item.
    /// </summary>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    IMultiSelectionItem<T> Select();
}