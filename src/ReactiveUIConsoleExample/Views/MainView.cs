// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUIConsoleExample.ViewModels;
using Spectre.Console;
using Spectre.Console.Rx;

namespace ReactiveUIConsoleExample.Views;

/// <summary>
/// MainView.
/// </summary>
public sealed class MainView : IViewFor<MainViewModel>
{
    /// <summary>
    /// Gets or sets the ViewModel corresponding to this specific View. This should be
    /// a DependencyProperty if you're using XAML.
    /// </summary>
    object? IViewFor.ViewModel { get => ViewModel; set => ViewModel = (MainViewModel?)value; }

    /// <summary>
    /// Gets or sets the ViewModel corresponding to this specific View. This should be
    /// a DependencyProperty if you're using XAML.
    /// </summary>
    public MainViewModel? ViewModel { get; set; }

    /// <summary>
    /// Renders the asynchronous.
    /// </summary>
    /// <param name="ct">The ct.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<int> RenderAsync(CancellationToken ct = default)
    {
        if (ViewModel is null)
        {
            return 0;
        }

        // Build reactive table UI
        var table = new Table().Expand().BorderColor(Color.Grey78);

        var completion = new TaskCompletionSource<int>();
        using var exitCts = CancellationTokenSource.CreateLinkedTokenSource(ct);

        IDisposable? clockSub = null;

        AnsiConsoleRx
            .Live(table, ld => ld.AutoClear(false).Overflow(VerticalOverflow.Ellipsis).Cropping(VerticalOverflowCropping.Top))
            .ObserveOn(AnsiConsoleRx.Scheduler)
            .Subscribe(
            ctx =>
            {
                // Initial build (animated)
                ctx
                    .Update(120, () => table.AddColumn("Main"))
                    .Update(80, () => table.AddRow(new Markup($"Welcome, [yellow]{ViewModel.UserName}[/]")))
                    .Update(80, () => table.AddRow(new Markup($"Time: [aqua]{ViewModel.Now:HH:mm:ss}[/]")))
                    .Update(40, () => table.AddRow(Text.Empty))
                    .Update(40, () => table.AddRow(new Markup("Press [red]Esc[/] to exit.")));

                // Reactive time updates (update row index 1, column 0)
                clockSub = Observable.Interval(TimeSpan.FromSeconds(1))
                    .ObserveOn(AnsiConsoleRx.Scheduler)
                    .Subscribe(_ =>
                    {
                        table.Rows.Update(1, 0, new Markup($"Time: [aqua]{ViewModel.Now:HH:mm:ss}[/]"));
                        ctx.Refresh();
                    });

                // Stop updates and finish when exit requested
                exitCts.Token.Register(() =>
                {
                    clockSub?.Dispose();
                    ctx.IsFinished();
                });
            },
            ex => completion.TrySetException(ex),
            () => completion.TrySetResult(0));

        // Key loop for exit
        while (!exitCts.IsCancellationRequested)
        {
            var key = System.Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
            {
                ViewModel.Exit.Execute().Subscribe();
                exitCts.Cancel();
                break;
            }
        }

        return await completion.Task.ConfigureAwait(false);
    }
}
