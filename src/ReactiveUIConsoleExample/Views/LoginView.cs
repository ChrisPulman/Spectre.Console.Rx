// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using ReactiveUI;
using ReactiveUIConsoleExample.ViewModels;
using Spectre.Console.Rx;

namespace ReactiveUIConsoleExample.Views;

/// <summary>
/// Console login view using Spectre.Console and ReactiveUI, fully reactive.
/// </summary>
public sealed class LoginView : ReactiveConsoleView<LoginViewModel>
{
    /// <summary>
    /// Render the login view and handle user input reactively.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task that completes when the view finishes rendering.</returns>
    public override async Task RenderAsync(CancellationToken ct = default)
    {
        if (ViewModel is null)
        {
            return;
        }

        var table = new Table().Expand().BorderColor(Color.CadetBlue);
        using var exitCts = CancellationTokenSource.CreateLinkedTokenSource(ct);

        // State
        var user = string.Empty;
        var pass = string.Empty;
        var focusIndex = 0; // 0 = user, 1 = pass
        var done = false;

        static string Mask(string s) => new('*', s.Length);
        string FocusLabel(string label, int row) => row == focusIndex ? $"[yellow]{label}[/]" : label;

        await RenderAsync(
            table,
            ctx =>
            {
                // Initial build (animated)
                ctx
                    .Update(100, () => table.AddColumn("Field"))
                    .Update(80, () => table.AddColumn("Value"))
                    .Update(60, () => table.AddRow(new Markup(FocusLabel("User", 0)), new Markup(user)))
                    .Update(60, () => table.AddRow(new Markup(FocusLabel("Pass", 1)), new Markup(Mask(pass))))
                    .Update(40, () => table.AddRow(Text.Empty, Text.Empty))
                    .Update(40, () => table.AddRow(new Markup(string.Empty), new Markup("[grey]Enter submit • Tab switch • Esc cancel[/]")));

                // Background key stream -> UI scheduler
                var keyStream = Observable.Create<ConsoleKeyInfo>(async obs =>
                {
                    try
                    {
                        while (!exitCts.IsCancellationRequested)
                        {
                            var key = await AnsiConsole.Console.Input.ReadKeyAsync(true, exitCts.Token).ConfigureAwait(false);
                            if (key.HasValue)
                            {
                                obs.OnNext(key.Value);
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // ignore
                    }
                    finally
                    {
                        obs.OnCompleted();
                    }
                })
                .SubscribeOn(System.Reactive.Concurrency.TaskPoolScheduler.Default)
                .ObserveOn(AnsiConsoleRx.Scheduler);

                keyStream.Subscribe(key =>
                {
                    if (done)
                    {
                        return;
                    }

                    if (key.Key == ConsoleKey.Escape)
                    {
                        done = true;
                        exitCts.Cancel();
                        return;
                    }

                    if (key.Key == ConsoleKey.Tab)
                    {
                        focusIndex = (focusIndex + 1) % 2;
                        table.Rows.Update(0, 0, new Markup(FocusLabel("User", 0)));
                        table.Rows.Update(1, 0, new Markup(FocusLabel("Pass", 1)));
                        ctx.Refresh();
                        return;
                    }

                    if (key.Key == ConsoleKey.Enter)
                    {
                        if (focusIndex == 0)
                        {
                            focusIndex = 1;
                            table.Rows.Update(0, 0, new Markup(FocusLabel("User", 0)));
                            table.Rows.Update(1, 0, new Markup(FocusLabel("Pass", 1)));
                            ctx.Refresh();
                            return;
                        }

                        // submit
                        done = true;
                        exitCts.Cancel();
                        return;
                    }

                    if (key.Key == ConsoleKey.Backspace)
                    {
                        if (focusIndex == 0 && user.Length > 0)
                        {
                            user = user.Substring(0, user.Length - 1);
                            table.Rows.Update(0, 1, new Markup(user));
                        }
                        else if (focusIndex == 1 && pass.Length > 0)
                        {
                            pass = pass.Substring(0, pass.Length - 1);
                            table.Rows.Update(1, 1, new Markup(Mask(pass)));
                        }

                        ctx.Refresh();
                        return;
                    }

                    // Normal character input
                    var ch = key.KeyChar;
                    if (!char.IsControl(ch))
                    {
                        if (focusIndex == 0)
                        {
                            user += ch;
                            table.Rows.Update(0, 1, new Markup(user));
                        }
                        else
                        {
                            pass += ch;
                            table.Rows.Update(1, 1, new Markup(Mask(pass)));
                        }

                        ctx.Refresh();
                    }
                });
            },
            ct: exitCts.Token);

        // If user filled both fields (or pressed Enter on second), navigate
        if (!ct.IsCancellationRequested && !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass))
        {
            ViewModel.UserName = user;
            ViewModel.Password = pass;
            await ViewModel.Login.Execute().ToTask().ConfigureAwait(false);
        }
    }
}
