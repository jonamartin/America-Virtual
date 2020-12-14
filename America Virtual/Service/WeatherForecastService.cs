using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Models;

namespace America_Virtual
{
    public class WeatherForecastService
    {
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
            string requestUri = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric&lang=es", city, ConfigurationService.GetConfigurationKey<string>("OpenWeatherAPIKey"));
            dynamic json = await Request(requestUri);
            WeatherForecast weatherForecast = new WeatherForecast{
                day = DateTime.Today.ToString("dddd"),
                weatherDescription = json.weather[0].description,
                celsiusTemperature = (int)json.main.temp,
                humidity = json.main.humidity,
                windSpeed = json.wind.speed,
                longitude = json.coord.lon,
                latitude = json.coord.lat,
                weatherId = json.weather[0].id
            };
            return weatherForecast;
        }
        public string UnixTimeToDayOfWeek(long unixtime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
            return dtDateTime.ToString("dddd");
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecastByCity(string latitude, string longitude)
        {
            string requestUri = string.Format("https://api.openweathermap.org/data/2.5/onecall?lat={0}&lon={1}&exclude=current,minutely,hourly,alerts&appid={2}&units=metric&lang=sp", latitude, longitude, GetConfigurationKey("OpenWeatherAPIKey"));
            dynamic json = await Request(requestUri);
            List<WeatherForecast> list = new List<WeatherForecast>();
            for(int i = 0; i <= 5; i++)
            {
                dynamic day = json.daily[i];
                WeatherForecast weatherForecast = new WeatherForecast{
                day =  UnixTimeToDayOfWeek(Int64.Parse(day.dt.ToString())),
                weatherDescription = day.weather[0].description,
                celsiusTemperature = day.temp.day,
                rainProbability = day.pop*100,
                humidity = day.humidity,
                windSpeed = day.wind_speed,
                latitude = latitude,
                longitude = longitude,
                weatherId = day.weather[0].id

                };
                list.Add(weatherForecast);
            }
            return list;     
        }        
    }
}
