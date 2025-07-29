using FMCP.Temperature;
using ModelContextProtocol.Server;
using Serilog;
using System.ComponentModel;
using System.Text;

namespace FMCP.Weather;

public sealed class WeatherTools
{
	private readonly IWeatherService _service;

	public WeatherTools(IWeatherService service)
	{
		_service = service;
	}

	[McpServerTool]
	[Description("Retrieves real-time weather for the specific location. Returns formatted temperature in the configured unit system (Celsius/Fahrenheit/Kelvin)")]
	public async Task<string> GetCurrentWeather(
		[Description("Latitude coordinate in decimal degrees (WGS84)")] float latitude,
		[Description("Longitude coordinate in decimal degrees (WGS84)")] float longitude,
		CancellationToken ct = default)
	{
		try
		{
			var response = await _service.GetCurrentWeather(new(latitude, longitude), ct);

			if (!response.IsSuccess)
			{
				Log.Warning("Failed to get current weather: {Error}", response.Error.Description);

				return response.Error.Description;
			}

			return $"Current temperature: {response.Value.Current?.Temperature:F2}°{TemperatureSettings.Symbol}";
		}
		catch (Exception exception)
		{
			Log.Error(exception, "Error occurred while getting current weather");

			return "An error occurred while retrieving weather data. Please try again later.";
		}
	}

	[McpServerTool]
	[Description("Provides multi-day weather forecast including temperature variations throughout the day. Returns a list of formatted temperature readings for morning, day, evening and night periods")]
	public async Task<string> GetWeatherForecast(
		[Description("Latitude coordinate in decimal degrees (WGS84)")] float latitude,
		[Description("Longitude coordinate in decimal degrees (WGS84)")] float longitude,
		[Description("Forecast duration in maximum 8 days")] int period,
		CancellationToken ct = default)
	{
		try
		{
			var response = await _service.GetWeatherForecast(new(latitude, longitude, period), ct);

			if (!response.IsSuccess)
			{
				Log.Warning("Failed to get weather forecast: {Error}", response.Error.Description);

				return response.Error.Description;
			}

			var stringBuilder = new StringBuilder();
			foreach (var daily in response.Value.Forecast)
			{
				stringBuilder.AppendLine($"🌅 Morning: {daily.Temperature?.Morning}°{TemperatureSettings.Symbol}");
				stringBuilder.AppendLine($"☀️ Day: {daily.Temperature?.Day}°{TemperatureSettings.Symbol}");
				stringBuilder.AppendLine($"🌅 Evening: {daily.Temperature?.Evening}°{TemperatureSettings.Symbol}");
				stringBuilder.AppendLine($"🌑 Night: {daily.Temperature?.Night}°{TemperatureSettings.Symbol}");
			}

			return stringBuilder.ToString();
		}
		catch (Exception exception)
		{
			Log.Error(exception, "Error occurred while getting weather forecast");

			return "An error occurred while retrieving weather forecast. Please try again later.";
		}
	}

	[McpServerTool]
	[Description("Retrieves active weather alerts and warnings for the specified location")]
	public async Task<string> GetWeatherAlerts(
		[Description("Latitude coordinate in decimal degrees (WGS84)")] float latitude,
		[Description("Longitude coordinate in decimal degrees (WGS84)")] float longitude,
		CancellationToken ct = default)
	{
		try
		{
			var response = await _service.GetWeatherAlerts(new(latitude, longitude), ct);

			if (!response.IsSuccess)
			{
				Log.Warning("Failed to get alerts: {Error}", response.Error.Description);

				return response.Error.Description;
			}

			var stringBuilder = new StringBuilder();
			foreach (var alert in response.Value.Alerts)
			{
				stringBuilder.AppendLine($"📢 Alert from {alert.SenderName}");
				stringBuilder.AppendLine($"⚠️ Event: {alert.Event}");
				stringBuilder.AppendLine($"⏱️ Starts at {DateTimeOffset.FromUnixTimeSeconds(alert.StartsInUnixSeconds):g}");
				stringBuilder.AppendLine($"⏱️ Ends at {DateTimeOffset.FromUnixTimeSeconds(alert.EndsInUnixSeconds):g}");
				stringBuilder.AppendLine($"📝 Details: {alert.Description}");
				stringBuilder.AppendLine($"🏷️ Tags: {string.Join(", ", alert.Tags)}");
			}

			return stringBuilder.ToString();
		}
		catch (Exception exception)
		{
			Log.Error(exception, "Unexpected error while getting alerts");

			return "An error occurred while retrieving weather alerts. Please try again later.";
		}
	}
}