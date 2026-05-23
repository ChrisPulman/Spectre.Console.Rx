// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ReactiveUI;
using ReactiveUI.Builder;
using ReactiveUIConsoleExample.ViewModels;
using ReactiveUIConsoleExample.Views;
using Spectre.Console.Rx;
using Spectre.Console.Rx.ReactiveUI;

RxAppBuilder
    .CreateReactiveUIBuilder()
    .WithSpectreConsoleRx()
    .Build();

var smokeTest = args.Contains("--smoke-test", StringComparer.OrdinalIgnoreCase);
var interactive = AnsiConsole.Profile.Capabilities.Interactive;

if (smokeTest)
{
    var smokeApp = new AppViewModel();
    smokeApp.NavigateToLogin();

    if (smokeApp.Router.GetCurrentViewModel() is not LoginViewModel)
    {
        throw new InvalidOperationException("ReactiveUI routing did not initialize the login route.");
    }

    var submitApp = new AppViewModel();
    var submitLogin = new LoginViewModel(submitApp);
    await submitLogin.SubmitAsync("demo", "password");

    if (submitApp.Router.GetCurrentViewModel() is not MainViewModel)
    {
        throw new InvalidOperationException("ReactiveUI login did not navigate to the main route.");
    }

    var completedByRoute = false;
    using var smokeTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
    using var renderCancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

    await smokeApp.RunAsync(
        async current =>
        {
            switch (current)
            {
                case LoginViewModel login:
                    await ReactiveConsoleAppViewModel.RenderViewAsync<LoginView, LoginViewModel>(login, renderCancellation.Token);
                    smokeApp.Exit();
                    break;
                case null:
                    completedByRoute = true;
                    break;
            }
        },
        smokeTimeout.Token);

    if (!completedByRoute)
    {
        throw new InvalidOperationException("ReactiveUI routing did not complete after the smoke render.");
    }

    Console.WriteLine("ReactiveUIConsoleExample initialized.");
    return;
}

if (!interactive)
{
    Console.WriteLine("ReactiveUIConsoleExample requires an interactive terminal. Use --smoke-test for non-interactive verification.");
    return;
}

var app = new AppViewModel();
app.NavigateToLogin();

await app.RunAsync(async current =>
{
    switch (current)
    {
        case LoginViewModel lvm:
            await ReactiveConsoleAppViewModel.RenderViewAsync<LoginView, LoginViewModel>(lvm);
            break;
        case MainViewModel mvm:
            await ReactiveConsoleAppViewModel.RenderViewAsync<MainView, MainViewModel>(mvm);
            break;
        default:
            // Exit app when no more screens.
            break;
    }
});
