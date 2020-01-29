param(
    [Parameter(Mandatory = $true)]
    [string]
    $BuildDate,

    [Parameter(Mandatory = $true)]
    [string]
    $GitRef
)

$Path = Join-Path $PSScriptRoot 'src/Tetrominoes/AppVersion.cs'

$Content = Get-Content -Raw -Path $Path
$Content = $Content.Replace('DateTime.UtcNow.ToString("O")', "`"${BuildDate}`"")
$Content = $Content.Replace('CommitHash = "local"', "CommitHash = `"${GitRef}`"")

$Content | Set-Content -Path $Path
