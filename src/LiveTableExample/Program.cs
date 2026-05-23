// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using Spectre.Console.Rx;

namespace LiveTable;

/// <summary>
/// Program.
/// </summary>
public static class Program
{
    private const int NumberOfRows = 10;

    private static readonly Random _random = new();
    private static readonly string[] _exchanges = new string[]
    {
            "SGD", "SEK", "PLN",
            "MYR", "EUR", "USD",
            "AUD", "JPY", "CNH",
            "HKD", "CAD", "INR",
            "DKK", "GBP", "RUB",
            "NZD", "MXN", "IDR",
            "TWD", "THB", "VND",
    };

    /// <summary>
    /// Defines the entry point of the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
        var continuous = args.Contains("--continuous", StringComparer.OrdinalIgnoreCase);
        using var exit = new CancellationTokenSource();
        using var completed = new ManualResetEventSlim(false);
        Exception? error = null;

        Console.CancelKeyPress += (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            exit.Cancel();
        };

        var table = new Table().Expand().BorderColor(Color.Grey);
        table.AddColumn("[yellow]Source currency[/]");
        table.AddColumn("[yellow]Destination currency[/]");
        table.AddColumn("[yellow]Exchange rate[/]");

        AnsiConsole.MarkupLine(continuous
            ? "Press [yellow]CTRL+C[/] to exit"
            : "Streaming [yellow]30[/] exchange-rate updates");

        using var subscription = AnsiConsoleRx.Live(table, p =>
            p.AutoClear(false)
            .Overflow(VerticalOverflow.Ellipsis)
            .Cropping(VerticalOverflowCropping.Bottom))
            .ObserveOn(AnsiConsoleRx.Scheduler)
            .SelectMany(ctx =>
            {
                // Add some initial rows
                foreach (var a in Enumerable.Range(0, NumberOfRows))
                {
                    AddExchangeRateRow(table);
                }

                ctx.Refresh();
                var ticks = Observable.Interval(TimeSpan.FromMilliseconds(400));
                if (!continuous)
                {
                    ticks = ticks.Take(30);
                }

                return ticks
                    .TakeUntil(FromCancellation(exit.Token))
                    .ObserveOn(AnsiConsoleRx.Scheduler)
                    .Do(_ =>
                    {
                        // Continuously update the table
                        if (table.Rows.Count > NumberOfRows)
                        {
                            table.Rows.RemoveAt(0);
                        }

                        AddExchangeRateRow(table);
                        ctx.Refresh();
                    })
                    .Select(_ => Unit.Default)
                    .Finally(ctx.IsFinished);
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
            throw new InvalidOperationException("Live table example failed.", error);
        }
    }

    private static IObservable<Unit> FromCancellation(CancellationToken token) =>
        Observable.Create<Unit>(observer =>
        {
            if (token.IsCancellationRequested)
            {
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
                return Disposable.Empty;
            }

            var registration = token.Register(() =>
            {
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
            });

            return Disposable.Create(registration.Dispose);
        });

    private static void AddExchangeRateRow(Table table)
    {
        var (source, destination, rate) = GetExchangeRate();
        table.AddRow(source, destination, _random.NextDouble() > 0.35D ? $"[green]{rate}[/]" : $"[red]{rate}[/]");
    }

    private static (string Source, string Destination, double Rate) GetExchangeRate()
    {
        var source = _exchanges[_random.Next(0, _exchanges.Length)];
        var dest = _exchanges[_random.Next(0, _exchanges.Length)];
        var rate = 200 / ((_random.NextDouble() * 320) + 1);

        while (source == dest)
        {
            dest = _exchanges[_random.Next(0, _exchanges.Length)];
        }

        return (source, dest, rate);
    }
}
