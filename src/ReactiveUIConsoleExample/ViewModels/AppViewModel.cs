// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive;
using ReactiveUI;

namespace ReactiveUIConsoleExample.ViewModels;

/// <summary>
/// Application root implementing ReactiveUI routing.
/// </summary>
public sealed class AppViewModel : ReactiveObject, IScreen
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppViewModel"/> class.
    /// </summary>
    public AppViewModel()
    {
        Router = new RoutingState();
    }

    /// <summary>
    /// Gets the Router associated with this Screen.
    /// </summary>
    public RoutingState Router { get; }

    /// <summary>
    /// Navigate to the initial view (Login).
    /// </summary>
    public void NavigateToLogin() => Router.Navigate.Execute(new LoginViewModel(this)).Subscribe();
}
