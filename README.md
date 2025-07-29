# Real Weather MCP Server - Test Assignment

## Setup Instructions

1. **Ensure Prerequisites Are Installed**:

   - [.NET SDK 8.0 or later](https://dotnet.microsoft.com/download)
   - A valid OpenWeatherMap API key

2. **Clone the Repository**:

   ```bash
   git clone https://github.com/prog1910/fmcp.git
   cd fmcp
   ```

3. **Restore NuGet Dependencies**:

   ```bash
   dotnet restore
   ```

4. **Configure the OpenWeatherMap API Key in `mcp.json`**:

   ```bash
   WEATHER_API_KEY=your_api_key_here
   ```

5. **Build the Application**:

   ```bash
   dotnet build
   ```

6. **Run the Application**:

   ```bash
   dotnet run
   ```

---

## Testing Instructions

1. **Run Unit Tests**:

   ```bash
   dotnet test
   ```

2. **Logging Output**:
   - Console: real-time log output
   - File: `logs/weather-YYYYMMDD.log`

## Structure

Implementation leverages Vertical Slice Architecture (VSA) to modularize features, integrated with MCP for structured model-context management. The weather service exposes this functionality and communicates externally via HTTP client to an Open Map Weather API.

## Features with Details

### Tools

- `GetCurrentWeather`

  - Input: Location coordinates (latitude, longitude) derived from the name.
  - Output: Current temperature only (literal current temp).

- `GetWeatherForecast`

  - Input: Location coordinates.
  - Output: Up to 8-day forecast, with temperatures for morning, day, evening, and night each day (per OpenWeather documentation).

- `GetWeatherAlerts`
  - Input: Location coordinates.
  - Output: Weather alerts if any nearby threats or warnings exist
  - Example check location: Baikonur.

### Units Configuration

**Environment Variable**: `WEATHER_UNITS`

**Supported Values (case-insensitive)**:

- `"metric", "celsius", "c"` → Celsius (used as `units=metric`)
- `"imperial", "fahrenheit", "f"` → Fahrenheit (used as `units=imperial`)
- `"standard", "kelvin", "k"` → Kelvin (used as `units=standard`)

**Default value is metric** if value is missing or invalid.
