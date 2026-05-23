// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using Spectre.Console.Rx;

namespace Progress;

/// <summary>
/// Program.
/// </summary>
public static class Program
{
    /// <summary>
    /// Defines the entry point of the application.
    /// </summary>
    public static void Main()
    {
        AnsiConsole.MarkupLine("[yellow]Initializing warp drive[/]...");
        using var completed = new ManualResetEventSlim(false);
        Exception? error = null;

        // Show progress
        using var subscription = AnsiConsoleRx.Progress(p =>
            p.AutoClear(false)
            .Columns(new ProgressColumn[]
            {
                    new TaskDescriptionColumn(),    // Task description
                    new ProgressBarColumn(),        // Progress bar
                    new PercentageColumn(),         // Percentage
                    new RemainingTimeColumn(),      // Remaining time
                    new SpinnerColumn(),            // Spinner
            }))
            .ObserveOn(AnsiConsoleRx.Scheduler)
            .SelectMany(ctx =>
            {
                var random = new Random(DateTime.Now.Millisecond);

                // Create some tasks
                var tasks = CreateTasks(ctx, random);
                var warpTask = ctx.AddTask("Going to warp", autoStart: false).IsIndeterminate();

                var primaryTasks = Observable
                    .Interval(TimeSpan.FromMilliseconds(100), AnsiConsoleRx.Scheduler)
                    .Do(_ =>
                    {
                        // Increment progress
                        foreach (var (task, increment) in tasks)
                        {
                            task.Increment(random.NextDouble() * increment);
                        }

                        // Write some random things to the terminal
                        if (random.NextDouble() < 0.1)
                        {
                            WriteLogMessage();
                        }
                    })
                    .TakeWhile(_ => !ctx.IsFinished)
                    .Select(_ => Unit.Default)
                    .IgnoreElements();

                var warpTaskProgress = Observable
                    .Defer(() =>
                    {
                        // Now start the "warp" task
                        warpTask.StartTask();
                        warpTask.IsIndeterminate(false);

                        return Observable
                            .Interval(TimeSpan.FromMilliseconds(100), AnsiConsoleRx.Scheduler)
                            .Do(_ => warpTask.Increment(12 * random.NextDouble()))
                            .TakeWhile(_ => !warpTask.IsFinished)
                            .Select(_ => Unit.Default)
                            .IgnoreElements();
                    });

                var done = Observable.Start(() => WriteLogMessage("[green]Done![/]"), AnsiConsoleRx.Scheduler);

                return primaryTasks
                    .Concat(warpTaskProgress)
                    .Concat(done)
                    .Finally(ctx.Complete);
            })
            .Subscribe(
                _ => { },
                ex =>
                {
                    error = ex;
                    completed.Set();
                },
                completed.Set);

        completed.Wait();

        if (error is not null)
        {
            throw new InvalidOperationException("Progress example failed.", error);
        }
    }

    private static List<(ProgressTask Task, int Delay)> CreateTasks(ProgressContext context, Random random)
    {
        var tasks = new List<(ProgressTask, int)>();
        while (tasks.Count < 5)
        {
            if (DescriptionGenerator.TryGenerate(out var name))
            {
                tasks.Add((context.AddTask(name), random.Next(2, 10)));
            }
        }

        return tasks;
    }

    private static void WriteLogMessage() => AnsiConsole.MarkupLine(
            "[grey]LOG:[/] " +
            DescriptionGenerator.Generate() +
            "[grey]...[/]");

    private static void WriteLogMessage(string message) => AnsiConsole.MarkupLine(
            "[grey]LOG:[/] " +
            message +
            "[grey]...[/]");
}
