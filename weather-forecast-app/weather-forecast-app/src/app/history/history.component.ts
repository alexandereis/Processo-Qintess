import { Component, OnInit } from '@angular/core';
import { WeatherForecastService, WeatherForecast } from '../services/weather-forecast.service';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit {
  weatherHistory: WeatherForecast[] = [];
  displayedHistory: WeatherForecast[] = [];
  currentPage: number = 1;
  itemsPerPage: number = 6; // Propriedade para definir quantos itens serão exibidos por página

  constructor(private weatherService: WeatherForecastService) { }

  ngOnInit(): void {
    this.getHistory();
  }

  // Método para obter o histórico de previsões do tempo do serviço
  getHistory(): void {
    this.weatherService.getWeatherHistory().subscribe(
      data => {
        if (Array.isArray(data)) { // Validação para garantir que os dados são um array
          this.weatherHistory = data; // Armazena o histórico de previsões na propriedade weatherHistory
          this.updateDisplayedHistory(); // Atualiza as previsões exibidas na página atual
        } else {
          console.error('Formato de dados inválido', data);
        }
      },
      error => console.error('Erro ao buscar histórico de previsões do tempo', error)
    );
  }

  // Método para atualizar as previsões exibidas na página atual
  updateDisplayedHistory(): void {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.displayedHistory = this.weatherHistory.slice(startIndex, endIndex);
  }

  nextPage(): void {
    if (this.currentPage * this.itemsPerPage < this.weatherHistory.length) {
      this.currentPage++;
      this.updateDisplayedHistory();
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updateDisplayedHistory();
    }
  }
}
