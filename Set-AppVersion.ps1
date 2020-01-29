param(
    [Parameter(Mandatory = $true)]
    [string]
    $BuildDate,

    [Parameter(Mandatory = $true)]
    [string]
    $GitRef
)

# Ignore for the refs portion, and just get the important information.
If ($GitRef.StartsWith('refs/heads/')) {
    $GitRef = $GitRef.Substring(11)
}
If ($GitRef.StartsWith('refs/tags/')) {
    $GitRef = $GitRef.Substring(10)
}

# Update the code file for easy access to the information.
$Path = Join-Path $PSScriptRoot 'src/Tetrominoes/AppVersion.cs'
$Content = Get-Content -Raw -Path $Path
$Content = $Content.Replace('DateTime.UtcNow.ToString("O")', "`"${BuildDate}`"")
$Content = $Content.Replace('GitRef = "local"', "GitRef = `"${GitRef}`"")
$Content | Set-Content -Path $Path

# When the git ref is a version, make sure the
# binaries are also tagged with that version.
$Version = $null
If ([System.Version]::TryParse($GitRef, [ref]$Version)) {
    # Hacky way of setting version, should probably be done
    # through dotnet cli instead, but lazy.
    $Projects = @(
        'src/Tetrominoes/Tetrominoes.csproj', 
        'src/Tetrominoes.OpenGL/Tetrominoes.OpenGL.csproj'
    )
    $Projects | ForEach-Object {
        $Content = Get-Content -Raw -Path $_
        $Content = $Content.Replace(
            '<Version>0.0.0</Version>', 
            "<Version>${Version}</Version>"
        )
        $Content | Set-Content -Path $_
    }
}
