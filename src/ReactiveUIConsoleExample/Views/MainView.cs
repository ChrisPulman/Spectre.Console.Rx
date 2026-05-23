// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUIConsoleExample.ViewModels;
using Spectre.Console.Rx;
using Spectre.Console.Rx.ReactiveUI;

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
        var shouldExit = false;

        await RenderAsync(
            table,
            async ctx =>
            {
                // Initial build (animated)
                ctx
                    .Update(120, () => table.AddColumn("Main"))
                    .Update(80, () => table.AddRow(new Markup($"Welcome, [yellow]{ViewModel.UserName}[/]")))
                    .Update(80, () => table.AddRow(new Markup($"Time: [aqua]{ViewModel.Now:HH:mm:ss}[/]")))
                    .Update(40, () => table.AddRow(Text.Empty))
                    .Update(40, () => table.AddRow(new Markup("Press [red]Esc[/] to exit.")));

                var completion = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);
                void CompleteExit()
                {
                    shouldExit = true;
                    completion.TrySetResult(null);
                    exitCts.Cancel();
                }

                using var cancellation = exitCts.Token.Register(() => completion.TrySetResult(null));
                using var clockSub = Observable.Interval(TimeSpan.FromSeconds(1))
                    .ObserveOn(RxSchedulers.MainThreadScheduler)
                    .Subscribe(_ =>
                    {
                        table.Rows.Update(1, 0, new Markup($"Time: [aqua]{ViewModel.Now:HH:mm:ss}[/]"));
                        ctx.Refresh();
                    });
                using var keySub = ReadKeys(exitCts.Token).Subscribe(
                    key =>
                    {
                        if (key.Key == ConsoleKey.Escape)
                        {
                            CompleteExit();
                        }
                    },
                    _ => completion.TrySetResult(null),
                    () => completion.TrySetResult(null));

                await completion.Task.ConfigureAwait(false);
                ctx.IsFinished();
            },
            configure: ld => ld.AutoClear(false).Overflow(VerticalOverflow.Ellipsis).Cropping(VerticalOverflowCropping.Top),
            ct: exitCts.Token);

        if (shouldExit)
        {
            ViewModel.ExitApplication();
        }
    }
}
