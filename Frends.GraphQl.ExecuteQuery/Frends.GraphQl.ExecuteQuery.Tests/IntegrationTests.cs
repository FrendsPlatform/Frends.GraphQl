using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using Frends.GraphQl.ExecuteQuery.Definitions;
using NUnit.Framework;

namespace Frends.GraphQl.ExecuteQuery.Tests;

[TestFixture]
public class IntegrationTests
{
    private readonly string dockerfileDir = Path.Combine(Directory.GetCurrentDirectory(), "docker");
    private IFutureDockerImage image;
    private IContainer container;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        image = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(dockerfileDir)
            .WithDockerfile("Dockerfile")
            .Build();
        await image.CreateAsync().ConfigureAwait(false);
        container = new ContainerBuilder()
            .WithImage(image)
            .WithName("graph-ql-tests")
            .WithCleanUp(true)
            .WithPortBinding(4000, 4000)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(4000))
            .Build();
        await container.StartAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (image != null) await image.DisposeAsync();
        if (container != null) await container.DisposeAsync();
    }

    [Test]
    public async Task GetRequestRunsCorrectly()
    {
        var result = await GraphQl.ExecuteQuery(TestData.InitialInput(), TestData.InitialConnection(), TestData.InitialOptions(), CancellationToken.None);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.EquivalentTo(TestData.AdvancedOutputObject()));
    }

    [Test]
    public async Task PostRequestRunsCorrectly()
    {
        var con = TestData.InitialConnection();
        con.Method = Method.Post;

        var result = await GraphQl.ExecuteQuery(TestData.InitialInput(), con, TestData.InitialOptions(), CancellationToken.None);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.EquivalentTo(TestData.AdvancedOutputObject()));
    }

    [Test]
    public async Task SimpleQueryRunsCorrectly()
    {
        var input = TestData.InitialInput();
        input.Query = TestData.SimpleQuery;

        var result = await GraphQl.ExecuteQuery(input, TestData.InitialConnection(), TestData.InitialOptions(), CancellationToken.None);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.EquivalentTo(TestData.SimpleOutputObject()));
    }

    [Test]
    public async Task QueryWithVariablesRunsCorrectly()
    {
        var input = TestData.InitialInput();
        input.Query = TestData.AdvancedQuery;

        var result = await GraphQl.ExecuteQuery(input, TestData.InitialConnection(), TestData.InitialOptions(), CancellationToken.None);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.EquivalentTo(TestData.AdvancedOutputObject()));
    }

    // We can't directly check the header value,
    // but test env is set up with optional authentication,
    // so if it's added and is incorrect, it will show an error with passed values in a message
    [Test]
    public async Task BasicAuthHeaderAddedCorrectly()
    {
        const string invalidUsername = "invalid-user";
        const string invalidPassword = "invalid-secret";
        var con = TestData.InitialConnection();
        con.Authentication = Authentication.Basic;
        con.Username = invalidUsername;
        con.Password = invalidPassword;

        var result = await GraphQl.ExecuteQuery(TestData.InitialInput(), con, TestData.InitialOptions(), CancellationToken.None);

        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.ToString(), Contains.Substring(invalidUsername));
        Assert.That(result.Data.ToString(), Contains.Substring(invalidPassword));
    }

    // We can't directly check the header value,
    // but test env is set up with optional authentication,
    // so if it's added and is incorrect, it will show an error with passed values in a message
    [Test]
    public async Task OAuthHeaderAddedCorrectly()
    {
        const string invalidToken = "invalid-oauth-token";
        var con = TestData.InitialConnection();
        con.Authentication = Authentication.OAuth;
        con.BearerToken = invalidToken;

        var result = await GraphQl.ExecuteQuery(TestData.InitialInput(), con, TestData.InitialOptions(), CancellationToken.None);

        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.ToString(), Contains.Substring(invalidToken));
    }

    // We can't directly check the header value,
    // but test env is set up with an optional header,
    // so if it's added and its value is incorrect, it will show an error with passed values in a message
    [Test]
    public async Task CustomHeadersAddedCorrectly()
    {
        const string headerKey = "Foo";
        const string headerValue = "NotBar";
        var con = TestData.InitialConnection();
        con.Headers =
        [
            new Header { Name = headerKey, Value = headerValue },
        ];

        var result = await GraphQl.ExecuteQuery(TestData.InitialInput(), con, TestData.InitialOptions(), CancellationToken.None);

        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.ToString(), Contains.Substring(headerKey));
        Assert.That(result.Data.ToString(), Contains.Substring(headerValue));
    }

    [Test]
    public Task WhenThrowOnFailureFlagIsTrue_ThrowsError()
    {
        var con = TestData.InitialConnection();
        con.EndpointUrl = string.Empty;
        var opt = TestData.InitialOptions();
        opt.ThrowErrorOnFailure = true;

        Assert.ThrowsAsync<Exception>(Action);

        return Task.CompletedTask;

        async Task Action() => await GraphQl.ExecuteQuery(TestData.InitialInput(), con, opt, CancellationToken.None);
    }

    [Test]
    public async Task WhenThrowOnFailureFlagIsFalse_ReturnsResult()
    {
        var con = TestData.InitialConnection();
        con.EndpointUrl = string.Empty;
        var opt = TestData.InitialOptions();
        opt.ThrowErrorOnFailure = false;

        var result = await GraphQl.ExecuteQuery(TestData.InitialInput(), con, opt, CancellationToken.None);

        Assert.That(result.Success, Is.False);
    }

    // We can't directly check if ConnectionTimeoutSeconds is set correctly,
    // but we can check if it fails when it is set up to 0 and test env sleeps for 1 second
    [Test]
    public async Task FailsWhenConnectionTimeoutEnds()
    {
        var opt = TestData.InitialOptions();
        opt.ConnectionTimeoutSeconds = 0;

        var result = await GraphQl.ExecuteQuery(TestData.InitialInput(), TestData.InitialConnection(), opt, CancellationToken.None);

        Assert.That(result.Success, Is.False);
    }
}
