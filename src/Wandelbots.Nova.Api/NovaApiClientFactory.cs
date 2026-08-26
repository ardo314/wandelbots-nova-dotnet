using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace Wandelbots.Nova.Api;

public static class NovaApiClientFactory
{
    /// <summary>Creates a NOVA API client that sends a bearer token with every request.</summary>
    public static NovaApiClient Create(string instanceUrl, string accessToken, HttpClient? httpClient = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        return Create(instanceUrl, new BearerTokenAuthenticationProvider(accessToken), httpClient);
    }

    /// <summary>
    /// Creates a NOVA API client without authentication for instances that explicitly allow anonymous access.
    /// </summary>
    public static NovaApiClient CreateAnonymous(string instanceUrl, HttpClient? httpClient = null) =>
        Create(instanceUrl, new AnonymousAuthenticationProvider(), httpClient);

    private static NovaApiClient Create(
        string instanceUrl,
        IAuthenticationProvider authentication,
        HttpClient? httpClient)
    {
        if (!Uri.TryCreate(instanceUrl, UriKind.Absolute, out var instanceUri))
            throw new ArgumentException("A valid absolute NOVA instance URL is required.", nameof(instanceUrl));

        var adapter = httpClient is null
            ? new HttpClientRequestAdapter(authentication)
            : new HttpClientRequestAdapter(authentication, httpClient: httpClient);
        adapter.BaseUrl = new Uri(instanceUri, "/api/v2").AbsoluteUri.TrimEnd('/');
        return new NovaApiClient(adapter);
    }
}
