using FMCP.Results;
using FMCP.Weather.DTOs;

namespace FMCP.Weather;

public interface IWeatherService
{
	Task<Result<GetCurrentWeatherResponse>> GetCurrentWeather(GetCurrentWeatherRequest request, CancellationToken ct = default);

	Task<Result<GetWeatherForecastResponse>> GetWeatherForecast(GetWeatherForecastRequest request, CancellationToken ct = default);

	Task<Result<GetWeatherAlertsResponse>> GetWeatherAlerts(GetWeatherAlertsRequest request, CancellationToken ct = default);
}