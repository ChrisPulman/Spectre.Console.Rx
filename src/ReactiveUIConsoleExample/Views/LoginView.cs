// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using ReactiveUIConsoleExample.ViewModels;
using Spectre.Console.Rx;
using Spectre.Console.Rx.ReactiveUI;

namespace ReactiveUIConsoleExample.Views;

/// <summary>
/// Console login view using Spectre.Console and ReactiveUI, fully reactive.
/// </summary>
public sealed class LoginView : ReactiveConsoleView<LoginViewModel>
{
    private enum LoginResult
    {
        Cancel,
        Exit,
        Submit
    }

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
        var user = ViewModel.UserName;
        var pass = ViewModel.Password;
        var focusIndex = 0; // 0 = user, 1 = pass
        var done = false;
        var result = LoginResult.Cancel;

        static string Mask(string s) => new('*', s.Length);
        string FocusLabel(string label, int row) => row == focusIndex ? $"[yellow]{label}[/]" : label;

        await RenderAsync(
            table,
            async ctx =>
            {
                // Initial build (animated)
                ctx
                    .Update(100, () => table.AddColumn("Field"))
                    .Update(80, () => table.AddColumn("Value"))
                    .Update(60, () => table.AddRow(new Markup(FocusLabel("User", 0)), new Markup(user)))
                    .Update(60, () => table.AddRow(new Markup(FocusLabel("Pass", 1)), new Markup(Mask(pass))))
                    .Update(40, () => table.AddRow(Text.Empty, Text.Empty))
                    .Update(40, () => table.AddRow(new Markup(string.Empty), new Markup("[grey]Enter submit - Tab switch - Esc cancel[/]")));

                var completion = new TaskCompletionSource<LoginResult>(TaskCreationOptions.RunContinuationsAsynchronously);
                void Complete(LoginResult value)
                {
                    if (done)
                    {
                        return;
                    }

                    done = true;
                    completion.TrySetResult(value);
                    exitCts.Cancel();
                }

                using var cancellation = exitCts.Token.Register(() => completion.TrySetResult(LoginResult.Cancel));
                using var keySubscription = ReadKeys(exitCts.Token).Subscribe(
                    key =>
                    {
                        if (done)
                        {
                            return;
                        }

                        if (key.Key == ConsoleKey.Escape)
                        {
                            Complete(LoginResult.Exit);
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

                            if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass))
                            {
                                Complete(LoginResult.Submit);
                            }

                            return;
                        }

                        if (key.Key == ConsoleKey.Backspace)
                        {
                            if (focusIndex == 0 && user.Length > 0)
                            {
                                user = user.Substring(0, user.Length - 1);
                                ViewModel.UserName = user;
                                table.Rows.Update(0, 1, new Markup(user));
                            }
                            else if (focusIndex == 1 && pass.Length > 0)
                            {
                                pass = pass.Substring(0, pass.Length - 1);
                                ViewModel.Password = pass;
                                table.Rows.Update(1, 1, new Markup(Mask(pass)));
                            }

                            ctx.Refresh();
                            return;
                        }

                        var ch = key.KeyChar;
                        if (!char.IsControl(ch))
                        {
                            if (focusIndex == 0)
                            {
                                user += ch;
                                ViewModel.UserName = user;
                                table.Rows.Update(0, 1, new Markup(user));
                            }
                            else
                            {
                                pass += ch;
                                ViewModel.Password = pass;
                                table.Rows.Update(1, 1, new Markup(Mask(pass)));
                            }

                            ctx.Refresh();
                        }
                    },
                    _ => completion.TrySetResult(LoginResult.Cancel),
                    () => completion.TrySetResult(LoginResult.Cancel));

                result = await completion.Task.ConfigureAwait(false);
                ctx.IsFinished();
            },
            ct: exitCts.Token);

        switch (result)
        {
            case LoginResult.Exit:
                ViewModel.HostScreen.Router.NavigationStack.Clear();
                break;
            case LoginResult.Submit when !ct.IsCancellationRequested:
                await ViewModel.SubmitAsync(user, pass).ConfigureAwait(false);
                break;
        }
    }
}
