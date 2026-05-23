// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
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
    submitLogin.UserName = "demo";
    submitLogin.Password = "password";

    using var submitCompleted = new ManualResetEventSlim(false);
    Exception? submitError = null;
    using var submitSubscription = submitLogin.Login.Execute().Subscribe(
        _ => { },
        ex =>
        {
            submitError = ex;
            submitCompleted.Set();
        },
        submitCompleted.Set);

    if (!submitCompleted.Wait(TimeSpan.FromSeconds(5)))
    {
        throw new TimeoutException("ReactiveUI login command did not complete.");
    }

    if (submitError is not null)
    {
        throw new InvalidOperationException("ReactiveUI login command failed.", submitError);
    }

    if (submitApp.Router.GetCurrentViewModel() is not MainViewModel)
    {
        throw new InvalidOperationException("ReactiveUI login did not navigate to the main route.");
    }

    var completedByRoute = false;
    using var renderCancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
    using var routeCompleted = new ManualResetEventSlim(false);
    Exception? routeError = null;

    using var routeSubscription = SubscribeRoutes(
        smokeApp,
        current =>
        {
            switch (current)
            {
                case LoginViewModel login:
                    return Observable
                        .FromAsync(() => ReactiveConsoleAppViewModel.RenderViewAsync<LoginView, LoginViewModel>(login, renderCancellation.Token))
                        .Do(_ => smokeApp.Exit())
                        .Select(_ => Unit.Default);
                case null:
                    completedByRoute = true;
                    routeCompleted.Set();
                    return Observable.Return(Unit.Default);
                default:
                    return Observable.Return(Unit.Default);
            }
        },
        ex =>
        {
            routeError = ex;
            routeCompleted.Set();
        });

    if (!routeCompleted.Wait(TimeSpan.FromSeconds(5)))
    {
        throw new TimeoutException("ReactiveUI routing did not complete after the smoke render.");
    }

    if (routeError is not null)
    {
        throw new InvalidOperationException("ReactiveUI routing failed during the smoke render.", routeError);
    }

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
using var completed = new ManualResetEventSlim(false);
Exception? error = null;

using var appSubscription = SubscribeRoutes(
    app,
    current =>
        current switch
        {
            LoginViewModel login => Observable
                .FromAsync(() => ReactiveConsoleAppViewModel.RenderViewAsync<LoginView, LoginViewModel>(login))
                .Select(_ => Unit.Default),
            MainViewModel main => Observable
                .FromAsync(() => ReactiveConsoleAppViewModel.RenderViewAsync<MainView, MainViewModel>(main))
                .Select(_ => Unit.Default),
            _ => Observable.Defer(() =>
            {
                completed.Set();
                return Observable.Return(Unit.Default);
            }),
        },
    ex =>
    {
        error = ex;
        completed.Set();
    });

completed.Wait();

if (error is not null)
{
    throw new InvalidOperationException("ReactiveUI console application failed.", error);
}

static IDisposable SubscribeRoutes(
    AppViewModel app,
    Func<IRoutableViewModel?, IObservable<Unit>> renderRoute,
    Action<Exception> onError)
{
    return app.Router.CurrentViewModel
        .StartWith(app.Router.GetCurrentViewModel())
        .DistinctUntilChanged()
        .Select(route => Observable.Defer(() =>
        {
            AnsiConsole.Clear();
            return renderRoute(route);
        }))
        .Concat()
        .Subscribe(_ => { }, onError);
}
