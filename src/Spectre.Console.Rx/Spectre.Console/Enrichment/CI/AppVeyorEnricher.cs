// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Enrichment;

internal sealed class AppVeyorEnricher : IProfileEnricher
{
    public string Name => "AppVeyor";

    public bool Enabled(IDictionary<string, string> environmentVariables) => environmentVariables.ContainsKey("APPVEYOR");

    public void Enrich(Profile profile) => profile.Capabilities.Interactive = false;
}
