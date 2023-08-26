// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Spectre.Console.Rx;

namespace Status;

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
        AnsiConsoleRx
            .Status(
                "[yellow]Initializing warp drive[/]",
                p => p.AutoRefresh(true).Spinner(Spinner.Known.Default))
            .ObserveOn(AnsiConsoleRx.Scheduler)
            .Subscribe(ctx =>
            {
                ctx.Schedule(async context =>
                {
                    // Initialize
                    await context.Sleep(TimeSpan.FromMilliseconds(3000));
                    WriteLogMessage("Starting gravimetric field displacement manifold");
                    await context.Sleep(TimeSpan.FromMilliseconds(1000));
                    WriteLogMessage("Warming up deuterium chamber");
                    await context.Sleep(TimeSpan.FromMilliseconds(2000));
                    WriteLogMessage("Generating antideuterium");

                    // Warp nacelles
                    await context.Sleep(TimeSpan.FromMilliseconds(3000));
                    ctx.Spinner(Spinner.Known.BouncingBar);
                    ctx.Status("[bold blue]Unfolding warp nacelles[/]");
                    WriteLogMessage("Unfolding left warp nacelle");
                    await context.Sleep(TimeSpan.FromMilliseconds(2000));
                    WriteLogMessage("Left warp nacelle [green]online[/]");
                    WriteLogMessage("Unfolding right warp nacelle");
                    await context.Sleep(TimeSpan.FromMilliseconds(1000));
                    WriteLogMessage("Right warp nacelle [green]online[/]");

                    // Warp bubble
                    await context.Sleep(TimeSpan.FromMilliseconds(3000));
                    ctx.Spinner(Spinner.Known.Star2);
                    ctx.Status("[bold blue]Generating warp bubble[/]");
                    await context.Sleep(TimeSpan.FromMilliseconds(3000));
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.Status("[bold blue]Stabilizing warp bubble[/]");

                    // Safety
                    ctx.Spinner(Spinner.Known.Monkey);
                    ctx.Status("[bold blue]Performing safety checks[/]");
                    WriteLogMessage("Enabling interior dampening");
                    await context.Sleep(TimeSpan.FromMilliseconds(2000));
                    WriteLogMessage("Interior dampening [green]enabled[/]");

                    // Warp!
                    await context.Sleep(TimeSpan.FromMilliseconds(3000));
                    ctx.Spinner(Spinner.Known.Moon);
                    WriteLogMessage("Preparing for warp");
                    await context.Sleep(TimeSpan.FromMilliseconds(1000));
                    for (var warp = 1; warp < 10; warp++)
                    {
                        ctx.Status($"[bold blue]Warp {warp}[/]");
                        await context.Sleep(TimeSpan.FromMilliseconds(500));
                    }

                    // Done
                    WriteLogMessage("[bold green]Crusing at Warp 9.8[/]");
                });
            });
    }

    private static void WriteLogMessage(string message) => AnsiConsole.MarkupLine($"[grey]LOG:[/] {message}[grey]...[/]");
}
