using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace America_Virtual.Controllers
{
    [ApiController]
    [Route("WeatherForecast")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        WeatherForecastService weatherForecastService = new WeatherForecastService();
        UsersService usersService = new UsersService();

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Current")]
        public async Task<ActionResult> CurrentWeather(string location)
        {
                WeatherForecast weatherForecast = await weatherForecastService.GetCurrentByCity(location);
                _logger.LogInformation("CurrentWeatherForecast For: " + location + JsonConvert.SerializeObject(weatherForecast));
                return Ok(weatherForecast);           
        }

        [HttpGet("Forecast")]
        public async Task<ActionResult> WeatherForecast(string latitude, string longitude)
        {
                IEnumerable<WeatherForecast> weatherForecasts = await weatherForecastService.GetForecastByCity(latitude, longitude);
                _logger.LogInformation("ForecastWeather For: " + latitude + "," + longitude + weatherForecasts);
                return Ok(weatherForecasts);
        }
    }
}
