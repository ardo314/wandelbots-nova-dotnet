$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot

Push-Location $root
try {
    dotnet tool restore
    if ($LASTEXITCODE -ne 0) { throw 'Tool restore failed.' }
    dotnet kiota generate --openapi specs/public.openapi.yaml --language CSharp --class-name NovaApiClient --namespace-name Wandelbots.Nova.Api --output src/Wandelbots.Nova.Api/Generated --clean-output
    if ($LASTEXITCODE -ne 0) { throw 'Client generation failed.' }
}
finally { Pop-Location }
