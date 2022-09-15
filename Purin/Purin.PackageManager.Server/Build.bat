@echo OFF
dotnet build -c Release -r ubuntu.20.04-x64  -o bin/Publish --no-self-contained
if %ERRORLEVEL% NEQ 0 EXIT /B 1 
docker build . -t joebart457/purin-pm-server:0.0.1