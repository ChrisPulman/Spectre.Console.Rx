namespace Spectre.Console.Rx.Enrichment;

internal sealed class BambooEnricher : IProfileEnricher
{
    public string Name => "Bamboo";

    public bool Enabled(IDictionary<string, string> environmentVariables) => environmentVariables.ContainsKey("bamboo_buildNumber");

    public void Enrich(Profile profile) => profile.Capabilities.Interactive = false;
}