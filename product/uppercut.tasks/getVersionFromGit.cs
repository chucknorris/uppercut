using System;
using System.Diagnostics;
using log4net;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace uppercut.tasks
{
    using System.Collections.Generic;
    using System.IO;
    using Castle.Windsor;
    using infrastructure.containers;
    using infrastructure.extensions;
    using infrastructure.filesystem;
    using infrastructure.logging;
    using infrastructure.logging.custom;
    using ILog = log4net.ILog;

    [TaskName("getVersionFromGit")]
    public class GetVersionFromGit : Task
    {
        private readonly ILog the_logger = LogManager.GetLogger(typeof(GetVersionFromGit));

        private const string git_version_arguments = "describe --abbrev=64";
        private const string git_create_tag_arguments = "tag -a -m\"for uppercut versioning\" versioning.for.uc";

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

        [TaskAttribute("gitExecutableWithPath", Required = false)]
        [StringValidator(AllowEmpty = false)]
        public string git_path { get; set; }

        [TaskAttribute("repoDirectory", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string repo_directory { get; set; }

        [TaskAttribute("repoPath", Required = false)]
        [StringValidator(AllowEmpty = false)]
        public string repo_path { get; set; }

        [TaskAttribute("output", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string output_file { get; set; }

        #endregion

        public void run_the_task()
        {
            Container.initialize_with(build_the_container());
            set_up_properties();
            IFileSystemAccess file_system = Container.get_an_instance_of<IFileSystemAccess>();

            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing(
                "Attempting to resolve version. Must have a tag to get versioning from. If you do not, one will be created.");

            output_file = Path.Combine(Project.BaseDirectory, output_file);

            try
            {
                bool had_to_create_tags = false;
                StreamReader version_output = null;
                string version_info_full = string.Empty;

                infrastructure.logging.Log.bound_to(this).log_an_info_event_containing(Project.BaseDirectory);
                //run_external_application("cmd", git_path + " " + git_version_arguments + " > " + output_file, repo_directory, true);
                infrastructure.logging.Log.bound_to(this).log_an_info_event_containing("Running cmd /c {0} {1} on {2}", git_path, git_version_arguments, repo_path);
                version_output = run_external_application("cmd", "/c " + git_path + " " + git_version_arguments, repo_directory, true);
                version_info_full = version_output.ReadToEnd();

                if (version_info_full.to_lower().Contains("fatal") || string.IsNullOrEmpty(version_info_full))
                {
                    infrastructure.logging.Log.bound_to(this).log_an_info_event_containing("Running cmd /c {0} {1} on {2}", git_path, git_create_tag_arguments, repo_path);
                    had_to_create_tags = true;
                    run_external_application("cmd", "/c " + git_path + " " + git_create_tag_arguments, repo_directory, true);
                }

                if (had_to_create_tags)
                {
                    infrastructure.logging.Log.bound_to(this).log_an_info_event_containing("Running cmd /c {0} {1} on {2}", git_path, git_version_arguments, repo_path);
                    version_output = run_external_application("cmd", "/c " + git_path + " " + git_version_arguments, repo_directory, true);
                    version_info_full = version_output.ReadToEnd();
                }

                save_xml_file(version_info_full, output_file, file_system);
            }
            catch (Exception exception)
            {
                infrastructure.logging.Log.bound_to(this).log_an_error_event_containing(
                    "{0} encountered an error:{1}{2}",
                    "getVersionFromGit",
                    Environment.NewLine,
                    exception);
                throw;
            }
        }

        private void save_xml_file(string version_info_full, string output_file, IFileSystemAccess file_system)
        {
            string revision = "0";
            string revision_hash = "0";

            if (!string.IsNullOrEmpty(version_info_full))
            {
                version_info_full = version_info_full.Replace(Environment.NewLine, "");
                string[] version_parts = version_info_full.Split('-');

                if (version_parts.Length == 3)
                {
                    revision = version_parts[1];
                    revision_hash = version_parts[2];
                }
            }

            string xml_file = string.Format(@"<?xml version=""1.0"" ?>
<version>
    <revision>{0}</revision>
    <hash>{1}</hash>
</version>", revision, revision_hash.to_lower().Replace("\n", ""));

            File.WriteAllText(output_file, xml_file);
            //file_system.create_file(output_file).
        }

        private StreamReader run_external_application(string app_path, string args, string working_directory, bool wait_for_exit)
        {
            ProcessStartInfo process_info = new ProcessStartInfo(app_path, string.Format(" {0}", args));
            process_info.UseShellExecute = false;
            process_info.WorkingDirectory = working_directory;
            process_info.RedirectStandardOutput = true;
            process_info.RedirectStandardError = true;
            process_info.CreateNoWindow = true;
            StreamReader standard_output;
            StreamReader error_output;

            using (Process process = new Process())
            {
                process.StartInfo = process_info;
                process.Start();
                standard_output = process.StandardOutput;
                error_output = process.StandardError;
                if (wait_for_exit)
                {
                    process.WaitForExit();
                }
            }
            //infrastructure.logging.Log.bound_to(this).log_an_info_event_containing(standard_output.ReadToEnd());
            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing(error_output.ReadToEnd());

            return standard_output;
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
            if (string.IsNullOrEmpty(git_path))
            {
                git_path = "git";
            }
        }
    }
}