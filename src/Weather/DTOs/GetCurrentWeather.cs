using System.Text.Json.Serialization;

namespace FMCP.Weather.DTOs;

public sealed record GetCurrentWeatherRequest(float Latitude, float Longitude);

public sealed record GetCurrentWeatherResponse
{
	[JsonPropertyName("current")]
	public CurrentWeather? Current { get; init; }

	public record CurrentWeather
	{
		[JsonPropertyName("temp")]
		public float Temperature { get; init; }
	}
}