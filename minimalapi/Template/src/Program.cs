using EndpointDefinition;
using Microsoft.Identity.Web;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole(static consoleLogOptions => consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace);

// Add Microsoft Entra authentication
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();

builder.Services.AddEndpointDefinitions(typeof(Program));
builder.Services.ConfigureHttpJsonOptions(static options => options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpointDefinitions(builder.Environment);

app.MapHealthChecks("/health");
app.MapGet("/", () => "Hello Template!");

await app.RunAsync();