namespace Frends.GraphQl.ExecuteQuery.Definitions;

/// <summary>
/// Request header.
/// </summary>
public class Header
{
    /// <summary>
    /// Name of header.
    /// <example>Content-Type</example>
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Value of the header.
    /// <example>application/json</example>
    /// </summary>
    public string Value { get; set; } = string.Empty;
}