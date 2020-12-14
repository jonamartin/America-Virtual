using America_Virtual.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace America_Virtual.Controllers
{
    [ApiController]
    [Route("User")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly UsuariosService UsuariosService;
        public LoginController()
        {
            this.UsuariosService = new UsuariosService();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult Login([FromBody] User userDto)
        {
            var user = UsuariosService.Login(userDto);

            if (user is null)
            {
                return BadRequest(new { error = "Usuario o contraseña incorrecto" });
            }
            else
            {
                string tokenString = GenJwt(user);

                return Ok(new { token = tokenString });
            }
        }
        public string GenJwt(User user)
        {
            string jwtKey = ConfigurationService.GetConfigurationKey<string>("JwtKey");
            string jwtIssuer = ConfigurationService.GetConfigurationKey<string>("JwtIssuer");
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf,
                new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp,
                new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              jwtIssuer,
              jwtIssuer,
              claims,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
