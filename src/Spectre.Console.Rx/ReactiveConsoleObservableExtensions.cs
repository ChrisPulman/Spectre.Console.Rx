// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Spectre.Console.Rx;

/// <summary>
/// Awaitable helpers for reactive console observables.
/// </summary>
public static class ReactiveConsoleObservableExtensions
{
    /// <summary>
    /// Runs a console observable and awaits its completion.
    /// </summary>
    /// <typeparam name="TContext">The context type.</typeparam>
    /// <param name="source">The observable source.</param>
    /// <param name="onNext">The action to execute for each context.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> that completes when the observable completes.</returns>
    public static Task RunAsync<TContext>(
        this IObservable<TContext> source,
        Action<TContext> onNext,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return RunAsync(
            source,
            (context, _) =>
            {
                onNext(context);
                return Task.CompletedTask;
            },
            ct);
    }

    /// <summary>
    /// Runs a console observable and awaits its completion.
    /// </summary>
    /// <typeparam name="TContext">The context type.</typeparam>
    /// <param name="source">The observable source.</param>
    /// <param name="onNext">The asynchronous action to execute for each context.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> that completes when the observable completes.</returns>
    public static Task RunAsync<TContext>(
        this IObservable<TContext> source,
        Func<TContext, Task> onNext,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return RunAsync(
            source,
            (context, _) => onNext(context),
            ct);
    }

    /// <summary>
    /// Runs a console observable and awaits its completion.
    /// </summary>
    /// <typeparam name="TContext">The context type.</typeparam>
    /// <param name="source">The observable source.</param>
    /// <param name="onNext">The asynchronous action to execute for each context.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> that completes when the observable completes.</returns>
    public static async Task RunAsync<TContext>(
        this IObservable<TContext> source,
        Func<TContext, CancellationToken, Task> onNext,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(onNext);

        var completion = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);
        using var cancellation = ct.Register(() => completion.TrySetCanceled(ct));
        using var subscription = new SerialDisposable();

        subscription.Disposable = source
            .Select(context => Observable.FromAsync(async innerCt =>
            {
                using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct, innerCt);
                await onNext(context, linked.Token).ConfigureAwait(false);
            }))
            .Concat()
            .Subscribe(
                _ => { },
                ex => completion.TrySetException(ex),
                () => completion.TrySetResult(null));

        await completion.Task.ConfigureAwait(false);
    }
}
