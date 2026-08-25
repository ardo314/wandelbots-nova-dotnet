namespace Wandelbots.Nova.Api.Tests;

public sealed class NovaApiClientFactoryTests
{
    [Fact]
    public void CreatesClientForValidInstanceUrl() =>
        Assert.NotNull(NovaApiClientFactory.Create("https://example.instance.wandelbots.io", "test-token"));

    [Fact]
    public void RejectsRelativeInstanceUrl() =>
        Assert.Throws<ArgumentException>(() => NovaApiClientFactory.Create("relative", "test-token"));
}
