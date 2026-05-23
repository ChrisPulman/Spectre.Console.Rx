// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Spectre.Console.Rx.ReactiveUI;

namespace ReactiveUIConsoleExample.ViewModels;

/// <summary>
/// Application root implementing ReactiveUI routing.
/// </summary>
public sealed class AppViewModel : ReactiveConsoleAppViewModel
{
    /// <summary>
    /// Navigate to the initial view (Login).
    /// </summary>
    public void NavigateToLogin() => Router.Navigate.Execute(new LoginViewModel(this)).Subscribe();
}
