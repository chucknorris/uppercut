Project UppercuT - Builds in Seconds, Not Days
=======

![UppercuT](http://github.com/ferventcoder/uppercut/raw/master/docs/logo/UppercuT_Logo_Small.jpg "UppercuT - insanely easy. Insanely.")

# LICENSE
Apache 2.0 - see docs/legal (just LEGAL in the zip folder)

# IMPORTANT
NOTE: If you are looking at the source - please run build.bat before opening the solution. It creates the SolutionVersion.cs file that is necessary for a successful build.

# INFO
## Overview
UppercuT is automated .NET build framework that is templated NAnt with conventions. UppercuT is the insanely easy to use build framework.  

It seeks to solve both maintenance concerns and ease of build to help you concentrate on what you really want to do: write code. Upgrading the build should take seconds, not hours. And that is where UppercuT will beat any other automated build system hands down.  
UppercuT uses conventions and has a simple configuration file for you to edit. Getting from zero to build takes literally less than five minutes. If you are still writing your own build scripts, you are working too hard.   

UppercuT is extremely powerful because it is customizable and extendable. Every step of the build process is customizable with a pre, post and replace hook.  

UppercuT is not a build server, but it integrates nicely with CruiseControl.NET, TeamCity, Hudson, etc.  

## Getting started with UppercuT
### Downloads
 You can download UppercuT from [http://code.google.com/p/uppercut/downloads/list](http://code.google.com/p/uppercut/downloads/list)

 You can also obtain a copy from the build server at [http://teamcity.codebetter.com](http://teamcity.codebetter.com).

### Source
This is the best way to get to the bleeding edge of what we are doing.

1. Clone the source down to your machine. 
  `git clone git://github.com/chucknorris/uppercut.git`  
2. Type `cd uppercut`  
3. Type `git config core.autocrlf true` to set line endings to auto convert for this repository  
4. Type `git status`. You should not see any files to change.
5. Run `build.bat`. NOTE: You must have git on the path (open a regular command line and type git).
  
  
# REQUIREMENTS
* .NET Framework 3.5 
* source control on the command line and in PATH environment variable - svn for Subversion / tf for TFS / git for Git

# DONATE
Donations Accepted - If you enjoy using this product or it has saved you time and money in some way, please consider making a donation.  
It helps keep to the product updated, pays for site hosting, etc. https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=4410250

# RELEASE NOTES
=0.9.0.286=  
* NUnit tester now runs console version. This fixes a TestFixtureSetup issue - see http://code.google.com/p/uppercut/issues/detail?id=10 for details (r285)  
  
=0.9.0.282=  
* Mercurial (HG) support for versioning (r282)  
  
=0.9.0.273=  
* Enhanced Git Versioning to better work with Hudson (r272)  
  
=0.9.0.266=  
* Adding the ability to use PowerShell to write custom tasks. To run powershell - you need to set unrestricted if you are not going to sign the scripts. Because this possibly exposes a possible security hole, it is turned off by default. There is a property (allow.powershell.unrestricted) you can set to true in the Uppercut.config file. Look in the external tools section for the setting. (r266)  
* Adding the ability to use Ruby to write custom tasks. (r265)  
* Enhanced support of Git versioning on TeamCity. (r263)  
* Fixed a small bug in NAnt related to running .NET 4.0. (r262)  
* DocBuilder will do both .template and .xml now. (r247)  
  
=0.9.0.246=  
* Fixed - Git versioning had an issue with creating the initial tag for versioning - see http://code.google.com/p/uppercut/issues/detail?id=9 for details (r243)  
* Zip nows versions the zip file name by default. Check your zip.post.build. You may need to remove or change zip.post.build. (r240)  
* Support for .NET 4.0 beta2 has been added. (r238)  
  
=0.9.0.235=  
* UppercuT supports Git for versioning - The numbering system is branch specific.  
* Added ILMERGE tasks to the samples  
* Code is now built to a folder under build_output. This is the same folder it goes to under code_drop, so in the case of uppercut, build_output\UppercuT\ instead of just build_output.  
  * BREAKING CHANGE: For your custom tasks, you may need to make changes.  

# CREDITS
see docs/legal/CREDITS (just LEGAL/Credits in the zip folder)