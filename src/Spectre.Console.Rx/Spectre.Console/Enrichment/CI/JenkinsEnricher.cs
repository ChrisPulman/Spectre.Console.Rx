namespace Spectre.Console.Rx.Enrichment;

internal sealed class JenkinsEnricher : IProfileEnricher
{
    public string Name => "Jenkins";

    public bool Enabled(IDictionary<string, string> environmentVariables) => environmentVariables.ContainsKey("JENKINS_URL");

    public void Enrich(Profile profile) => profile.Capabilities.Interactive = false;
}