using America_Virtual.Controllers;
using America_Virtual.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace America_Virtual
{
    public class UsersService
    {
        public User Login(User user)
        {
            var foundUser = ConfigurationService.GetConfigurationSection<User[]>("Users").FirstOrDefault(u => u.Email == user.Email);

            if (foundUser is null)
            {
                return null;
            }

            bool verify = BCrypt.Net.BCrypt.Verify(user.Password, foundUser.Password);

            if (!verify)
            {
                return null;
            }

            foundUser.Password = null;
            return foundUser;
        }

        public string GenJwt(User user)
        {
            string jwtKey =  ConfigurationService.GetConfigurationKey<string>("JwtKey");
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

        public bool IsValidToken(string token)
        {
            SecurityToken result;
            string jwt = token.Split(' ')[1];
            string jwtKey = ConfigurationService.GetConfigurationKey<string>("JwtKey");
            string jwtIssuer = ConfigurationService.GetConfigurationKey<string>("JwtIssuer");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            JwtSecurityTokenHandler jwtSecurity = new JwtSecurityTokenHandler();
            var user = jwtSecurity.ValidateToken(jwt, new TokenValidationParameters { ValidIssuer = jwtIssuer, ValidAudience = jwtIssuer, IssuerSigningKey = securityKey }, out result);

            string email =  user.FindFirstValue("sub");
            var foundUser = ConfigurationService.GetConfigurationSection<User[]>("Users").FirstOrDefault(u => u.Email == email);

            return foundUser != null;
        }
    }
}
