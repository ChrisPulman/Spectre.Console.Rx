namespace Spectre.Console.Rx.Enrichment;

internal sealed class TfsEnricher : IProfileEnricher
{
    public string Name => "TFS";

    public bool Enabled(IDictionary<string, string> environmentVariables) => environmentVariables.ContainsKey("TF_BUILD");

    public void Enrich(Profile profile) => profile.Capabilities.Interactive = false;
}