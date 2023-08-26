// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reactive.Linq;
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
    /// <param name="args">The arguments.</param>
    public static void Main(string[] args)
    {
        var table = new Table().Expand().BorderColor(Color.Grey);
        table.AddColumn("[yellow]Source currency[/]");
        table.AddColumn("[yellow]Destination currency[/]");
        table.AddColumn("[yellow]Exchange rate[/]");

        AnsiConsole.MarkupLine("Press [yellow]CTRL+C[/] to exit");

        AnsiConsoleRx.Live(table, p =>
            p.AutoClear(false)
            .Overflow(VerticalOverflow.Ellipsis)
            .Cropping(VerticalOverflowCropping.Bottom))
            .ObserveOn(AnsiConsoleRx.Scheduler)
            .Do(_ =>
            {
                // Add some initial rows
                foreach (var a in Enumerable.Range(0, NumberOfRows))
                {
                    AddExchangeRateRow(table);
                }
            })
            .CombineLatest(Observable.Interval(TimeSpan.FromMilliseconds(400)), (ctx, _) => ctx)
            .Subscribe(ctx =>
            {
                // Continously update the table
                // More rows than we want?
                if (table.Rows.Count > NumberOfRows)
                {
                    // Remove the first one
                    table.Rows.RemoveAt(0);
                }

                // Add a new row
                AddExchangeRateRow(table);

                // Refresh and wait for a while
                ctx.Refresh();
            });

        Console.ReadLine();
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
