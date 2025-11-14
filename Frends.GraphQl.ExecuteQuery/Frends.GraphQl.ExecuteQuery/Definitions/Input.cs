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

    /// <summary>
    /// The name of the operation to execute. Required if the query contains multiple operations.
    /// </summary>
    /// <example>GetUser</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string? OperationName { get; init; }

    /// <summary>
    /// Additional entries for protocol extensions. This is a map of additional metadata.
    /// </summary>
    /// <example>[{"key" : "persistedQuery", "value" : {"version": 1, "sha256Hash": "abc123"}}]</example>
    public Variable[] Extensions { get; init; } = [];
}
