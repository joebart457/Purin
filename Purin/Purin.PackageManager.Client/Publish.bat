@echo OFF
rmdir bin\Publish /s /q
if %ERRORLEVEL% NEQ 0 EXIT /B 1 
dotnet publish -r linux-x64 --self-contained -o bin/Publish/Linux_x64
if %ERRORLEVEL% NEQ 0 EXIT /B 1 
dotnet publish -r win-x64 --self-contained -o bin/Publish/Win_x64
if %ERRORLEVEL% NEQ 0 EXIT /B 1 
dotnet publish -r osx-x64 --self-contained -o bin/Publish/OSX_x64
if %ERRORLEVEL% NEQ 0 EXIT /B 1 
PowerShell.exe -Command "cd bin/Publish; Compress-Archive -Path Linux_x64/* -DestinationPath Linux_x64.zip" 
if %ERRORLEVEL% NEQ 0 EXIT /B 1 
PowerShell.exe -Command "cd bin/Publish; Compress-Archive -Path Win_x64/* -DestinationPath Win_x64.zip" 
if %ERRORLEVEL% NEQ 0 EXIT /B 1 
PowerShell.exe -Command "cd bin/Publish; Compress-Archive -Path OSX_x64/* -DestinationPath OSX_x64.zip" 
if %ERRORLEVEL% NEQ 0 EXIT /B 1 