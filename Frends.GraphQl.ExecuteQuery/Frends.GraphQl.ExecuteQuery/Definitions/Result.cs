using Newtonsoft.Json.Linq;

namespace Frends.GraphQl.ExecuteQuery.Definitions;

/// <summary>
/// Result of the task.
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="headers">Response headers.</param>
    /// <param name="data">Response data.</param>
    /// <param name="success">Indicates if the task completed successfully.</param>
    /// <param name="error">Error that occurred during task execution.</param>
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
