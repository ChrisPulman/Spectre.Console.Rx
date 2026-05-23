// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Disposables;

namespace Tests;

/// <summary>
/// Tests for the Spectre console scheduler.
/// </summary>
public sealed class SpectreConsoleSchedulerTests
{
    /// <summary>
    /// Verifies that scheduled work runs on the scheduler thread.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WhenWorkIsScheduled_ThenItRunsOnSchedulerThread()
    {
        using var scheduler = new SpectreConsoleScheduler();
        var completed = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);

        using var subscription = scheduler.Schedule(Unit.Default, (_, _) =>
        {
            completed.SetResult(Environment.CurrentManagedThreadId);
            return Disposable.Empty;
        });

        var scheduledThread = await completed.Task.WaitAsync(TimeSpan.FromSeconds(2));

        scheduledThread.ShouldBe(scheduler.ThreadId);
    }

    /// <summary>
    /// Verifies that scheduling from another thread does not wait for the scheduled action to finish.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WhenScheduledActionBlocks_ThenScheduleReturnsImmediately()
    {
        using var scheduler = new SpectreConsoleScheduler();
        using var started = new ManualResetEventSlim(false);
        using var release = new ManualResetEventSlim(false);
        IDisposable? scheduled = null;

        var scheduleTask = Task.Run(() =>
            scheduler.Schedule(Unit.Default, (_, _) =>
            {
                started.Set();
                release.Wait(TimeSpan.FromSeconds(5)).ShouldBeTrue();
                return Disposable.Empty;
            }));

        try
        {
            started.Wait(TimeSpan.FromSeconds(2)).ShouldBeTrue();
            scheduled = await scheduleTask.WaitAsync(TimeSpan.FromMilliseconds(250));
        }
        finally
        {
            release.Set();
        }

        scheduled.Dispose();
    }
}
