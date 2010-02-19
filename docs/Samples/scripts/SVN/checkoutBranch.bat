@echo off

SET DEVFOLDER=C:\CODE\
SET REPOSITORIES=http://somewhere.com/svn/
SET BRANCHES=branches

if '%1' == '' goto usage
if '%2' == '' goto usage
if '%3' NEQ '' goto usage
if '%1' == '/?' goto usage
if '%1' == '-?' goto usage
if '%1' == '?' goto usage
if '%1' == '/help' goto usage

svn co %REPOSITORIES%%1/%BRANCHES%/%2/ "%DEVFOLDER%\%1-%2"

if %ERRORLEVEL% NEQ 0 goto usage

goto finish

:usage
echo.
echo Usage: checkoutBranch.bat repositoryName branchName
echo Example checkout.bat Bob Feature1
echo.
goto finish

:finish
