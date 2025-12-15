using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using ProfileApi.Configurations;
using ProfileApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProfileApi.Controllers
{
    [EnableRateLimiting("fixed")]
    [Route("profile/api/authenticate")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfigurationSetup _configurationSetup;

        public AuthenticateController(IConfigurationSetup configurationSetup) 
        {
            _configurationSetup = configurationSetup;
        }        

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]        
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            var audienceByClient = GetAudienceByClient(request.Client);

            if(string.IsNullOrEmpty(audienceByClient))
            {
                return Unauthorized("Invalid credentials. Review your client and apikey");
            }

            var expectedApiKey = GetApiKeyByClient(request.Client);

            if (request.ApiKey != expectedApiKey)
            {
                return Unauthorized("Invalid credentials. Review your client and apikey");
            }

            if (string.IsNullOrEmpty(expectedApiKey))
            {
                return Unauthorized("Invalid credentials. Review your client and apikey");
            }

            var accessToken = GenerateToken(audienceByClient);            
            return Ok(accessToken);
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
