using System;

namespace WeatherForecastApi.Models
{
    public class WeatherForecastEntity
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public double Temperature { get; set; }
        public DateTime Date { get; set; }
        public string Icon { get; set; }
    }
}
