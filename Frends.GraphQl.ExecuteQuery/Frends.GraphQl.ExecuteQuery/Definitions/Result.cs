using Newtonsoft.Json.Linq;

namespace Frends.GraphQl.ExecuteQuery.Definitions;

/// <summary>
/// Result of the task.
/// </summary>
public class Result
{
    internal Result(Header[] headers, JObject? data = null, bool success = true, Error? error = null)
    {
        Success = success;
        Data = data;
        Headers = headers;
        Error = error;
    }

    /// <summary>
    /// Indicates if the task completed successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Content of the returned response
    /// </summary>
    /// <example>foobar,foobar</example>
    public JObject? Data { get; set; }

    /// <summary>
    /// Array of response headers
    /// </summary>
    /// <example>Name: Header, Value: HeaderValue</example>
    public Header[] Headers { get; set; }

    /// <summary>
    /// Error that occurred during task execution.
    /// </summary>
    /// <example>object { string Message, object { Exception exception } AdditionalInfo }</example>
    public Error? Error { get; set; }
}
