// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Enrichment;

internal sealed class MyGetEnricher : IProfileEnricher
{
    public string Name => "MyGet";

    public bool Enabled(IDictionary<string, string> environmentVariables)
    {
        if (environmentVariables.TryGetValue("BuildRunner", out var value))
        {
            return value?.Equals("MyGet", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        return false;
    }

    public void Enrich(Profile profile) => profile.Capabilities.Interactive = false;
}