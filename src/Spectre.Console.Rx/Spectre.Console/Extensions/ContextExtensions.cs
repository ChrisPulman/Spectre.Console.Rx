// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Reactive scheduling helpers for live Spectre.Console contexts.
/// </summary>
public static class ContextExtensions
{
    /// <summary>
    /// Schedules an action until the completion predicate returns <c>true</c>.
    /// </summary>
    /// <param name="context">The progress context.</param>
    /// <param name="delay">The delay between action executions.</param>
    /// <param name="isComplete">The completion predicate.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this ProgressContext context, TimeSpan delay, Func<bool> isComplete, Action action)
    {
        ArgumentNullException.ThrowIfNull(context);

        return ScheduleLoopAsync(context.Refresh, delay, isComplete, action);
    }

    /// <summary>
    /// Schedules an action until the progress context is finished.
    /// </summary>
    /// <param name="context">The progress context.</param>
    /// <param name="delay">The delay between action executions.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this ProgressContext context, TimeSpan delay, Action action)
    {
        ArgumentNullException.ThrowIfNull(context);

        return ScheduleLoopAsync(context.Refresh, delay, () => context.IsFinished, action);
    }

    /// <summary>
    /// Schedules an action until the completion predicate returns <c>true</c>.
    /// </summary>
    /// <param name="context">The status context.</param>
    /// <param name="delay">The delay between action executions.</param>
    /// <param name="isComplete">The completion predicate.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this StatusContext context, TimeSpan delay, Func<bool> isComplete, Action action)
    {
        ArgumentNullException.ThrowIfNull(context);

        return ScheduleLoopAsync(context.Refresh, delay, isComplete, action);
    }

    /// <summary>
    /// Schedules an action until the status context is marked finished.
    /// </summary>
    /// <param name="context">The status context.</param>
    /// <param name="delay">The delay between action executions.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this StatusContext context, TimeSpan delay, Action action)
    {
        ArgumentNullException.ThrowIfNull(context);

        return ScheduleLoopAsync(context.Refresh, delay, context.HasFinished, action);
    }

    /// <summary>
    /// Schedules an action until the completion predicate returns <c>true</c>.
    /// </summary>
    /// <param name="context">The live display context.</param>
    /// <param name="delay">The delay between action executions.</param>
    /// <param name="isComplete">The completion predicate.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this LiveDisplayContext context, TimeSpan delay, Func<bool> isComplete, Action action)
    {
        ArgumentNullException.ThrowIfNull(context);

        return ScheduleLoopAsync(context.Refresh, delay, isComplete, action);
    }

    /// <summary>
    /// Schedules an action until the live display context is marked finished.
    /// </summary>
    /// <param name="context">The live display context.</param>
    /// <param name="delay">The delay between action executions.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this LiveDisplayContext context, TimeSpan delay, Action action)
    {
        ArgumentNullException.ThrowIfNull(context);

        return ScheduleLoopAsync(context.Refresh, delay, context.HasFinished, action);
    }

    /// <summary>
    /// Schedules an action until the completion predicate returns <c>true</c>.
    /// </summary>
    /// <param name="context">The legacy context.</param>
    /// <param name="delay">The delay between action executions.</param>
    /// <param name="isComplete">The completion predicate.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this IContext context, TimeSpan delay, Func<bool> isComplete, Action action)
    {
        ArgumentNullException.ThrowIfNull(context);

        return ScheduleLoopAsync(context.Refresh, delay, isComplete, action);
    }

    /// <summary>
    /// Schedules an action until the legacy context is finished.
    /// </summary>
    /// <param name="context">The legacy context.</param>
    /// <param name="delay">The delay between action executions.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this IContext context, TimeSpan delay, Action action)
    {
        ArgumentNullException.ThrowIfNull(context);

        return ScheduleLoopAsync(context.Refresh, delay, () => context.IsFinished, action);
    }

    /// <summary>
    /// Schedules an action on the Spectre console scheduler.
    /// </summary>
    /// <param name="context">The progress context.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this ProgressContext context, Action<SpectreConsoleScheduler> action) => ScheduleOnce(context, action);

    /// <summary>
    /// Schedules an action on the Spectre console scheduler.
    /// </summary>
    /// <param name="context">The status context.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this StatusContext context, Action<SpectreConsoleScheduler> action) => ScheduleOnce(context, action);

    /// <summary>
    /// Schedules an action on the Spectre console scheduler.
    /// </summary>
    /// <param name="context">The live display context.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this LiveDisplayContext context, Action<SpectreConsoleScheduler> action) => ScheduleOnce(context, action);

    /// <summary>
    /// Schedules an action on the Spectre console scheduler.
    /// </summary>
    /// <param name="context">The legacy context.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this IContext context, Action<SpectreConsoleScheduler> action) => ScheduleOnce(context, action);

    /// <summary>
    /// Schedules an asynchronous action on the Spectre console scheduler.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="context">The progress context.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task{T}" /> representing the asynchronous operation.</returns>
    public static Task<T> Schedule<T>(this ProgressContext context, Func<SpectreConsoleScheduler, Task<T>> action) => ScheduleAsync(context, action);

    /// <summary>
    /// Schedules an asynchronous action on the Spectre console scheduler.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="context">The status context.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task{T}" /> representing the asynchronous operation.</returns>
    public static Task<T> Schedule<T>(this StatusContext context, Func<SpectreConsoleScheduler, Task<T>> action) => ScheduleAsync(context, action);

    /// <summary>
    /// Schedules an asynchronous action on the Spectre console scheduler.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="context">The live display context.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task{T}" /> representing the asynchronous operation.</returns>
    public static Task<T> Schedule<T>(this LiveDisplayContext context, Func<SpectreConsoleScheduler, Task<T>> action) => ScheduleAsync(context, action);

    /// <summary>
    /// Schedules an asynchronous action on the Spectre console scheduler.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="context">The legacy context.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task{T}" /> representing the asynchronous operation.</returns>
    public static Task<T> Schedule<T>(this IContext context, Func<SpectreConsoleScheduler, Task<T>> action) => ScheduleAsync(context, action);

    /// <summary>
    /// Schedules an action on the Spectre console scheduler until the completion predicate returns <c>true</c>.
    /// </summary>
    /// <param name="context">The progress context.</param>
    /// <param name="isComplete">The completion predicate.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this ProgressContext context, Func<bool> isComplete, Action<SpectreConsoleScheduler> action) => ScheduleWhile(context, isComplete, action);

    /// <summary>
    /// Schedules an action on the Spectre console scheduler until the completion predicate returns <c>true</c>.
    /// </summary>
    /// <param name="context">The status context.</param>
    /// <param name="isComplete">The completion predicate.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this StatusContext context, Func<bool> isComplete, Action<SpectreConsoleScheduler> action) => ScheduleWhile(context, isComplete, action);

    /// <summary>
    /// Schedules an action on the Spectre console scheduler until the completion predicate returns <c>true</c>.
    /// </summary>
    /// <param name="context">The live display context.</param>
    /// <param name="isComplete">The completion predicate.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this LiveDisplayContext context, Func<bool> isComplete, Action<SpectreConsoleScheduler> action) => ScheduleWhile(context, isComplete, action);

    /// <summary>
    /// Schedules an action on the Spectre console scheduler until the completion predicate returns <c>true</c>.
    /// </summary>
    /// <param name="context">The legacy context.</param>
    /// <param name="isComplete">The completion predicate.</param>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public static Task Schedule(this IContext context, Func<bool> isComplete, Action<SpectreConsoleScheduler> action) => ScheduleWhile(context, isComplete, action);

    private static SpectreConsoleScheduler CurrentScheduler => (SpectreConsoleScheduler)SpectreConsoleScheduler.Instance;

    private static async Task ScheduleLoopAsync(Action refresh, TimeSpan delay, Func<bool> isComplete, Action action)
    {
        ArgumentNullException.ThrowIfNull(refresh);
        ArgumentNullException.ThrowIfNull(isComplete);
        ArgumentNullException.ThrowIfNull(action);

        while (!isComplete())
        {
            action();
            refresh();
            await CurrentScheduler.Sleep(delay).ConfigureAwait(false);
        }
    }

    private static Task ScheduleOnce<TContext>(TContext context, Action<SpectreConsoleScheduler> action)
        where TContext : class
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(action);

        action(CurrentScheduler);
        return Task.CompletedTask;
    }

    private static Task<T> ScheduleAsync<TContext, T>(TContext context, Func<SpectreConsoleScheduler, Task<T>> action)
        where TContext : class
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(action);

        return action(CurrentScheduler);
    }

    private static async Task ScheduleWhile<TContext>(TContext context, Func<bool> isComplete, Action<SpectreConsoleScheduler> action)
        where TContext : class
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(isComplete);
        ArgumentNullException.ThrowIfNull(action);

        while (!isComplete())
        {
            action(CurrentScheduler);
            await CurrentScheduler.Sleep(TimeSpan.Zero).ConfigureAwait(false);
        }
    }
}
