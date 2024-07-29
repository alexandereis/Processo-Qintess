import { Component, OnInit } from '@angular/core';
import { WeatherForecastService, WeatherForecast } from '../services/weather-forecast.service';

@Component({
  selector: 'app-forecast',
  templateUrl: './forecast.component.html',
  styleUrls: ['./forecast.component.css']
})
export class ForecastComponent implements OnInit {
  city: string = '';
  currentWeather: WeatherForecast | null = null;
  forecast: WeatherForecast[] = [];

  constructor(private weatherService: WeatherForecastService) { }

  ngOnInit(): void { }

  // Método para buscar a previsão do tempo para a cidade especificada
  searchWeather(): void {
    if (this.city.trim()) { // Verifica se o nome da cidade não está vazio
      this.weatherService.getWeatherForecast(this.city).subscribe(
        data => {
          if (Array.isArray(data) && data.length > 0) { // Validação para garantir que os dados são um array e não estão vazios
            this.currentWeather = data[0]; // Armazena a previsão do tempo atual
            this.forecast = data.slice(1); // Armazena a previsão do tempo para os próximos dias
          } else {
            console.warn('Nenhum dado de previsão encontrado', data);
            this.currentWeather = null; // Se não houver dados, define a previsão atual como null
            this.forecast = []; // E limpa a previsão dos próximos dias
          }
        },
        error => console.error('Erro ao buscar previsão do tempo', error) 
      );
    } else {
      console.warn('O campo cidade não pode estar vazio');
    }
  }
}
