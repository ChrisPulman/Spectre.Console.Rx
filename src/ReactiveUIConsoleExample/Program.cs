// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ReactiveUI;
using ReactiveUIConsoleExample.ViewModels;
using ReactiveUIConsoleExample.Views;
using Spectre.Console.Rx;

var app = new AppViewModel();
app.NavigateToLogin();

await app.SubscribeAsync(async current =>
{
    switch (current)
    {
        case LoginViewModel lvm:
            await app.RenderViewAsync<LoginView, LoginViewModel>(lvm);
            break;
        case MainViewModel mvm:
            await app.RenderViewAsync<MainView, MainViewModel>(mvm);
            break;
        default:
            // Exit app when no more screens
            Environment.Exit(0);
            break;
    }
});
