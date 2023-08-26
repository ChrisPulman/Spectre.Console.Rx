// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a status display.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Status"/> class.
/// </remarks>
/// <param name="console">The console.</param>
public sealed class Status(IAnsiConsole console)
{
    private readonly IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));

    /// <summary>
    /// Gets or sets the spinner.
    /// </summary>
    public Spinner? Spinner { get; set; }

    /// <summary>
    /// Gets or sets the spinner style.
    /// </summary>
    public Style? SpinnerStyle { get; set; } = Color.Yellow;

    /// <summary>
    /// Gets or sets a value indicating whether or not status
    /// should auto refresh. Defaults to <c>true</c>.
    /// </summary>
    public bool AutoRefresh { get; set; } = true;

    /// <summary>
    /// Starts a new status display.
    /// </summary>
    /// <param name="status">The status to display.</param>
    /// <param name="action">The action to execute.</param>
    public void Start(string status, Action<StatusContext> action)
    {
        var task = StartAsync(status, ctx =>
        {
            action(ctx);
            return Task.CompletedTask;
        });

        task.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Starts a new status display.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="status">The status to display.</param>
    /// <param name="func">The action to execute.</param>
    /// <returns>The result.</returns>
    public T Start<T>(string status, Func<StatusContext, T> func)
    {
        var task = StartAsync(status, ctx => Task.FromResult(func(ctx)));
        return task.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Starts a new status display.
    /// </summary>
    /// <param name="status">The status to display.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task StartAsync(string status, Func<StatusContext, Task> action)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        _ = await StartAsync<object?>(status, async statusContext =>
        {
            await action(statusContext).ConfigureAwait(false);
            return default;
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Starts a new status display and returns a result.
    /// </summary>
    /// <typeparam name="T">The result type of task.</typeparam>
    /// <param name="status">The status to display.</param>
    /// <param name="func">The action to execute.</param>
    /// <returns>A <see cref="Task{T}"/> representing the asynchronous operation.</returns>
    public async Task<T> StartAsync<T>(string status, Func<StatusContext, Task<T>> func)
    {
        if (func is null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        // Set the progress columns
        var spinnerColumn = new SpinnerColumn(Spinner ?? Spinner.Known.Default)
        {
            Style = SpinnerStyle ?? Style.Plain,
        };

        var progress = new Progress(_console)
        {
            FallbackRenderer = new FallbackStatusRenderer(),
            AutoClear = true,
            AutoRefresh = AutoRefresh,
        };

        progress.Columns(new ProgressColumn[]
        {
                spinnerColumn,
                new TaskDescriptionColumn(),
        });

        return await progress.StartAsync(async ctx =>
        {
            var statusContext = new StatusContext(ctx, ctx.AddTask(status), spinnerColumn);
            return await func(statusContext).ConfigureAwait(false);
        }).ConfigureAwait(false);
    }
}
