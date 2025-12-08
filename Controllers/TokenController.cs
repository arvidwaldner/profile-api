using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProfileApi.SchemaFilters.Attributes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProfileApi.Controllers
{
    [DevOnly]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;
        public TokenController(IConfiguration config) => _config = config;

        [HttpGet("test")]
        public IActionResult GetTestToken()
        {
            // Only allow in Development environment
            if (!Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.Equals("Development") ?? true)
                return Forbid();

            var signingKey = _config["Jwt:SigningKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: new[] { new Claim("sub", "testuser") },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(jwt);
        }
    }
}
