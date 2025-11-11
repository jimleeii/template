using EndpointDefinition;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole(static consoleLogOptions => consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace);
builder.Services.AddEndpointDefinitions(typeof(Program));
builder.Services.ConfigureHttpJsonOptions(static options => options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals);

var app = builder.Build();
app.UseEndpointDefinitions(builder.Environment);

app.MapGet("/", () => "Hello Template!");

await app.RunAsync();