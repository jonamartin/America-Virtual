using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace America_Virtual
{
    public class ConfigurationService
    {
        public static T GetConfigurationKey<T>(string keyName)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json").Build();
            return config.GetValue<T>(keyName);
        }
    }
}
