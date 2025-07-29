namespace FMCP.Temperature;

public static class TemperatureSettings
{
	private static readonly TemperatureUnits _units = ParseUnits(Environment.GetEnvironmentVariable("UNITS") ?? "metric");

	public static char Symbol => _units switch
	{
		TemperatureUnits.Kelvin => 'K',
		TemperatureUnits.Fahrenheit => 'F',
		TemperatureUnits.Celsius => 'C',
		_ => throw new NotSupportedException($"Unit {_units} not supported")
	};

	public static string UnitsValue => _units switch
	{
		TemperatureUnits.Kelvin => "standard",
		TemperatureUnits.Fahrenheit => "imperial",
		TemperatureUnits.Celsius => "metric",
		_ => throw new NotSupportedException($"Unit {_units} not supported"),
	};

	private static TemperatureUnits ParseUnits(string? unitsValue) => unitsValue switch
	{
		string s when s.Equals("standard", StringComparison.OrdinalIgnoreCase)
			|| s.Equals("kelvin", StringComparison.OrdinalIgnoreCase)
			|| s.Equals("k", StringComparison.OrdinalIgnoreCase) => TemperatureUnits.Kelvin,

		string s when s.Equals("imperial", StringComparison.OrdinalIgnoreCase)
			|| s.Equals("fahrenheit", StringComparison.OrdinalIgnoreCase)
			|| s.Equals("f", StringComparison.OrdinalIgnoreCase) => TemperatureUnits.Fahrenheit,

		string s when s.Equals("metric", StringComparison.OrdinalIgnoreCase)
			|| s.Equals("celsius", StringComparison.OrdinalIgnoreCase)
			|| s.Equals("c", StringComparison.OrdinalIgnoreCase) => TemperatureUnits.Celsius,

		_ => throw new ArgumentException($"Unknown units value: {unitsValue}")
	};
}