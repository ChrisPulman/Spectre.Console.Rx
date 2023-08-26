// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Contains settings for profile enrichment.
/// </summary>
public sealed class ProfileEnrichment
{
    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// any default enrichers should be added.
    /// </summary>
    /// <remarks>Defaults to <c>true</c>.</remarks>
    public bool UseDefaultEnrichers { get; set; } = true;

    /// <summary>
    /// Gets or sets the list of custom enrichers to use.
    /// </summary>
    public List<IProfileEnricher>? Enrichers { get; set; }
}