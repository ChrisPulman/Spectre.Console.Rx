// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Enrichment;

internal sealed class GitLabEnricher : IProfileEnricher
{
    public string Name => "GitLab";

    public bool Enabled(IDictionary<string, string> environmentVariables)
    {
        if (environmentVariables.TryGetValue("CI_SERVER", out var value))
        {
            return value?.Equals("yes", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        return false;
    }

    public void Enrich(Profile profile) => profile.Capabilities.Interactive = false;
}