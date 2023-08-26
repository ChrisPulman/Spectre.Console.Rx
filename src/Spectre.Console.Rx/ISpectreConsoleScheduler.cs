// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Concurrency;

namespace Spectre.Console.Rx;

/// <summary>
/// ISpectreConsoleScheduler.
/// </summary>
/// <seealso cref="IDisposable" />
public interface ISpectreConsoleScheduler : IScheduler, IStopwatchProvider, IServiceProvider, IDisposable
{
    /// <summary>
    /// Gets a value indicating whether this instance is running.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
    /// </value>
    bool IsRunning { get; }

    /// <summary>
    /// Gets the synchronization context.
    /// </summary>
    /// <value>
    /// The synchronization context.
    /// </value>
    SynchronizationContext SynchronizationContext { get; }

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start();

    /// <summary>
    /// Stops this instance.
    /// </summary>
    void Stop();
}
