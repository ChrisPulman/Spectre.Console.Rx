// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive;
using ReactiveUI;
using ReactiveUIConsoleExample.Views;
using Spectre.Console.Rx;

namespace ReactiveUIConsoleExample.ViewModels;

/// <summary>
/// Application root implementing ReactiveUI routing.
/// </summary>
public sealed class AppViewModel : ReactiveObject, IScreen
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppViewModel"/> class.
    /// </summary>
    public AppViewModel() => Router = new RoutingState();

    /// <summary>
    /// Gets the Router associated with this Screen.
    /// </summary>
    public RoutingState Router { get; }

    /// <summary>
    /// Renders the view asynchronous.
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <param name="viewModel">The view model.</param>
    /// <param name="ct">The ct.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static Task RenderViewAsync<TView, TViewModel>(TViewModel viewModel, CancellationToken ct = default)
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
    /// Subscribes the asynchronous.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SubscribeAsync(Action<IRoutableViewModel> action)
    {
        Router.CurrentViewModel.Subscribe(current =>
        {
            AnsiConsole.Clear();
            action(current);
        });
        await Task.Delay(-1);
    }

    /// <summary>
    /// Navigate to the initial view (Login).
    /// </summary>
    public void NavigateToLogin() => Router.Navigate.Execute(new LoginViewModel(this)).Subscribe();
}
