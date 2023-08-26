// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Enrichment;

internal sealed class BitbucketEnricher : IProfileEnricher
{
    public string Name => "Bitbucket";

    public bool Enabled(IDictionary<string, string> environmentVariables) => environmentVariables.ContainsKey("BITBUCKET_REPO_OWNER") ||
            environmentVariables.ContainsKey("BITBUCKET_REPO_SLUG") ||
            environmentVariables.ContainsKey("BITBUCKET_COMMIT");

    public void Enrich(Profile profile) => profile.Capabilities.Interactive = false;
}
