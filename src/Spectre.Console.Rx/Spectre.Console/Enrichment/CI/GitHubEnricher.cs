// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Enrichment;

internal sealed class GitHubEnricher : IProfileEnricher
{
    public string Name => "GitHub";

    public bool Enabled(IDictionary<string, string> environmentVariables)
    {
        if (environmentVariables.TryGetValue("GITHUB_ACTIONS", out var value))
        {
            return value?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        return false;
    }

    public void Enrich(Profile profile)
    {
        profile.Capabilities.Ansi = true;
        profile.Capabilities.Legacy = false;
        profile.Capabilities.Interactive = false;
        profile.Capabilities.Links = false;
    }
}
