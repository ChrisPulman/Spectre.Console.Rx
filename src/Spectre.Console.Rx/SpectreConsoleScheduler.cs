// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace Spectre.Console.Rx;

internal class SpectreConsoleScheduler : IScheduler
{
    // The current time according to this scheduler
    public DateTimeOffset Now => DateTimeOffset.Now;

    // Schedules an action to be executed as soon as possible
    public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
    {
        // Return an disposable
        var result = action(this, state);
        return result;
    }

    // Schedules an action to be executed after a specified relative due time
    public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
    {
        // Use a timer to delay the execution
        var timer = new Timer(_ => action(this, state));

        // Start the timer
        timer.Change(dueTime, TimeSpan.FromMilliseconds(-1));

        // Return a disposable that disposes the timer
        return Disposable.Create(() => timer.Dispose());
    }

    // Schedules an action to be executed at a specified absolute due time
    public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
    {
        // Calculate the relative time span from now
        var dueTimeSpan = dueTime - Now;

        // Delegate to the relative time overload
        return Schedule(state, dueTimeSpan, action);
    }
}
