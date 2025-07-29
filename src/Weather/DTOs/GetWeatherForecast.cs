using System.Text.Json.Serialization;

namespace FMCP.Weather.DTOs;

public sealed record GetWeatherForecastRequest(float Latitude, float Longitude, int Period);

public sealed record GetWeatherForecastResponse
{
	[JsonPropertyName("daily")]
	public ICollection<DailyWeather> Forecast { get; init; } = Array.Empty<DailyWeather>();

	public record DailyWeather
	{
		[JsonPropertyName("temp")]
		public DailyTemperature? Temperature { get; init; }

		public record DailyTemperature
		{
			[JsonPropertyName("morn")]
			public float Morning { get; init; }
			[JsonPropertyName("day")]
			public float Day { get; init; }
			[JsonPropertyName("eve")]
			public float Evening { get; init; }
			[JsonPropertyName("night")]
			public float Night { get; init; }
		}
	}
};