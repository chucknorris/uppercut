@echo off

::Project UppercuT - http://uppercut.googlecode.com
::No edits to this file are required - http://uppercut.pbwiki.com

SET DIR=%~d0%~p0%

call "%DIR%build.bat"

SET build.config.settings="%DIR%Settings\UppercuT.config"
"%DIR%lib\Nant\nant.exe" /f:.\build\updateAssemblies.build -D:build.config.settings=%build.config.settings%
