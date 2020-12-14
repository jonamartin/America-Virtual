using America_Virtual.Controllers;
using America_Virtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace America_Virtual
{
    public class UsuariosService
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
    }
}
