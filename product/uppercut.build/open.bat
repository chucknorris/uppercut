@echo off

::Project UppercuT - http://uppercut.googlecode.com
::No edits to this file are required - http://uppercut.pbwiki.com

SET DIR=%cd%
SET BUILD_DIR=%~d0%~p0%
SET NANT="%BUILD_DIR%lib\Nant\nant.exe"
SET build.config.settings="%DIR%\.uppercut"

%NANT% -logger:NAnt.Core.DefaultLogger -quiet /f:"%BUILD_DIR%build\open.build" -D:build.config.settings=%build.config.settings%