// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Determines ANSI escape sequence support.
/// </summary>
public enum AnsiSupport
{
    /// <summary>
    /// ANSI escape sequence support should
    /// be detected by the system.
    /// </summary>
    Detect = 0,

    /// <summary>
    /// ANSI escape sequences are supported.
    /// </summary>
    Yes = 1,

    /// <summary>
    /// ANSI escape sequences are not supported.
    /// </summary>
    No = 2,
}