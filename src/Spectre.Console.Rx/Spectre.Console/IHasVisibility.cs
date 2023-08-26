// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents something that can be hidden.
/// </summary>
public interface IHasVisibility
{
    /// <summary>
    /// Gets or sets a value indicating whether or not the object should
    /// be visible or not.
    /// </summary>
    bool IsVisible { get; set; }
}