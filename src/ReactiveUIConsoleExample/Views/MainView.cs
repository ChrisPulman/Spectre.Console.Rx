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
public sealed class MainView : ReactiveConsoleView<MainViewModel>
{
    /// <summary>
    /// Render the main view reactively.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task that completes when the view finishes rendering.</returns>
    public override async Task RenderAsync(CancellationToken ct = default)
    {
        if (ViewModel is null)
        {
            return;
        }

        var table = new Table().Expand().BorderColor(Color.Grey78);
        using var exitCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        IDisposable? clockSub = null;

        // Start key loop on background thread to request exit
        var keyTask = Task.Run(() =>
        {
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
        });

        await RenderAsync(
            table,
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
            configure: ld => ld.AutoClear(false).Overflow(VerticalOverflow.Ellipsis).Cropping(VerticalOverflowCropping.Top),
            ct: exitCts.Token);

        // Ensure key loop completes
        try
        {
            await keyTask.ConfigureAwait(false);
        }
        catch
        {
            // ignored
        }
    }
}
