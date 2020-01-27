param(
  [Parameter(Mandatory = $True)]
  [string]
  $Path,
  
  [string]
  $Platform = 'DesktopGL',
  
  [string]
  $Profile = 'Reach',
  
  [switch]
  $Compress,
  
  [switch]
  $Force
)

If ((Test-Path $Path) -and (-not ($Force))) {
    Throw "File ${Path} already exists. Use -Force to override."
}

"
#----------------------------- Global Properties ----------------------------#

/outputDir:bin
/intermediateDir:obj
/platform:${Platform}
/config:
/profile:${Profile}
/compress:${Compress}

#-------------------------------- References --------------------------------#

#---------------------------------- Content ---------------------------------#" | Set-Content -Path $Path

Push-Location (Split-Path $Path -Parent)
$XnbNames = New-Object 'System.Collections.Generic.HashSet[string]'
Get-ChildItem -Exclude @('bin', 'obj') -Directory | Get-ChildItem -File -Recurse | ForEach-Object {
    $RelativePath = (Resolve-Path -Relative $_.FullName).Substring(2).Replace('\', '/')
    
    $Extension = [System.IO.Path]::GetExtension($_.FullName)
    switch ($Extension) {
      '.hlsl' {
        "
#begin ${RelativePath}
/importer:EffectImporter
/processor:EffectProcessor
/processorParam:DebugMode=Auto
/build:${RelativePath}" | Add-Content $Path
      }
      '.png' {
          "
#begin ${RelativePath}
/importer:TextureImporter
/processor:TextureProcessor
/processorParam:ColorKeyColor=255,0,255,255
/processorParam:ColorKeyEnabled=True
/processorParam:GenerateMipmaps=False
/processorParam:PremultiplyAlpha=True
/processorParam:ResizeToPowerOfTwo=False
/processorParam:MakeSquare=False
/processorParam:TextureFormat=Color
/build:${RelativePath}" | Add-Content $Path
      }
      '.spritefont' {
          "
#begin ${RelativePath}
/importer:FontDescriptionImporter
/processor:FontDescriptionProcessor
/processorParam:PremultiplyAlpha=True
/processorParam:TextureFormat=Compressed
/build:${RelativePath}" | Add-Content $Path
      }
      '.xml' { 
          "
#begin ${RelativePath}
/importer:XmlImporter
/processor:PassThroughProcessor
/build:${RelativePath}" | Add-Content $Path
      }
      '.wav' {
          "
#begin ${RelativePath}
/importer:WavImporter
/processor:SoundEffectProcessor
/processorParam:Quality=Best
/build:${RelativePath}" | Add-Content $Path
      }
      '.ogg' {
        "
#begin ${RelativePath}
/importer:OggImporter
/processor:SongProcessor
/processorParam:Quality=Best
/build:${RelativePath}" | Add-Content $Path
      }
      Default {
        Write-Warning "${RelativePath} is ignored due to file type"
        Return
      }
    }
    $XnbPath = Join-Path (Split-Path -Parent $RelativePath) ([System.IO.Path]::GetFileNameWithoutExtension($RelativePath))
    If (-not $XnbNames.Add($XnbPath)) {
      Write-Warning "${RelativePath} is a duplicate"
    }
}
Pop-Location