// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Spectre.Console.Rx;

/// <summary>
/// AnsiConsoleRx.
/// </summary>
public static class AnsiConsoleRx
{
    private static readonly object _lock = new();
    private static readonly List<ProgressTask> _tasks = new();

    /// <summary>
    /// Gets the scheduler.
    /// </summary>
    /// <value>
    /// The scheduler.
    /// </value>
    public static ISpectreConsoleScheduler Scheduler => SpectreConsoleScheduler.Instance;

    /// <summary>
    /// Creates a new <see cref="Progress" /> instance.
    /// </summary>
    /// <param name="addProperties">The added properties.</param>
    /// <returns>
    /// A <see cref="Progress" /> instance.
    /// </returns>
    public static IObservable<ProgressContext> Progress(Func<Progress, Progress>? addProperties = null) =>
        Observable.Create<ProgressContext>(async observer =>
        {
               await AnsiConsole
                    .Progress()
                    .AddProgressProperties(addProperties)
                    .StartAsync(async ctx =>
                    {
                        SynchronizationContext.SetSynchronizationContext(Scheduler.SynchronizationContext);
                        observer.OnNext(ctx);
                        while (!ctx.IsFinished)
                        {
                            await Task.Yield();
                        }

                        observer.OnCompleted();
                    });

               return Disposable.Create(() =>
                {
                    lock (_lock)
                    {
                        _tasks.Clear();
                    }
                });
           }).SubscribeOn(Scheduler);

    /// <summary>
    /// Statuses the specified status.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="addProperties">The add properties.</param>
    /// <returns>A StatusContext.</returns>
    public static IObservable<StatusContext> Status(string status, Func<Status, Status>? addProperties = null) =>
        Observable.Create<StatusContext>(async observer =>
        {
            var complete = false;
            await AnsiConsole
                .Status()
                .AddStatusProperties(addProperties)
                .StartAsync(status, async ctx =>
                {
                    observer.OnNext(ctx);
                    while (!complete)
                    {
                        await Task.Yield();
                    }

                    observer.OnCompleted();
                });

            return Disposable.Create(() => complete = true);
        }).SubscribeOn(Scheduler);

    /// <summary>
    /// Adds the elements of the given collection to the end of this list. If
    /// required, the capacity of the list is increased to twice the previous
    /// capacity or the new size, whichever is larger.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="func">The function.</param>
    /// <returns>The context and tasks added.</returns>
    public static IObservable<(ProgressContext context, ProgressTask[] tasks)> AddTasks(this IObservable<ProgressContext> source, Func<ProgressContext, ProgressTask[]> func) =>
        source.Select(ctx =>
        {
            lock (_lock)
            {
                _tasks.AddRange(func(ctx));
            }

            return (ctx, _tasks.ToArray());
        }).SubscribeOn(Scheduler);

    /// <summary>
    /// Creates a new <see cref="LiveDisplay" /> instance.
    /// </summary>
    /// <param name="renderable">The renderable.</param>
    /// <param name="addProperties">The add properties.</param>
    /// <returns>
    /// A <see cref="LiveDisplay" /> instance.
    /// </returns>
    public static IObservable<LiveDisplayContext> Live(IRenderable renderable, Func<LiveDisplay, LiveDisplay>? addProperties = null) =>
        Observable.Create<LiveDisplayContext>(async observer =>
        {
            var complete = false;
            await AnsiConsole
                .Live(renderable)
                .AddLiveDisplayProperties(addProperties)
                .StartAsync(async ctx =>
                {
                    observer.OnNext(ctx);
                    while (!complete)
                    {
                        await Task.Yield();
                    }

                    observer.OnCompleted();
                });

            return Disposable.Create(() => complete = true);
        }).SubscribeOn(Scheduler);

    /// <summary>
    /// Updates the specified delay.
    /// </summary>
    /// <param name="ctx">The CTX.</param>
    /// <param name="delay">The delay.</param>
    /// <param name="action">The action.</param>
    /// <returns>A Live Display Context.</returns>
    public static LiveDisplayContext Update(this LiveDisplayContext ctx, int delay, Action action)
    {
        if (ctx is null)
        {
            throw new ArgumentNullException(nameof(ctx));
        }

        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        action();
        ctx.Refresh();
        Thread.Sleep(delay);
        return ctx;
    }

    private static LiveDisplay AddLiveDisplayProperties(this LiveDisplay liveDisplay, Func<LiveDisplay, LiveDisplay>? addProperties)
    {
        if (addProperties is null)
        {
            return liveDisplay;
        }

        return addProperties(liveDisplay);
    }

    private static Progress AddProgressProperties(this Progress progress, Func<Progress, Progress>? addProperties)
    {
        if (addProperties is null)
        {
            return progress;
        }

        return addProperties(progress);
    }

    private static Status AddStatusProperties(this Status status, Func<Status, Status>? addProperties)
    {
        if (addProperties is null)
        {
            return status;
        }

        return addProperties(status);
    }
}
