@echo off

if '%1' == '' goto usage
if '%2' NEQ '' goto usage
if '%1' == '/?' goto usage
if '%1' == '-?' goto usage
if '%1' == '?' goto usage
if '%1' == '/help' goto usage

svn ci -m%1

if %ERRORLEVEL% NEQ 0 goto usage

goto finish

:usage
echo.
echo Usage: commit "message"
echo Example: commit.bat "This is my check in comment with quotes around it"
echo.
goto finish

:finish
