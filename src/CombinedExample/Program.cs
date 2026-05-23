// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Linq;
using ProgressDemo;
using Spectre.Console.Rx;

// NOTE: This code executes synchronously at the process boundary, but each demo is composed as
//       an observable subscription. Only one console context can be active at a time because the
//       console context is not thread safe.
var table = new Table();
using var completed = new ManualResetEventSlim(false);
Exception? error = null;

using var subscription = Observable
    .Concat(
        RunStatusDemo(),
        Observable.Defer(() =>
        {
            WriteLogMessage("[bold green]Crusing at Warp 9[/]");
            return RunLiveDemo(table);
        }),
        Observable.Defer(() =>
        {
            AnsiConsole.MarkupLine("[yellow]Initializing warp drive[/]...");
            return RunProgressDemo();
        }),
        Observable.Start(() => WriteLogMessage("[green]Combined demo complete[/]"), AnsiConsoleRx.Scheduler))
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
    throw new InvalidOperationException("Combined example failed.", error);
}

static IObservable<Unit> RunStatusDemo() =>
    AnsiConsoleRx
        .Status(
            "[yellow]Initializing warp drive[/]",
            p => p.AutoRefresh(true).Spinner(Spinner.Known.Default))
        .ObserveOn(AnsiConsoleRx.Scheduler)
        .SelectMany(RunStatusSequence);

static IObservable<Unit> RunLiveDemo(Table table) =>
    AnsiConsoleRx.Live(table, ld => ld.AutoClear(false).Overflow(VerticalOverflow.Ellipsis).Cropping(VerticalOverflowCropping.Top))
        .ObserveOn(AnsiConsoleRx.Scheduler)
        .Do(ctx =>
        {
            // Columns
            ctx.Update(230, () => table.AddColumn("Release date"))
            .Update(230, () => table.AddColumn("Title"))
            .Update(230, () => table.AddColumn("Budget"))
            .Update(230, () => table.AddColumn("Opening Weekend"))
            .Update(230, () => table.AddColumn("Box office"))

            // Rows
            .Update(70, () => table.AddRow("May 25, 1977", "[yellow]Star Wars[/] [grey]Ep.[/] [u]IV[/]", "$11,000,000", "$1,554,475", "$775,398,007"))
            .Update(70, () => table.AddRow("May 21, 1980", "[yellow]Star Wars[/] [grey]Ep.[/] [u]V[/]", "$18,000,000", "$4,910,483", "$547,969,004"))
            .Update(70, () => table.AddRow("May 25, 1983", "[yellow]Star Wars[/] [grey]Ep.[/] [u]VI[/]", "$32,500,000", "$23,019,618", "$475,106,177"))
            .Update(70, () => table.AddRow("May 19, 1999", "[yellow]Star Wars[/] [grey]Ep.[/] [u]I[/]", "$115,000,000", "$64,810,870", "$1,027,044,677"))
            .Update(70, () => table.AddRow("May 16, 2002", "[yellow]Star Wars[/] [grey]Ep.[/] [u]II[/]", "$115,000,000", "$80,027,814", "$649,436,358"))
            .Update(70, () => table.AddRow("May 19, 2005", "[yellow]Star Wars[/] [grey]Ep.[/] [u]III[/]", "$113,000,000", "$108,435,841", "$850,035,635"))
            .Update(70, () => table.AddRow("Dec 18, 2015", "[yellow]Star Wars[/] [grey]Ep.[/] [u]VII[/]", "$245,000,000", "$247,966,675", "$2,068,223,624"))
            .Update(70, () => table.AddRow("Dec 15, 2017", "[yellow]Star Wars[/] [grey]Ep.[/] [u]VIII[/]", "$317,000,000", "$220,009,584", "$1,333,539,889"))
            .Update(70, () => table.AddRow("Dec 20, 2019", "[yellow]Star Wars[/] [grey]Ep.[/] [u]IX[/]", "$245,000,000", "$177,383,864", "$1,074,114,248"))

            // Column footer
            .Update(230, () => table.Columns[2].Footer("$1,633,000,000"))
            .Update(230, () => table.Columns[3].Footer("$928,119,224"))
            .Update(400, () => table.Columns[4].Footer("$10,318,030,576"))

            // Column alignment
            .Update(230, () => table.Columns[2].RightAligned())
            .Update(230, () => table.Columns[3].RightAligned())
            .Update(400, () => table.Columns[4].RightAligned())

            // Column titles
            .Update(70, () => table.Columns[0].Header("[bold]Release date[/]"))
            .Update(70, () => table.Columns[1].Header("[bold]Title[/]"))
            .Update(70, () => table.Columns[2].Header("[red bold]Budget[/]"))
            .Update(70, () => table.Columns[3].Header("[green bold]Opening Weekend[/]"))
            .Update(400, () => table.Columns[4].Header("[blue bold]Box office[/]"))

            // Footers
            .Update(70, () => table.Columns[2].Footer("[red bold]$1,633,000,000[/]"))
            .Update(70, () => table.Columns[3].Footer("[green bold]$928,119,224[/]"))
            .Update(400, () => table.Columns[4].Footer("[blue bold]$10,318,030,576[/]"))

            // Title
            .Update(500, () => table.Title("Star Wars Movies"))
            .Update(400, () => table.Title("[[ [yellow]Star Wars Movies[/] ]]"))

            // Borders
            .Update(230, () => table.BorderColor(Color.Yellow))
            .Update(230, () => table.MinimalBorder())
            .Update(230, () => table.SimpleBorder())
            .Update(230, () => table.SimpleHeavyBorder())

            // Caption
            .Update(400, () => table.Caption("[[ [blue]THE END[/] ]]"))

            // Finish animation and allow next animation to start
            .IsFinished();
        })
        .Select(_ => Unit.Default);

static IObservable<Unit> RunProgressDemo() =>
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
        .SelectMany(ctx =>
        {
            var random = new Random(DateTime.Now.Millisecond);

            // Create some tasks
            var tasks = CreateTasks(ctx, random);
            var warpTask = ctx.AddTask("Going to warp", autoStart: false).IsIndeterminate();

            var primaryTasks = Observable
                .Interval(TimeSpan.FromMilliseconds(100), AnsiConsoleRx.Scheduler)
                .Do(_ =>
                {
                    // Increment progress
                    foreach (var (task, increment) in tasks)
                    {
                        task.Increment(random.NextDouble() * increment);
                    }

                    // Write some random things to the terminal
                    if (random.NextDouble() < 0.1)
                    {
                        GenerateLogMessage();
                    }
                })
                .TakeWhile(_ => !ctx.IsFinished)
                .Select(_ => Unit.Default)
                .IgnoreElements();

            var warpTaskProgress = Observable
                .Defer(() =>
                {
                    // Now start the "warp" task
                    warpTask.StartTask();
                    warpTask.IsIndeterminate(false);

                    return Observable
                        .Interval(TimeSpan.FromMilliseconds(100), AnsiConsoleRx.Scheduler)
                        .Do(_ => warpTask.Increment(12 * random.NextDouble()))
                        .TakeWhile(_ => !warpTask.IsFinished)
                        .Select(_ => Unit.Default)
                        .IgnoreElements();
                });

            var done = Observable.Start(() => WriteLogMessage("[green]Done![/]"), AnsiConsoleRx.Scheduler);

            return primaryTasks
                .Concat(warpTaskProgress)
                .Concat(done)
                .Finally(ctx.Complete);
        });

static IObservable<Unit> RunStatusSequence(StatusContext ctx)
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
        .Concat(Do(ctx.IsFinished));
}

static IObservable<Unit> Delay(TimeSpan delay) =>
    Observable.Timer(delay, AnsiConsoleRx.Scheduler).Select(_ => Unit.Default);

static IObservable<Unit> DelayThen(TimeSpan delay, Action action) =>
    Delay(delay).Do(_ => action());

static IObservable<Unit> Do(Action action) =>
    Observable.Defer(() =>
    {
        action();
        return Observable.Return(Unit.Default, AnsiConsoleRx.Scheduler);
    });

static List<(ProgressTask Task, int Delay)> CreateTasks(ProgressContext context, Random random)
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

static void GenerateLogMessage() => AnsiConsole.MarkupLine(
        "[grey]LOG:[/] " +
        DescriptionGenerator.Generate() +
        "[grey]...[/]");

static void WriteLogMessage(string message) => AnsiConsole.MarkupLine($"[grey]LOG:[/] {message}[grey]...[/]");
