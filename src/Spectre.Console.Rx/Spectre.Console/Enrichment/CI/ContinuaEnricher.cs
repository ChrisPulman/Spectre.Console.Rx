namespace Spectre.Console.Rx.Enrichment;

internal sealed class ContinuaEnricher : IProfileEnricher
{
    public string Name => "ContinuaCI";

    public bool Enabled(IDictionary<string, string> environmentVariables) => environmentVariables.ContainsKey("ContinuaCI.Version");

    public void Enrich(Profile profile) => profile.Capabilities.Interactive = false;
}