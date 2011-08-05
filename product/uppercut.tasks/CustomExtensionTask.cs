namespace uppercut.tasks
{
    using System.Collections;
    using System.IO;
    using System.Text;
    using infrastructure.console;
    using infrastructure.containers;
    using infrastructure.filesystem;
    using NAnt.Core;
    using NAnt.Core.Attributes;
    using NAnt.Core.Tasks;

    [TaskName("customExtension")]
    public class CustomExtensionTask : UppercuTTaskBase
    {
        //private readonly string nant = ".\\lib\\nant\\nant.exe";
        private readonly string powershell = "poweshell.exe";
        private readonly string ruby = "ruby.exe";

        [TaskAttribute("extends", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string extends { get; set; }

        [TaskAttribute("type", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string extension_type { get; set; }

        public override void set_up_properties() { }

        public override void run_the_task()
        {
            var file_system = Container.get_an_instance_of<IFileSystemAccess>();

            var custom_extensions_folder = Project.Properties["folder.build_scripts_custom"];
            var build_folder = Project.Properties["folder.build_scripts"];
            bool an_extension_exists = false;

            string file_name = get_file_name(custom_extensions_folder, build_folder, file_system);
            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing("Running custom tasks if {0} exists (or with .ps1/.rb added to the end of it).", file_name);

            //foreach (DictionaryEntry property in base.Project.Properties)
            //{
            //    infrastructure.logging.Log.bound_to(this).log_a_debug_event_containing("Project|{0}:{1}", property.Key, property.Value);
            //    //set these all on the shell execution
            //}
            if (file_system.file_exists(file_name))
            {
                an_extension_exists = true;
                log_file_found(file_name,file_system);
                run_nant_core_task(file_name);
            }

            //todo: put a whole bunch of environment properties out on the shell just prior
            var powershell_extension = file_system.get_full_path(file_system.path_combine(file_name, ".ps1"));
            if (file_system.file_exists(powershell_extension))
            {
                an_extension_exists = true;
                log_file_found(powershell_extension, file_system);
                CommandRunner.run(powershell, powershell_extension, true);
            }
            var ruby_extension = file_system.get_full_path(file_system.path_combine(file_name, ".rb"));
            if (file_system.file_exists(ruby_extension))
            {
                an_extension_exists = true;
                log_file_found(ruby_extension,file_system);
                CommandRunner.run(ruby, ruby_extension, true);
            }

            if (an_extension_exists && extension_type.to_lower() == "replace")
            {
                Project.Properties["is.replaced"] = "true";
            }
        }
        
        #region deprecated

        private string arrange_arguments_for_nant(string file_name, PropertyDictionary properties)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("/f:\"{0}\"{1}", file_name, expand_properties(" -D:", "=", properties));

            return sb.ToString();
        }

        private string expand_properties(string front_item, string equality_sign, PropertyDictionary properties)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry property in properties)
            {
                if (!is_nant_property(property) && !is_readonly_property(property) && !is_unpassable_property(property))
                {
                    sb.AppendFormat("{0}\"{1}\"{2}\"{3}\"", front_item, property.Key, equality_sign, property.Value);
                }
            }
            return sb.ToString();
        }
        
        private bool is_nant_property(DictionaryEntry property)
        {
            //if (property.Key.ToString().to_lower().Contains("nant.settings.")) return true;
            if (property.Key.ToString().to_lower().Equals("nant.filename")) return true;
            if (property.Key.ToString().to_lower().Equals("nant.location")) return true;
            if (property.Key.ToString().to_lower().Equals("nant.onsuccess")) return true;
            if (property.Key.ToString().to_lower().Equals("nant.onfailure")) return true;
            if (property.Key.ToString().to_lower().Equals("nant.project.basedir")) return true;
            if (property.Key.ToString().to_lower().Equals("nant.project.buildfile")) return true;
            if (property.Key.ToString().to_lower().Equals("nant.project.default")) return true;
            if (property.Key.ToString().to_lower().Equals("nant.project.name")) return true;
            if (property.Key.ToString().to_lower().Equals("nant.version")) return true;

            return false;
        }

        private bool is_readonly_property(DictionaryEntry property)
        {
            //if (Project.Properties.IsReadOnlyProperty(property.Key.ToString())) return true;

            return false;
        }

        private bool is_unpassable_property(DictionaryEntry property)
        {
            if (property.Key.ToString().to_lower() == "dirs.current") return true;
            if (property.Key.ToString().to_lower() == "dirs.build") return true;
            if (property.Key.ToString().to_lower() == "path.separator") return true;

            return false;
        }
        
        #endregion
        
        private void log_file_found(string file_name,IFileSystemAccess file_system)
        {
            infrastructure.logging.Log.bound_to(this).log_a_warning_event_containing("            [echo] ");
            infrastructure.logging.Log.bound_to(this).log_a_warning_event_containing("            [echo] ====================");
            infrastructure.logging.Log.bound_to(this).log_a_warning_event_containing("            [echo] {0} {1} Extension", file_system.get_file_name_without_extension_from(extends), extension_type.ToUpper());
            infrastructure.logging.Log.bound_to(this).log_a_warning_event_containing("            [echo] ====================");
            infrastructure.logging.Log.bound_to(this).log_a_warning_event_containing("            [echo] Running '{0}' - details will be in the build.log.", file_name);
        }

        private string get_file_name(string custom_extensions_folder, string build_folder, IFileSystemAccess file_system)
        {

            string base_path = Project.BaseDirectory.to_lower().Replace(build_folder.to_lower(), custom_extensions_folder.to_lower());
            string file_name = string.Format("{0}.{1}{2}", file_system.get_file_name_without_extension_from(extends), extension_type,
                                             file_system.get_file_extension_from(extends));

            return file_system.get_full_path(file_system.path_combine(base_path, file_name));
        }

        private void run_nant_core_task(string file_path)
        {
            var build_file = new FileInfo(file_path);
          
            var nant_task = new NAntTask();
            nant_task.Project = this.Project;
            nant_task.FailOnError = this.FailOnError;
            nant_task.Verbose = true;
            //nant_task.Threshold = Level.Info;

            
            nant_task.BuildFile = build_file;
            nant_task.InheritRefs = false;
            nant_task.InheritAll = true;
            
            nant_task.Execute();
        }
    }
}