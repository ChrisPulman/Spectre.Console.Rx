// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace Spectre.Console.Rx.ReactiveUI;

/// <summary>
/// Base application view model for Spectre.Console.Rx ReactiveUI applications.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ReactiveConsoleAppViewModel"/> class.
/// </remarks>
/// <param name="router">The routing state to use. When omitted, a new routing state is created.</param>
public class ReactiveConsoleAppViewModel(RoutingState? router = null) : ReactiveObject, IScreen
{
    /// <inheritdoc />
    public RoutingState Router { get; } = router ?? new RoutingState();

    /// <summary>
    /// Renders a route with the specified reactive console view.
    /// </summary>
    /// <typeparam name="TView">The view type.</typeparam>
    /// <typeparam name="TViewModel">The view model type.</typeparam>
    /// <param name="viewModel">The view model to render.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static Task RenderViewAsync<TView, TViewModel>(TViewModel? viewModel, CancellationToken ct = default)
        where TView : ReactiveConsoleView<TViewModel>, new()
        where TViewModel : class, IRoutableViewModel
    {
        if (viewModel is null)
        {
            return Task.CompletedTask;
        }

        var view = new TView
        {
            ViewModel = viewModel,
        };

        return view.RenderAsync(ct);
    }

    /// <summary>
    /// Runs the application route loop until navigation is cleared, the route stream completes, or cancellation is requested.
    /// </summary>
    /// <param name="renderAsync">The route renderer.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task RunAsync(Func<IRoutableViewModel?, Task> renderAsync, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(renderAsync);

        return RunAsync((route, _) => renderAsync(route), ct);
    }

    /// <summary>
    /// Subscribes to route changes until navigation is cleared, the route stream completes, or cancellation is requested.
    /// </summary>
    /// <param name="renderAsync">The route renderer.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task SubscribeAsync(Func<IRoutableViewModel?, Task> renderAsync, CancellationToken ct = default) =>
        RunAsync(renderAsync, ct);

    /// <summary>
    /// Runs the application route loop until navigation is cleared, the route stream completes, or cancellation is requested.
    /// </summary>
    /// <param name="renderAsync">The route renderer.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task RunAsync(Func<IRoutableViewModel?, CancellationToken, Task> renderAsync, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(renderAsync);

        var completion = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);
        using var cancellation = ct.Register(() => completion.TrySetResult(null));
        using var subscription = Router.CurrentViewModel
            .StartWith(Router.GetCurrentViewModel())
            .DistinctUntilChanged(ReferenceRouteComparer.Instance)
            .Select(route => Observable.FromAsync(async () =>
            {
                try
                {
                    AnsiConsole.Clear();
                    await renderAsync(route, ct).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    completion.TrySetResult(null);
                    return;
                }

                if (route is null)
                {
                    completion.TrySetResult(null);
                }
            }))
            .Concat()
            .Subscribe(
                _ => { },
                ex => completion.TrySetException(ex),
                () => completion.TrySetResult(null));

        await completion.Task.ConfigureAwait(false);
    }

    /// <summary>
    /// Subscribes to route changes until navigation is cleared, the route stream completes, or cancellation is requested.
    /// </summary>
    /// <param name="renderAsync">The route renderer.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task SubscribeAsync(Func<IRoutableViewModel?, CancellationToken, Task> renderAsync, CancellationToken ct = default) =>
        RunAsync(renderAsync, ct);

    /// <summary>
    /// Clears the navigation stack and completes the application route loop.
    /// </summary>
    public void Exit() => Router.NavigationStack.Clear();

    private sealed class ReferenceRouteComparer : IEqualityComparer<IRoutableViewModel?>
    {
        public static readonly ReferenceRouteComparer Instance = new();

        public bool Equals(IRoutableViewModel? x, IRoutableViewModel? y) => ReferenceEquals(x, y);

        public int GetHashCode(IRoutableViewModel? obj) => obj is null ? 0 : RuntimeHelpers.GetHashCode(obj);
    }
}
