namespace Template.Models;

/// <summary>
/// JSON-RPC 2.0 Error Codes
/// </summary>
public static class JsonRpcErrorCodes
{
    /// <summary>
    /// Parse error.
    /// </summary>
    public const int ParseError = -32700;

    /// <summary>
    /// Invalid request.
    /// </summary>
    public const int InvalidRequest = -32600;

    /// <summary>
    /// Method not found.
    /// </summary>
    public const int MethodNotFound = -32601;

    /// <summary>
    /// Invalid method parameters.
    /// </summary>
    public const int InvalidMethodParameters = -32602;

    /// <summary>
    /// Internal error.
    /// </summary>
    public const int InternalError = -32603;
}