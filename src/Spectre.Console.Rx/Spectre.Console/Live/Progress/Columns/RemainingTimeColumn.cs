// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// A column showing the remaining time of a task.
/// </summary>
public sealed class RemainingTimeColumn : ProgressColumn
{
    /// <summary>
    /// Gets or sets the style of the remaining time text.
    /// </summary>
    public Style Style { get; set; } = Color.Blue;

    /// <inheritdoc/>
    protected internal override bool NoWrap => true;

    /// <inheritdoc/>
    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
    {
        var remaining = task.RemainingTime;
        if (remaining == null)
        {
            return new Markup("--:--:--");
        }

        if (remaining.Value.TotalHours > 99)
        {
            return new Markup("**:**:**");
        }

        return new Text($"{remaining.Value:hh\\:mm\\:ss}", Style ?? Style.Plain);
    }

    /// <inheritdoc/>
    public override int? GetColumnWidth(RenderOptions options) => 8;
}
