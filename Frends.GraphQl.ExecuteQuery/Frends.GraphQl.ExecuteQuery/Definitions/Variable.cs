namespace Frends.GraphQl.ExecuteQuery.Definitions;

/// <summary>
/// Class for variables used in GraphQl queries
/// </summary>
public class Variable
{
    /// <summary>
    /// Key of the variable
    /// </summary>
    public required string Key { get; init; }

    /// <summary>
    /// Value of the variable. Can be a string, number, boolean, object, or array.
    /// </summary>
    public required object? Value { get; init; }
}