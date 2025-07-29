using FMCP.Weather;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Debug()
	.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
	.Enrich.FromLogContext()
	.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
	.WriteTo.File("logs/weather-.log", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
	.CreateLogger();

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddHttpClient<WeatherService>("OpenMapWeatherClient", options =>
{
	options.BaseAddress = new Uri("https://api.openweathermap.org/data/3.0/");
});

builder.Services
	.AddMcpServer()
	.WithStdioServerTransport()
	.WithTools<WeatherTools>();

await builder.Build().RunAsync();