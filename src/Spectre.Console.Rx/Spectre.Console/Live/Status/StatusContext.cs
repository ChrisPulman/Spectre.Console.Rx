// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a context that can be used to interact with a <see cref="Status"/>.
/// </summary>
public sealed class StatusContext : IContext
{
    private readonly ProgressContext _context;
    private readonly ProgressTask _task;
    private readonly SpinnerColumn _spinnerColumn;

    internal StatusContext(ProgressContext context, ProgressTask task, SpinnerColumn spinnerColumn)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _task = task ?? throw new ArgumentNullException(nameof(task));
        _spinnerColumn = spinnerColumn ?? throw new ArgumentNullException(nameof(spinnerColumn));
    }

    /// <summary>
    /// Gets or sets the current status.
    /// </summary>
    public string Status
    {
        get => _task.Description;
        set => SetStatus(value);
    }

    /// <summary>
    /// Gets or sets the current spinner.
    /// </summary>
    public Spinner Spinner
    {
        get => _spinnerColumn.Spinner;
        set => SetSpinner(value);
    }

    /// <summary>
    /// Gets or sets the current spinner style.
    /// </summary>
    public Style? SpinnerStyle
    {
        get => _spinnerColumn.Style;
        set => _spinnerColumn.Style = value;
    }

    /// <summary>
    /// Refreshes the status.
    /// </summary>
    public void Refresh() => _context.Refresh();

    private void SetStatus(string status)
    {
        if (status is null)
        {
            throw new ArgumentNullException(nameof(status));
        }

        _task.Description = status;
    }

    private void SetSpinner(Spinner spinner)
    {
        if (spinner is null)
        {
            throw new ArgumentNullException(nameof(spinner));
        }

        _spinnerColumn.Spinner = spinner;
    }
}
