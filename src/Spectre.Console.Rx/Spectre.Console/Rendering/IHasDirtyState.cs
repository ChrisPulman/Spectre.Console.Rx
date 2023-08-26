// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Represents something that can be dirty.
/// </summary>
public interface IHasDirtyState
{
    /// <summary>
    /// Gets a value indicating whether the object is dirty.
    /// </summary>
    bool IsDirty { get; }
}