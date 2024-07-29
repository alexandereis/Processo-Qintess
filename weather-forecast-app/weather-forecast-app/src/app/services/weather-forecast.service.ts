import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface WeatherForecast {
  id: number;
  city: string;
  description: string;
  temperature: number;
  date: string;
  icon?: string;
}

@Injectable({
  providedIn: 'root'
})
export class WeatherForecastService {
  private backendUrl: string = 'http://localhost:5000/api/weatherforecast'; // URL do backend

  constructor(private http: HttpClient) { }

  getWeatherForecast(city: string): Observable<WeatherForecast[]> {
    // Faz uma requisição HTTP GET para buscar a previsão do tempo de uma cidade específica
    return this.http.get<WeatherForecast[]>(`${this.backendUrl}/${city}`);
  }

  getWeatherHistory(): Observable<WeatherForecast[]> {
    // Faz uma requisição HTTP GET para buscar o histórico de previsões do tempo
    return this.http.get<WeatherForecast[]>(`${this.backendUrl}/history`);
  }
}
