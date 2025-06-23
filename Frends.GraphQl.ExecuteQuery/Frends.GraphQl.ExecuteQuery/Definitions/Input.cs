using System.ComponentModel.DataAnnotations;

namespace Frends.GraphQl.ExecuteQuery.Definitions;

/// <summary>
/// Essential parameters.
/// </summary>
public class Input
{
    /// <summary>
    /// GraphQl Query
    /// </summary>
    /// <example>query ($surname: String!) { users(surname: $surname) { id name } }</example>
    [DisplayFormat(DataFormatString = "Text")]
    public required string Query { get; set; }

    /// <summary>
    /// GraphQl variables for query
    /// </summary>
    /// <example>[{"key" : "surname", "value" : "Doe"}]</example>
    public Variable[] Variables { get; init; } = [];
}
