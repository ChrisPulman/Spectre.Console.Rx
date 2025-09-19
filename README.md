![Alt](https://repobeats.axiom.co/api/embed/a6232388fa6d7a8d647834b31d9d87a88d046ef9.svg "Repobeats analytics image")

# Spectre.Console.Rx
Reactive extensions for Spectre.Console. Compose terminal animations with Rx and keep all UI mutations on a single, deterministic scheduler.

- Reactive wrappers for Spectre.Console `Status`, `Progress`, and `Live`
- Single-threaded UI scheduler to keep the console thread-safe
- Fluent, chainable context APIs
- Works on .NET Standard 2.0, .NET 8, and .NET 9

Visit https://spectreconsole.net for Spectre.Console docs and https://github.com/spectreconsole/spectre.console for the original source.

## Packages

- Spectre.Console.Rx
- Spectre.Console.Rx.Json (optional JSON helpers)

Install:

```bash
# Core
dotnet add package Spectre.Console.Rx

# Optional JSON helpers
dotnet add package Spectre.Console.Rx.Json
```

## Target frameworks
- .NET Standard 2.0
- .NET 8
- .NET 9

## Core concept
The console is not thread-safe. Spectre.Console.Rx enforces a single UI thread via `AnsiConsoleRx.Scheduler` so all reactive pipelines render on the same thread. Drive animations by pushing work to that scheduler and mark contexts finished when done.

Key points:
- Always switch to the UI scheduler: `.ObserveOn(AnsiConsoleRx.Scheduler)`
- Only one console context (Status/Progress/Live) should run at a time
- Call `IsFinished()` on the context to end rendering/animation loops

## Quick start

### Status
```csharp
using System.Reactive.Linq;
using Spectre.Console.Rx;

AnsiConsoleRx
    .Status("[yellow]Initializing[/]", s => s.AutoRefresh(true))
    .ObserveOn(AnsiConsoleRx.Scheduler)
    .Subscribe(ctx => ctx.Schedule(async scheduler =>
    {
        await scheduler.Sleep(TimeSpan.FromMilliseconds(800));
        ctx.Status("[blue]Warming up[/]");
        ctx.Spinner(Spinner.Known.Dots);

        await scheduler.Sleep(TimeSpan.FromMilliseconds(800));
        ctx.Status("[green]Ready[/]");

        ctx.IsFinished();
    }));
```

### Live (animate a renderable)
```csharp
using System.Reactive.Linq;
using Spectre.Console.Rx;

var table = new Table().AddColumn("Col").AddRow("Row 1");

AnsiConsoleRx
    .Live(table, ld => ld.AutoClear(false))
    .ObserveOn(AnsiConsoleRx.Scheduler)
    .Subscribe(ctx =>
    {
        // Chain updates in-place (blocking) for simple demos
        ctx
            .Update(250, () => table.AddRow("Row 2"))
            .Update(250, () => table.AddRow("Row 3"))
            .Update(250, () => table.AddRow("Done"));

        ctx.IsFinished();
    });
```

### Progress
```csharp
using System.Reactive.Linq;
using Spectre.Console.Rx;

AnsiConsoleRx
    .Progress(p => p.AutoClear(false)
        .Columns(new ProgressColumn[]
        {
            new TaskDescriptionColumn(),
            new ProgressBarColumn(),
            new PercentageColumn(),
            new RemainingTimeColumn(),
            new SpinnerColumn(),
        }))
    .ObserveOn(AnsiConsoleRx.Scheduler)
    .Subscribe(async ctx =>
    {
        var t1 = ctx.AddTask("Downloading").IsIndeterminate(false);
        var t2 = ctx.AddTask("Processing");

        await ctx.Schedule(TimeSpan.FromMilliseconds(100), () =>
        {
            t1.Increment(2);
            if (t1.Value >= 100) t2.Increment(4);
        });

        // Progress completes automatically when all started tasks are finished
    });
```

## Threading model and scheduling
- `AnsiConsoleRx.Scheduler` is a single-threaded scheduler. It hosts a `SynchronizationContext` and processes posted work sequentially.
- Use `.ObserveOn(AnsiConsoleRx.Scheduler)` in your observable pipelines before mutating UI.
- Use context scheduling helpers to perform timed loops without blocking your app thread.

### Context scheduling helpers
All helpers run on the Spectre scheduler and update the UI safely.

- `Task Schedule(this IContext context, TimeSpan delay, Func<bool> isComplete, Action action)`
  - Executes `action` repeatedly every `delay` until `isComplete()` returns true. Calls `context.Refresh()` after each iteration.

- `Task Schedule(this IContext context, Action<SpectreConsoleScheduler> action)`
  - Executes an action on the scheduler. Useful to sequence async sleeps and UI updates.

- `Task<T> Schedule<T>(this IContext context, Func<SpectreConsoleScheduler, Task<T>> action)`
  - Executes an async function on the scheduler and returns its result.

- `Task Schedule(this IContext context, TimeSpan delay, Action action)` (ProgressContext only)
  - Repeats `action` every `delay` until all started progress tasks report finished.

The `SpectreConsoleScheduler` provides `Sleep(TimeSpan)` for non-blocking waits and follows `IScheduler` from Rx.

## API reference (most used)

### AnsiConsoleRx
- `ISpectreConsoleScheduler Scheduler` — the UI scheduler instance
- `IObservable<StatusContext> Status(string status, Func<Status, Status>? configure = null)`
- `IObservable<ProgressContext> Progress(Func<Progress, Progress>? configure = null)`
- `IObservable<LiveDisplayContext> Live(IRenderable renderable, Func<LiveDisplay, LiveDisplay>? configure = null)`
- `IObservable<(ProgressContext context, ProgressTask[] tasks)> AddTasks(this IObservable<ProgressContext>, Func<ProgressContext, ProgressTask[]> factory)`
- `LiveDisplayContext Update(this LiveDisplayContext ctx, int delayMs, Action action)` — simple chained updates (demo-friendly)
- `void IsFinished(this LiveDisplayContext ctx)` / `void IsFinished(this StatusContext ctx)` — signal completion

### StatusContext
- Properties: `string Status`, `Spinner Spinner`, `Style? SpinnerStyle`, `bool IsFinished`
- Methods: `void Refresh()`
- Extensions: `Status(string)`, `Spinner(Spinner)`, `SpinnerStyle(Style?)`, `IsFinished()`

### ProgressContext
- Properties: `bool IsFinished`
- Methods: `ProgressTask AddTask(...)`, `void Refresh()`, `Task Schedule(TimeSpan delay, Action action)`

### LiveDisplayContext
- Properties: `bool IsFinished`
- Methods: `void UpdateTarget(IRenderable? target)`, `void Refresh()`
- Extensions: `Update(int delayMs, Action)`, `IsFinished()`

## JSON helpers (optional)
`Spectre.Console.Rx.Json` adds helpers to pretty print JSON with Spectre.Console widgets.

```csharp
using Spectre.Console.Rx.Json;

var panel = new Panel(new JsonText(jsonString))
    .Header("Data")
    .SquareBorder()
    .BorderColor(Color.LightSkyBlue1);

AnsiConsole.Write(panel);
```

## Best practices
- Only one Status/Progress/Live should be active at a time (the console is single-threaded)
- Always `.ObserveOn(AnsiConsoleRx.Scheduler)` before mutating UI
- End animations with `IsFinished()` so observables complete
- Prefer scheduler `Sleep` over `Thread.Sleep` to keep the pump responsive
- Keep heavy work off the UI scheduler; compute elsewhere, then push results to UI via `ObserveOn`

## Troubleshooting
- Nothing renders / pipeline hangs
  - Ensure `.ObserveOn(AnsiConsoleRx.Scheduler)` is applied before `Subscribe` that updates UI
  - Make sure you call `IsFinished()` on the context when done
- Flicker or interleaved output
  - Avoid running multiple contexts concurrently; sequence them
- Exceptions swallowed
  - Wrap your subscription in `Subscribe(onNext, onError, onCompleted)` and log `onError`

## Examples
See example projects under `src`:
- `LiveExample`, `LiveTableExample`
- `StatusExample`
- `ProgressExample`
- `CombinedExample`

## License
MIT License. See the LICENSE file for details.
