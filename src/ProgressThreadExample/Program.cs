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
    public static void Main()
    {
        // The list of objects used as input to the REST API call
        IList<string> cfs = new List<string>();
        for (var i = 0; i < 100; i++)
        {
            cfs.Add($"Custom Format {i}");
        }

        // Some time later...
        AnsiConsoleRx.Progress()
            .AddTasks(ctx => new[]
            {
                ctx.AddTask("Deleting Custom Formats").MaxValue(cfs.Count),
            })
            .ObserveOn(AnsiConsoleRx.Scheduler)
            .Subscribe(
            ctx =>
            {
                var rng = new Random();

                cfs.ToObservable()
                    .Select(_ => Observable.FromAsync(async ct => await Task.Delay(TimeSpan.FromSeconds(rng.Next(1, 5)), ct), AnsiConsoleRx.Scheduler))
                    .Merge(8)
                    .ObserveOn(SpectreConsoleScheduler.Instance)
                    .Subscribe(_ => ctx.tasks[0].Increment(1));
            },
            () => AnsiConsole.MarkupLine("[blue]Done![/]")); // Render a message when the sequence completes
    }
}
