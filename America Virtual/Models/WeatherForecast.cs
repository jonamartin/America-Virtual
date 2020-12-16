using System;

namespace America_Virtual
{
    public class WeatherForecast
    {
        public string day {get; set;}
        public string weatherDescription {get; set;}
        public decimal celsiusTemperature {get; set;}
        public decimal fahrenheitTemperature => Math.Round((this.celsiusTemperature*1.8m)+32,1);
        public int rainProbability {get; set;}
        public int humidity {get; set;}
        public float windSpeed {get; set;}
        public int weatherId { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
    }
}