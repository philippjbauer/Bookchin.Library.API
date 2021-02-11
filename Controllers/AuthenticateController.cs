using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Bookchin.Library.API.Controllers.ViewModels;
using Bookchin.Library.API.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Bookchin.Library.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;

        public AuthenticateController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel data)
        {
            ApplicationUser user = _userManager.Users
                .FirstOrDefault(u => u.UserName == data.Username);

            if (user == null || await _userManager.CheckPasswordAsync(user, data.Password))
            {
                return Unauthorized();
            }

            string jwtIssuer = _configuration.GetValue<string>("Jwt:Issuer");
            string jwtSecret = _configuration.GetValue<string>("Jwt:Secret");

            JwtSecurityToken token = this.CreateJwtToken(user, jwtIssuer, jwtSecret);

            return Ok(new JwtTokenViewModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }

        private JwtSecurityToken CreateJwtToken(ApplicationUser user, string jwtIssuer, string jwtSecret)
        {
            var authClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                expires: DateTime.Now.AddDays(5),
                claims: authClaims,
                signingCredentials: signingCredentials
            );

            return token;
        }
    }
}