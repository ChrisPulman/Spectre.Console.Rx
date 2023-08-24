// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Spectre.Console;
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
            .Subscribe(
            ctx =>
            {
                var rng = new Random();

                cfs.ToObservable()
                    .Select(x => Observable.FromAsync(async ct =>
                    {
                        await Task.Delay(TimeSpan.FromSeconds(rng.Next(1, 5)), ct);
                    }))
                    .Merge(8)
                    .Subscribe(_ =>
                    {
                        ctx.tasks[0].Increment(1); // Not thread safe; needs to invoke on the thread that subscribed; not sure if this is ObserveOn or SubscribeOn
                    });
            },
            () =>
            {
                // Render a message when the sequence completes
                AnsiConsole.MarkupLine("[blue]Done![/]");
            });

        Console.ReadLine();
    }
}
