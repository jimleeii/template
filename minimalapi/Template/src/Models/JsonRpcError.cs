namespace Template.Models;

/// <summary>
/// JSON-RPC 2.0 Error
/// </summary>
public class JsonRpcError
{
    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional error data (optional).
    /// </summary>
    public object? Data { get; set; }
}