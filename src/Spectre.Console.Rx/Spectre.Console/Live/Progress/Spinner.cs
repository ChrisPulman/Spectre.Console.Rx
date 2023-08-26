// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a spinner used in a <see cref="SpinnerColumn"/>.
/// </summary>
public abstract partial class Spinner
{
    /// <summary>
    /// Gets the update interval for the spinner.
    /// </summary>
    public abstract TimeSpan Interval { get; }

    /// <summary>
    /// Gets a value indicating whether or not the spinner
    /// uses Unicode characters.
    /// </summary>
    public abstract bool IsUnicode { get; }

    /// <summary>
    /// Gets the spinner frames.
    /// </summary>
    public abstract IReadOnlyList<string> Frames { get; }
}