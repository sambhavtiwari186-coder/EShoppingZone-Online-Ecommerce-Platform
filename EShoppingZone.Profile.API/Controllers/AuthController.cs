using EShoppingZone.Profile.API.Services;
using EShoppingZone.Profile.API.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EShoppingZone.Profile.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ProfileService _profileService;
        private readonly IConfiguration _configuration;

        public AuthController(ProfileService profileService, IConfiguration configuration)
        {
            _profileService = profileService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _profileService.FindByEmailIdAsync(request.EmailId);
            if (user == null || !_profileService.VerifyPassword(user, request.Password))
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            if (user.IsSuspended)
            {
                return Unauthorized(new { Message = "Your account has been suspended. Please contact support." });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        [HttpPost("register/customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] UserProfile profile)
        {
            await _profileService.AddCustomerAsync(profile);
            return Ok(profile);
        }

        [HttpPost("register/merchant")]
        public async Task<IActionResult> RegisterMerchant([FromBody] UserProfile profile)
        {
            await _profileService.AddMerchantAsync(profile);
            return Ok(profile);
        }

        [HttpGet("github-login")]
        public IActionResult GithubLogin()
        {
            // Placeholder: Typically you'd redirect to GitHub OAuth, handle callback, and issue a JWT.
            return Ok(new { Message = "GitHub login endpoint" });
        }

        private string GenerateJwtToken(UserProfile user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "SecretKeyVeryLongStringToSignToken1234!"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.EmailId),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("ProfileId", user.ProfileId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string EmailId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
