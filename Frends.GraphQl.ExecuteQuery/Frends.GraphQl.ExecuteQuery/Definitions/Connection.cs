using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.GraphQl.ExecuteQuery.Definitions;

/// <summary>
/// Connection parameters.
/// </summary>
public class Connection
{
    /// <summary>
    /// Url to GraphQl server.
    /// </summary>
    /// <example>http://localhost:4000</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string EndpointUrl { get; set; } = string.Empty;

    /// <summary>
    /// Method of authenticating a request
    /// </summary>
    /// <example>OAuth</example>
    public Authentication Authentication { get; set; }

    /// <summary>
    /// If Basic Authentication is selected, you should use a username
    /// </summary>
    /// <example>Username</example>
    [UIHint(nameof(Authentication), "", Authentication.Basic)]
    public string? Username { get; set; }

    /// <summary>
    /// Password for the username
    /// </summary>
    /// <example>Password123</example>
    [PasswordPropertyText]
    [UIHint(nameof(Authentication), "", Authentication.Basic)]
    public string? Password { get; set; }

    /// <summary>
    /// Bearer token to be used for request. Token will be added as an Authorization header.
    /// </summary>
    /// <example>Token123</example>
    [PasswordPropertyText]
    [UIHint(nameof(Authentication), "", Authentication.OAuth)]
    public string? BearerToken { get; set; }

    /// <summary>
    /// Path to the client certificate file (PFX/P12 format).
    /// </summary>
    /// <example>C:\certs\client.pfx</example>
    [DisplayFormat(DataFormatString = "Text")]
    [UIHint(nameof(Authentication), "", Authentication.ClientCertificate)]
    public string? CertificatePath { get; set; }

    /// <summary>
    /// Password for the client certificate file. Leave empty if the certificate has no password.
    /// </summary>
    /// <example>CertPassword123</example>
    [PasswordPropertyText]
    [UIHint(nameof(Authentication), "", Authentication.ClientCertificate)]
    public string? CertificatePassword { get; set; }

    /// <summary>
    /// The HTTP Method to be used with the request.
    /// </summary>
    /// <example>GET</example>
    [DefaultValue(Method.Post)]
    public Method Method { get; set; } = Method.Post;

    /// <summary>
    /// List of HTTP headers to be added to the request.
    /// </summary>
    /// <example>Name: Header, Value: HeaderValue</example>
    public Header[] Headers { get; set; } = [];
}
