// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// A column showing the elapsed time of a task.
/// </summary>
public sealed class ElapsedTimeColumn : ProgressColumn
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
        var elapsed = task.ElapsedTime;
        if (elapsed == null)
        {
            return new Markup("--:--:--");
        }

        if (elapsed.Value.TotalHours > 99)
        {
            return new Markup("**:**:**");
        }

        return new Text($"{elapsed.Value:hh\\:mm\\:ss}", Style ?? Style.Plain);
    }

    /// <inheritdoc/>
    public override int? GetColumnWidth(RenderOptions options) => 8;
}
