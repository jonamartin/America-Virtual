using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace America_Virtual.Controllers
{
    [ApiController]
    [Route("WeatherForecast")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        WeatherForecastService weatherForecastService = new WeatherForecastService();

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Current")]
        public async Task<ActionResult> CurrentWeatherForecast(string location)
        {
            return Ok(await weatherForecastService.GetCurrentByCity(location));
        }

        [HttpGet("Forecast")]
        public async Task<ActionResult> ForecastWeatherForecast(string location)
        {      
            return Ok(await weatherForecastService.GetForecastByCity(location));
        }
    }
}
