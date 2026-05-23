// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Builder;
using Spectre.Console.Rx.ReactiveUI;

namespace Tests;

/// <summary>
/// Tests for route-changing console input flows.
/// </summary>
public sealed class ReactiveConsoleInputFlowTests
{
    /// <summary>
    /// Verifies that one submit key and one exit key drive the expected routed screens.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WhenLoginAndExitKeysArePressedOnce_ThenEachActionRunsOnce()
    {
        RxAppBuilder
            .CreateReactiveUIBuilder()
            .WithSpectreConsoleRx()
            .Build();

        var original = AnsiConsole.Console;
        using var console = new TestConsole();

        console.Input.PushText("demo");
        console.Input.PushKey(ConsoleKey.Enter);
        console.Input.PushText("password");
        console.Input.PushKey(ConsoleKey.Enter);
        console.Input.PushKey(ConsoleKey.Escape);

        AnsiConsole.Console = console;

        try
        {
            var app = new TestAppViewModel();
            var seenRoutes = new List<string?>();

            await app.Router.Navigate.Execute(new TestLoginViewModel(app)).ToTask();

            await app.RunAsync(async route =>
            {
                seenRoutes.Add(route?.UrlPathSegment);

                switch (route)
                {
                    case TestLoginViewModel login:
                        await ReactiveConsoleAppViewModel.RenderViewAsync<TestLoginView, TestLoginViewModel>(login);
                        break;
                    case TestMainViewModel main:
                        await ReactiveConsoleAppViewModel.RenderViewAsync<TestMainView, TestMainViewModel>(main);
                        break;
                }
            }).WaitAsync(TimeSpan.FromSeconds(5));

            seenRoutes.ShouldBe(["login", "main", null]);
            app.LoginSubmitCount.ShouldBe(1);
            app.MainExitCount.ShouldBe(1);
        }
        finally
        {
            AnsiConsole.Console = original;
        }
    }

    private sealed class TestAppViewModel : ReactiveConsoleAppViewModel
    {
        public int LoginSubmitCount { get; private set; }

        public int MainExitCount { get; private set; }

        public async Task SubmitLoginAsync(string userName)
        {
            LoginSubmitCount++;
            await Router.Navigate.Execute(new TestMainViewModel(this, userName)).ToTask();
        }

        public void ExitMain()
        {
            MainExitCount++;
            Exit();
        }
    }

    private sealed class TestLoginViewModel(TestAppViewModel host) : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "login";

        public IScreen HostScreen { get; } = host;

        public TestAppViewModel App { get; } = host;
    }

    private sealed class TestMainViewModel(TestAppViewModel host, string userName) : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "main";

        public IScreen HostScreen { get; } = host;

        public TestAppViewModel App { get; } = host;

        public string UserName { get; } = userName;
    }

    private sealed class TestLoginView : ReactiveConsoleView<TestLoginViewModel>
    {
        public override async Task RenderAsync(CancellationToken ct = default)
        {
            if (ViewModel is null)
            {
                return;
            }

            using var screenCancellation = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var userName = string.Empty;
            var password = string.Empty;
            var focusIndex = 0;
            var result = false;
            var done = false;

            await RenderAsync(
                new Text("login"),
                async ctx =>
                {
                    var completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                    void Complete(bool value)
                    {
                        if (done)
                        {
                            return;
                        }

                        done = true;
                        result = value;
                        completion.TrySetResult(value);
                        screenCancellation.Cancel();
                    }

                    using var cancellation = screenCancellation.Token.Register(() => completion.TrySetResult(false));
                    using var keys = ReadKeys(screenCancellation.Token).Subscribe(key =>
                    {
                        if (done)
                        {
                            return;
                        }

                        if (key.Key == ConsoleKey.Enter)
                        {
                            if (focusIndex == 0)
                            {
                                focusIndex = 1;
                            }
                            else if (userName.Length > 0 && password.Length > 0)
                            {
                                Complete(true);
                            }

                            return;
                        }

                        if (!char.IsControl(key.KeyChar))
                        {
                            if (focusIndex == 0)
                            {
                                userName += key.KeyChar;
                            }
                            else
                            {
                                password += key.KeyChar;
                            }
                        }
                    });

                    await completion.Task;
                    ctx.IsFinished();
                },
                ct: screenCancellation.Token);

            if (result)
            {
                await ViewModel.App.SubmitLoginAsync(userName);
            }
        }
    }

    private sealed class TestMainView : ReactiveConsoleView<TestMainViewModel>
    {
        public override async Task RenderAsync(CancellationToken ct = default)
        {
            if (ViewModel is null)
            {
                return;
            }

            using var screenCancellation = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var exit = false;

            await RenderAsync(
                new Text($"main {ViewModel.UserName}"),
                async ctx =>
                {
                    var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
                    void Complete()
                    {
                        exit = true;
                        completion.TrySetResult();
                        screenCancellation.Cancel();
                    }

                    using var cancellation = screenCancellation.Token.Register(() => completion.TrySetResult());
                    using var keys = ReadKeys(screenCancellation.Token).Subscribe(key =>
                    {
                        if (key.Key == ConsoleKey.Escape)
                        {
                            Complete();
                        }
                    });

                    await completion.Task;
                    ctx.IsFinished();
                },
                ct: screenCancellation.Token);

            if (exit)
            {
                ViewModel.App.ExitMain();
            }
        }
    }
}
