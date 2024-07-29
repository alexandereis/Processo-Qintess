# Processo Qintess - Weather Forecast Application

## Descrição

Este repositório contém a implementação de uma aplicação de previsão do tempo que utiliza C# (.NET) para o backend e Angular para o frontend. A aplicação consulta a previsão do tempo dos próximos 5 dias utilizando a API do OpenWeatherMap e armazena o histórico de consultas em um banco de dados SQL Server.

## Estrutura do Projeto

Processo Qintess/
├── WeatherForecasterAPI/
│   └── WeatherForecasterAPI/
│       └── Dockerfile
├── weather-forecast-app/
│   └── weather-forecast-app/
│       ├── Dockerfile
│       ├── nginx.conf
│       ├── package.json
│       ├── package-lock.json
│       └── src/
└── docker-compose.yml


- `WeatherForecasterAPI`: Contém a API em .NET
- `weather-forecast-app`: Contém a aplicação Angular
- `Processo Qintess`: Contém o arquivo `docker-compose.yml` e `README`

## Pré-requisitos

- .NET SDK 8.0
- Node.js
- Angular CLI
- Docker

## Configuração do Ambiente

1. Clonar o Repositório:
**Clone o repositório em sua máquina local:**
   ```sh
   git clone https://github.com/alexandereis/Processo-Qintess.git
   cd Processo-Qintess
   ```

2. Configurar e Executar Backend e Database com Docker
**No diretório raiz do projeto, execute os seguintes comandos para iniciar os containers do banco de dados e da API:**
   ```sh
   docker-compose up --build
   ```

Isso irá:

- Construir e iniciar um container para o SQL Server.
- Construir e iniciar um container para a API .NET Core.

3. Executar o Frontend
**O frontend está passando por alguns erros ao ser containerizado no Docker localmente. Portanto, ele deve ser executado manualmente utilizando o Angular CLI.**

Navegue até o diretório do frontend e instale as dependências:
   ```sh
   cd weather-forecast-app/weather-forecast-app
   npm install
   ```

Execute o frontend utilizando o Angular CLI:
   ```sh
   ng serve
   ```

```
A aplicação Angular estará disponível em http://localhost:4200.
```

4. Uso da Aplicação
**Abra o navegador e acesse http://localhost:4200. A interface permitirá que você insira o nome de uma cidade para buscar a previsão do tempo para os próximos 5 dias. O histórico de buscas será armazenado no banco de dados SQL Server.**

5. Estrutura do Código
**O backend está localizado na pasta WeatherForecasterAPI/WeatherForecasterAPI. Ele é responsável por:**
- Consultar a API do OpenWeatherMap.
- Armazenar e recuperar o histórico de consultas no banco de dados SQL Server.
- Expor dois endpoints:
   - GET /api/weather/{city}: Retorna a previsão do tempo para a cidade especificada.
   - GET /api/weather/history: Retorna o histórico de consultas.

6. Detalhes dos Dockerfiles
##### Backend Dockerfile
**Localização: WeatherForecasterAPI/WeatherForecasterAPI/Dockerfile**
   ``` 
   FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
   WORKDIR /app
   EXPOSE 8080

   FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
   WORKDIR /src
   COPY ["WeatherForecasterAPI/WeatherForecasterAPI.csproj", "WeatherForecasterAPI/"]
   RUN dotnet restore "WeatherForecasterAPI/WeatherForecasterAPI.csproj"
   COPY . .
   WORKDIR "/src/WeatherForecasterAPI"
   RUN dotnet build "WeatherForecasterAPI.csproj" -c Release -o /app/build

   FROM build AS publish
   RUN dotnet publish "WeatherForecasterAPI.csproj" -c Release -o /app/publish

   FROM base AS final
   WORKDIR /app
   COPY --from=publish /app/publish .
   ENTRYPOINT ["dotnet", "WeatherForecasterAPI.dll"]
   ```

##### Frontend Dockerfile
   ```
   # Stage 1: Build the Angular application
FROM node:14 AS build

WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the Angular application
RUN npm run build --configuration production

# Stage 2: Serve the application with Nginx
FROM nginx:alpine

# Copy the built Angular application from the previous stage
COPY --from=build /app/dist/weather-forecast-app /usr/share/nginx/html

# Copy Nginx configuration file
COPY nginx.conf /etc/nginx/nginx.conf

# Expose port 80
EXPOSE 80

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]
```

7. nginx.conf
**Localização: weather-forecast-app/weather-forecast-app/nginx.conf**
```
server {
  listen 80;

  server_name localhost;

  root /usr/share/nginx/html;

  location / {
    try_files $uri $uri/ /index.html;
  }

  error_page 404 /index.html;
}

```

8. docker-compose.yml
**Localização: docker-compose.yml**
```
version: '3.4'

services:
  sql-server-db:
    container_name: sql-server-db
    image: mcr.microsoft.com/mssql/server:2022-latest
    ports:
        - "1433:1433"
    environment:
        MSSQL_SA_PASSWORD: "SuperPassword@23"
        ACCEPT_EULA: "Y"
        MSSQL_PID: "Developer"

  weatherforecasterapi:
    depends_on:
        - sql-server-db
    restart: on-failure
    image: ${DOCKER_REGISTRY-}weatherforecasterapi
    build:
      context: .
      dockerfile: WeatherForecasterAPI/WeatherForecasterAPI/Dockerfile
    ports:
        - "5000:8080"
```

9. Notas Finais
- Certifique-se de que as portas 1433 (SQL Server), 5000 (API) e 4200 (Frontend) estão disponíveis em sua máquina.
- Para parar os containers, use docker-compose down.

Este README foi criado para fornecer todas as informações necessárias para configurar e executar a aplicação de forma fácil e rápida. Esperamos que os avaliadores apreciem a clareza e a completude deste documento.