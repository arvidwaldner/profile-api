namespace ProfileApi.Configurations
{
    public interface IConfigurationSetup
    {
        Dictionary<string, string> ApiKeys { get; set; }
        JwtOptions Jwt { get; set; }
        List<AudienceApiKeyMap> AudienceApiKeyMap { get; set; }
    }

    public class ConfigurationSetup : IConfigurationSetup
    {
        public Dictionary<string, string> ApiKeys { get; set; }
        public JwtOptions Jwt { get; set; }
        public List<AudienceApiKeyMap> AudienceApiKeyMap { get; set; }
    }
}

