#!/bin/bash
# stty -echo

# ::Project UppercuT - http://uppercut.googlecode.com
# ::No edits to this file are required - http://uppercut.pbwiki.com

./build.sh

mono ./lib/NAnt/NAnt.exe /logger:"NAnt.Core.DefaultLogger" /nologo /quiet /f:"$(cd $(dirname "$0"); pwd)/build/zip.build" /D:build.config.settings="$(cd $(dirname "$0"); pwd)/settings/UppercuT.config" $*
