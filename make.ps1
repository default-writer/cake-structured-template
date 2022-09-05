$ErrorActionPreference = 'Stop'

Set-Location -LiteralPath $PSScriptRoot

dotnet cake @args
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
