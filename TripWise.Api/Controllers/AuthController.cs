using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TripWise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _config = config;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // for demo: validate user credentials (in real app, check against DB lokup + password hashing)
            if (request.Username == "admin" && request.Password == "password@123")
            {
                var token = GenerateJwtToken(request.Username);
                return Ok(new { token });
            }
            return Unauthorized();
        }

        private string GenerateJwtToken(string username)
        {
            var jwtKey = _config.GetValue<string>("Jwt:Key")
                ?? throw new InvalidOperationException("JWT Key is missing in configuration");
            var jwtIssuer = _config.GetValue<string>("Jwt:Issuer")
                ?? throw new InvalidOperationException("JWT Issuer missing in configuration");
            var key = _config.GetValue<string>("Jwt:Key");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
