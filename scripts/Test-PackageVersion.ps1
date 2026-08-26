param(
    [Parameter(Mandatory = $true)]
    [string] $Tag,
    [string] $CompatibilityFile = (Join-Path $PSScriptRoot '../eng/Compatibility.props'),
    [string] $OpenApiFile = (Join-Path $PSScriptRoot '../specs/public.openapi.yaml')
)

$ErrorActionPreference = 'Stop'

if ($Tag -notmatch '^v(?<nova>(?:0|[1-9]\d*)\.(?:0|[1-9]\d*)\.(?:0|[1-9]\d*))(?:\.(?<revision>[1-9]\d*))?(?<suffix>-(?:[0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*))?$') {
    throw "Tag '$Tag' is invalid. Expected v<NOVA major>.<NOVA minor>.<NOVA patch>[.<client revision>][-<prerelease>]."
}

[xml] $compatibility = Get-Content -Raw $CompatibilityFile
$novaProductVersion = [string] $compatibility.Project.PropertyGroup.NovaProductVersion
$declaredOpenApiVersion = [string] $compatibility.Project.PropertyGroup.OpenApiVersion

if ($Matches.nova -ne $novaProductVersion) {
    throw "Tag '$Tag' targets NOVA $($Matches.nova), but eng/Compatibility.props declares NOVA $novaProductVersion."
}

$openApiVersionMatch = Select-String -Path $OpenApiFile -Pattern '^  version:\s*(?<version>\S+)\s*$' | Select-Object -First 1
if ($null -eq $openApiVersionMatch) {
    throw "Could not read info.version from '$OpenApiFile'."
}

$archivedOpenApiVersion = $openApiVersionMatch.Matches[0].Groups['version'].Value
if ($archivedOpenApiVersion -ne $declaredOpenApiVersion) {
    throw "The archived OpenAPI version is $archivedOpenApiVersion, but eng/Compatibility.props declares $declaredOpenApiVersion."
}

$packageVersion = $Tag.Substring(1)
$clientRevision = if ($Matches.revision) { $Matches.revision } else { '0' }

[pscustomobject]@{
    PackageVersion = $packageVersion
    NovaProductVersion = $novaProductVersion
    OpenApiVersion = $declaredOpenApiVersion
    ClientRevision = $clientRevision
} | ConvertTo-Json -Compress
