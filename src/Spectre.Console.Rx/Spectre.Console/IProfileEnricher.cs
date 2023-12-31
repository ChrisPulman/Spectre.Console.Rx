// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents something that can enrich a profile.
/// </summary>
public interface IProfileEnricher
{
    /// <summary>
    /// Gets the name of the enricher.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets whether or not this enricher is enabled.
    /// </summary>
    /// <param name="environmentVariables">The environment variables.</param>
    /// <returns>Whether or not this enricher is enabled.</returns>
    bool Enabled(IDictionary<string, string> environmentVariables);

    /// <summary>
    /// Enriches the profile.
    /// </summary>
    /// <param name="profile">The profile to enrich.</param>
    void Enrich(Profile profile);
}