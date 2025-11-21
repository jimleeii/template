# Template - ASP.NET Core Minimal API

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A modern ASP.NET Core Minimal API template featuring the Endpoint Definition pattern and Model Context Protocol (MCP) integration.

## Features

- **✨ Minimal API Architecture** - Clean, lightweight API structure using ASP.NET Core 10.0
- **📦 Endpoint Definition Pattern** - Modular endpoint organization for better code maintainability
- **🤖 Model Context Protocol (MCP) Integration** - Built-in MCP server for AI tool interactions
- **📚 OpenAPI/Swagger Documentation** - Automatic API documentation in development mode
- **🔧 Dependency Injection** - Built-in DI container for services
- **⚡ Output Caching** - Performance optimization with configurable caching
- **🔄 JSON-RPC 2.0 Support** - Standardized JSON-RPC response format for API endpoints
- **🔐 Microsoft Entra ID Authentication** - Built-in JWT authentication with role-based authorization
- **🔌 WebSocket Support** - Real-time bidirectional communication capabilities

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- Visual Studio 2022 or Visual Studio Code

## Getting Started

### Option 1: Install as dotnet Template

Install this project as a reusable dotnet template:

```bash
# Install from local path
dotnet new install ./

# Or install from NuGet (if published)
dotnet new install Template

# Create a new project from the template
dotnet new template -n MyApiProject

# Uninstall the template (if needed)
dotnet new uninstall Template
```

### Option 2: Clone and Build

```bash
git clone <repository-url>
cd Template
dotnet restore
dotnet build
```

### Run the Application

```bash
cd src
dotnet run
```

The application will start on:
- HTTP: `http://localhost:5243`
- HTTPS: `https://localhost:7145`

### Access Documentation

When running in Development mode, access:
- **Swagger UI**: `https://localhost:7145/swagger`
- **OpenAPI Spec**: `https://localhost:7145/openapi/v1.json`

## Project Structure

```
Template/
├── src/
│   ├── EndpointDefinitions/       # Modular endpoint definitions
│   │   ├── McpEndpointDefinition.cs
│   │   ├── OpenApiEndpointDefinition.cs
│   │   ├── SecureEndpointDefinition.cs
│   │   ├── SwaggerEndpointDefinition.cs
│   │   ├── WeatherForecastEndpointDefinition.cs
│   │   └── WebSocketEndpointDefinition.cs
│   ├── Extensions/                # Extension methods
│   │   └── StringExtensions.cs
│   ├── Models/                    # Data models
│   │   ├── WeatherForecast.cs
│   │   ├── JsonRpcResponse.cs
│   │   ├── JsonRpcError.cs
│   │   └── JsonRpcErrorCodes.cs
│   ├── Services/                  # Business logic services
│   │   ├── IWeatherForecastService.cs
│   │   └── WeatherForecastService.cs
│   ├── Tools/                     # MCP tools
│   │   └── WeatherQueryTool.cs
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── Program.cs                 # Application entry point
│   ├── GlobalUsings.cs            # Global using directives
│   └── Template.csproj
├── ENTRA_SETUP.md                 # Microsoft Entra ID setup guide
└── Template.sln
```

## Architecture

### Endpoint Definition Pattern

This template uses the [EndpointDefinition](https://www.nuget.org/packages/EndpointDefinition/) library to organize endpoints modularly. Each endpoint definition class implements `IEndpointDefinition` and provides:

- `DefineServices()` - Register services in the DI container
- `DefineEndpoints()` - Configure API endpoints

Example:
```csharp
public class WeatherForecastEndpointDefinition : IEndpointDefinition
{
    public void DefineServices(IServiceCollection services)
    {
        services.AddSingleton<IWeatherForecastService, WeatherForecastService>();
    }

    public void DefineEndpoints(WebApplication app, IWebHostEnvironment env)
    {
        app.MapGet("api/WeatherForecast", WeatherForecastAsync);
    }
}
```

### JSON-RPC 2.0 Response Format

API endpoints return responses in JSON-RPC 2.0 format, providing a standardized structure for success and error handling:

**Success Response:**
```json
{
  "jsonrpc": "2.0",
  "result": { /* your data */ },
  "id": "request-id"
}
```

**Error Response:**
```json
{
  "jsonrpc": "2.0",
  "error": {
    "code": -32603,
    "message": "Internal error",
    "data": { /* additional error details */ }
  },
  "id": "request-id"
}
```

**Standard Error Codes:**
- `-32700` - Parse error
- `-32600` - Invalid request
- `-32601` - Method not found
- `-32602` - Invalid method parameters
- `-32603` - Internal error

### Model Context Protocol (MCP)

The template includes MCP server capabilities, allowing AI assistants to interact with your API as tools. MCP endpoints are available at:
- HTTP Transport: `http://localhost:5243/mcp/sse`
- Stdio Transport: Available for local integrations

#### Available MCP Tools

**QueryAsync** - Get 5-day weather forecast
```
Queries weather forecast data and returns the next 5 days of results
```

**QueryByDaysAsync** - Get custom-day weather forecast
```
Parameters:
  - days: The number of days of weather forecast data to return
```

## API Endpoints

### Weather Forecast

**GET** `/api/WeatherForecast?id={requestId}`

Returns a 5-day weather forecast with temperature and conditions in JSON-RPC 2.0 format.

**Query Parameters:**
- `id` (optional) - JSON-RPC request ID. If not provided, a GUID will be generated.

**Response:**
```json
{
  "jsonrpc": "2.0",
  "result": [
    {
      "date": "2025-11-11",
      "temperatureC": 25,
      "temperatureF": 77,
      "summary": "Warm"
    }
  ],
  "id": "request-id-here"
```

### Root

**GET** `/`

Returns a simple greeting message.

**Response:**
```
Hello Template!
```

### Secure Endpoints

**GET** `/api/secure`

Returns secure data. Requires authentication.

**Response:**
```json
{
  "message": "This is a secured endpoint",
  "timestamp": "2025-11-21T12:00:00Z"
}
```

**GET** `/api/admin`

Returns admin data. Requires authentication with Admin role.

**Response:**
```json
{
  "message": "This is an admin-only endpoint",
  "timestamp": "2025-11-21T12:00:00Z"
}
```

### WebSocket

**WS** `/ws`

WebSocket endpoint for real-time weather updates. Sends weather forecast data at regular intervals.

**Connection:** `ws://localhost:5243/ws` or `wss://localhost:7145/ws`

**Messages Received:**
```json
{
  "date": "2025-11-21",
  "temperatureC": 25,
  "temperatureF": 77,
  "summary": "Warm"
}
```

## Configuration

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "your-domain.onmicrosoft.com",
    "TenantId": "your-tenant-id",
    "ClientId": "your-client-id",
    "Audience": "your-audience"
  }
}
```

### Microsoft Entra ID Setup

For detailed instructions on setting up Microsoft Entra ID authentication, see [ENTRA_SETUP.md](ENTRA_SETUP.md).

### Launch Profiles

The project includes three launch profiles:
- **http** - HTTP only on port 5243
- **https** - HTTPS on port 7145, HTTP on port 5243
- **IIS Express** - For IIS development

## Dependencies

- **[EndpointDefinition](https://www.nuget.org/packages/EndpointDefinition/)** (1.0.3) - Modular endpoint organization
- **[Microsoft.AspNetCore.OpenApi](https://www.nuget.org/packages/Microsoft.AspNetCore.OpenApi/)** (10.0.0) - OpenAPI support
- **[Microsoft.Identity.Web](https://www.nuget.org/packages/Microsoft.Identity.Web/)** (4.1.0) - Microsoft Entra ID authentication
- **[ModelContextProtocol.AspNetCore](https://www.nuget.org/packages/ModelContextProtocol.AspNetCore/)** (0.4.0-preview.3) - MCP integration
- **[Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/)** (10.0.1) - Swagger UI

## Development

### Adding New Endpoints

1. Create a new class implementing `IEndpointDefinition` in the `EndpointDefinitions` folder
2. Implement `DefineServices()` to register dependencies
3. Implement `DefineEndpoints()` to map routes
4. The endpoint will be automatically discovered and registered

### Adding MCP Tools

1. Create a static class in the `Tools` folder
2. Decorate the class with `[McpServerToolType]`
3. Create static methods decorated with `[McpServerTool]` and `[Description]`
4. Tools will be automatically discovered via `WithToolsFromAssembly()`

### Testing

Run tests using:
```bash
dotnet test
```

## Build and Publish

### Build for Release

```bash
dotnet build --configuration Release
```

### Publish

```bash
dotnet publish --configuration Release --output ./publish
```

## Environment Variables

- `ASPNETCORE_ENVIRONMENT` - Set to `Development`, `Staging`, or `Production`
- `ASPNETCORE_URLS` - Override default URLs (e.g., `http://localhost:8080`)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Author

**Wei Li**

## Version

2.0.0

---

For more information on ASP.NET Core Minimal APIs, visit the [official documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis).
