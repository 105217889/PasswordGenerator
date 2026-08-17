@echo off
setlocal
cd /d "%~dp0"

if not exist output mkdir output

echo  STEP 1: candle.exe  (compile .wxs -^> .wixobj)
"%WIX%bin\candle.exe" SampleApp.wxs ^
    -dBinDir="..\SampleApp\bin\Release" ^
    -out output\SampleApp.wixobj
if errorlevel 1 goto fail

echo  STEP 2: light.exe   (link .wixobj -^> .msi)
"%WIX%bin\light.exe" output\SampleApp.wixobj ^
    -out output\SampleAppSetup.msi
if errorlevel 1 goto fail

echo  BUILD SUCCEEDED
dir output\*.msi
exit /b 0

:fail
echo *** BUILD FAILED ***
exit /b 1