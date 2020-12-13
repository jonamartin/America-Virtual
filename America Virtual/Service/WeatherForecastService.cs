using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace America_Virtual
{
    public class WeatherForecastService
    {

        public async Task<WeatherForecast> getCurrentByCity(string city)
        {
            HttpClient httpClient = new HttpClient();
            string requestUri = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric&lang=es", city, "be4ac631bbbe6eee5fa5db08362561f3");
            var response = await httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(jsonString);
            WeatherForecast weatherForecast = new WeatherForecast{
                day = DateTime.Today.ToString("dddd"),
                weatherDescription = json.weather[0].description,
                celsiusTemperature = json.main.temp,
                humidity = json.main.humidity,
                windSpeed = json.wind.speed
            };
            return weatherForecast;
        }
            
    }
}
