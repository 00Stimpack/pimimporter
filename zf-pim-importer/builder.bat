@echo off
SETLOCAL

REM Check if the correct number of arguments are provided.
IF "%~2"=="" (
    echo Usage: %~nx0 project_path_file source_directory
    exit /b 1
)

REM Assign the arguments to variables.
SET "project_path_file=%~1"
SET "source=%~2"

REM Check if the project path file exists.
IF NOT EXIST "%project_path_file%" (
    echo Project path file not found: %project_path_file%
    exit /b 2
)

REM Read the project path from the file.
SET /P target=<"%project_path_file%"

REM Validate if the target path is a directory.
IF NOT EXIST "%target%" (
    echo Invalid directory in the project path file: %target%
    exit /b 3
)

SET "target=%target%\PimImporter"

REM Delete the target path if it exists, then recreate it.
IF EXIST "%target%" (
    echo Target directory found. Deleting: %target%
    rmdir /S /Q "%target%"
)

echo Target directory not found. Creating: %target%
mkdir "%target%"

REM If the directory creation failed, exit.
IF NOT EXIST "%target%" (
    echo Failed to create target directory: %target%
    exit /b 4
)

REM Check if the source directory exists.
IF NOT EXIST "%source%" (
    echo Source directory not found: %source%
    exit /b 5
)

echo Copying from %source% to %target%
xcopy /Y /E "%source%\*" "%target%\"


REM Copy the contents of the source directory to the target directory.
IF EXIST ".\Libs\*" (
    echo Copying from bin to %target%
    REM xcopy /Y /E ".\Libs\*" "%target%\"
)



REM Check if the executable exists.
IF NOT EXIST "%target%\ZF_Pim_Importer.exe" (
    echo Executable not found: %target%\ZF_Pim_Importer.exe
    exit /b 6
)

echo Starting %target%\ZF_Pim_Importer.exe
cd %target%
REM .\ZF_Pim_Importer.exe
REM start "" "%target%\ZF_Pim_Importer.exe"

REM Check if the application started successfully.
IF ERRORLEVEL 1 (
    echo Failed to start the application: %target%\ZF_Pim_Importer.exe
    exit /b 7
)


ENDLOCAL
