Name "Tetrominoes.OpenGL"
OutFile "Tetrominoes-OpenGL-win-x86.exe"
InstallDir '$PROGRAMFILES32\JonCloud Studios\Tetrominoes\'

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
; InstallDirRegKey HKLM "Software\NSIS_Example2" "Install_Dir"

RequestExecutionLevel admin

;--------------------------------

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

Section "Game"

  SectionIn RO
  
  SetOutPath $INSTDIR
  File /r "src\Tetrominoes.OpenGL\bin\Release\netcoreapp3.1\win-x86\publish\*.*"
  
  ; Write the installation path into the registry
  ; WriteRegStr HKLM SOFTWARE\NSIS_Example2 "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Tetrominoes.OpenGL" "DisplayName" "Tetrominoes.OpenGL"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Tetrominoes.OpenGL" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Tetrominoes.OpenGL" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Tetrominoes.OpenGL" "NoRepair" 1
  WriteUninstaller "$INSTDIR\uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\Tetrominoes.OpenGL"
  CreateShortcut "$SMPROGRAMS\Tetrominoes.OpenGL\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortcut "$SMPROGRAMS\Tetrominoes.OpenGL\Tetrominoes.OpenGL.lnk" "$INSTDIR\Tetrominoes.OpenGL.exe" "" "$INSTDIR\Tetrominoes.OpenGL.exe" 0
  
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Tetrominoes.OpenGL"
  ;DeleteRegKey HKLM SOFTWARE\NSIS_Example2

  Delete "$INSTDIR\*.*"
  Delete "$SMPROGRAMS\Tetrominoes.OpenGL\*.*"
  RMDir "$SMPROGRAMS\Tetrominoes.OpenGL"
  RMDir "$INSTDIR"

SectionEnd
