using Frends.GraphQl.ExecuteQuery.Definitions;
using Newtonsoft.Json.Linq;

namespace Frends.GraphQl.ExecuteQuery.Tests;

public static class TestData
{
    public const string SimpleQuery = "{users{name}}";
    public const string AdvancedQuery = "query ($surname: String!) {users(surname: $surname) {name}}";
    private const string AdvancedOutputString = """ {"data": {"users": [{"name": "John"},{"name": "Marry"}]}} """;
    private const string SimpleOutputString = """ { "data": { "users": [{ "name": "John"},{ "name": "Marry"},{ "name": "Marry"}]}} """;

    public static JObject AdvancedOutputObject() => JObject.Parse(AdvancedOutputString);

    public static JObject SimpleOutputObject() => JObject.Parse(SimpleOutputString);

    public static Input InitialInput() => new()
    {
        Query = AdvancedQuery,
        Variables = [new Variable { Key = "surname", Value = "Doe" }],
    };

    public static Connection InitialConnection() => new()
    {
        EndpointUrl = "http://localhost:4000",
        Authentication = Authentication.None,
        Username = null,
        Password = null,
        BearerToken = null,
        Method = Method.Get,
        Headers = [],
    };

    public static Options InitialOptions() => new()
    {
        ThrowErrorOnFailure = false,
        ErrorMessageOnFailure = "test message",
        ConnectionTimeoutSeconds = 30,
        AllowInvalidCertificate = true,
    };
}
