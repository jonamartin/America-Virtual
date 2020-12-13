using System;

namespace America_Virtual
{
    public class WeatherForecast
    {
        public string day {get; set;}
        public string weatherDescription {get; set;}
        public float celsiusTemperature {get; set;}
        public float farenheitTemperature => (this.celsiusTemperature*1.8f)+32;
        public int rainProbability {get; set;}
        public int humidity {get; set;}
        public float windSpeed {get; set;}
    }
}