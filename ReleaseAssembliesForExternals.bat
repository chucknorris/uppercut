@echo off

::Project UppercuT - http://uppercut.googlecode.com
::No edits to this file are required - http://uppercut.pbwiki.com

SET DIR=%cd%
SET BUILD_DIR=%~d0%~p0%

call "%BUILD_DIR%build.bat" %*
if %ERRORLEVEL% NEQ 0 goto errors


SET NANT="%DIR%\lib\Nant\nant.exe"
SET build.config.settings="%DIR%\settings\UppercuT.config"

%NANT% /f:%BUILD_DIR%build\updateAssemblies.build -D:build.config.settings=%build.config.settings%

if %ERRORLEVEL% NEQ 0 goto errors

goto finish

:errors
EXIT /B %ERRORLEVEL%

:finish