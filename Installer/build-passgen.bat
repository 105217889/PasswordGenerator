@echo off
setlocal
cd /d "%~dp0"

set BIN=..\PassGen\bin\Release
set MSI=output\PasswordGenerator-1.0.0.msi

if not exist output mkdir output

echo  Source binaries:
dir /b "%BIN%\*.exe" "%BIN%\*.dll" "%BIN%\*.config" 2>nul

echo  STEP 1: candle.exe  (compile .wxs -^> .wixobj)
"%WIX%bin\candle.exe" PassGen.wxs ^
    -dBinDir="%BIN%" ^
    -ext WixUIExtension ^
    -ext WixNetFxExtension ^
    -out output\PassGen.wixobj
if errorlevel 1 goto fail

echo  STEP 2: light.exe   (link .wixobj -^> .msi)
"%WIX%bin\light.exe" output\PassGen.wixobj ^
    -ext WixUIExtension ^
    -ext WixNetFxExtension ^
    -out "%MSI%"
if errorlevel 1 goto fail

echo  BUILD SUCCEEDED
dir output\*.msi
certutil -hashfile "%MSI%" SHA256
exit /b 0

:fail
echo.
echo *** BUILD FAILED ***
exit /b 1