using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.GraphQl.ExecuteQuery.Definitions;

/// <summary>
/// Additional parameters.
/// </summary>
public class Options
{
    /// <summary>
    /// Whether to throw an error on failure.
    /// </summary>
    /// <example>false</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Overrides the error message on failure.
    /// </summary>
    /// <example>Custom error message</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ErrorMessageOnFailure { get; init; } = string.Empty;

    /// <summary>
    /// Timeout in seconds to be used for the connection and operation.
    /// </summary>
    /// <example>30</example>
    [DefaultValue(30)]
    public int ConnectionTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Do not throw an exception on a certificate error.
    /// </summary>
    /// <example>true</example>
    public bool AllowInvalidCertificate { get; init; }
}
