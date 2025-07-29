using FMCP.Results;
using FMCP.Temperature;
using FMCP.Weather.DTOs;

using System.Net;
using System.Text.Json;

namespace FMCP.Weather;

public sealed class WeatherService : IWeatherService
{
	private readonly string _key = Environment.GetEnvironmentVariable("WEATHER_API_KEY") ?? throw new InvalidOperationException("The weather API key is not configured.");
	private readonly HttpClient _client;

	public WeatherService(IHttpClientFactory clientFactory)
	{
		_client = clientFactory.CreateClient("OpenMapWeatherClient");
	}

	public async Task<Result<GetCurrentWeatherResponse>> GetCurrentWeather(GetCurrentWeatherRequest request, CancellationToken ct = default)
	{
		return await GetWeatherData<GetCurrentWeatherResponse>(
			url: $"{_client.BaseAddress}onecall?lat={request.Latitude}&lon={request.Longitude}&exclude=minutely,hourly,daily,alerts&units={TemperatureSettings.UnitsValue}&appid={_key}",
			notFoundError: WeatherErrors.LocationNotFound(latitude: request.Latitude, longitude: request.Longitude),
			invalidFormatError: WeatherErrors.InvalidDataFormat(),
			ct);
	}

	public async Task<Result<GetWeatherForecastResponse>> GetWeatherForecast(GetWeatherForecastRequest request, CancellationToken ct = default)
	{
		return await GetWeatherData<GetWeatherForecastResponse>(
			url: $"{_client.BaseAddress}onecall?lat={request.Latitude}&lon={request.Longitude}&exclude=current,minutely,hourly,alerts&units={TemperatureSettings.UnitsValue}&appid={_key}",
			notFoundError: WeatherErrors.LocationNotFound(latitude: request.Latitude, longitude: request.Longitude),
			invalidFormatError: WeatherErrors.InvalidDataFormat(),
			ct);
	}

	public async Task<Result<GetWeatherAlertsResponse>> GetWeatherAlerts(GetWeatherAlertsRequest request, CancellationToken ct = default)
	{
		return await GetWeatherData<GetWeatherAlertsResponse>(
			url: $"{_client.BaseAddress}onecall?lat={request.Latitude}&lon={request.Longitude}&exclude=current,minutely,hourly,daily&units={TemperatureSettings.UnitsValue}&appid={_key}",
			notFoundError: WeatherErrors.LocationNotFound(latitude: request.Latitude, longitude: request.Longitude),
			invalidFormatError: WeatherErrors.InvalidDataFormat(),
			ct);
	}

	private async Task<Result<TResponse>> GetWeatherData<TResponse>(string url, Error notFoundError, Error invalidFormatError, CancellationToken ct = default)
	{
		var httpResponse = await _client.GetAsync(url, ct);

		if (httpResponse.StatusCode == HttpStatusCode.NotFound)
		{
			return notFoundError;
		}

		httpResponse.EnsureSuccessStatusCode();

		var content = await httpResponse.Content.ReadAsStringAsync(ct);
		var response = JsonSerializer.Deserialize<TResponse>(content);

		return response ?? Result<TResponse>.Failure(invalidFormatError);
	}
}