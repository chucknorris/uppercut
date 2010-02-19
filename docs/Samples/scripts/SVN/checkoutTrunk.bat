@echo off

SET DEVFOLDER=C:\CODE\
SET REPOSITORIES=http://somewhere.com/svn/
SET TRUNK=trunk

if '%1' == '' goto usage
if '%2' NEQ '' goto usage
if '%3' NEQ '' goto usage
if '%1' == '/?' goto usage
if '%1' == '-?' goto usage
if '%1' == '?' goto usage
if '%1' == '/help' goto usage

svn co %REPOSITORIES%%1/%TRUNK%/ "%DEVFOLDER%\%1"

if %ERRORLEVEL% NEQ 0 goto usage

goto finish

:usage
echo.
echo Usage: checkoutTrunk.bat repositoryName
echo Example checkoutTrunk.bat Bob
echo.
goto finish

:finish
