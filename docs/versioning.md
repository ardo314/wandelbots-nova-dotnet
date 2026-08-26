# Package versioning

`Wandelbots.Nova.Api` uses NuGet's four-part version model to preserve the exact
supported NOVA product release while allowing fixes to the generated .NET client.

```text
<NOVA major>.<NOVA minor>.<NOVA patch>[.<client revision>][-<prerelease>]
```

The first three components always identify the NOVA product release. The optional
fourth component is the client revision for that release. NuGet normalizes a zero
fourth component away, so the first stable client has no fourth component.

Examples:

| Package version | Meaning |
| --- | --- |
| `26.6.2` | Initial stable client for NOVA 26.6.2 |
| `26.6.2.1` | First client-only fix for NOVA 26.6.2 |
| `26.6.2.2` | Second client-only fix for NOVA 26.6.2 |
| `26.6.3-beta.1` | First beta of the initial client for NOVA 26.6.3 |
| `26.6.3.1-rc.1` | First release candidate of client revision 1 for NOVA 26.6.3 |

## Authoritative versions

`eng/Compatibility.props` is the release manifest. `NovaProductVersion` records
the NOVA product release against which the archived specifications were obtained.
It is the authoritative compatibility version used in the package number.

The OpenAPI document's independent `info.version` is recorded separately as
`OpenApiVersion`. It does not determine the NuGet package version. Before a
release, CI verifies that `OpenApiVersion` agrees with the archived
`specs/public.openapi.yaml` document.

Both values are included in the package release notes and as assembly metadata.

## Release rules

- A new NOVA patch release resets the client revision: `26.6.2.3` is followed by
  `26.6.3` when the compatibility target changes to NOVA 26.6.3.
- A client-only fix increments the fourth component.
- Regenerating from unchanged specifications without changing the resulting
  package does not produce a release.
- Regenerating from a changed specification for the same NOVA release increments
  the client revision.
- Prereleases append a standard NuGet suffix to the intended stable version.
- Release tags have a `v` prefix and must exactly match the declared NOVA version.

The previously published `0.1.0` package predates this policy. It remains a legacy
package whose NOVA compatibility cannot be inferred from its version. Consumers
should migrate to the first `26.6.0` or later policy-compliant release.
