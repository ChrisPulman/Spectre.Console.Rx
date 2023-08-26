// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Exception settings.
/// </summary>
public sealed class ExceptionSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionSettings"/> class.
    /// </summary>
    public ExceptionSettings()
    {
        Format = ExceptionFormats.Default;
        Style = new ExceptionStyle();
    }

    /// <summary>
    /// Gets or sets the exception format.
    /// </summary>
    public ExceptionFormats Format { get; set; }

    /// <summary>
    /// Gets or sets the exception style.
    /// </summary>
    public ExceptionStyle Style { get; set; }
}
