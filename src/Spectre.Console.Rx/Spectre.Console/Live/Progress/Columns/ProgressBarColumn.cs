// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// A column showing task progress as a progress bar.
/// </summary>
public sealed class ProgressBarColumn : ProgressColumn
{
    /// <summary>
    /// Gets or sets the width of the column.
    /// </summary>
    public int? Width { get; set; } = 40;

    /// <summary>
    /// Gets or sets the style of completed portions of the progress bar.
    /// </summary>
    public Style CompletedStyle { get; set; } = Color.Yellow;

    /// <summary>
    /// Gets or sets the style of a finished progress bar.
    /// </summary>
    public Style FinishedStyle { get; set; } = Color.Green;

    /// <summary>
    /// Gets or sets the style of remaining portions of the progress bar.
    /// </summary>
    public Style RemainingStyle { get; set; } = Color.Grey;

    /// <summary>
    /// Gets or sets the style of an indeterminate progress bar.
    /// </summary>
    public Style IndeterminateStyle { get; set; } = ProgressBar.DefaultPulseStyle;

    /// <inheritdoc/>
    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime) => new ProgressBar
    {
        MaxValue = task.MaxValue,
        Value = task.Value,
        Width = Width,
        CompletedStyle = CompletedStyle,
        FinishedStyle = FinishedStyle,
        RemainingStyle = RemainingStyle,
        IndeterminateStyle = IndeterminateStyle,
        IsIndeterminate = task.IsIndeterminate,
    };
}