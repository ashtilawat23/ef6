@ECHO OFF

:: This command launches a Visual Studio solution with environment variables required to use a local version of the .NET Core SDK.

:: This tells .NET Core to use .dotnet\dotnet.exe
SET DOTNET_ROOT=%~dp0.dotnet\

:: This tells .NET Core not to go looking for .NET Core in other places
SET DOTNET_MULTILEVEL_LOOKUP=0

:: Put our local dotnet.exe on PATH first so Visual Studio knows which one to use
SET PATH=%DOTNET_ROOT%;%PATH%

SET sln=%1

IF NOT EXIST "%DOTNET_ROOT%\dotnet.exe" (
    call restore.cmd
)

:: Add validation for .NET SDK version
SET MIN_SDK_VERSION=6.0.100
FOR /F "tokens=* USEBACKQ" %%F IN (`dotnet --version`) DO (
    SET SDK_VERSION=%%F
)
IF "%SDK_VERSION%" LSS "%MIN_SDK_VERSION%" (
    echo Error: .NET SDK version %MIN_SDK_VERSION% or higher is required
    exit /b 1
)

echo ProTip^: You can drag and drop a solution file onto startvs.cmd

IF "%sln%"=="" (
    echo Error^: Expected argument ^<SLN_FILE^>
    echo Usage^: startvs.cmd ^<SLN_FILE^>

    exit /b 1
)

start %sln%
