namespace Frends.GraphQl.ExecuteQuery.Definitions;

/// <summary>
/// Request header.
/// </summary>
public class Header
{
    /// <summary>
    /// Name of header.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Value of the header.
    /// </summary>
    public string Value { get; set; } = string.Empty;
}