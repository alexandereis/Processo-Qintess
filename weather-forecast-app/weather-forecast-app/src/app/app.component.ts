import { Component } from '@angular/core';
import { WeatherForecastService, WeatherForecast } from './services/weather-forecast.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  city: string = '';
  currentWeather: WeatherForecast[] | undefined;
  weatherHistory: WeatherForecast[] | undefined;

  constructor(private weatherService: WeatherForecastService) { }

  // Método chamado quando o usuário pesquisa a previsão do tempo para uma cidade
  searchWeather() {
    // Verifica se o nome da cidade não está vazio
    if (this.city.trim()) {
      this.weatherService.getWeatherForecast(this.city)
        .subscribe(
          data => this.currentWeather = data,
          error => console.error('Erro ao buscar previsão do tempo', error)
        );
    } else {
      console.warn('O campo cidade não pode estar vazio');
    }
  }

  // Método chamado para obter o histórico de previsões do tempo
  getHistory() {
    this.weatherService.getWeatherHistory()
      .subscribe(
        data => this.weatherHistory = data,
        error => console.error('Erro ao buscar histórico de previsões do tempo', error)
      );
  }
}
