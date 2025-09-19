// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using ReactiveUI;
using Spectre.Console.Rx;
using Spectre.Console.Rx.Rendering;

namespace ReactiveUIConsoleExample.Views;

/// <summary>
/// Base class for reactive console views backed by Spectre.Console.Rx and ReactiveUI.
/// Provides helpers to render a Live display and await completion.
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
    /// Gets or sets the strongly-typed view model instance.
    /// </summary>
    public TViewModel? ViewModel { get; set; }

    /// <summary>
    /// Render the view. Derived classes implement their own rendering and interaction.
    /// </summary>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>A task that completes when the view is finished.</returns>
    public abstract Task RenderAsync(CancellationToken ct = default);

    /// <summary>
    /// Render a Spectre Live display reactively and await completion.
    /// </summary>
    /// <param name="renderable">The initial renderable to host in the live display.</param>
    /// <param name="execute">Execute action invoked when the live context starts (on UI scheduler).</param>
    /// <param name="completed">Optional completion callback invoked when the live completes.</param>
    /// <param name="configure">Optional LiveDisplay configuration.</param>
    /// <param name="ct">Cancellation token to end the live.</param>
    /// <returns>A task that completes when the live completes.</returns>
    protected Task RenderAsync(
        IRenderable renderable,
        Action<LiveDisplayContext> execute,
        Action? completed = null,
        Func<LiveDisplay, LiveDisplay>? configure = null,
        CancellationToken ct = default)
    {
        var tcs = new TaskCompletionSource<bool>();

        AnsiConsoleRx
            .Live(renderable, configure)
            .ObserveOn(AnsiConsoleRx.Scheduler)
            .Subscribe(
                ctx =>
                {
                    // Run caller logic
                    execute(ctx);

                    // End when requested
                    if (ct.CanBeCanceled)
                    {
                        ct.Register(() => ctx.IsFinished());
                    }
                },
                ex => tcs.TrySetException(ex),
                () =>
                {
                    completed?.Invoke();
                    tcs.TrySetResult(true);
                });

        return tcs.Task;
    }

    /// <summary>
    /// Render a Spectre Live display reactively and await completion (async execute overload).
    /// </summary>
    /// <param name="renderable">The initial renderable to host in the live display.</param>
    /// <param name="executeAsync">Async execute function invoked when the live context starts (on UI scheduler).</param>
    /// <param name="completed">Optional completion callback invoked when the live completes.</param>
    /// <param name="configure">Optional LiveDisplay configuration.</param>
    /// <param name="ct">Cancellation token to end the live.</param>
    /// <returns>A task that completes when the live completes.</returns>
    protected Task RenderAsync(
        IRenderable renderable,
        Func<LiveDisplayContext, Task> executeAsync,
        Action? completed = null,
        Func<LiveDisplay, LiveDisplay>? configure = null,
        CancellationToken ct = default)
    {
        var tcs = new TaskCompletionSource<bool>();

        AnsiConsoleRx
            .Live(renderable, configure)
            .ObserveOn(AnsiConsoleRx.Scheduler)
            .Subscribe(
                async ctx =>
                {
                    // Run caller logic
                    await executeAsync(ctx).ConfigureAwait(false);

                    // End when requested
                    if (ct.CanBeCanceled)
                    {
                        ct.Register(() => ctx.IsFinished());
                    }
                },
                ex => tcs.TrySetException(ex),
                () =>
                {
                    completed?.Invoke();
                    tcs.TrySetResult(true);
                });

        return tcs.Task;
    }
}
