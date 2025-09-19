// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ReactiveUI;
using ReactiveUIConsoleExample.ViewModels;
using ReactiveUIConsoleExample.Views;
using Spectre.Console.Rx;

var app = new AppViewModel();
app.NavigateToLogin();

// Very small console host for ReactiveUI Router: render current VM with its view
while (true)
{
    var current = app.Router.GetCurrentViewModel();
    if (current is LoginViewModel lvm)
    {
        AnsiConsole.Clear();
        var view = new LoginView { ViewModel = lvm };
        await view.RenderAsync();
        continue;
    }
    else if (current is MainViewModel mvm)
    {
        AnsiConsole.Clear();
        var view = new MainView { ViewModel = mvm };
        await view.RenderAsync();
        break; // exit after main view exits
    }
    else if (current is null)
    {
        break;
    }
}
