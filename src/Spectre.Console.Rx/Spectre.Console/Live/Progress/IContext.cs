// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// IContext.
/// </summary>
public interface IContext : IDisposable
{
    /// <summary>
    /// Gets a value indicating whether this instance is finished.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is finished; otherwise, <c>false</c>.
    /// </value>
    bool IsFinished { get; }

    /// <summary>
    /// Refreshes the current progress.
    /// </summary>
    void Refresh();
}
