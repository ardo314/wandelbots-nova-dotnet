using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace Wandelbots.Nova.Api;

public sealed class BearerTokenAuthenticationProvider(string accessToken) : IAuthenticationProvider
{
    public Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (string.IsNullOrWhiteSpace(accessToken))
            throw new InvalidOperationException("The NOVA access token must not be empty.");

        request.Headers.TryAdd("Authorization", $"Bearer {accessToken}");
        return Task.CompletedTask;
    }
}
