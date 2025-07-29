using FMCP.Results;

namespace FMCP.Weather;

public static class WeatherErrors
{
	public static Error LocationNotFound(float latitude, float longitude) => new("Location.NotFound", $"Data not available for coordinates ({latitude}; {longitude}).");

	public static Error InvalidDataFormat(string message = "") => new("Data.Invalid", $"Invalid data format. {message}");
}