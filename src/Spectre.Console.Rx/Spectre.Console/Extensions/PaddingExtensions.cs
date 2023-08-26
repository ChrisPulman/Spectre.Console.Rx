// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Contains extension methods for <see cref="Padding"/>.
/// </summary>
public static class PaddingExtensions
{
    /// <summary>
    /// Gets the left padding.
    /// </summary>
    /// <param name="padding">The padding.</param>
    /// <returns>The left padding or zero if <c>padding</c> is null.</returns>
    public static int GetLeftSafe(this Padding? padding) => padding?.Left ?? 0;

    /// <summary>
    /// Gets the right padding.
    /// </summary>
    /// <param name="padding">The padding.</param>
    /// <returns>The right padding or zero if <c>padding</c> is null.</returns>
    public static int GetRightSafe(this Padding? padding) => padding?.Right ?? 0;

    /// <summary>
    /// Gets the top padding.
    /// </summary>
    /// <param name="padding">The padding.</param>
    /// <returns>The top padding or zero if <c>padding</c> is null.</returns>
    public static int GetTopSafe(this Padding? padding) => padding?.Top ?? 0;

    /// <summary>
    /// Gets the bottom padding.
    /// </summary>
    /// <param name="padding">The padding.</param>
    /// <returns>The bottom padding or zero if <c>padding</c> is null.</returns>
    public static int GetBottomSafe(this Padding? padding) => padding?.Bottom ?? 0;
}