using Microsoft.EntityFrameworkCore;
using WeatherForecastApi.Models;

namespace WeatherForecastApi.Data
{
    public class WeatherForecastDbContext : DbContext
    {
        public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options) : base(options)
        {
        }

        public DbSet<WeatherForecastEntity> WeatherForecasts { get; set; }
    }
}
