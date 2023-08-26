// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
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

        // Show progress
        AnsiConsoleRx.Progress(p =>
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
            .Subscribe(async ctx =>
            {
                var random = new Random(DateTime.Now.Millisecond);

                // Create some tasks
                var tasks = CreateTasks(ctx, random);
                var warpTask = ctx.AddTask("Going to warp", autoStart: false).IsIndeterminate();

                await ctx.Schedule(
                    TimeSpan.FromMilliseconds(100),
                    () => ctx.IsFinished,
                    () =>
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
                    });

                // Now start the "warp" task
                warpTask.StartTask().IsIndeterminate(false);
                await ctx.Schedule(
                    TimeSpan.FromMilliseconds(100),
                    () => warpTask.Increment(12 * random.NextDouble()));

                // Done
                await ctx.Schedule(_ => WriteLogMessage("[green]Done![/]"));
            });
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
