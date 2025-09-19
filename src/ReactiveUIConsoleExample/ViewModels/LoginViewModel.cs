// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace ReactiveUIConsoleExample.ViewModels;

/// <summary>
/// LoginViewModel.
/// </summary>
/// <seealso cref="ReactiveUI.ReactiveObject" />
/// <seealso cref="ReactiveUI.IRoutableViewModel" />
public sealed class LoginViewModel : ReactiveObject, IRoutableViewModel
{
    private string _userName = string.Empty;
    private string _password = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
    /// </summary>
    /// <param name="host">The host.</param>
    public LoginViewModel(IScreen host)
    {
        HostScreen = host;

        var canLogin = this.WhenAnyValue(x => x.UserName, x => x.Password, (u, p) => !string.IsNullOrWhiteSpace(u) && !string.IsNullOrWhiteSpace(p));

        Login = ReactiveCommand.CreateFromTask(
            async () =>
        {
            // Simulate auth delay on background thread then navigate
            await Task.Delay(400);
            await HostScreen.Router.Navigate.Execute(new MainViewModel(HostScreen, UserName));
        },
            canLogin);

        Exit = ReactiveCommand.Create(() => HostScreen.Router.NavigateBack.Execute().Subscribe());
    }

    /// <summary>
    /// Gets a string token representing the current ViewModel, such as 'login' or 'user'.
    /// </summary>
    public string UrlPathSegment => "login";

    /// <summary>
    /// Gets the IScreen that this ViewModel is currently being shown in. This
    /// is usually passed into the ViewModel in the Constructor and saved
    /// as a ReadOnly Property.
    /// </summary>
    public IScreen HostScreen { get; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>
    /// The name of the user.
    /// </value>
    public string UserName
    {
        get => _userName;
        set => this.RaiseAndSetIfChanged(ref _userName, value);
    }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>
    /// The password.
    /// </value>
    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    /// <summary>
    /// Gets the login.
    /// </summary>
    /// <value>
    /// The login.
    /// </value>
    public ReactiveCommand<Unit, Unit> Login { get; }

    /// <summary>
    /// Gets the exit.
    /// </summary>
    /// <value>
    /// The exit.
    /// </value>
    public ReactiveCommand<Unit, IDisposable> Exit { get; }
}
