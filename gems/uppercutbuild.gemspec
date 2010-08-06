version = File.read(File.expand_path("../VERSION",__FILE__)).strip

Gem::Specification.new do |spec|
  spec.platform    = Gem::Platform::RUBY
  spec.name        = 'uppercutbuild'
  spec.version     = version
  spec.files = Dir['lib/**/*'] + Dir['docs/**/*'] + Dir['bin/**/*']
  spec.bindir = 'bin'
  spec.executables << 'uppercutbuild'
  
  spec.add_dependency('thor','>= 0.13.8')
  
  spec.summary     = 'UppercuT - Conventional Builds For .NET'
  spec.description = <<-EOF 
UppercuT is THE conventional build for .NET!  
UppercuT seeks to solve both maintenance concerns and ease of build to help you concentrate on what you really want to do: write code. Upgrading the build should take seconds, not hours. And that is where UppercuT will beat any other automated build system hands down. 
UppercuT uses conventions and has a simple configuration file for you to edit. Getting from zero to build takes literally less than five minutes. If you are still writing your own build scripts, you are working too hard. 
UppercuT is extremely powerful because it is customizable and extendable. Every step of the build process is customizable with a pre, post and replace hook. 
UppercuT is not a build server, but it integrates nicely with CruiseControl.NET, TeamCity, Hudson, etc.
EOF
 
  spec.authors            = ['Rob "FerventCoder" Reynolds','Dru Sellers']
  spec.email             = 'chucknorrisframework@googlegroups.com'
  spec.homepage          = 'http://projectuppercut.org'
  spec.rubyforge_project = 'uppercutbuild'
end