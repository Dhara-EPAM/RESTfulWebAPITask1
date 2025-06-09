using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace RESTfulWebAPITask1.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Hardcoded validation
            if (request.Username == "manager" && request.Password == "password123")
            {
                var token = GenerateJwtToken("Manager");
                return Ok(new { token });
            }

            if (request.Username == "customer" && request.Password == "password123")
            {
                var token = GenerateJwtToken("StoreCustomer");
                return Ok(new { token });
            }

            return Unauthorized();
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            // Validate refresh token
            if (request.RefreshToken == "sample-refresh-token")
            {
                var token = GenerateJwtToken(request.Role); // Token refreshed for the same role
                return Ok(new { token });
            }

            return Unauthorized();
        }

        private string GenerateJwtToken(string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, "Authenticated User")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
        public string Role { get; set; }
    }

}
