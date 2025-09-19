// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace ReactiveUIConsoleExample.ViewModels;

/// <summary>
/// MainViewModel.
/// </summary>
/// <seealso cref="ReactiveUI.ReactiveObject" />
/// <seealso cref="ReactiveUI.IRoutableViewModel" />
public sealed class MainViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly ObservableAsPropertyHelper<DateTime> _now;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    /// <param name="host">The host.</param>
    /// <param name="userName">Name of the user.</param>
    public MainViewModel(IScreen host, string userName)
    {
        HostScreen = host;
        UserName = userName;

        // Dynamic element: a ticking clock
        _now = Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(_ => DateTime.Now)
            .StartWith(DateTime.Now)
            .ToProperty(this, vm => vm.Now);

        Exit = ReactiveCommand.Create(() =>
        {
            // Exit app by clearing the router and not pushing any more screens.
            // Consumers can observe this command to stop rendering.
        });

        Back = ReactiveCommand.Create(() => HostScreen.Router.NavigateBack.Execute().Subscribe());
    }

    /// <summary>
    /// Gets a string token representing the current ViewModel, such as 'login' or 'user'.
    /// </summary>
    public string UrlPathSegment => "main";

    /// <summary>
    /// Gets the IScreen that this ViewModel is currently being shown in. This
    /// is usually passed into the ViewModel in the Constructor and saved
    /// as a ReadOnly Property.
    /// </summary>
    public IScreen HostScreen { get; }

    /// <summary>
    /// Gets the name of the user.
    /// </summary>
    /// <value>
    /// The name of the user.
    /// </value>
    public string UserName { get; }

    /// <summary>
    /// Gets the now.
    /// </summary>
    /// <value>
    /// The now.
    /// </value>
    public DateTime Now => _now.Value;

    /// <summary>
    /// Gets the exit.
    /// </summary>
    /// <value>
    /// The exit.
    /// </value>
    public ReactiveCommand<Unit, Unit> Exit { get; }

    /// <summary>
    /// Gets the back.
    /// </summary>
    /// <value>
    /// The back.
    /// </value>
    public ReactiveCommand<Unit, IDisposable> Back { get; }
}
