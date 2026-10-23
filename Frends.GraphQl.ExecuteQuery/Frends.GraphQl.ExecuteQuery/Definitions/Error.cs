namespace Frends.GraphQl.ExecuteQuery.Definitions;

/// <summary>
/// Error that occurred during the task.
/// </summary>
public class Error
{
    /// <summary>
    /// Summary of the error.
    /// </summary>
    /// <example>Unable to join strings.</example>
    required public string Message { get; set; }

    /// <summary>
    /// Additional information about the error.
    /// </summary>
    /// <example>object { Exception Exception }</example>
    required public dynamic AdditionalInfo { get; set; }
}
