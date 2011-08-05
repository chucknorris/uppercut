### install chocolatey ###
$url = "http://packages.nuget.org/v1/Package/Download/Chocolatey/0.9.8.6"
$tempDir = "$env:TEMP\chocolatey\chocInstall"
if (![System.IO.Directory]::Exists($tempDir)) {[System.IO.Directory]::CreateDirectory($tempDir)}
$file = Join-Path $tempDir "chocolatey.zip"

# download the package
Write-Host "Downloading $url to $file"
$downloader = new-object System.Net.WebClient
$downloader.DownloadFile($url, $file)

# unzip the package
Write-Host "Extracting $file to $destination..."
$shellApplication = new-object -com shell.application 
$zipPackage = $shellApplication.NameSpace($file) 
$destinationFolder = $shellApplication.NameSpace($tempDir) 
$destinationFolder.CopyHere($zipPackage.Items(),0x10)

# call chocolatey install
Write-Host "Installing chocolatey on this machine"
$chocInstallPS1 = "$tempDir\tools\chocolateyInstall.ps1"
& $chocInstallPS1
### finish installing chocolatey ###

# update chocolatey to the latest version
Write-Host "Updating chocolatey to the latest version"
cup chocolatey

# install nuget if it is missing
cinstm nuget.commandline

#restore the nuget packages
$nugetConfigs = Get-ChildItem '.\' -Recurse | ?{$_.name -match "packages\.config"} | select

if ($nugetConfigs -ne '') {
  foreach ($nugetConfig in $nugetConfigs) {
    Write-Host "restoring packages from $($nugetConfig.FullName)"
    nuget install $($nugetConfig.FullName) /OutputDirectory packages
  }
}

# install the uppercut libraries
nuget install NuGet.Core -version 1.4.20615.9012 -OutputDirectory lib -x
nuget install NuGet.CommandLine -version 1.4.20615.182 -OutputDirectory lib -x
nuget install eazfuscator.net -version 2.9.41.774 -OutputDirectory lib -x
nuget install nunit -version 2.5.9.10348 -OutputDirectory lib -x

nuget install uppercut.bdddoc -version 1.5 -OutputDirectory lib -x -source http://www.myget.org/F/uppercut/
nuget install uppercut.mbunit2 -version 1.5 -OutputDirectory lib -x -source http://www.myget.org/F/uppercut/
nuget install uppercut.developwithpassion.bdd -version 1.5 -OutputDirectory lib -x -source http://www.myget.org/F/uppercut/
nuget install uppercut.ilmerge -version 1.5 -OutputDirectory lib -x -source http://www.myget.org/F/uppercut/
nuget install uppercut.moma -version 1.5 -OutputDirectory lib -x -source http://www.myget.org/F/uppercut/
nuget install uppercut.nant -version 1.5 -OutputDirectory lib -x -source http://www.myget.org/F/uppercut/
nuget install uppercut.ncover -version 1.5 -OutputDirectory lib -x -source http://www.myget.org/F/uppercut/
nuget install uppercut.xunit -version 1.5 -OutputDirectory lib -x -source http://www.myget.org/F/uppercut/
#future nuget install uppercut.storevil -version 1.5 -OutputDirectory lib -x -source http://www.myget.org/F/uppercut/


#TODO move tools contents to top level folders and nuget.commandline moves to NuGet
