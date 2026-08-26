# Wandelbots NOVA .NET client

An unofficial, generated C# client for the public [Wandelbots NOVA REST API](https://portal.wandelbots.io/docs/api/v2/ui/#/).

The repository archives the public OpenAPI and AsyncAPI specifications, generates the REST client with Microsoft Kiota, and checks Wandelbots' documentation daily for changes. Upstream changes are compiled and tested before an automated pull request is opened.

## Build

Requires the .NET 9 SDK.

```powershell
dotnet tool restore
./scripts/Generate-Client.ps1
dotnet build
dotnet test
```

## Usage

```csharp
using Wandelbots.Nova.Api;

var nova = NovaApiClientFactory.Create(
    "https://example.instance.wandelbots.io",
    Environment.GetEnvironmentVariable("NOVA_ACCESS_TOKEN")!);

var cells = await nova.Cells.GetAsync();
```

Never commit NOVA access tokens. Load them from environment variables or a secret store.

## Specifications

- [REST OpenAPI](https://portal.wandelbots.io/docs/api/v2/ui/public.openapi.yaml)
- [NATS/JetStream AsyncAPI](https://portal.wandelbots.io/docs/api/v2/async/asyncapi.yaml)

The REST document's API version is distinct from NOVA product release versions, so updates are detected by content rather than inferred version numbers.

## Package versioning

Package versions preserve the exact NOVA product release in their first three
components. An optional fourth component identifies client-only fixes. For example,
`26.6.2.1` is the first client revision for NOVA 26.6.2. The OpenAPI version is
recorded separately in package metadata. See [the versioning policy](docs/versioning.md)
for release rules and more examples.

## Known limitations

Kiota reports that several streaming endpoints use GET request bodies, which OpenAPI generation ignores. Those endpoints and the NATS/JetStream surface require dedicated transports. Kiota also reports one polymorphic upstream schema without a discriminator. CI catches resulting compilation failures.

GitHub Packages is the initial package registry. NuGet.org publishing remains
disabled until package ownership and signing are configured.

This project is not affiliated with or endorsed by Wandelbots GmbH.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md). Every issue or independent change must use a separate branch and pull request. Pull requests must remain unmerged until `ardo314` explicitly approves them.
