// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Threading.Tasks;
using ReactiveUI;
using ReactiveUIConsoleExample.ViewModels;
using Spectre.Console.Rx;

namespace ReactiveUIConsoleExample.Views;

/// <summary>
/// Console login view using Spectre.Console and ReactiveUI.
/// </summary>
public sealed class LoginView : IViewFor<LoginViewModel>
{
    /// <summary>
    /// Gets or sets the ViewModel corresponding to this specific View. This should be
    /// a DependencyProperty if you're using XAML.
    /// </summary>
    object? IViewFor.ViewModel { get => ViewModel; set => ViewModel = (LoginViewModel?)value; }

    /// <summary>
    /// Gets or sets the ViewModel corresponding to this specific View. This should be
    /// a DependencyProperty if you're using XAML.
    /// </summary>
    public LoginViewModel? ViewModel { get; set; }

    /// <summary>
    /// Renders the asynchronous.
    /// </summary>
    /// <param name="ct">The ct.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task RenderAsync(CancellationToken ct = default)
    {
        if (ViewModel is null)
        {
            return;
        }

        var table = new Table().NoBorder();
        table.AddColumn(string.Empty).AddColumn(string.Empty);
        table.AddRow("User:", string.Empty);
        table.AddRow("Pass:", string.Empty);

        var panel = new Panel(table)
            .Header("Login")
            .HeaderAlignment(Justify.Center)
            .SquareBorder()
            .BorderColor(Color.CadetBlue)
            .Padding(1, 1, 1, 0); // add padding so prompts do not look outside

        AnsiConsole.Write(panel);

        // Move cursor inside panel area under the table rows and prompt there
        AnsiConsole.WriteLine();
        AnsiConsole.Markup("[grey]Enter credentials below[/]\n");

        ViewModel.UserName = AnsiConsole.Prompt(new TextPrompt<string>("[green]User[/]: "));
        ViewModel.Password = AnsiConsole.Prompt(new TextPrompt<string>("[green]Pass[/]: ").Secret('*'));

        await ViewModel.Login.Execute().ToTask();
    }
}
