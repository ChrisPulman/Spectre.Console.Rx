// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Contains extension methods for <see cref="StatusContext"/>.
/// </summary>
public static class StatusContextExtensions
{
    /// <summary>
    /// Sets the status message.
    /// </summary>
    /// <param name="context">The status context.</param>
    /// <param name="status">The status message.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static StatusContext Status(this StatusContext context, string status)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.Status = status;
        return context;
    }

    /// <summary>
    /// Sets the spinner.
    /// </summary>
    /// <param name="context">The status context.</param>
    /// <param name="spinner">The spinner.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static StatusContext Spinner(this StatusContext context, Spinner spinner)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.Spinner = spinner;
        return context;
    }

    /// <summary>
    /// Sets the spinner style.
    /// </summary>
    /// <param name="context">The status context.</param>
    /// <param name="style">The spinner style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static StatusContext SpinnerStyle(this StatusContext context, Style? style)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.SpinnerStyle = style;
        return context;
    }
}