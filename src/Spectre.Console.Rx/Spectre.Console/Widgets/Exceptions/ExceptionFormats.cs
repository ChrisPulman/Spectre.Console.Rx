// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents how an exception is formatted.
/// </summary>
[Flags]
public enum ExceptionFormats
{
    /// <summary>
    /// The default formatting.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Whether or not paths should be shortened.
    /// </summary>
    ShortenPaths = 1,

    /// <summary>
    /// Whether or not types should be shortened.
    /// </summary>
    ShortenTypes = 2,

    /// <summary>
    /// Whether or not methods should be shortened.
    /// </summary>
    ShortenMethods = 4,

    /// <summary>
    /// Whether or not to show paths as links in the terminal.
    /// </summary>
    ShowLinks = 8,

    /// <summary>
    /// Shortens everything that can be shortened.
    /// </summary>
    ShortenEverything = ShortenMethods | ShortenTypes | ShortenPaths,
}
