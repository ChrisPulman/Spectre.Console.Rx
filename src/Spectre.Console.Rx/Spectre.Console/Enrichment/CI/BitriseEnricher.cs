namespace Spectre.Console.Rx.Enrichment;

internal sealed class BitriseEnricher : IProfileEnricher
{
    public string Name => "Bitrise";

    public bool Enabled(IDictionary<string, string> environmentVariables) => environmentVariables.ContainsKey("BITRISE_BUILD_URL");

    public void Enrich(Profile profile) => profile.Capabilities.Interactive = false;
}