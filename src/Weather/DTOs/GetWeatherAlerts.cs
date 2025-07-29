using System.Text.Json.Serialization;

namespace FMCP.Weather.DTOs;

public sealed record GetWeatherAlertsRequest(float Latitude, float Longitude);

public sealed record GetWeatherAlertsResponse
{
	[JsonPropertyName("alerts")]
	public ICollection<WeatherAlert> Alerts { get; init; } = Array.Empty<WeatherAlert>();

	public record WeatherAlert
	{
		[JsonPropertyName("sender_name")]
		public string SenderName { get; init; }
		[JsonPropertyName("event")]
		public string Event { get; init; }
		[JsonPropertyName("start")]
		public long StartsInUnixSeconds { get; init; }
		[JsonPropertyName("end")]
		public long EndsInUnixSeconds { get; init; }
		[JsonPropertyName("description")]
		public string Description { get; init; }
		[JsonPropertyName("tags")]
		public ICollection<string> Tags { get; init; } = Array.Empty<string>();
	}
}