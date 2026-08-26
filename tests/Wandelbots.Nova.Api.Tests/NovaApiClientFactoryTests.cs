namespace Wandelbots.Nova.Api.Tests;

public sealed class NovaApiClientFactoryTests
{
    [Fact]
    public void CreatesClientForValidInstanceUrl() =>
        Assert.NotNull(NovaApiClientFactory.Create("https://example.instance.wandelbots.io", "test-token"));

    [Fact]
    public void RejectsRelativeInstanceUrl() =>
        Assert.Throws<ArgumentException>(() => NovaApiClientFactory.Create("relative", "test-token"));

    [Fact]
    public void RejectsEmptyAccessToken() =>
        Assert.Throws<ArgumentException>(() => NovaApiClientFactory.Create("https://example.instance.wandelbots.io", ""));

    [Fact]
    public void CreatesAnonymousClientForValidInstanceUrl() =>
        Assert.NotNull(NovaApiClientFactory.CreateAnonymous("http://localhost"));

    [Fact]
    public Task AnonymousClientDoesNotSendAuthorizationHeader() =>
        AssertAuthorizationHeaderAsync(accessToken: null, expectedScheme: null, expectedParameter: null);

    [Fact]
    public Task AuthenticatedClientSendsBearerToken() =>
        AssertAuthorizationHeaderAsync("test-token", "Bearer", "test-token");

    private static async Task AssertAuthorizationHeaderAsync(
        string? accessToken,
        string? expectedScheme,
        string? expectedParameter)
    {
        var handler = new RecordingHandler();
        using var httpClient = new HttpClient(handler);
        var client = accessToken is null
            ? NovaApiClientFactory.CreateAnonymous("https://example.instance.wandelbots.io", httpClient)
            : NovaApiClientFactory.Create("https://example.instance.wandelbots.io", accessToken, httpClient);

        await client.Cells.GetAsync();

        Assert.Equal(expectedScheme, handler.AuthorizationScheme);
        Assert.Equal(expectedParameter, handler.AuthorizationParameter);
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        public string? AuthorizationScheme { get; private set; }
        public string? AuthorizationParameter { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            AuthorizationScheme = request.Headers.Authorization?.Scheme;
            AuthorizationParameter = request.Headers.Authorization?.Parameter;
            return Task.FromResult(new HttpResponseMessage(global::System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent("[]", global::System.Text.Encoding.UTF8, "application/json")
            });
        }
    }
}
