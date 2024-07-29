using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeatherForecastApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WeatherForecastApi.Data;

namespace WeatherForecastApi.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly WeatherForecastDbContext _context;

        public WeatherService(HttpClient httpClient, IConfiguration configuration, WeatherForecastDbContext context)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenWeatherMap:ApiKey"] ?? throw new ArgumentNullException(nameof(configuration), "API Key not found");
            _context = context;
        }

        // Método assíncrono que obtém a previsão do tempo para uma cidade e salva os dados no banco de dados
        public async Task<List<WeatherForecast>> GetWeatherForecastAsync(string city)
        {
            // Faz uma solicitação HTTP GET para obter a previsão do tempo da API OpenWeatherMap no formato JSON
            var response = await _httpClient.GetFromJsonAsync<WeatherForecastResponse>(
                $"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid={_apiKey}&lang=pt_br&units=metric");

            // Verifica se a resposta é nula ou não contém a lista de previsões
            if (response?.List == null)
                throw new InvalidOperationException("Unable to get weather forecast");

            // Converte a resposta da API em uma lista de objetos WeatherForecast
            var forecasts = response.List
                .Where(f => !string.IsNullOrEmpty(f.DtTxt)) // Garante que DtTxt não seja nulo ou vazio
                .Select(f => new WeatherForecast
                {
                    City = response.City.Name,
                    Description = f.Weather.FirstOrDefault()?.Description ?? "No description available",
                    Temperature = f.Main.Temp,
                    Date = f.DtTxt,
                    Icon = f.Weather.FirstOrDefault()?.Icon
                })
                .ToList();

            // Agrupa as previsões por data e seleciona o registro com a hora mais próxima da hora atual
            var groupedForecasts = forecasts
                .GroupBy(f => DateTime.Parse(f.Date).Date)
                .Select(g => g.OrderBy(f => Math.Abs((DateTime.Parse(f.Date) - DateTime.Now).TotalHours)).First())
                .ToList();

            // Converte as previsões agrupadas em entidades para salvar no banco de dados
            var forecastEntities = groupedForecasts
                .Select(f => new WeatherForecastEntity
                {
                    City = f.City,
                    Description = f.Description,
                    Temperature = f.Temperature,
                    Date = DateTime.Parse(f.Date),
                    Icon = f.Icon
                })
                .ToList();

            // Adiciona as entidades ao contexto e salva as mudanças no banco de dados
            _context.WeatherForecasts.AddRange(forecastEntities);
            await _context.SaveChangesAsync();

            // Retorna a lista de previsões agrupadas
            return groupedForecasts;
        }
    }
}
