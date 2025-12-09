using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using ProfileApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProfileApi.Controllers
{
    [EnableRateLimiting("fixed")]
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthenticateController(IConfiguration config) => _config = config;

        [HttpPost]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            var validKey = _config["ApiKey"];
            var audiences = _config.GetSection("Jwt:Audiences").Get<string[]>();
            if (request.ApiKey == validKey && audiences.Contains(request.Client))
            {
                var token = GenerateToken(request.Client);
                return Ok(new { access_token = token });
            }
            return Unauthorized();
        }

        private string GenerateToken(string audience)
        {
            var signingKey = _config["Jwt:SigningKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var issuer = _config["Jwt:Issuer"];

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
