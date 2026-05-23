// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ReactiveUI;
using Spectre.Console.Rx.ReactiveUI;

namespace Tests;

/// <summary>
/// Tests for the ReactiveUI console application host.
/// </summary>
public sealed class ReactiveConsoleAppViewModelTests
{
    /// <summary>
    /// Ensures that clearing navigation completes the reactive route loop.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WhenNavigationIsCleared_ThenRunAsyncCompletes()
    {
        var app = new ReactiveConsoleAppViewModel();
        var route = new TestRoute(app);
        var seenRoutes = new List<string?>();

        app.Router.Navigate.Execute(route).Subscribe();

        await app.RunAsync(current =>
        {
            seenRoutes.Add(current?.UrlPathSegment);

            if (current is TestRoute)
            {
                app.Exit();
            }

            return Task.CompletedTask;
        }).WaitAsync(TimeSpan.FromSeconds(5));

        seenRoutes.ShouldBe(["test", null]);
    }

    private sealed class TestRoute(IScreen hostScreen) : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "test";

        public IScreen HostScreen { get; } = hostScreen;
    }
}
