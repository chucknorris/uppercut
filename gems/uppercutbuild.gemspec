version = File.read(File.expand_path("../UPPERCUT_VERSION",__FILE__)).strip

Gem::Specification.new do |s|
  s.platform    = Gem::Platform::RUBY
  s.name        = 'uppercutbuild'
  s.version     = version
  s.files = Dir['lib/**/*']

  
  s.summary     = 'UppercuT - Conventional Builds For .NET'
  s.description = 'UppercuT is THE conventional build for .NET'
  
  s.author            = 'Rob "FerventCoder" Reynolds'
  s.email             = 'chucknorrisframework@googlegroups.com'
  s.homepage          = 'http://projectuppercut.org'
  s.rubyforge_project = 'uppercutbuild'
end