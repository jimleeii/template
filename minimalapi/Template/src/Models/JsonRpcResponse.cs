namespace Template.Models;

/// <summary>
/// JSON-RPC 2.0 Response
/// </summary>
/// <typeparam name="T">The type of the result</typeparam>
public class JsonRpcResponse<T>
{
    /// <summary>
    /// Gets or sets the JSON-RPC version (must be "2.0").
    /// </summary>
    public string Jsonrpc { get; set; } = "2.0";

    /// <summary>
    /// Gets or sets the result (required on success).
    /// </summary>
    public T? Result { get; set; }

    /// <summary>
    /// Gets or sets the error (required on error).
    /// </summary>
    public JsonRpcError? Error { get; set; }

    /// <summary>
    /// Gets or sets the request ID.
    /// </summary>
    public object? Id { get; set; }
}