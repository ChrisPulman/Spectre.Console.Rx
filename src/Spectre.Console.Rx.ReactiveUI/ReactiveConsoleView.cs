// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using System.Reactive.Disposables;
using ReactiveUI;
using Spectre.Console.Rx.Rendering;

namespace Spectre.Console.Rx.ReactiveUI;

/// <summary>
/// Base class for ReactiveUI console views backed by Spectre.Console.Rx live rendering.
/// </summary>
/// <typeparam name="TViewModel">The view model type for the view.</typeparam>
public abstract class ReactiveConsoleView<TViewModel> : IViewFor<TViewModel>
    where TViewModel : class
{
    /// <inheritdoc />
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TViewModel?)value;
    }

    /// <summary>
    /// Gets or sets the strongly typed view model instance.
    /// </summary>
    public TViewModel? ViewModel { get; set; }

    /// <summary>
    /// Renders the view.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public abstract Task RenderAsync(CancellationToken ct = default);

    /// <summary>
    /// Creates a reactive stream of console key presses.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <param name="intercept">Whether to intercept the key press.</param>
    /// <returns>An observable sequence of key presses.</returns>
    protected static IObservable<ConsoleKeyInfo> ReadKeys(CancellationToken ct, bool intercept = true) =>
        Observable.Create<ConsoleKeyInfo>(observer =>
        {
            var gate = new object();
            var source = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var sourceDisposed = false;

            void CancelSource()
            {
                lock (gate)
                {
                    if (sourceDisposed)
                    {
                        return;
                    }

                    source.Cancel();
                }
            }

            void DisposeSource()
            {
                lock (gate)
                {
                    if (sourceDisposed)
                    {
                        return;
                    }

                    sourceDisposed = true;
                    source.Dispose();
                }
            }

            _ = Task.Run(async () =>
            {
                var notifyCompleted = true;

                try
                {
                    while (!source.IsCancellationRequested)
                    {
                        var key = await AnsiConsole.Console.Input.ReadKeyAsync(intercept, source.Token).ConfigureAwait(false);
                        if (key.HasValue)
                        {
                            observer.OnNext(key.Value);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Cancellation is a normal way to close a console screen.
                }
                catch (ObjectDisposedException)
                {
                    // Disposal can race with the polling read loop during screen teardown.
                }
                catch (Exception ex)
                {
                    notifyCompleted = false;
                    observer.OnError(ex);
                    return;
                }
                finally
                {
                    if (notifyCompleted)
                    {
                        observer.OnCompleted();
                    }

                    DisposeSource();
                }
            });

            return Disposable.Create(CancelSource);
        });

    /// <summary>
    /// Renders a Spectre live display reactively and awaits completion.
    /// </summary>
    /// <param name="renderable">The initial renderable to host in the live display.</param>
    /// <param name="execute">The action invoked when the live context starts.</param>
    /// <param name="completed">The optional completion callback.</param>
    /// <param name="configure">The optional live display configuration.</param>
    /// <param name="ct">The cancellation token used to finish the live display.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected Task RenderAsync(
        IRenderable renderable,
        Action<LiveDisplayContext> execute,
        Action? completed = null,
        Func<LiveDisplay, LiveDisplay>? configure = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(execute);

        return RenderLiveAsync(renderable, execute, completed, configure, ct);
    }

    /// <summary>
    /// Renders a Spectre live display reactively and awaits completion.
    /// </summary>
    /// <param name="renderable">The initial renderable to host in the live display.</param>
    /// <param name="executeAsync">The asynchronous action invoked when the live context starts.</param>
    /// <param name="completed">The optional completion callback.</param>
    /// <param name="configure">The optional live display configuration.</param>
    /// <param name="ct">The cancellation token used to finish the live display.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected Task RenderAsync(
        IRenderable renderable,
        Func<LiveDisplayContext, Task> executeAsync,
        Action? completed = null,
        Func<LiveDisplay, LiveDisplay>? configure = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(executeAsync);

        return RenderLiveAsync(renderable, executeAsync, completed, configure, ct);
    }

    private static async Task RenderLiveAsync(
        IRenderable renderable,
        Action<LiveDisplayContext> execute,
        Action? completed,
        Func<LiveDisplay, LiveDisplay>? configure,
        CancellationToken ct)
    {
        CancellationTokenRegistration cancellation = default;

        try
        {
            await AnsiConsoleRx
                .Live(renderable, configure)
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .RunAsync(
                    ctx =>
                    {
                        if (ct.CanBeCanceled)
                        {
                            cancellation = ct.Register(() => ctx.IsFinished());
                        }

                        execute(ctx);
                    },
                    CancellationToken.None)
                .ConfigureAwait(false);

            completed?.Invoke();
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            completed?.Invoke();
        }
        finally
        {
            cancellation.Dispose();
        }
    }

    private static async Task RenderLiveAsync(
        IRenderable renderable,
        Func<LiveDisplayContext, Task> executeAsync,
        Action? completed,
        Func<LiveDisplay, LiveDisplay>? configure,
        CancellationToken ct)
    {
        CancellationTokenRegistration cancellation = default;

        try
        {
            await AnsiConsoleRx
                .Live(renderable, configure)
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .RunAsync(
                    async ctx =>
                    {
                        if (ct.CanBeCanceled)
                        {
                            cancellation = ct.Register(() => ctx.IsFinished());
                        }

                        await executeAsync(ctx).ConfigureAwait(false);
                    },
                    CancellationToken.None)
                .ConfigureAwait(false);

            completed?.Invoke();
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            completed?.Invoke();
        }
        finally
        {
            cancellation.Dispose();
        }
    }
}
