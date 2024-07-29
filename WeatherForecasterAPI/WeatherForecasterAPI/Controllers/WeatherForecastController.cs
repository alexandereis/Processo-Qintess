using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherForecastApi.Data;
using WeatherForecastApi.Models;
using WeatherForecastApi.Services;

namespace WeatherForecastApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherService _weatherService;
        private readonly WeatherForecastDbContext _context;

        public WeatherForecastController(WeatherService weatherService, WeatherForecastDbContext context)
        {
            _weatherService = weatherService;
            _context = context;
        }

        // Endpoint para obter a previs�o do tempo de uma cidade espec�fica
        // Exemplo de chamada: GET /api/weatherforecast/{city}
        [HttpGet("{city}")]
        public async Task<IActionResult> Get(string city)
        {
            // Chama o servi�o para obter a previs�o do tempo
            var forecast = await _weatherService.GetWeatherForecastAsync(city);
            return Ok(forecast);
        }

        // Endpoint para obter o hist�rico de previs�es salvas no banco de dados
        // Exemplo de chamada: GET /api/weatherforecast/history
        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            // Recupera todo o hist�rico de previs�es do banco de dados
            var history = _context.WeatherForecasts.ToList();
            return Ok(history);
        }
    }
}
