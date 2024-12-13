using Serilog;
using Web;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<CorrelationIdInterceptor>();
builder.Services.AddHttpClient("test")
    .AddHttpMessageHandler<CorrelationIdInterceptor>();

var app = builder.Build();

app.UseMiddleware<TraceMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (IHttpClientFactory clientFactory) =>
    {
        var httpClient = clientFactory.CreateClient("test"); 
        return httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>("http://localhost:5279/weatherforecast2");
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.UseSerilogRequestLogging();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}