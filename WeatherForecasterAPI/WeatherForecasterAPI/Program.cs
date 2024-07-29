using Microsoft.EntityFrameworkCore;
using WeatherForecastApi.Data;
using WeatherForecastApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<WeatherService>(); // Adiciona o serviço WeatherService com tempo de vida Scoped
builder.Services.AddHttpClient(); // Adiciona suporte a HttpClient para fazer chamadas HTTP

// Configura o DbContext para usar o SQL Server com a string de conexão fornecida no appsettings.json
builder.Services.AddDbContext<WeatherForecastDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Garante que o banco de dados seja criado e as migrações sejam aplicadas
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WeatherForecastDbContext>();
    dbContext.Database.Migrate(); // Aplica quaisquer migrações pendentes ao banco de dados
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar CORS
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
