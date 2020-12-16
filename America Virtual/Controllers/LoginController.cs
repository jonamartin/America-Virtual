using America_Virtual.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly UsersService UsuariosService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            this.UsuariosService = new UsersService();
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult Login([FromBody] User userDto)
        {
            var user = UsuariosService.Login(userDto);

            if (user is null)
            {
                _logger.LogInformation("Login For: " + userDto.Email + " failed.");
                return StatusCode(403, new { error = "Usuario o contraseña incorrecto" });
            }
            else
            {
                string tokenString = UsuariosService.GenJwt(user);
                _logger.LogInformation("Login For: " + userDto.Email + "succesfully.");

                return Ok(new { token = tokenString });
            }
        }
        
    }
}
