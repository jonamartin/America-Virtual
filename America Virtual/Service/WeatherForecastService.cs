using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace America_Virtual
{
    public class WeatherForecastService
    {
        string GetConfigurationKey(string keyName)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json").Build();
            return config.GetValue<string>(keyName);
        }

        public async Task<dynamic> Request(string requestUri)
        {
            HttpClient httpClient = new HttpClient();            
            var response = await httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(jsonString);
            return json;
        }
        public async Task<WeatherForecast> GetCurrentByCity(string city)
        {
            string requestUri = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric&lang=es", city, GetConfigurationKey("OpenWeatherAPIKey"));
            dynamic json = await Request(requestUri);
            WeatherForecast weatherForecast = new WeatherForecast{
                day = DateTime.Today.ToString("dddd"),
                weatherDescription = json.weather[0].description,
                celsiusTemperature = json.main.temp,
            };
            return weatherForecast;
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecastByCity(string city)
        {
            string requestUri = string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&appid={1}&units=metric&lang=es",city, GetConfigurationKey("OpenWeatherAPIKey"));
            dynamic json = await Request(requestUri);
            List<WeatherForecast> list = new List<WeatherForecast>();
            foreach(dynamic day in json.list)
            {
                WeatherForecast weatherForecast = new WeatherForecast{
                day = day.dt,
                celsiusTemperature = day.main.temp,
                weatherDescription = day.weather[0].description,
                };
                list.Add(weatherForecast);
            }
            return list;     
        }        
    }
}
