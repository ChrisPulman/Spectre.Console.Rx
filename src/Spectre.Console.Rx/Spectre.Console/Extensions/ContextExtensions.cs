// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Concurrency;

namespace Spectre.Console.Rx;

/// <summary>
/// ContextExtensions.
/// </summary>
public static class ContextExtensions
{
    /// <summary>
    /// Schedules the specified action.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="delay">The delay.</param>
    /// <param name="isComplete">The is complete.</param>
    /// <param name="action">The action.</param>
    /// <returns>
    /// A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    public static Task Schedule(this IContext context, TimeSpan delay, Func<bool> isComplete, Action action)
    {
        var consoleScheduler = new SpectreConsoleScheduler();
        var lockTillComplete = new SemaphoreSlim(1);
        lockTillComplete.Wait();
        consoleScheduler.Schedule(async () =>
        {
            while (!isComplete())
            {
                action();
                context.Refresh();
                await consoleScheduler.Sleep(delay);
            }

            lockTillComplete.Release();
        });

        lockTillComplete.Wait();
        lockTillComplete.Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Schedules the specified action.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="action">The action.</param>
    /// <returns>
    /// A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    [SuppressMessage("Roslynator", "RCS1175:Unused 'this' parameter.", Justification = "Extension Method")]
    public static Task Schedule(this IContext context, Action<SpectreConsoleScheduler> action)
    {
        var consoleScheduler = new SpectreConsoleScheduler();
        var lockTillComplete = new SemaphoreSlim(1);
        lockTillComplete.Wait();
        consoleScheduler.Schedule(() =>
        {
            action(consoleScheduler);
            lockTillComplete.Release();
        });

        lockTillComplete.Wait();
        lockTillComplete.Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Schedules the specified action.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <param name="context">The context.</param>
    /// <param name="action">The action.</param>
    /// <returns>
    /// A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    [SuppressMessage("Roslynator", "RCS1175:Unused 'this' parameter.", Justification = "Extension Method")]
    public static Task<T> Schedule<T>(this IContext context, Func<SpectreConsoleScheduler, Task<T>> action)
    {
        var consoleScheduler = new SpectreConsoleScheduler();
        var lockTillComplete = new SemaphoreSlim(1);
        lockTillComplete.Wait();
        T result = default!;
        consoleScheduler.Schedule(async () =>
        {
            result = await action(consoleScheduler);
            lockTillComplete.Release();
        });

        lockTillComplete.Wait();
        lockTillComplete.Dispose();
        return Task.FromResult(result);
    }

    /// <summary>
    /// Schedules the specified is complete.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="isComplete">The is complete.</param>
    /// <param name="action">The action.</param>
    /// <returns>
    /// A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    [SuppressMessage("Roslynator", "RCS1175:Unused 'this' parameter.", Justification = "Extension Method")]
    public static Task Schedule(this IContext context, Func<bool> isComplete, Action<SpectreConsoleScheduler> action)
    {
        var consoleScheduler = new SpectreConsoleScheduler();
        var lockTillComplete = new SemaphoreSlim(1);
        lockTillComplete.Wait();
        consoleScheduler.Schedule(() =>
        {
            while (!isComplete())
            {
                action(consoleScheduler);
            }

            lockTillComplete.Release();
        });

        lockTillComplete.Wait();
        lockTillComplete.Dispose();
        return Task.CompletedTask;
    }
}
