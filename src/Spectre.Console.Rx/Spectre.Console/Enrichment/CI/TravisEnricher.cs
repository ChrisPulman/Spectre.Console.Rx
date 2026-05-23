namespace Spectre.Console.Rx.Enrichment;

internal sealed class TravisEnricher : IProfileEnricher
{
    public string Name => "Travis";

    public bool Enabled(IDictionary<string, string> environmentVariables) => environmentVariables.ContainsKey("TRAVIS");

    public void Enrich(Profile profile) => profile.Capabilities.Interactive = false;
}