using Microsoft.Kiota.Http.HttpClientLibrary;

namespace Wandelbots.Nova.Api;

public static class NovaApiClientFactory
{
    public static NovaApiClient Create(string instanceUrl, string accessToken, HttpClient? httpClient = null)
    {
        if (!Uri.TryCreate(instanceUrl, UriKind.Absolute, out var instanceUri))
            throw new ArgumentException("A valid absolute NOVA instance URL is required.", nameof(instanceUrl));

        var authentication = new BearerTokenAuthenticationProvider(accessToken);
        var adapter = httpClient is null
            ? new HttpClientRequestAdapter(authentication)
            : new HttpClientRequestAdapter(authentication, httpClient: httpClient);
        adapter.BaseUrl = new Uri(instanceUri, "/api/v2").AbsoluteUri.TrimEnd('/');
        return new NovaApiClient(adapter);
    }
}
