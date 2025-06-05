using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Frends.GraphQl.ExecuteQuery.Definitions;
using Frends.GraphQl.ExecuteQuery.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Frends.GraphQl.ExecuteQuery;

/// <summary>
/// Task class to execute a query in GraphQl.
/// </summary>
public static class GraphQl
{
    /// <summary>
    /// GraphQl query will be executed after running this task.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends-GraphQl-ExecuteQuery)
    /// </summary>
    /// <param name="input">Contains query and variables.</param>
    /// <param name="connection">Connection info.</param>
    /// <param name="options">Additional parameters for handling error and timeouts</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>object { Header[] headers, JObject data, bool success, Error error }</returns>
    public static async Task<Result> ExecuteQuery(
        [PropertyTab] Input input,
        [PropertyTab] Connection connection,
        [PropertyTab] Options options,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(connection.EndpointUrl)) throw new ArgumentNullException(connection.EndpointUrl, "Url can not be empty.");
            var httpClient = CreateHttpClient(connection, options);
            var request = PrepareRequest(input, connection);

            var response = await httpClient.SendAsync(request, cancellationToken);
            var responseHeaders = GetResponseHeaders(response.Headers, response.Content.Headers);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var contentObject = JObject.Parse(content);

            return new Result(responseHeaders, contentObject);
        }
        catch (Exception ex)
        {
            return ErrorHandler.Handle(ex, options.ThrowErrorOnFailure, options.ErrorMessageOnFailure);
        }
    }

    private static HttpClient CreateHttpClient(Connection connection, Options options)
    {
        var handler = new HttpClientHandler();
        if (options.AllowInvalidCertificate)
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
        var httpClient = new HttpClient(handler);
        httpClient.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(options.ConnectionTimeoutSeconds));
        foreach (var header in connection.Headers)
        {
            httpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
        }

        var authHeader = CreateAuthenticationHeader(connection);
        if (authHeader is not null) httpClient.DefaultRequestHeaders.Add(authHeader.Name, authHeader.Value);
        return httpClient;
    }

    private static Header? CreateAuthenticationHeader(Connection connection)
    {
        var authHeader = new Header { Name = "Authorization" };
        switch (connection.Authentication)
        {
            case Authentication.Basic:
                authHeader.Value = $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{connection.Username}:{connection.Password}"))}";
                return authHeader;
            case Authentication.OAuth:
                authHeader.Value = $"Bearer {connection.BearerToken}";
                return authHeader;
            case Authentication.None:
            default:
                return null;
        }
    }

    private static HttpRequestMessage PrepareRequest(Input input, Connection connection)
    {
        switch (connection.Method)
        {
            case Method.Get:
                var encodedQuery = HttpUtility.UrlEncode(input.Query);
                var variablesString = "{";

                foreach (var variable in input.Variables)
                {
                    variablesString += $"\"{variable.Key}\" : \"{variable.Value}\",";
                }

                variablesString = variablesString.TrimEnd(',');
                variablesString += "}";

                var encodedVariables = HttpUtility.UrlEncode(variablesString);
                var uri = new Uri($"{connection.EndpointUrl}?query={encodedQuery}&variables={encodedVariables}");
                var getRequest = new HttpRequestMessage(HttpMethod.Get, uri);
                return getRequest;
            case Method.Post:
                var variablesDictionary = input.Variables.ToDictionary(v => v.Key, v => v.Value);
                var payload = new
                {
                    query = input.Query,
                    variables = variablesDictionary,
                };
                var json = JsonConvert.SerializeObject(payload);
                var postRequest = new HttpRequestMessage(HttpMethod.Post, connection.EndpointUrl)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json"),
                };
                return postRequest;
            default:
                throw new ArgumentOutOfRangeException(connection.Method.ToString());
        }
    }

    private static Header[] GetResponseHeaders(HttpResponseHeaders responseMessageHeaders, HttpContentHeaders contentHeaders)
    {
        var responseHeaders = responseMessageHeaders.ToDictionary(h => h.Key, h => string.Join(";", h.Value));
        var allHeaders = contentHeaders.ToDictionary(h => h.Key, h => string.Join(";", h.Value));
        responseHeaders.ToList().ForEach(x => allHeaders[x.Key] = x.Value);

        var result = new List<Header>();
        foreach (var header in allHeaders)
        {
            result.Add(new Header
            {
                Name = header.Key,
                Value = header.Value,
            });
        }

        return result.ToArray();
    }
}
