using Microsoft.IdentityModel.Tokens;
using ProfileApi.Configurations;
using ProfileApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProfileApi.Services
{
    public interface IAuthenticateService
    {
        string Authenticate(AuthRequest authRequest);
    }

    public class AuthenticateService : IAuthenticateService
    {
        private readonly IConfigurationSetup _configurationSetup;

        public AuthenticateService(IConfigurationSetup configurationSetup)
        {
            _configurationSetup = configurationSetup;
        }

        public string Authenticate(AuthRequest request)
        {
            var audienceByClient = GetAudienceByClient(request.Client);

            if (string.IsNullOrEmpty(audienceByClient))
            {
                Console.WriteLine("Audience not found for client: " + request.Client);
                throw new UnauthorizedAccessException("Invalid credentials. Review your client and apikey");
            }

            var expectedApiKey = GetApiKeyByClient(request.Client);

            if (request.ApiKey != expectedApiKey)
            {
                Console.WriteLine("API key mismatch for client: " + request.Client);
                throw new UnauthorizedAccessException("Invalid credentials. Review your client and apikey");
            }

            if (string.IsNullOrEmpty(expectedApiKey))
            {
                Console.WriteLine("API key not found for client: " + request.Client);
                throw new UnauthorizedAccessException("Invalid credentials. Review your client and apikey");
            }

            var accessToken = GenerateToken(audienceByClient);
            return accessToken;
        }

        private string GetAudienceByClient(string client)
        {
            var apiKeyMapAudienceSection = _configurationSetup.AudienceApiKeyMap;
            var mapping = apiKeyMapAudienceSection.FirstOrDefault(m => m.ApiKeyName == client);
            return mapping?.Audience;
        }

        private string GetApiKeyByClient(string client)
        {
            var apiKeysSection = _configurationSetup.ApiKeys;

            if (apiKeysSection.TryGetValue(client, out var apiKey))
            {
                return apiKey;
            }

            return null;
        }

        private string GenerateToken(string audience)
        {
            var signingKey = _configurationSetup.Jwt.SigningKey;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var issuer = _configurationSetup.Jwt.Issuer;

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: new[] { new Claim("sub", "testuser") },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
