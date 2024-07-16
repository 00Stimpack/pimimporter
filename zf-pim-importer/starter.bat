@echo off
SETLOCAL

REM Check if the correct number of arguments are provided.
IF "%~2"=="" (
    echo Usage: %~nx0 project_path_file source_directory
    exit /b 1
)

REM Assign the arguments to variables.
SET "project_path_file=%~1"
SET "target=%~2"

echo Starting %target%\ZF_Pim_Importer.exe
cd %target%
REM .\ZF_Pim_Importer.exe
start "" "ZF_Pim_Importer.exe"

REM Check if the application started successfully.
IF ERRORLEVEL 1 (
    echo Failed to start the application: %target%\ZF_Pim_Importer.exe
    exit /b 7
)


ENDLOCAL
