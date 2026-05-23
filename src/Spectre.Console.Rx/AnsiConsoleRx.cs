// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Spectre.Console.Rx;

/// <summary>
/// Reactive entry points for Spectre.Console live experiences.
/// </summary>
public static class AnsiConsoleRx
{
    private static readonly object _lock = new();
    private static readonly List<ProgressTask> _tasks = [];
    private static readonly SemaphoreSlim _lockTillComplete = new(1, 1);
    private static readonly ConditionalWeakTable<StatusContext, ContextCompletion> _statusCompletions = new();
    private static readonly ConditionalWeakTable<LiveDisplayContext, ContextCompletion> _liveCompletions = new();
    private static readonly TimeSpan _completionPollInterval = TimeSpan.FromMilliseconds(10);

    /// <summary>
    /// Gets the scheduler.
    /// </summary>
    /// <value>
    /// The scheduler.
    /// </value>
    public static ISpectreConsoleScheduler Scheduler => SpectreConsoleScheduler.Instance;

    /// <summary>
    /// Creates a new <see cref="Progress" /> observable.
    /// </summary>
    /// <param name="addProperties">The progress configuration callback.</param>
    /// <returns>
    /// A <see cref="ProgressContext" /> observable.
    /// </returns>
    public static IObservable<ProgressContext> Progress(Func<Progress, Progress>? addProperties = null) =>
        CreateExclusiveObservable<ProgressContext>(async (observer, cancellationToken) =>
        {
            await AnsiConsole
                .Progress()
                .AddProgressProperties(addProperties)
                .StartAsync(async ctx =>
                {
                    observer.OnNext(ctx);
                    await WaitUntilAsync(() => ctx.IsFinished, cancellationToken).ConfigureAwait(false);
                })
                .ConfigureAwait(false);
        }, clearProgressTasks: true);

    /// <summary>
    /// Creates a status observable.
    /// </summary>
    /// <param name="status">The initial status message.</param>
    /// <param name="addProperties">The status configuration callback.</param>
    /// <returns>A <see cref="StatusContext" /> observable.</returns>
    public static IObservable<StatusContext> Status(string status, Func<Status, Status>? addProperties = null)
    {
        ArgumentNullException.ThrowIfNull(status);

        return CreateExclusiveObservable<StatusContext>(async (observer, cancellationToken) =>
        {
            await AnsiConsole
                .Status()
                .AddStatusProperties(addProperties)
                .StartAsync(status, async ctx =>
                {
                    var completion = GetStatusCompletion(ctx);
                    observer.OnNext(ctx);
                    await completion.WaitAsync(cancellationToken).ConfigureAwait(false);
                })
                .ConfigureAwait(false);
        });
    }

    /// <summary>
    /// Creates a new <see cref="LiveDisplay" /> observable.
    /// </summary>
    /// <param name="renderable">The renderable.</param>
    /// <param name="addProperties">The live display configuration callback.</param>
    /// <returns>
    /// A <see cref="LiveDisplayContext" /> observable.
    /// </returns>
    public static IObservable<LiveDisplayContext> Live(IRenderable renderable, Func<LiveDisplay, LiveDisplay>? addProperties = null)
    {
        ArgumentNullException.ThrowIfNull(renderable);

        return CreateExclusiveObservable<LiveDisplayContext>(async (observer, cancellationToken) =>
        {
            await AnsiConsole
                .Live(renderable)
                .AddLiveDisplayProperties(addProperties)
                .StartAsync(async ctx =>
                {
                    var completion = GetLiveCompletion(ctx);
                    observer.OnNext(ctx);
                    await completion.WaitAsync(cancellationToken).ConfigureAwait(false);
                })
                .ConfigureAwait(false);
        });
    }

    /// <summary>
    /// Adds progress tasks to the current progress context.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="func">The task factory.</param>
    /// <returns>The context and tasks added.</returns>
    public static IObservable<(ProgressContext context, ProgressTask[] tasks)> AddTasks(this IObservable<ProgressContext> source, Func<ProgressContext, ProgressTask[]> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        return source.Select(ctx =>
        {
            lock (_lock)
            {
                _tasks.AddRange(func(ctx));
            }

            return (ctx, _tasks.ToArray());
        }).SubscribeOn(Scheduler);
    }

    /// <summary>
    /// Updates the specified live display context.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="delay">The delay in milliseconds.</param>
    /// <param name="action">The action.</param>
    /// <returns>A live display context.</returns>
    public static LiveDisplayContext Update(this LiveDisplayContext ctx, int delay, Action action)
    {
        ArgumentNullException.ThrowIfNull(ctx);
        ArgumentNullException.ThrowIfNull(action);

        action();
        ctx.Refresh();
        Thread.Sleep(delay);
        return ctx;
    }

    /// <summary>
    /// Marks the live display observable as finished.
    /// </summary>
    /// <param name="ctx">The live display context.</param>
    public static void IsFinished(this LiveDisplayContext ctx)
    {
        ArgumentNullException.ThrowIfNull(ctx);

        GetLiveCompletion(ctx).Complete();
    }

    /// <summary>
    /// Marks the status observable as finished.
    /// </summary>
    /// <param name="ctx">The status context.</param>
    public static void IsFinished(this StatusContext ctx)
    {
        ArgumentNullException.ThrowIfNull(ctx);

        GetStatusCompletion(ctx).Complete();
    }

    /// <summary>
    /// Marks the live display observable as complete.
    /// </summary>
    /// <param name="ctx">The live display context.</param>
    public static void Complete(this LiveDisplayContext ctx) => IsFinished(ctx);

    /// <summary>
    /// Marks the status observable as complete.
    /// </summary>
    /// <param name="ctx">The status context.</param>
    public static void Complete(this StatusContext ctx) => IsFinished(ctx);

    /// <summary>
    /// Gets a value indicating whether the live display context has been marked complete.
    /// </summary>
    /// <param name="ctx">The live display context.</param>
    /// <returns><c>true</c> when the context has been completed; otherwise, <c>false</c>.</returns>
    public static bool HasFinished(this LiveDisplayContext ctx)
    {
        ArgumentNullException.ThrowIfNull(ctx);

        return GetLiveCompletion(ctx).IsCompleted;
    }

    /// <summary>
    /// Gets a value indicating whether the status context has been marked complete.
    /// </summary>
    /// <param name="ctx">The status context.</param>
    /// <returns><c>true</c> when the context has been completed; otherwise, <c>false</c>.</returns>
    public static bool HasFinished(this StatusContext ctx)
    {
        ArgumentNullException.ThrowIfNull(ctx);

        return GetStatusCompletion(ctx).IsCompleted;
    }

    private static IObservable<TContext> CreateExclusiveObservable<TContext>(
        Func<IObserver<TContext>, CancellationToken, Task> run,
        bool clearProgressTasks = false) =>
        Observable.Using(
            resourceFactory: () => new SemaphoreSlimReleaser(_lockTillComplete),
            observableFactory: _resource => Observable.Create<TContext>(observer =>
            {
                var cancellation = new CancellationDisposable();
                var scheduled = Scheduler.Schedule(Unit.Default, (_, _) =>
                {
                    _ = RunObserverAsync(observer, cancellation.Token, run, clearProgressTasks);
                    return Disposable.Empty;
                });

                return new CompositeDisposable(scheduled, cancellation);
            }))
            .SubscribeOn(Scheduler);

    private static async Task RunObserverAsync<TContext>(
        IObserver<TContext> observer,
        CancellationToken cancellationToken,
        Func<IObserver<TContext>, CancellationToken, Task> run,
        bool clearProgressTasks)
    {
        try
        {
            await run(observer, cancellationToken).ConfigureAwait(false);

            if (!cancellationToken.IsCancellationRequested)
            {
                observer.OnCompleted();
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            observer.OnError(ex);
        }
        finally
        {
            if (clearProgressTasks)
            {
                lock (_lock)
                {
                    _tasks.Clear();
                }
            }
        }
    }

    private static async Task WaitUntilAsync(Func<bool> predicate, CancellationToken cancellationToken)
    {
        while (!predicate())
        {
            await Task.Delay(_completionPollInterval, cancellationToken).ConfigureAwait(false);
        }
    }

    private static ContextCompletion GetStatusCompletion(StatusContext ctx) =>
        _statusCompletions.GetValue(ctx, _ => new ContextCompletion());

    private static ContextCompletion GetLiveCompletion(LiveDisplayContext ctx) =>
        _liveCompletions.GetValue(ctx, _ => new ContextCompletion());

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

    private sealed class ContextCompletion
    {
        private readonly TaskCompletionSource<object?> _completion = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public bool IsCompleted => _completion.Task.IsCompleted;

        public void Complete() => _completion.TrySetResult(null);

        public Task WaitAsync(CancellationToken cancellationToken)
        {
            if (!cancellationToken.CanBeCanceled)
            {
                return _completion.Task;
            }

            return WaitWithCancellationAsync(cancellationToken);
        }

        private async Task WaitWithCancellationAsync(CancellationToken cancellationToken)
        {
            var cancellation = Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            var completed = await Task.WhenAny(_completion.Task, cancellation).ConfigureAwait(false);

            if (completed == cancellation)
            {
                await cancellation.ConfigureAwait(false);
            }

            await _completion.Task.ConfigureAwait(false);
        }
    }

    private sealed class SemaphoreSlimReleaser : IDisposable
    {
        private readonly SemaphoreSlim _semaphore;
        private bool _disposed;

        public SemaphoreSlimReleaser(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
            _semaphore.Wait();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _semaphore.Release();
        }
    }
}
