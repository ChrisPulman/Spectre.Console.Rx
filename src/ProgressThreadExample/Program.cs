// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
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

        using var completed = new ManualResetEventSlim(false);
        Exception? error = null;

        // Some time later...
        using var subscription = AnsiConsoleRx.Progress()
            .AddTasks(ctx => new[]
            {
                ctx.AddTask("Deleting Custom Formats").MaxValue(cfs.Count),
            })
            .ObserveOn(AnsiConsoleRx.Scheduler)
            .SelectMany(ctx =>
            {
                var rng = new Random();

                return cfs
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
                    .Do(_ => ctx.tasks[0].Increment(1))
                    .Select(_ => Unit.Default)
                    .Finally(() =>
                    {
                        ctx.context.Complete();
                        AnsiConsole.MarkupLine("[blue]Done![/]"); // Render a message when the sequence completes
                    });
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
            throw new InvalidOperationException("Progress thread example failed.", error);
        }
    }
}
