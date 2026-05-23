// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task Main(string[] args)
    {
        var continuous = args.Contains("--continuous", StringComparer.OrdinalIgnoreCase);
        using var exit = new CancellationTokenSource();
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

        await AnsiConsoleRx.Live(table, p =>
            p.AutoClear(false)
            .Overflow(VerticalOverflow.Ellipsis)
            .Cropping(VerticalOverflowCropping.Bottom))
            .ObserveOn(AnsiConsoleRx.Scheduler)
            .RunAsync(async ctx =>
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

                try
                {
                    await ticks
                        .ObserveOn(AnsiConsoleRx.Scheduler)
                        .RunAsync(
                            _ =>
                            {
                                // Continuously update the table
                                if (table.Rows.Count > NumberOfRows)
                                {
                                    table.Rows.RemoveAt(0);
                                }

                                AddExchangeRateRow(table);
                                ctx.Refresh();
                            },
                            exit.Token)
                        .ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (exit.IsCancellationRequested)
                {
                    // The user requested a clean shutdown.
                }
                finally
                {
                    ctx.IsFinished();
                }
            });
    }

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
