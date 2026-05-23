// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task Main()
    {
        // The list of objects used as input to the REST API call
        IList<string> cfs = new List<string>();
        for (var i = 0; i < 100; i++)
        {
            cfs.Add($"Custom Format {i}");
        }

        // Some time later...
        await AnsiConsoleRx.Progress()
            .AddTasks(ctx => new[]
            {
                ctx.AddTask("Deleting Custom Formats").MaxValue(cfs.Count),
            })
            .ObserveOn(AnsiConsoleRx.Scheduler)
            .RunAsync(async ctx =>
            {
                var rng = new Random();

                await cfs
                    .ToObservable()
                    .Select(item => Observable.FromAsync(
                        async ct =>
                        {
                            await Task.Delay(TimeSpan.FromSeconds(rng.Next(1, 5)), ct).ConfigureAwait(false);
                            return item;
                        },
                        AnsiConsoleRx.Scheduler))
                    .Merge(8)
                    .ObserveOn(SpectreConsoleScheduler.Instance)
                    .RunAsync(_ => ctx.tasks[0].Increment(1));

                AnsiConsole.MarkupLine("[blue]Done![/]"); // Render a message when the sequence completes
            });
    }
}
