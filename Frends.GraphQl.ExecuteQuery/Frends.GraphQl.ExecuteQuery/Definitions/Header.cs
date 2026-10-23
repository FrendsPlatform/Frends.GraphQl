namespace Frends.GraphQl.ExecuteQuery.Definitions;

/// <summary>
/// Request header.
/// </summary>
public class Header
{
    /// <summary>
    /// Name of header.
    /// </summary>
    /// <example>Content-Type</example>
    public required string Name { get; init; }

    /// <summary>
    /// Value of the header.
    /// </summary>
    /// <example>application/json</example>
    public string Value { get; set; } = string.Empty;
}