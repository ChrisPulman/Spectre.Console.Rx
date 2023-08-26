// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents vertical overflow.
/// </summary>
public enum VerticalOverflow
{
    /// <summary>
    /// Crop overflow.
    /// </summary>
    Crop = 0,

    /// <summary>
    /// Add an ellipsis at the end.
    /// </summary>
    Ellipsis = 1,

    /// <summary>
    /// Do not do anything about overflow.
    /// </summary>
    Visible = 2,
}