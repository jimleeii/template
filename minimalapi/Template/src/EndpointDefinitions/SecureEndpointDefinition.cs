using EndpointDefinition;

namespace Template.EndpointDefinitions;

/// <summary>
/// The secure endpoint definition.
/// </summary>
public class SecureEndpointDefinition : IEndpointDefinition
{
    /// <summary>
    /// Defines the endpoints.
    /// </summary>
    /// <param name="app">The app.</param>
    /// <param name="env">The environment.</param>
    public void DefineEndpoints(WebApplication app, IWebHostEnvironment env)
    {
        app.MapGet("/api/secure", () => new
        {
            Message = "This is a secured endpoint",
            Timestamp = DateTime.UtcNow
        })
        .RequireAuthorization()
        .WithName("GetSecureData");

        app.MapGet("/api/admin", () => new
        {
            Message = "This is an admin-only endpoint",
            Timestamp = DateTime.UtcNow
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin"))
        .WithName("GetAdminData");
    }

    /// <summary>
    /// Defines the services.
    /// </summary>
    /// <param name="services">The services.</param>
    public void DefineServices(IServiceCollection services)
    {
        // No additional services needed for this endpoint
    }
}
