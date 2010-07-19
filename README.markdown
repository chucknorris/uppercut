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
  
## Join the mailing list
 Ask questions - get answers - [http://groups.google.com/group/chucknorrisframework](http://groups.google.com/group/chucknorrisframework)  
  
## Getting started with UppercuT
### Downloads
 You can download UppercuT from [http://code.google.com/p/uppercut/downloads/list](http://code.google.com/p/uppercut/downloads/list)
  
 You can also obtain a copy from the build server at [http://teamcity.codebetter.com](http://teamcity.codebetter.com).
  
### Gems  
If you have Ruby 1.8.6+ (and Gems 1.3.7+) installed, you can get the current release of UppercuT to your machine the fastest!  
  
1. Type 'gem install uppercutbuild'  
2. At the top level directory (trunk or branch name) type 'uppercutbuild install' for bringing in uppercut for the first time or 'uppercutbuild upgrade' if you already uppercut and are just wanting to upgrade the build folder.   
  
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
=0.9.0.337=  
* ILMerge is now a step of the build process. Please check the configuration for the new setting. (r337)  
* UppercuT now has the command 'uppercutbuild upgrade'. (r336)  
* Zip will not include the gems folder. (r334)  
* UppercuT (through gems) can copy UppercuT to a solution directory by issuing 'uppercutbuild install' at the top level directory (trunk or branch name). (r333)  
  
=0.9.0.328=  
* Now supports building gems - see http://code.google.com/p/uppercut/issues/detail?id=16 for details.  (r328)  
* The version hash for SVN and TFS should just use version.revision. (r321)  
* General fix - when a step of the build process fails the build, any extensions or custom tasks related to it should also fail the build. (r319)  
  
=0.9.0.318=  
* Nitriq support. Please check for a new setting in the config. (r317)  
* OutputPath is configurable not to be overridden by UppercuT - see http://code.google.com/p/uppercut/issues/detail?id=15 for details.   Please check for a new setting in the config. (r316)  
* Multi-targeting for building your product on several frameworks at once  - see http://code.google.com/p/uppercut/issues/detail?id=14 for details. (r316)  
* Zip uses the version hash (which is only different if you are git or mercurial). (r313)  
  
=0.9.0.306=  
* Added a framework switch for NUnit - handling .NET 4.0 Support (r305)  
* Zip.build handles HG versioning (r304)  
* Nitriq support. Please check for a new setting in the config. (r302)  
* NUnit default is now 2.5.5. (r300)  
* CLS Compliance is now an opt in setting. Please check for a new setting in the config. (r299)  
  
=0.9.0.297=  
* .NET 4.0 Support (r296)  
* Fixed a bug with property setting in tfs.step (r293)  
* Uppercut now houses it's assemblies in the build folder. Please ensure you compare your NAnt folder and remove the assemblies that are no longer there if upgrading (r292)  
  
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