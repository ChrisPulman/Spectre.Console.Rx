# Spectre.Console.Rx

Reactive console applications built on a Spectre.Console-compatible API.

`Spectre.Console.Rx` provides observable-first entry points for live console
experiences such as status spinners, progress bars, live displays, prompts,
renderables, ANSI output, JSON rendering, image rendering, ReactiveUI routing,
and testable console output.

The solution contains the core reactive console library, optional integration
libraries, examples, benchmarks, source generators, and TUnit tests.

![Alt](https://repobeats.axiom.co/api/embed/616d5f8ba9f6f0d24125b559aefbcbb763c3dc58.svg "Repobeats analytics image")

## Packages

| Project | Target frameworks | Purpose |
| --- | --- | --- |
| `Spectre.Console.Rx` | `netstandard2.0`, `net8.0`, `net9.0`, `net10.0` | Core reactive console API and Spectre.Console-compatible renderables, prompts, live display, status, progress, markup, tables, layouts, widgets, colors, and scheduler. |
| `Spectre.Console.Rx.Ansi` | `netstandard2.0`, `net8.0`, `net9.0`, `net10.0` | ANSI writer, ANSI capabilities, color systems, markup parsing, styles, links, escape helpers, and generated color data. |
| `Spectre.Console.Rx.Json` | `netstandard2.0`, `net8.0`, `net9.0`, `net10.0` | Render JSON as styled Spectre.Console renderables and parse JSON into a small syntax tree. |
| `Spectre.Console.Rx.ImageSharp` | `net8.0`, `net9.0`, `net10.0` | Render image files, streams, and byte buffers in the console using ImageSharp. |
| `Spectre.Console.Rx.ReactiveUI` | `net8.0`, `net9.0`, `net10.0` | Configure ReactiveUI for console apps and build routable live console views. |
| `Spectre.Console.Rx.Testing` | `netstandard2.0`, `net8.0`, `net9.0`, `net10.0` | Test console, fake input, console capabilities, output capture, and assertion helpers. |
| `Spectre.Console.Rx.SourceGenerator` | `netstandard2.0` | Build-time source generator used by the solution for generated console data such as colors, spinners, and emoji. |

## Installation

Use the core package for reactive live console applications.

```bash
dotnet add package Spectre.Console.Rx
```

Add optional packages only when needed.

```bash
dotnet add package Spectre.Console.Rx.Json
dotnet add package Spectre.Console.Rx.ImageSharp
dotnet add package Spectre.Console.Rx.ReactiveUI
dotnet add package Spectre.Console.Rx.Testing
```

Reactive examples use `System.Reactive` operators such as `Observable.Interval`,
`SelectMany`, `Concat`, `ObserveOn`, `Finally`, and `Subscribe`.

```bash
dotnet add package System.Reactive
```

## Core Concepts

`Spectre.Console.Rx` is reactive-first:

- A console operation is represented as an `IObservable<TContext>`.
- `AnsiConsoleRx.Status(...)`, `AnsiConsoleRx.Live(...)`, and `AnsiConsoleRx.Progress(...)` create observable console contexts.
- The context is emitted once the Spectre console operation is active.
- You compose work with Rx operators.
- You end live operations by calling `ctx.IsFinished()` or `ctx.Complete()`.
- Console mutations should run on `AnsiConsoleRx.Scheduler`.
- Only one live console context should be active at a time. Compose sequential operations with `Concat`.

The solution also keeps a Spectre.Console-compatible API surface in the
`Spectre.Console.Rx` namespace, so familiar renderables such as `Table`,
`Panel`, `Markup`, `Progress`, `LiveDisplay`, `TextPrompt<T>`, and `Canvas`
can be used directly.

## Observable-First Status

```csharp
using System.Reactive;
using System.Reactive.Linq;
using Spectre.Console.Rx;

using var completed = new ManualResetEventSlim(false);
Exception? error = null;

using var subscription = AnsiConsoleRx
    .Status(
        "[yellow]Initializing[/]",
        status => status.AutoRefresh(true).Spinner(Spinner.Known.Default))
    .ObserveOn(AnsiConsoleRx.Scheduler)
    .SelectMany(ctx =>
        Observable
            .Timer(TimeSpan.FromSeconds(1), AnsiConsoleRx.Scheduler)
            .Do(_ => ctx.Status("[green]Ready[/]"))
            .Select(_ => Unit.Default)
            .Finally(ctx.IsFinished))
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
    throw error;
}
```

## Observable-First Live Display

```csharp
using System.Reactive;
using System.Reactive.Linq;
using Spectre.Console.Rx;

var table = new Table()
    .Expand()
    .AddColumn("[yellow]Time[/]")
    .AddColumn("[yellow]Value[/]");

using var completed = new ManualResetEventSlim(false);

using var subscription = AnsiConsoleRx
    .Live(table, live => live.AutoClear(false))
    .ObserveOn(AnsiConsoleRx.Scheduler)
    .SelectMany(ctx =>
        Observable
            .Interval(TimeSpan.FromMilliseconds(250), AnsiConsoleRx.Scheduler)
            .Take(20)
            .Do(i =>
            {
                table.AddRow(DateTime.Now.ToLongTimeString(), i.ToString());
                ctx.Refresh();
            })
            .Select(_ => Unit.Default)
            .Finally(ctx.IsFinished))
    .Subscribe(
        _ => { },
        ex => throw ex,
        completed.Set);

completed.Wait();
```

## Observable-First Progress

```csharp
using System.Reactive;
using System.Reactive.Linq;
using Spectre.Console.Rx;

using var completed = new ManualResetEventSlim(false);

using var subscription = AnsiConsoleRx
    .Progress(progress =>
        progress.Columns(
            new TaskDescriptionColumn(),
            new ProgressBarColumn(),
            new PercentageColumn(),
            new RemainingTimeColumn(),
            new SpinnerColumn()))
    .ObserveOn(AnsiConsoleRx.Scheduler)
    .SelectMany(ctx =>
    {
        var build = ctx.AddTask("Build");
        var test = ctx.AddTask("Test");

        return Observable
            .Interval(TimeSpan.FromMilliseconds(100), AnsiConsoleRx.Scheduler)
            .Do(_ =>
            {
                build.Increment(5);
                test.Increment(3);
            })
            .TakeWhile(_ => !ctx.IsFinished)
            .IgnoreElements()
            .Finally(ctx.Complete);
    })
    .Subscribe(
        _ => { },
        ex => throw ex,
        completed.Set);

completed.Wait();
```

## Sequential Console Operations

Only one live console operation can own the terminal at a time. Use `Concat` to
run operations in order.

```csharp
using System.Reactive;
using System.Reactive.Linq;
using Spectre.Console.Rx;

IObservable<Unit> StatusStep() =>
    AnsiConsoleRx
        .Status("Preparing")
        .ObserveOn(AnsiConsoleRx.Scheduler)
        .SelectMany(ctx =>
            Observable
                .Timer(TimeSpan.FromSeconds(1), AnsiConsoleRx.Scheduler)
                .Do(_ => ctx.Status("Prepared"))
                .Select(_ => Unit.Default)
                .Finally(ctx.IsFinished));

IObservable<Unit> LiveStep(Table table) =>
    AnsiConsoleRx
        .Live(table)
        .ObserveOn(AnsiConsoleRx.Scheduler)
        .Do(ctx =>
        {
            table.AddColumn("Name");
            table.AddRow("Reactive live table");
            ctx.Refresh();
            ctx.IsFinished();
        })
        .Select(_ => Unit.Default);

var table = new Table();
using var completed = new ManualResetEventSlim(false);

using var subscription = Observable
    .Concat(StatusStep(), LiveStep(table))
    .Subscribe(_ => { }, ex => throw ex, completed.Set);

completed.Wait();
```

## API Reference: Spectre.Console.Rx

### `AnsiConsoleRx`

`AnsiConsoleRx` is the main entry point for reactive live console contexts.

| Member | Description |
| --- | --- |
| `Scheduler` | Singleton `ISpectreConsoleScheduler` used to serialize console work on the console scheduler thread. |
| `Progress(Func<Progress, Progress>? addProperties = null)` | Creates an `IObservable<ProgressContext>`. Configure columns, auto-clear, refresh behavior, and other progress settings in `addProperties`. |
| `Status(string status, Func<Status, Status>? addProperties = null)` | Creates an `IObservable<StatusContext>` using an initial status string. Configure spinner, style, auto-refresh, and related status settings in `addProperties`. |
| `Live(IRenderable renderable, Func<LiveDisplay, LiveDisplay>? addProperties = null)` | Creates an `IObservable<LiveDisplayContext>` for a live renderable such as a `Table`, `Panel`, `Layout`, `Canvas`, or any `IRenderable`. |
| `AddTasks(this IObservable<ProgressContext> source, Func<ProgressContext, ProgressTask[]> func)` | Extension that adds progress tasks to a progress context and emits `(context, tasks)`. |
| `Update(this LiveDisplayContext ctx, int delay, Action action)` | Executes an action, refreshes the live display, blocks for `delay` milliseconds, and returns the context for fluent animation. |
| `UpdateAsync(this LiveDisplayContext ctx, TimeSpan delay, Action action, CancellationToken ct = default)` | Async version of `Update`. Executes an action, refreshes, then delays. |
| `IsFinished(this LiveDisplayContext ctx)` | Completes a live display observable. |
| `IsFinished(this StatusContext ctx)` | Finishes the status context and completes the status observable. |
| `Complete(this ProgressContext ctx)` | Completes a progress observable. |
| `Complete(this LiveDisplayContext ctx)` | Alias for `IsFinished(LiveDisplayContext)`. |
| `Complete(this StatusContext ctx)` | Alias for `IsFinished(StatusContext)`. |
| `HasFinished(this LiveDisplayContext ctx)` | Returns whether the live display has been completed through `IsFinished` or `Complete`. |
| `HasFinished(this StatusContext ctx)` | Returns whether the status context has been completed through `IsFinished` or `Complete`. |

### `ISpectreConsoleScheduler`

`ISpectreConsoleScheduler` extends `IScheduler`, `IStopwatchProvider`,
`IServiceProvider`, and `IDisposable`.

| Member | Description |
| --- | --- |
| `IsRunning` | Indicates whether the scheduler loop is active. |
| `SynchronizationContext` | The synchronization context used by the scheduler thread. |
| `Start()` | Starts the scheduler loop. The singleton scheduler starts itself during construction. |
| `Stop()` | Stops the scheduler loop. |
| `Sleep(TimeSpan delay, CancellationToken cancellationToken = default)` | Delay helper used by context scheduling loops. |

### `SpectreConsoleScheduler`

`SpectreConsoleScheduler` is the concrete local scheduler used by
`AnsiConsoleRx.Scheduler`.

| Member | Description |
| --- | --- |
| `SpectreConsoleScheduler()` | Creates a scheduler with its own background thread and synchronization context. |
| `Instance` | Shared singleton scheduler used by `AnsiConsoleRx`. |
| `IsRunning` | Indicates scheduler activity. |
| `ThreadId` | Managed thread id of the scheduler thread. |
| `SynchronizationContext` | Scheduler synchronization context. |
| `Start()` | Runs the synchronization context message loop. |
| `Stop()` | Requests the scheduler loop to stop. |
| `Sleep(TimeSpan, CancellationToken)` | Delays using `Task.Delay`. |
| `Schedule<TState>(TState, Func<IScheduler,TState,IDisposable>)` | Schedules immediate work. If already on the scheduler thread, the work runs inline. |
| `Schedule<TState>(TState, TimeSpan, Func<IScheduler,TState,IDisposable>)` | Schedules delayed work. |
| `Dispose()` | Stops and disposes scheduler resources. |

### Context Scheduling Extensions

`ContextExtensions` provides helpers for existing live contexts. They are useful
when an imperative Spectre.Console API needs to be driven from a reactive
context.

| Member | Contexts | Description |
| --- | --- | --- |
| `Schedule(TimeSpan delay, Func<bool> isComplete, Action action)` | `ProgressContext`, `StatusContext`, `LiveDisplayContext`, `IContext` | Repeats an action, refreshes the context, then delays until `isComplete` is true. |
| `Schedule(TimeSpan delay, Action action)` | `ProgressContext`, `StatusContext`, `LiveDisplayContext`, `IContext` | Repeats an action until the context is finished. |
| `Schedule(Action<SpectreConsoleScheduler> action)` | `ProgressContext`, `StatusContext`, `LiveDisplayContext`, `IContext` | Runs a single action with the current `SpectreConsoleScheduler`. |
| `Schedule(Func<SpectreConsoleScheduler, Task> action)` | `ProgressContext`, `StatusContext`, `LiveDisplayContext`, `IContext` | Runs an asynchronous action with the current scheduler. |
| `Schedule<T>(Func<SpectreConsoleScheduler, Task<T>> action)` | `ProgressContext`, `StatusContext`, `LiveDisplayContext`, `IContext` | Runs an asynchronous action with a result. |
| `Schedule(Func<bool> isComplete, Action<SpectreConsoleScheduler> action)` | `ProgressContext`, `StatusContext`, `LiveDisplayContext`, `IContext` | Repeats scheduler-aware work until a completion predicate returns true. |

### Awaitable Boundary Helper

`ReactiveConsoleObservableExtensions.RunAsync` is provided for application
boundaries and compatibility with `async Main`. It subscribes to an observable,
runs each context callback sequentially, and completes when the observable
completes.

Prefer direct `Subscribe` plus Rx composition inside examples and libraries.
Use `RunAsync` when the host must await a reactive console pipeline.

| Member | Description |
| --- | --- |
| `RunAsync<TContext>(this IObservable<TContext> source, Action<TContext> onNext, CancellationToken ct = default)` | Await completion while running a synchronous context callback. |
| `RunAsync<TContext>(this IObservable<TContext> source, Func<TContext, Task> onNext, CancellationToken ct = default)` | Await completion while running an asynchronous context callback. |
| `RunAsync<TContext>(this IObservable<TContext> source, Func<TContext, CancellationToken, Task> onNext, CancellationToken ct = default)` | Await completion while passing cancellation into the callback. |

```csharp
await AnsiConsoleRx
    .Status("Working")
    .ObserveOn(AnsiConsoleRx.Scheduler)
    .RunAsync(ctx =>
    {
        ctx.Status("Done");
        ctx.IsFinished();
    });
```

## Spectre.Console-Compatible Surface

The core package includes a Spectre.Console-compatible API surface in the
`Spectre.Console.Rx` namespace. The reactive entry points use the same renderable
and context types.

### Console and Output

| Feature | Common API |
| --- | --- |
| Console facade | `AnsiConsole`, `AnsiConsoleSettings`, `AnsiConsoleOutput`, `IAnsiConsole`, `IAnsiConsoleInput`, `IAnsiConsoleCursor`, `IExclusivityMode` |
| Console profile | `Profile`, `ProfileEnrichment`, `Capabilities`, `IReadOnlyCapabilities`, `ColorSystemSupport`, `AnsiSupport`, `InteractionSupport` |
| Writing | `AnsiConsole.Write`, `AnsiConsole.WriteLine`, `AnsiConsole.Markup`, `AnsiConsole.MarkupLine`, `AnsiConsole.WriteException`, `AnsiConsole.Clear` |
| Rendering pipeline | `IRenderable`, `Renderable`, `RenderOptions`, `RenderPipeline`, `Segment`, `Measurement`, `JustInTimeRenderable` |

### Markup, Text, and Styling

| Feature | Common API |
| --- | --- |
| Markup | `Markup`, `MarkupParser`, `MarkupTokenizer`, `MarkupToken`, `MarkupTokenKind`, `Markup.Escape`, `EscapeMarkup`, `RemoveMarkup` |
| Text | `Text`, `Paragraph`, `TextPath`, `Emoji`, `EmojiMarkup`, `FigletText` |
| Style | `Style`, `Decoration`, `Color`, `ColorSystem`, `ColorPalette`, generated named colors, `Style.Parse`, `Style.TryParse`, `Style.Combine`, `Style.ToMarkup` |
| Links | `Link`, OSC 8 link support through ANSI writer and styled segments |

### Layout and Containers

| Feature | Common API |
| --- | --- |
| Tables | `Table`, `TableColumn`, `TableRow`, `TableBorder`, table title, caption, footer, alignment, expansion, safe borders |
| Panels and rules | `Panel`, `Rule`, borders, padding, headers, expansion |
| Layouts | `Layout`, `Grid`, `Rows`, `Columns`, `Align`, `Padder`, `Padding`, `Justify`, `VerticalOverflow`, `VerticalOverflowCropping` |
| Trees | `Tree`, `TreeNode`, tree guides and styles |
| Calendars and charts | `Calendar`, `CalendarEvent`, `BarChart`, `BarChartItem`, `BreakdownChart`, `BreakdownChartItem` |
| Canvas | `Canvas`, pixel drawing, color-aware rendering |

### Prompts

| Feature | Common API |
| --- | --- |
| Text input | `TextPrompt<T>`, converters, validators, secret input, default values |
| Selection | `SelectionPrompt<T>`, `MultiSelectionPrompt<T>`, page size, more choices text, search, converters |
| Confirmation | `ConfirmPrompt` |
| Prompt contracts | `IPrompt<T>`, prompt styles, validation results, prompt choices |

Prompts are synchronous Spectre.Console-compatible APIs. For reactive
applications, run prompt calls from the scheduler and route the result into an
observable.

```csharp
var name = await Observable
    .Start(
        () => AnsiConsole.Prompt(
            new TextPrompt<string>("Name:")
                .Validate(value => string.IsNullOrWhiteSpace(value)
                    ? ValidationResult.Error("Name is required")
                    : ValidationResult.Success())),
        AnsiConsoleRx.Scheduler);
```

### Status, Progress, and Live

| Feature | Common API |
| --- | --- |
| Status | `Status`, `StatusContext`, spinner configuration, auto-refresh, status refresh |
| Progress | `Progress`, `ProgressContext`, `ProgressTask`, `ProgressColumn`, `TaskDescriptionColumn`, `ProgressBarColumn`, `PercentageColumn`, `RemainingTimeColumn`, `SpinnerColumn`, indeterminate tasks |
| Live display | `LiveDisplay`, `LiveDisplayContext`, auto-clear, overflow, cropping, refresh |

The reactive wrappers expose these same contexts through observables.

## API Reference: Spectre.Console.Rx.ReactiveUI

`Spectre.Console.Rx.ReactiveUI` configures ReactiveUI to use the console
scheduler and provides base types for routable console applications.

### `WithSpectreConsoleRx`

```csharp
using ReactiveUI.Builder;
using Spectre.Console.Rx.ReactiveUI;

RxAppBuilder
    .CreateReactiveUIBuilder()
    .WithSpectreConsoleRx()
    .Build();
```

| Parameter | Description |
| --- | --- |
| `mainThreadScheduler` | Optional main-thread scheduler. Defaults to `AnsiConsoleRx.Scheduler`. |
| `taskPoolScheduler` | Optional task-pool scheduler. Defaults to `TaskPoolScheduler.Default`. |
| `registerPlatformServices` | When true, registers ReactiveUI platform services after core services and schedulers. |

`WithSpectreConsoleRx` calls `WithCoreServices`, sets ReactiveUI's task-pool and
main-thread schedulers, and optionally calls `WithPlatformServices`.

### `ReactiveConsoleAppViewModel`

Base application view model for routed ReactiveUI console apps.

| Member | Description |
| --- | --- |
| `ReactiveConsoleAppViewModel(RoutingState? router = null)` | Creates an app view model with a supplied router or a new `RoutingState`. |
| `Router` | The ReactiveUI routing state. |
| `RenderViewAsync<TView,TViewModel>(TViewModel? viewModel, CancellationToken ct = default)` | Instantiates a `ReactiveConsoleView<TViewModel>`, assigns `ViewModel`, and renders it. |
| `RunAsync(Func<IRoutableViewModel?, Task> renderAsync, CancellationToken ct = default)` | Runs the route loop and awaits each route render. |
| `RunAsync(Func<IRoutableViewModel?, CancellationToken, Task> renderAsync, CancellationToken ct = default)` | Runs the route loop with cancellation-aware rendering. |
| `SubscribeAsync(Func<IRoutableViewModel?, Task> renderAsync, CancellationToken ct = default)` | Alias for `RunAsync`. |
| `SubscribeAsync(Func<IRoutableViewModel?, CancellationToken, Task> renderAsync, CancellationToken ct = default)` | Alias for cancellation-aware `RunAsync`. |
| `Exit()` | Clears the navigation stack and completes the route loop. |

### `ReactiveConsoleView<TViewModel>`

Base view for ReactiveUI console screens backed by a live display.

| Member | Description |
| --- | --- |
| `ViewModel` | Strongly typed view model. Also implements `IViewFor.ViewModel`. |
| `RenderAsync(CancellationToken ct = default)` | Abstract render method implemented by each view. |
| `ReadKeys(CancellationToken ct, bool intercept = true)` | Protected helper that returns `IObservable<ConsoleKeyInfo>` from console input. |
| `RenderAsync(IRenderable renderable, Action<LiveDisplayContext> execute, Action? completed = null, Func<LiveDisplay, LiveDisplay>? configure = null, CancellationToken ct = default)` | Protected helper that renders a live display and runs a synchronous callback. |
| `RenderAsync(IRenderable renderable, Func<LiveDisplayContext, Task> executeAsync, Action? completed = null, Func<LiveDisplay, LiveDisplay>? configure = null, CancellationToken ct = default)` | Protected helper that renders a live display and runs an async callback. |

### ReactiveUI Example

```csharp
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Builder;
using Spectre.Console.Rx;
using Spectre.Console.Rx.ReactiveUI;

RxAppBuilder
    .CreateReactiveUIBuilder()
    .WithSpectreConsoleRx()
    .Build();

public sealed class AppViewModel : ReactiveConsoleAppViewModel
{
    public void NavigateToLogin() =>
        Router.Navigate.Execute(new LoginViewModel(this)).Subscribe();
}

public sealed class LoginViewModel(AppViewModel app) : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment => "login";
    public IScreen HostScreen => app;
    public ReactiveCommand<Unit, Unit> Login { get; } =
        ReactiveCommand.Create(() =>
        {
            app.Router.Navigate.Execute(new MainViewModel(app)).Subscribe();
        });
}

public sealed class MainViewModel(AppViewModel app) : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment => "main";
    public IScreen HostScreen => app;
}

public sealed class LoginView : ReactiveConsoleView<LoginViewModel>
{
    public override Task RenderAsync(CancellationToken ct = default)
    {
        var table = new Table().AddColumn("Login").AddRow("Press Enter");

        return RenderAsync(
            table,
            async ctx =>
            {
                using var keys = ReadKeys(ct)
                    .Where(key => key.Key == ConsoleKey.Enter)
                    .Take(1)
                    .Subscribe(_ =>
                    {
                        ViewModel?.Login.Execute().Subscribe();
                        ctx.IsFinished();
                    });

                while (!ctx.HasFinished() && !ct.IsCancellationRequested)
                {
                    await AnsiConsoleRx.Scheduler.Sleep(TimeSpan.FromMilliseconds(16), ct);
                }
            },
            ct: ct);
    }
}
```

## API Reference: Spectre.Console.Rx.Json

`Spectre.Console.Rx.Json` renders JSON using the core renderable pipeline.

### `JsonText`

```csharp
using Spectre.Console.Rx;
using Spectre.Console.Rx.Json;

var json = """
{
  "name": "Spectre.Console.Rx",
  "reactive": true,
  "targets": ["net8.0", "net9.0", "net10.0"]
}
""";

AnsiConsole.Write(
    new JsonText(json)
        .MemberColor(Color.Aqua)
        .StringColor(Color.Yellow)
        .NumberColor(Color.Green)
        .BooleanColor(Color.Fuchsia)
        .Indentation("  "));
```

| Member | Description |
| --- | --- |
| `JsonText(string json)` | Creates a JSON renderable from a JSON string. |
| `BracesStyle` | Style used for `{` and `}`. |
| `BracketsStyle` | Style used for `[` and `]`. |
| `MemberStyle` | Style used for object member names. |
| `ColonStyle` | Style used for `:`. |
| `CommaStyle` | Style used for `,`. |
| `StringStyle` | Style used for string values. |
| `NumberStyle` | Style used for number values. |
| `BooleanStyle` | Style used for `true` and `false`. |
| `NullStyle` | Style used for `null`. |
| `Indentation` | Indentation string. Defaults to three spaces. |
| `Parser` | Optional `IJsonParser`. Setting it clears the cached syntax tree. |

### `JsonTextExtensions`

All extension methods return the same `JsonText` instance for fluent
configuration.

| Member | Description |
| --- | --- |
| `BracesStyle(Style? style)` | Sets brace style. |
| `BracketStyle(Style? style)` | Sets bracket style. |
| `MemberStyle(Style? style)` | Sets member-name style. |
| `ColonStyle(Style? style)` | Sets colon style. |
| `CommaStyle(Style? style)` | Sets comma style. |
| `StringStyle(Style? style)` | Sets string-literal style. |
| `NumberStyle(Style? style)` | Sets number style. |
| `BooleanStyle(Style? style)` | Sets boolean style. |
| `NullStyle(Style? style)` | Sets null style. |
| `BracesColor(Color color)` | Sets brace foreground color. |
| `BracketColor(Color color)` | Sets bracket foreground color. |
| `MemberColor(Color color)` | Sets member-name foreground color. |
| `ColonColor(Color color)` | Sets colon foreground color. |
| `CommaColor(Color color)` | Sets comma foreground color. |
| `StringColor(Color color)` | Sets string foreground color. |
| `NumberColor(Color color)` | Sets number foreground color. |
| `BooleanColor(Color color)` | Sets boolean foreground color. |
| `NullColor(Color color)` | Sets null foreground color. |
| `Indentation(string indentation = "   ")` | Sets indentation. |

### JSON Parser and Syntax Surface

| Member | Description |
| --- | --- |
| `IJsonParser.Parse(string json)` | Parses JSON into `JsonSyntax`. |
| `JsonSyntax` | Base JSON syntax node. |
| `JsonObject()` | Creates a JSON object syntax node. |
| `JsonObject.Members` | Object members. |
| `JsonArray()` | Creates a JSON array syntax node. |
| `JsonArray.Items` | Array items. |
| `JsonMember(string name, JsonSyntax value)` | Creates an object member syntax node. |
| `JsonMember.Name` | Member name. |
| `JsonMember.Value` | Member value node. |
| `JsonString(string lexeme)` | Creates a string syntax node. |
| `JsonString.Lexeme` | String token text. |
| `JsonBoolean(string lexeme)` | Creates a boolean syntax node. |
| `JsonBoolean.Lexeme` | Boolean token text. |
| `JsonNull(string lexeme)` | Creates a null syntax node. |
| `JsonNull.Lexeme` | Null token text. |

The default parser, tokenizer, token reader, number syntax node, and syntax
visitor are implementation details. Applications normally use `JsonText` for
rendering and provide a custom `IJsonParser` only when they need to replace the
default parsing behavior.

## API Reference: Spectre.Console.Rx.ImageSharp

`Spectre.Console.Rx.ImageSharp` renders images through the same renderable
pipeline as tables, panels, and canvas output.

```csharp
using SixLabors.ImageSharp.Processing;
using Spectre.Console.Rx;

AnsiConsole.Write(
    new CanvasImage("logo.png")
        .MaxWidth(80)
        .BicubicResampler()
        .Mutate(ctx => ctx.Grayscale()));
```

### `CanvasImage`

| Member | Description |
| --- | --- |
| `CanvasImage(string filename)` | Loads an image from a file. |
| `CanvasImage(ReadOnlySpan<byte> data)` | Loads an image from bytes. |
| `CanvasImage(Stream data)` | Loads an image from a stream. |
| `Width` | Source image width. |
| `Height` | Source image height. |
| `MaxWidth` | Maximum render width in console cells. |
| `PixelWidth` | Obsolete compatibility property. Not used by current rendering. |
| `Resampler` | Optional ImageSharp resampler used when scaling. Defaults to bicubic. |

### `CanvasImageExtensions`

| Member | Description |
| --- | --- |
| `MaxWidth(int? maxWidth)` | Sets maximum render width. |
| `NoMaxWidth()` | Clears maximum render width. |
| `PixelWidth(int width)` | Obsolete compatibility extension. |
| `Mutate(Action<IImageProcessingContext> action)` | Mutates the underlying ImageSharp image. |
| `BicubicResampler()` | Uses `KnownResamplers.Bicubic`. |
| `BilinearResampler()` | Uses `KnownResamplers.Triangle`. |
| `NearestNeighborResampler()` | Uses `KnownResamplers.NearestNeighbor`. |

## API Reference: Spectre.Console.Rx.Ansi

`Spectre.Console.Rx.Ansi` contains low-level ANSI, style, color, and markup
building blocks. The core package references it.

### `AnsiWriter`

`AnsiWriter` writes plain text, styled text, links, cursor movement, alternate
screen commands, and erase/scroll commands.

```csharp
using Spectre.Console.Rx;

using var writer = new StringWriter();
var ansi = new AnsiWriter(writer);

ansi
    .Foreground(Color.Green)
    .Write("ready")
    .ResetStyle()
    .WriteLine();
```

| Member | Description |
| --- | --- |
| `AnsiWriter(TextWriter output)` | Creates a writer and detects capabilities from the output. |
| `AnsiWriter(TextWriter output, AnsiCapabilities capabilities)` | Creates a writer with explicit capabilities. |
| `Capabilities` | ANSI, link, alternate-buffer, and color-system capabilities. |
| `Write(string text)` | Writes text. |
| `Write(int value)` | Writes an integer. |
| `Write(string text, Style style, Link? link = null)` | Writes styled text and optional OSC 8 link. |
| `WriteLine()` | Writes a blank line. |
| `WriteLine(string text)` | Writes text and a newline. |
| `WriteLine(string text, Style style, Link? link = null)` | Writes styled text and a newline. |
| `Style(Style style, Link? link = null)` | Emits SGR style codes and optional link start. |
| `ResetStyle()` | Resets style and closes an active link. |
| `Decoration(Decoration decoration)` | Emits decoration codes. |
| `Background(Color color)` | Sets background color. |
| `Foreground(Color color)` | Sets foreground color. |
| `BeginLink(Link link)` | Starts an OSC 8 link. |
| `BeginLink(string link, int? linkId = null)` | Starts an OSC 8 link with URL and optional id. |
| `EndLink()` | Ends an OSC 8 link. |
| `CursorPosition(int row, int column)` | Moves cursor to row and column. |
| `CursorHome()` | Moves cursor to the top-left position. |
| `CursorUp(int steps)` | Moves cursor up. |
| `CursorDown(int steps)` | Moves cursor down. |
| `CursorRight(int steps)` | Moves cursor right. |
| `CursorForward(int steps)` | Alias for moving right. |
| `CursorLeft(int steps)` | Moves cursor left. |
| `CursorBackward(int steps)` | Alias for moving left. |
| `ShowCursor()` | Shows the cursor. |
| `HideCursor()` | Hides the cursor. |
| `SaveCursor(bool stayOnPage = true)` | Saves cursor position. |
| `RestoreCursor(bool stayOnPage = true)` | Restores cursor position. |
| `CursorHorizontalAbsolute(int position)` | Moves to an absolute horizontal position. |
| `EnterAltScreen()` | Enters alternate screen buffer. |
| `ExitAltScreen()` | Exits alternate screen buffer. |
| `EraseInLine(int mode = 0)` | Erases part or all of the current line. |
| `EraseInDisplay(int mode = 0)` | Erases part or all of the display. |
| `ClearScrollback()` | Clears scrollback using display erase mode 3. |
| `CursorBackwardTabulation(int tabs = 1)` | Moves backward by tab stops. |
| `CursorHorizontalTabulation(int tabs = 1)` | Moves forward by tab stops. |
| `CursorNextLine(int lines = 1)` | Moves to following line. |
| `CursorPreviousLine(int lines = 1)` | Moves to previous line. |
| `Index()` | Moves cursor down one line, scrolling if needed. |
| `ReverseIndex()` | Moves cursor up one line, scrolling if needed. |
| `DeleteCharacter(int characters = 1)` | Deletes characters. |
| `SetCursorStyle(int style = 0)` | Sets terminal cursor style. |
| `DeleteLine(int lines = 0)` | Deletes lines. |
| `EraseCharacter(int characters = 1)` | Erases characters. |
| `InsertCharacter(int characters = 1)` | Inserts blank characters. |
| `InsertLine(int lines = 1)` | Inserts blank lines. |
| `ScrollDown(int lines = 1)` | Scrolls the display down. |
| `ScrollUp(int lines = 1)` | Scrolls the display up. |

### ANSI Support Types

| Member | Description |
| --- | --- |
| `AnsiWriterSettings.Ansi` | Desired ANSI support: `Detect`, `Yes`, or `No`. |
| `AnsiWriterSettings.ColorSystem` | Desired color system support. |
| `AnsiCapabilities.ColorSystem` | Active color system. |
| `AnsiCapabilities.Ansi` | Whether ANSI escape sequences are supported. |
| `AnsiCapabilities.Links` | Whether OSC 8 links are supported. |
| `AnsiCapabilities.AlternateBuffer` | Whether alternate screen buffer is supported. |
| `AnsiCapabilities.Create(TextWriter writer)` | Detects capabilities for a writer. |
| `AnsiCapabilities.Create(TextWriter writer, AnsiWriterSettings settings)` | Creates capabilities from a writer and settings. |
| `IReadOnlyAnsiCapabilities` | Read-only capability contract. |
| `AnsiSupport` | `Detect`, `Yes`, `No`. |
| `ColorSystem` | `NoColors`, `Legacy`, `Standard`, `EightBit`, `TrueColor`. |
| `ColorSystemSupport` | Capability configuration for color detection. |

### Color, Style, Markup, and Link Types

| Member | Description |
| --- | --- |
| `Color(byte red, byte green, byte blue)` | Creates a true-color value. |
| `Color.Default` | Default terminal color. |
| `Color.R`, `Color.G`, `Color.B` | RGB channels. |
| `Color.Blend(Color other, float factor)` | Blends two colors. |
| `Color.ToHex()` | Returns six-character hexadecimal RGB. |
| `Color.ExactOrClosest(ColorSystem system)` | Maps a color to the nearest supported color system value. |
| `Color.FromInt32(int number)` | Gets an ANSI palette color by number. |
| `Color.FromHex(string hex)` | Parses `#RRGGBB` or `#RGB`. |
| `Color.TryFromHex(string hex, out Color color)` | Tries to parse a hex color. |
| `Color.FromName(string name)` | Looks up a generated named color. |
| `Color.FromConsoleColor(ConsoleColor color)` | Converts from `ConsoleColor`. |
| `Color.ToConsoleColor(Color color)` | Converts to closest `ConsoleColor`. |
| `Color.ToMarkup()` | Converts to Spectre markup color syntax. |
| `Style()` | Creates a plain style. |
| `Style(Color? foreground = null, Color? background = null, Decoration? decoration = null)` | Creates a style. |
| `Style.Plain` | Default style with no foreground, background, or decoration. |
| `Style.Foreground`, `Style.Background`, `Style.Decoration` | Style values. |
| `Style.Combine(Style other)` | Combines styles. |
| `Style.Parse(string text)` | Parses markup style text. |
| `Style.TryParse(string text, out Style result)` | Tries to parse markup style text. |
| `Style.ToMarkup()` | Converts style back to markup text. |
| `StyleExtensions.Foreground(Color color)` | Creates a copy with a new foreground. |
| `StyleExtensions.Background(Color color)` | Creates a copy with a new background. |
| `StyleExtensions.Decoration(Decoration decoration)` | Creates a copy with a new decoration. |
| `Decoration` | Flags for bold, dim, italic, underline, blink, invert, conceal, strikethrough, and none. |
| `Link(string url)` | Represents an OSC 8 link. |
| `Link.Id` | Random link id. |
| `Link.Url` | Link URL. |
| `AnsiMarkupTagParser.Parse(string text)` | Parses a markup tag into style and optional link. |
| `AnsiMarkupTagParser.TryParse(...)` | Tries to parse a markup tag. |
| `AnsiMarkupHighlighter.Highlight(string markup, string query, Style style)` | Highlights query text inside markup. |
| `EscapeMarkup(this string? text)` | Escapes markup syntax. |
| `RemoveMarkup(this string? text)` | Removes markup syntax. |

## API Reference: Spectre.Console.Rx.Testing

Use `Spectre.Console.Rx.Testing` to test console output without relying on a
real terminal.

```csharp
using Spectre.Console.Rx;
using Spectre.Console.Rx.Testing;
using Shouldly;

using var console = new TestConsole()
    .Width(100)
    .SupportsAnsi(false)
    .SupportsUnicode(true);

var original = AnsiConsole.Console;
AnsiConsole.Console = console;

try
{
    AnsiConsole.MarkupLine("[green]Hello[/]");
    console.Output.ShouldContain("Hello");
}
finally
{
    AnsiConsole.Console = original;
}
```

### `TestConsole`

| Member | Description |
| --- | --- |
| `TestConsole()` | Creates a fake ANSI console with output captured in a `StringWriter`. |
| `Profile` | Console profile. |
| `ExclusivityMode` | No-op exclusivity mode. |
| `Input` | `TestConsoleInput` used to queue keys and text. |
| `Pipeline` | Render pipeline from the underlying console. |
| `Cursor` | No-op cursor unless ANSI sequence emission is enabled. |
| `Output` | Complete captured output. |
| `Lines` | Captured output split into normalized lines. |
| `EmitAnsiSequences` | When true, writes ANSI sequences. When false, control codes are stripped. |
| `Clear(bool home)` | Clears the console. |
| `Write(IRenderable renderable)` | Writes a renderable to the captured output. |
| `WriteAnsi(Action<AnsiWriter> action)` | Writes ANSI through `AnsiWriter`. |
| `Dispose()` | Disposes the captured output writer. |

### `TestConsoleInput`

| Member | Description |
| --- | --- |
| `PushText(string input)` | Queues every character in a string. |
| `PushTextWithEnter(string input)` | Queues text and then `ConsoleKey.Enter`. |
| `PushCharacter(char input)` | Queues a single character. |
| `PushKey(ConsoleKey input)` | Queues a key. |
| `PushKey(ConsoleKeyInfo consoleKeyInfo)` | Queues a full key info value. |
| `IsKeyAvailable()` | Returns whether queued input is available. |
| `ReadKey(bool intercept)` | Reads the next queued key or throws if no input is available. |
| `ReadKeyAsync(bool intercept, CancellationToken cancellationToken)` | Waits for queued input until cancellation. |

### Test Console Extensions

| Member | Description |
| --- | --- |
| `Colors(ColorSystem colors)` | Sets color system capability. |
| `SupportsAnsi(bool enable)` | Enables or disables ANSI capability. |
| `SupportsUnicode(bool enable)` | Enables or disables Unicode capability. |
| `Interactive()` | Marks the console as interactive. |
| `Width(int width)` | Sets console width. |
| `Height(int width)` | Sets console height. |
| `Size(Size size)` | Sets console width and height. |
| `EmitAnsiSequences()` | Enables ANSI sequence output and restores the real cursor implementation. |

### `TestCapabilities`

| Member | Description |
| --- | --- |
| `ColorSystem` | Fake color system support. |
| `Ansi` | Fake ANSI support. |
| `Links` | Fake OSC 8 link support. |
| `Legacy` | Fake legacy terminal support. |
| `Interactive` | Fake interactivity support. |
| `Unicode` | Fake Unicode support. |
| `AlternateBuffer` | Fake alternate buffer support. |
| `CreateRenderContext(IAnsiConsole console)` | Creates `RenderOptions` from the fake capabilities. |

## Source Generator

`Spectre.Console.Rx.SourceGenerator` is used at build time by the core and ANSI
projects. It generates strongly typed console data from solution inputs, such as
named colors, emoji, and spinner definitions. Consumers normally do not call it
directly.

## Examples

| Project | What it demonstrates |
| --- | --- |
| `StatusExample` | Observable status spinner updates, scheduled delays, log output, and `ctx.IsFinished()`. |
| `LiveExample` | Fluent live display animation using `LiveDisplayContext.Update`. |
| `LiveTableExample` | Reactive streaming table updates with cancellation and `Observable.Interval`. |
| `ProgressExample` | Reactive progress tasks, progress columns, indeterminate tasks, and observable completion. |
| `ProgressThreadExample` | Progress updates where background work reports into the console scheduler. |
| `CombinedExample` | Sequencing status, live display, and progress demos with `Observable.Concat`. |
| `ReactiveUIConsoleExample` | ReactiveUI initialization through `WithSpectreConsoleRx`, routed view models, reactive key handling, login navigation, and clean exit. |
| `UsingSpectreConsole` | Spectre.Console-compatible rendering examples, including JSON output. |
| `BenchmarkSuite1` | BenchmarkDotNet benchmarks for performance-sensitive console operations. |

Run an example:

```bash
dotnet run --project src/StatusExample/StatusExample.csproj
dotnet run --project src/LiveTableExample/LiveTableExample.csproj
dotnet run --project src/ReactiveUIConsoleExample/ReactiveUIConsoleExample.csproj
```

Some examples support non-interactive smoke tests:

```bash
dotnet run --project src/ReactiveUIConsoleExample/ReactiveUIConsoleExample.csproj -- --smoke-test
```

## Testing

The solution uses TUnit and Microsoft Testing Platform compatible test projects.

```bash
dotnet test src/Spectre.Console.Rx.slnx
```

Run a no-build test pass after building:

```bash
dotnet test src/Spectre.Console.Rx.slnx --no-build --no-restore
```

Testing packages and helpers:

- `TUnit` drives the test framework.
- `Verify.TUnit` is used for verified output where appropriate.
- `Shouldly` is used for readable assertions.
- `Spectre.Console.Rx.Testing` captures console output and queues fake input.

## Build

Build the full solution:

```bash
dotnet build src/Spectre.Console.Rx.slnx
```

Build with minimal output:

```bash
dotnet build src/Spectre.Console.Rx.slnx --no-restore -v:minimal
```

## Best Practices

- Compose console flows with observables and `Subscribe`.
- Use `ObserveOn(AnsiConsoleRx.Scheduler)` before mutating console contexts.
- Use `Observable.Timer`, `Observable.Interval`, `Concat`, `SelectMany`, and `Finally` instead of manual task loops where practical.
- Call `ctx.IsFinished()` for status and live displays.
- Call `ctx.Complete()` for progress contexts.
- Run one live console context at a time. Use `Observable.Concat` for sequences.
- Use `RunAsync` only at an application boundary that must await an observable.
- Keep prompt usage on the scheduler and adapt the prompt result back into an observable.
- Use `TestConsole` for deterministic tests instead of reading from or writing to the real terminal.
- Use the ReactiveUI package for routed console apps rather than configuring `RxApp` manually.

## Troubleshooting

### The app never exits

Make sure the active context is completed:

```csharp
ctx.IsFinished(); // status and live display
ctx.Complete();   // progress
```

### The console flickers or key presses are delayed

Make sure all console state changes are observed on the console scheduler:

```csharp
.ObserveOn(AnsiConsoleRx.Scheduler)
```

### Two live operations interfere with each other

Run them sequentially:

```csharp
Observable.Concat(firstLiveOperation, secondLiveOperation)
```

### ReactiveUI does not initialize

Configure ReactiveUI before creating routed view models:

```csharp
RxAppBuilder
    .CreateReactiveUIBuilder()
    .WithSpectreConsoleRx()
    .Build();
```

### Non-interactive CI cannot run a console example

Use smoke-test flags where available, or test rendering through
`Spectre.Console.Rx.Testing.TestConsole`.

## License

This project is licensed under the MIT license. See `LICENSE` in the project
root for details.
