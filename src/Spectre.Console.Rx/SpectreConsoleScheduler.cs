// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace Spectre.Console.Rx;

/// <summary>
/// SpectreConsoleScheduler.
/// </summary>
/// <seealso cref="LocalScheduler" />
/// <seealso cref="IDisposable" />
public class SpectreConsoleScheduler : LocalScheduler, ISpectreConsoleScheduler
{
    private static readonly SpectreConsoleSynchronizationContext _synchronizationContext = new();
    private bool _disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpectreConsoleScheduler"/> class.
    /// </summary>
    public SpectreConsoleScheduler()
    {
        var thread = new Thread(() =>
        {
            SynchronizationContext.SetSynchronizationContext(_synchronizationContext);
            _synchronizationContext.Post(() => ThreadId = Environment.CurrentManagedThreadId);
            Start();
        });

        thread.Start();
    }

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>
    /// The instance.
    /// </value>
    public static ISpectreConsoleScheduler Instance { get; } = new SpectreConsoleScheduler();

    /// <summary>
    /// Gets a value indicating whether this instance is running.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
    /// </value>
    public bool IsRunning { get; private set; }

    /// <summary>
    /// Gets the thread identifier.
    /// </summary>
    /// <value>
    /// The thread identifier.
    /// </value>
    public int ThreadId { get; private set; }

    /// <summary>
    /// Gets the synchronization context.
    /// </summary>
    /// <value>
    /// The synchronization context.
    /// </value>
    public SynchronizationContext SynchronizationContext => _synchronizationContext;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    public void Start()
    {
        if (IsRunning)
        {
            return;
        }

        IsRunning = true;
        _synchronizationContext.Start();
    }

    /// <summary>
    /// Stops this instance.
    /// </summary>
    public void Stop()
    {
        if (!IsRunning)
        {
            return;
        }

        IsRunning = false;
        _synchronizationContext.Stop();
    }

    /// <summary>
    /// Schedules an action to be executed.
    /// </summary>
    /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
    /// <param name="state">State passed to the action to be executed.</param>
    /// <param name="action">Action to be executed.</param>
    /// <returns>
    /// The disposable object used to cancel the scheduled action (best effort).
    /// </returns>
    /// <exception cref="ArgumentNullException">action.</exception>
    public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var lockTillComplete = new SemaphoreSlim(1);
        var d = new SingleAssignmentDisposable();

        // Ensure we only process a Single Action at a time.
        lockTillComplete.Wait();

        // If we're already on the right thread, just run the action.
        if (Environment.CurrentManagedThreadId == ThreadId)
        {
            if (!d.IsDisposed)
            {
                d.Disposable = action(this, state);
            }

            lockTillComplete.Release();
            return d;
        }

        // If we're not on the right thread, post the action to the thread.
        _synchronizationContext?.PostWithStartComplete(() =>
        {
            if (!d.IsDisposed)
            {
                d.Disposable = action(this, state);
            }

            lockTillComplete.Release();
        });

        // Wait for the thread to finish.
        lockTillComplete.Wait();
        lockTillComplete.Dispose();
        return d;
    }

    /// <summary>
    /// Schedules an action to be executed after dueTime.
    /// </summary>
    /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
    /// <param name="state">State passed to the action to be executed.</param>
    /// <param name="dueTime">Relative time after which to execute the action.</param>
    /// <param name="action">Action to be executed.</param>
    /// <returns>
    /// The disposable object used to cancel the scheduled action (best effort).
    /// </returns>
    /// <exception cref="ArgumentNullException">action.</exception>
    public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var dt = Scheduler.Normalize(dueTime);
        if (dt.Ticks == 0)
        {
            return Schedule(state, action);
        }

        return DefaultScheduler.Instance.Schedule(state, dt, (_, state1) => Schedule(state1, action));
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Stop();
                _synchronizationContext.Dispose();
            }

            _disposedValue = true;
        }
    }
}
