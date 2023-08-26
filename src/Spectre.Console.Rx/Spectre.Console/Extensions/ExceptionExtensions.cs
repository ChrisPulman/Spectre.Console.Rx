// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Contains extension methods for <see cref="Exception"/>.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Gets a <see cref="IRenderable"/> representation of the exception.
    /// </summary>
    /// <param name="exception">The exception to format.</param>
    /// <param name="format">The exception format options.</param>
    /// <returns>A <see cref="IRenderable"/> representing the exception.</returns>
    public static IRenderable GetRenderable(this Exception exception, ExceptionFormats format = ExceptionFormats.Default)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        return GetRenderable(exception, new ExceptionSettings
        {
            Format = format,
        });
    }

    /// <summary>
    /// Gets a <see cref="IRenderable"/> representation of the exception.
    /// </summary>
    /// <param name="exception">The exception to format.</param>
    /// <param name="settings">The exception settings.</param>
    /// <returns>A <see cref="IRenderable"/> representing the exception.</returns>
    public static IRenderable GetRenderable(this Exception exception, ExceptionSettings settings)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        return ExceptionFormatter.Format(exception, settings);
    }
}