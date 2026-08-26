using System.Reflection;

namespace Wandelbots.Nova.Api.Tests;

public class PackageMetadataTests
{
    [Fact]
    public void AssemblyRecordsCompatibilityVersions()
    {
        var metadata = typeof(NovaApiClientFactory).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .ToDictionary(attribute => attribute.Key, attribute => attribute.Value);

        Assert.Equal("26.6.0", metadata["NovaProductVersion"]);
        Assert.Equal("2.6.0", metadata["OpenApiVersion"]);
    }
}
