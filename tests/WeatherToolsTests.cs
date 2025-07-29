using FMCP.Weather;
using FMCP.Weather.DTOs;
using System.Net;
using System.Text;
using System.Text.Json;

namespace FMCP.Tests;

public sealed class WeatherServiceTests
{
	private readonly TestHttpMessageHandler _testHandler;
	private readonly IWeatherService _weatherService;
	private const string _baseAddress = "https://api.openweathermap.org/data/3.0/";

	public WeatherServiceTests()
	{
		Environment.SetEnvironmentVariable("WEATHER_API_KEY", "test_key");
		Environment.SetEnvironmentVariable("UNITS", "metric");

		_testHandler = new TestHttpMessageHandler();
		var httpClient = new HttpClient(_testHandler)
		{
			BaseAddress = new Uri(_baseAddress)
		};

		var clientFactory = new TestHttpClientFactory(httpClient);
		_weatherService = new WeatherService(clientFactory);
	}

	[Fact]
	public async Task GetCurrentWeather_ReturnsSuccess_WhenApiResponds()
	{
		// Arrange
		var expectedResponse = new GetCurrentWeatherResponse
		{
			Current = new() { Temperature = 25.5f }
		};

		SetupApiResponse("onecall?lat=40.4&lon=49.8&exclude=minutely,hourly,daily,alerts&units=metric&appid=test_key", expectedResponse);

		// Act
		var result = await _weatherService.GetCurrentWeather(new GetCurrentWeatherRequest(40.4f, 49.8f));

		// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(25.5f, result.Value.Current?.Temperature);
	}

	[Fact]
	public async Task GetCurrentWeather_ReturnsNotFound_WhenApiReturnsNull()
	{
		// Arrange
		SetupApiResponse("onecall?lat=40.4&lon=49.8&exclude=minutely,hourly,daily,alerts&units=metric&appid=test_key", null, HttpStatusCode.NotFound);

		// Act
		var result = await _weatherService.GetCurrentWeather(new GetCurrentWeatherRequest(40.4f, 49.8f));

		// Assert
		Assert.False(result.IsSuccess);
		Assert.Contains("NotFound", result.Error.Code);
	}

	private void SetupApiResponse(string urlSuffix, object response, HttpStatusCode statusCode = HttpStatusCode.OK)
	{
		var json = response != null ? JsonSerializer.Serialize(response) : string.Empty;

		_testHandler.SetupResponse($"{_baseAddress}{urlSuffix}", new HttpResponseMessage
		{
			StatusCode = statusCode,
			Content = new StringContent(json, Encoding.UTF8, "application/json")
		});
	}
}