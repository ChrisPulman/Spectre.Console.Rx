// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// A column showing a spinner.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SpinnerColumn"/> class.
/// </remarks>
/// <param name="spinner">The spinner to use.</param>
public sealed class SpinnerColumn(Spinner spinner) : ProgressColumn
{
    private const string ACCUMULATED = "SPINNER_ACCUMULATED";
    private const string INDEX = "SPINNER_INDEX";

    private readonly object _lock = new();
    private Spinner _spinner = spinner ?? throw new ArgumentNullException(nameof(spinner));
    private int? _maxWidth;
    private string? _completed;
    private string? _pending;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpinnerColumn"/> class.
    /// </summary>
    public SpinnerColumn()
        : this(Spinner.Known.Default)
    {
    }

    /// <summary>
    /// Gets or sets the <see cref="Rx.Spinner"/>.
    /// </summary>
    public Spinner Spinner
    {
        get => _spinner;
        set
        {
            lock (_lock)
            {
                _spinner = value ?? Spinner.Known.Default;
                _maxWidth = null;
            }
        }
    }

    /// <summary>
    /// Gets or sets the text that should be shown instead
    /// of the spinner once a task completes.
    /// </summary>
    public string? CompletedText
    {
        get => _completed;
        set
        {
            _completed = value;
            _maxWidth = null;
        }
    }

    /// <summary>
    /// Gets or sets the text that should be shown instead
    /// of the spinner before a task begins.
    /// </summary>
    public string? PendingText
    {
        get => _pending;
        set
        {
            _pending = value;
            _maxWidth = null;
        }
    }

    /// <summary>
    /// Gets or sets the completed style.
    /// </summary>
    public Style? CompletedStyle { get; set; }

    /// <summary>
    /// Gets or sets the pending style.
    /// </summary>
    public Style? PendingStyle { get; set; }

    /// <summary>
    /// Gets or sets the style of the spinner.
    /// </summary>
    public Style? Style { get; set; } = Color.Yellow;

    /// <inheritdoc/>
    protected internal override bool NoWrap => true;

    /// <inheritdoc/>
    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
    {
        var useAscii = !options.Unicode && _spinner.IsUnicode;
        var spinner = useAscii ? Spinner.Known.Ascii : _spinner ?? Spinner.Known.Default;

        if (!task.IsStarted)
        {
            return new Markup(PendingText ?? " ", PendingStyle ?? Style.Plain);
        }

        if (task.IsFinished)
        {
            return new Markup(CompletedText ?? " ", CompletedStyle ?? Style.Plain);
        }

        var accumulated = task.State.Update<double>(ACCUMULATED, acc => acc + deltaTime.TotalMilliseconds);
        if (accumulated >= spinner.Interval.TotalMilliseconds)
        {
            task.State.Update<double>(ACCUMULATED, _ => 0);
            task.State.Update<int>(INDEX, index => index + 1);
        }

        var index = task.State.Get<int>(INDEX);
        var frame = spinner.Frames[index % spinner.Frames.Count];
        return new Markup(frame.EscapeMarkup(), Style ?? Style.Plain);
    }

    /// <inheritdoc/>
    public override int? GetColumnWidth(RenderOptions options) => GetMaxWidth(options);

    private int GetMaxWidth(RenderOptions options)
    {
        lock (_lock)
        {
            if (_maxWidth == null)
            {
                var useAscii = !options.Unicode && _spinner.IsUnicode;
                var spinner = useAscii ? Spinner.Known.Ascii : _spinner ?? Spinner.Known.Default;

                _maxWidth = Math.Max(
                    Math.Max(
                    ((IRenderable)new Markup(PendingText ?? " ")).Measure(options, int.MaxValue).Max,
                    ((IRenderable)new Markup(CompletedText ?? " ")).Measure(options, int.MaxValue).Max),
                    spinner.Frames.Max(frame => Cell.GetCellLength(frame)));
            }

            return _maxWidth.Value;
        }
    }
}
