using EndpointDefinition;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Template.EndpointDefinitions;

/// <summary>
/// The WebSocket endpoint definition.
/// </summary>
public class WebSocketEndpointDefinition : IEndpointDefinition
{
    /// <summary>
    /// The JSON serializer options instance.
    /// </summary>
    private static readonly JsonSerializerOptions JsonSerializerOptionsInstance = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    /// <summary>
    /// Defines the endpoints.
    /// </summary>
    /// <param name="app">The app.</param>
    /// <param name="env">The environment.</param>
    public void DefineEndpoints(WebApplication app, IWebHostEnvironment env)
    {
        // Enable WebSocket support
        app.UseWebSockets();

        // Map the WebSocket endpoint at the "ws" route
        // e.g., ws://localhost:5000/ws
        // This endpoint will handle WebSocket requests and return weather forecast data.
        app.MapGet("/ws", async (HttpContext context, IWeatherForecastService weatherService) =>
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await HandleWebSocketAsync(webSocket, weatherService);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        });
    }

    /// <summary>
    /// Defines the services.
    /// </summary>
    /// <param name="services">The services.</param>
    public void DefineServices(IServiceCollection services)
    {
        // Weather forecast service is already registered in WeatherForecastEndpointDefinition
    }

    /// <summary>
    /// Handles the WebSocket connection and sends weather forecast data.
    /// </summary>
    /// <param name="webSocket">The WebSocket.</param>
    /// <param name="weatherService">The weather forecast service.</param>
    private static async Task HandleWebSocketAsync(WebSocket webSocket, IWeatherForecastService weatherService)
    {
        var buffer = new byte[1024 * 4];
        
        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
                else
                {
                    // Parse the received message to determine how many days to forecast
                    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    
                    int days = 5; // Default
                    if (int.TryParse(receivedMessage, out var parsedDays) && parsedDays > 0)
                    {
                        days = parsedDays;
                    }
                    
                    // Get weather forecast data
                    var forecasts = await weatherService.GetWeatherForecastByDaysAsync(days);
                    
                    // Serialize the weather forecast data to JSON
                    var jsonResponse = JsonSerializer.Serialize(forecasts, JsonSerializerOptionsInstance);
                    var responseBytes = Encoding.UTF8.GetBytes(jsonResponse);
                    
                    // Send the weather forecast data back to the client
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(responseBytes),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
                }
            }
        }
        catch (WebSocketException)
        {
            // Handle WebSocket exceptions (connection closed unexpectedly, etc.)
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Error occurred", CancellationToken.None);
            }
        }
    }
}
