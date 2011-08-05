namespace uppercut.tasks
{
    using System;
    using System.Collections;
    using System.Text;
    using infrastructure.console;
    using infrastructure.containers;
    using infrastructure.filesystem;
    using NAnt.Core;
    using NAnt.Core.Attributes;

    [TaskName("customExtension")]
    public class CustomExtensionTask : UppercuTTaskBase
    {
        private readonly string nant = ".\\lib\\nant\\nant.exe";
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
            
           // infrastructure.logging.Log.bound_to(this).log_an_info_event_containing("Found the custom extensions folder at .\\{0}", custom_extensions_folder);

            string file_name = get_file_name(custom_extensions_folder, build_folder,file_system);
            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing("Running custom tasks if {0} exists (or with .ps1/.rb added to the end of it).", file_name);

            foreach (DictionaryEntry property in base.Project.Properties)
            {
                infrastructure.logging.Log.bound_to(this).log_a_debug_event_containing("Project|{0}:{1}", property.Key, property.Value);
                //set these all on the shell execution
            }
            if (file_system.file_exists(file_name))
            {
                an_extension_exists = true;
                infrastructure.logging.Log.bound_to(this).log_a_warning_event_containing("{0} {1} Extension Point - Running '{2}'",extension_type.ToUpper(),file_system.get_file_name_without_extension_from(extends), file_name);
                CommandRunner.run(nant, arrange_arguments_for_nant(file_name, Project.Properties), true);
            }
            if (file_system.file_exists(file_system.path_combine(file_name, ".ps1")))
            {
                an_extension_exists = true;
                infrastructure.logging.Log.bound_to(this).log_a_warning_event_containing("{0} {1} Extension Point - Running '{2}.ps1'", extension_type.ToUpper(), file_system.get_file_name_without_extension_from(extends), file_name);
                CommandRunner.run(powershell, file_system.get_full_path(file_name), true);
            }
            if (file_system.file_exists(file_system.path_combine(file_name, ".rb")))
            {
                an_extension_exists = true;
                infrastructure.logging.Log.bound_to(this).log_a_warning_event_containing("{0} {1} Extension Point - Running '{2}.rb'", extension_type.ToUpper(), file_system.get_file_name_without_extension_from(extends), file_name);
                CommandRunner.run(ruby, file_system.get_full_path(file_name), true);
            }

            if (an_extension_exists && extension_type.to_lower() == "replace") {
                Project.Properties["is.replaced"] = "true";    
            }

        }

        private string arrange_arguments_for_nant(string file_name, PropertyDictionary properties)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("/f:'{0}'{1}", file_name, expand_properties(" -D:", "=", properties));

            return sb.ToString();
        }

        private string expand_properties(string front_item, string equality_sign, PropertyDictionary properties)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry property in properties)
            {
                if (!is_nant_property(property) && !is_readonly_property(property) && !is_unpassable_property(property))
                {
                    sb.AppendFormat("{0}'{1}'{2}'{3}'", front_item, property.Key, equality_sign, property.Value);
                }
            }
            return sb.ToString();
        }

        private bool is_nant_property(DictionaryEntry property) {
            if (property.Key.ToString().to_lower().Contains("nant.settings.")) return true;

            return false;
        }

        private bool is_readonly_property(DictionaryEntry property) {
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

        private string get_file_name(string custom_extensions_folder,string build_folder, IFileSystemAccess file_system) {
            //TODO: handle sub folder files
            string file_name = string.Format("{0}.{1}{2}", file_system.get_file_name_without_extension_from(extends), extension_type,
                                             file_system.get_file_extension_from(extends));

            return file_system.get_full_path(file_system.path_combine(custom_extensions_folder, file_name));
        }
    }
}