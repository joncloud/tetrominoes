param(
    [Parameter(Mandatory = $true)]
    [string]
    $BuildDate,

    [Parameter(Mandatory = $true)]
    [string]
    $GitRef
)

If ($GitRef.StartsWith('refs/heads/')) {
    $GitRef = $GitRef.Substring(11)
}
If ($GitRef.StartsWith('refs/tags/')) {
    $GitRef = $GitRef.Substring(10)
}

$Path = Join-Path $PSScriptRoot 'src/Tetrominoes/AppVersion.cs'

$Content = Get-Content -Raw -Path $Path
$Content = $Content.Replace('DateTime.UtcNow.ToString("O")', "`"${BuildDate}`"")
$Content = $Content.Replace('CommitHash = "local"', "CommitHash = `"${GitRef}`"")

$Content | Set-Content -Path $Path
