namespace uppercut.tasks
{
    using System;
    using infrastructure.containers;
    using infrastructure.filesystem;
    using NAnt.Core.Attributes;
    using template.builder;

    [TaskName("buildTemplateFiles")]
    public class BuildTemplateFiles : UppercuTTaskBase
    {
        private const string FILE_TEMPLATE_EXTENSION = ".TEMPLATE";

        #region Properties

        [TaskAttribute("settingsFilesDirectory", Required = false)]
        [StringValidator(AllowEmpty = false)]
        public string settings_files_directory { get; set; }

        [TaskAttribute("templatesDirectory", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string templates_directory { get; set; }

        [TaskAttribute("destinationDirectory", Required = false)]
        [StringValidator(AllowEmpty = false)]
        public string destination_directory { get; set; }

        [TaskAttribute("templateExtension", Required = false)]
        [StringValidator(AllowEmpty = false)]
        public string template_extension { get; set; }

        [TaskAttribute("keepExtension", Required = false)]
        [StringValidator(AllowEmpty = false)]
        public bool keep_extension { get; set; }

        [TaskAttribute("useEnvironmentSubdirectory", Required = false)]
        [StringValidator(AllowEmpty = false)]
        public bool use_environment_subdirectory { get; set; }

        #endregion

        public override void set_up_properties() {
            if (string.IsNullOrEmpty(destination_directory)) {
                //Project.BaseDirectory
                destination_directory = @"build_output\environment.files";
            }

            if (string.IsNullOrEmpty(settings_files_directory)) {
                settings_files_directory = @"settings";
            }

            if (string.IsNullOrEmpty(template_extension)) {
                template_extension = FILE_TEMPLATE_EXTENSION;
            }
        }

        public override void run_the_task() {
            infrastructure.logging.Log.bound_to(this).log_a_warning_event_containing(
                "Looking for template files starting in \"{0}\" and building to directory \"{1}\".",
                templates_directory,
                destination_directory);

            TemplateBuilder builder = new TemplateBuilder(Container.get_an_instance_of<IFileSystemAccess>());

            try {
                builder.build_template_files(settings_files_directory, templates_directory, destination_directory, template_extension, keep_extension,
                                             use_environment_subdirectory);
            }
            catch (Exception exception) {
                infrastructure.logging.Log.bound_to(this).log_an_error_event_containing(
                    "{0} encountered an error:{1}{2}",
                    "TemplateBuilder",
                    Environment.NewLine,
                    exception);
                throw;
            }
        }
    }
}