namespace Spectre.Console.Rx.Enrichment;

internal sealed class TeamCityEnricher : IProfileEnricher
{
    public string Name => "TeamCity";

    public bool Enabled(IDictionary<string, string> environmentVariables) => environmentVariables.ContainsKey("TEAMCITY_VERSION");

    public void Enrich(Profile profile) => profile.Capabilities.Interactive = false;
}