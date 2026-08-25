$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$specs = @{
    'public.openapi.yaml' = 'https://portal.wandelbots.io/docs/api/v2/ui/public.openapi.yaml'
    'asyncapi.yaml' = 'https://portal.wandelbots.io/docs/api/v2/async/asyncapi.yaml'
}

foreach ($entry in $specs.GetEnumerator()) {
    Invoke-WebRequest -Uri $entry.Value -OutFile (Join-Path $root "specs/$($entry.Key)")
    Write-Host "Updated $($entry.Key)"
}
