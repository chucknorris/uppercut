@echo off

SET REPOSITORIES=http://somewhere.com/svn/
SET TRUNK=trunk
SET BRANCHES=branches

if '%1' == '' goto usage
if '%2' == '' goto usage
if '%3' NEQ '' goto usage
if '%1' == '/?' goto usage
if '%1' == '-?' goto usage
if '%1' == '?' goto usage
if '%1' == '/help' goto usage

svn copy %REPOSITORIES%%1/%TRUNK%/ %REPOSITORIES%%1/%BRANCHES%/%2/ -m"Creating a branch for %2."

::svn up

if %ERRORLEVEL% NEQ 0 goto usage

goto finish

:usage
echo.
echo Usage: branch.bat repositoryName BranchName
echo Example branch.bat Bob Feature1
echo.
goto finish

:finish
