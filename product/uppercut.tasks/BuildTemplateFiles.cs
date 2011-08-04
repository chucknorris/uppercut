namespace uppercut.tasks
{
    using System;
    using System.Collections.Generic;
    using Castle.Windsor;
    using infrastructure.containers;
    using infrastructure.filesystem;
    using infrastructure.logging;
    using infrastructure.logging.custom;
    using log4net;
    using NAnt.Core;
    using NAnt.Core.Attributes;
    using template.builder;
    using ILog = log4net.ILog;

    [TaskName("buildTemplateFiles")]
    public class BuildTemplateFiles : Task
    {
        private const string FILE_TEMPLATE_EXTENSION = ".TEMPLATE";
        private readonly ILog the_logger = LogManager.GetLogger(typeof(BuildTemplateFiles));

        #region NAnt

        /// <summary>
        /// Executes the NAnt task
        /// </summary>
        protected override void ExecuteTask()
        {
            run_the_task();
        }

        #endregion

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

        public void run_the_task()
        {
            Container.initialize_with(build_the_container());
            set_up_properties();

            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing(
                "Looking for template files starting in \"{0}\" and building to directory \"{1}\".",
                templates_directory,
                destination_directory);

            TemplateBuilder builder = new TemplateBuilder(Container.get_an_instance_of<IFileSystemAccess>());

            try
            {
                builder.build_template_files(settings_files_directory, templates_directory, destination_directory, template_extension, keep_extension,use_environment_subdirectory);
            }
            catch (Exception exception)
            {
                infrastructure.logging.Log.bound_to(this).log_an_error_event_containing(
                    "{0} encountered an error:{1}{2}",
                    "TemplateBuilder",
                    Environment.NewLine,
                    exception);
                throw;
            }
        }

        private IContainer build_the_container()
        {
            IWindsorContainer windsor_container = new WindsorContainer();

            infrastructure.logging.ILog nant_logger = new NAntLogger(this);
            infrastructure.logging.ILog log4net_logger = new Log4NetLogger(the_logger);
            infrastructure.logging.ILog multi_logger = new MultipleLogger(new List<infrastructure.logging.ILog> { nant_logger, log4net_logger });

            windsor_container.Kernel.AddComponentInstance<infrastructure.logging.ILog>(multi_logger);
            windsor_container.AddComponent<IFileSystemAccess, WindowsFileSystemAccess>();
            windsor_container.AddComponent<ILogFactory, MultipleLoggerLogFactory>();

            return new infrastructure.containers.custom.WindsorContainer(windsor_container);
        }

        public void set_up_properties()
        {
            if (string.IsNullOrEmpty(destination_directory))
            {
                //Project.BaseDirectory
                destination_directory = @"build_output\environment.files";
            }

            if (string.IsNullOrEmpty(settings_files_directory))
            {
                settings_files_directory = @"settings";
            }

            if (string.IsNullOrEmpty(template_extension))
            {
                template_extension = FILE_TEMPLATE_EXTENSION;
            }
        }
    }
}