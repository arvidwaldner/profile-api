using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using ProfileApi.Configurations;
using ProfileApi.Exceptions;
using ProfileApi.Models;
using ProfileApi.Services;
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
        private readonly IAuthenticateService _authenticateService;
        
        public AuthenticateController(IAuthenticateService authenticateService) 
        {
            _authenticateService = authenticateService;
        }        

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]        
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public IActionResult Login([FromBody] AuthRequest request)
        {                    
            var accessToken = _authenticateService.Authenticate(request);            
            return Ok(accessToken);
        }       
    }
}
