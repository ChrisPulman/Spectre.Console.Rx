// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reactive;
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
        using var completed = new ManualResetEventSlim(false);
        Exception? error = null;

        using var subscription = AnsiConsoleRx
            .Status(
                "[yellow]Initializing warp drive[/]",
                p => p.AutoRefresh(true).Spinner(Spinner.Known.Default))
            .ObserveOn(AnsiConsoleRx.Scheduler)
            .SelectMany(RunStatusSequence)
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
            throw new InvalidOperationException("Status example failed.", error);
        }
    }

    private static IObservable<Unit> RunStatusSequence(StatusContext ctx)
    {
        var warpSteps = Enumerable
            .Range(1, 9)
            .Select(warp => Do(() => ctx.Status($"[bold blue]Warp {warp}[/]")).Concat(Delay(TimeSpan.FromMilliseconds(500))));

        return Observable
            .Concat(
                DelayThen(TimeSpan.FromMilliseconds(3000), () => WriteLogMessage("Starting gravimetric field displacement manifold")),
                DelayThen(TimeSpan.FromMilliseconds(1000), () => WriteLogMessage("Warming up deuterium chamber")),
                DelayThen(TimeSpan.FromMilliseconds(2000), () => WriteLogMessage("Generating antideuterium")),
                DelayThen(
                    TimeSpan.FromMilliseconds(3000),
                    () =>
                    {
                        ctx.Spinner(Spinner.Known.BouncingBar);
                        ctx.Status("[bold blue]Unfolding warp nacelles[/]");
                        WriteLogMessage("Unfolding left warp nacelle");
                    }),
                DelayThen(
                    TimeSpan.FromMilliseconds(2000),
                    () =>
                    {
                        WriteLogMessage("Left warp nacelle [green]online[/]");
                        WriteLogMessage("Unfolding right warp nacelle");
                    }),
                DelayThen(TimeSpan.FromMilliseconds(1000), () => WriteLogMessage("Right warp nacelle [green]online[/]")),
                DelayThen(
                    TimeSpan.FromMilliseconds(3000),
                    () =>
                    {
                        ctx.Spinner(Spinner.Known.Star2);
                        ctx.Status("[bold blue]Generating warp bubble[/]");
                    }),
                DelayThen(
                    TimeSpan.FromMilliseconds(3000),
                    () =>
                    {
                        ctx.Spinner(Spinner.Known.Star);
                        ctx.Status("[bold blue]Stabilizing warp bubble[/]");
                        ctx.Spinner(Spinner.Known.Monkey);
                        ctx.Status("[bold blue]Performing safety checks[/]");
                        WriteLogMessage("Enabling interior dampening");
                    }),
                DelayThen(TimeSpan.FromMilliseconds(2000), () => WriteLogMessage("Interior dampening [green]enabled[/]")),
                DelayThen(
                    TimeSpan.FromMilliseconds(3000),
                    () =>
                    {
                        ctx.Spinner(Spinner.Known.Moon);
                        WriteLogMessage("Preparing for warp");
                    }),
                Delay(TimeSpan.FromMilliseconds(1000)))
            .Concat(Observable.Concat(warpSteps))
            .Concat(Do(() =>
            {
                WriteLogMessage("[bold green]Crusing at Warp 9.8[/]");
                ctx.IsFinished();
            }));
    }

    private static IObservable<Unit> Delay(TimeSpan delay) =>
        Observable.Timer(delay, AnsiConsoleRx.Scheduler).Select(_ => Unit.Default);

    private static IObservable<Unit> DelayThen(TimeSpan delay, Action action) =>
        Delay(delay).Do(_ => action());

    private static IObservable<Unit> Do(Action action) =>
        Observable.Defer(() =>
        {
            action();
            return Observable.Return(Unit.Default, AnsiConsoleRx.Scheduler);
        });

    private static void WriteLogMessage(string message) => AnsiConsole.MarkupLine($"[grey]LOG:[/] {message}[grey]...[/]");
}
