// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tests;

/// <summary>
/// Tests for the reactive console entry points.
/// </summary>
public class AnsiConsoleRxTests
{
    /// <summary>
    /// Verifies that a progress observable can be completed explicitly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WhenProgressContextIsCompleted_ThenObservableCompletes()
    {
        var original = AnsiConsole.Console;
        AnsiConsole.Console = new TestConsole();

        try
        {
            var seenContext = false;
            var completed = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            using var subscription = AnsiConsoleRx.Progress()
                .Subscribe(
                    ctx =>
                    {
                        seenContext = true;
                        ctx.Complete();
                    },
                    completed.SetException,
                    completed.SetResult);

            await completed.Task.WaitAsync(TimeSpan.FromSeconds(5));

            seenContext.ShouldBeTrue();
        }
        finally
        {
            AnsiConsole.Console = original;
        }
    }

    /// <summary>
    /// Verifies that status contexts complete when marked finished.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WhenStatusContextIsCompleted_ThenObservableCompletes()
    {
        var original = AnsiConsole.Console;
        AnsiConsole.Console = new TestConsole();

        try
        {
            var seenContext = false;
            var completed = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            using var subscription = AnsiConsoleRx.Status("Working")
                .Subscribe(
                    ctx =>
                    {
                        seenContext = true;
                        ctx.Complete();
                    },
                    completed.SetException,
                    completed.SetResult);

            await completed.Task.WaitAsync(TimeSpan.FromSeconds(5));

            seenContext.ShouldBeTrue();
        }
        finally
        {
            AnsiConsole.Console = original;
        }
    }

    /// <summary>
    /// Verifies that live display contexts complete when marked finished.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task WhenLiveContextIsCompleted_ThenObservableCompletes()
    {
        var original = AnsiConsole.Console;
        AnsiConsole.Console = new TestConsole();

        try
        {
            var seenContext = false;
            var completed = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            using var subscription = AnsiConsoleRx.Live(new Text("value"))
                .Subscribe(
                    ctx =>
                    {
                        seenContext = true;
                        ctx.Complete();
                    },
                    completed.SetException,
                    completed.SetResult);

            await completed.Task.WaitAsync(TimeSpan.FromSeconds(5));

            seenContext.ShouldBeTrue();
        }
        finally
        {
            AnsiConsole.Console = original;
        }
    }
}
