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
        private const string git_sha1_hash_arguments = "log -1 --pretty=oneline";

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
                string describe_output = get_version_from_describe();
                string sha1_hash = get_sha1_hash();

                save_xml_file(get_revision_number_from(describe_output), sha1_hash, output_file, file_system);
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

        public string get_version_from_describe()
        {
            bool had_to_create_tags = false;
            StreamReader version_output = null;
            string git_describe_output = string.Empty;

            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing(Project.BaseDirectory);
            //run_external_application("cmd", git_path + " " + git_version_arguments + " > " + output_file, repo_directory, true);
            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing("Running cmd /c {0} {1} on {2}", git_path, git_version_arguments, repo_path);
            version_output = run_external_application("cmd", "/c " + git_path + " " + git_version_arguments, repo_directory, true);
            git_describe_output = version_output.ReadToEnd();

            if (git_describe_output.to_lower().Contains("fatal") || string.IsNullOrEmpty(git_describe_output))
            {
                infrastructure.logging.Log.bound_to(this).log_an_info_event_containing("Running cmd /c {0} {1} on {2}", git_path, git_create_tag_arguments, repo_path);
                had_to_create_tags = true;
                run_external_application("cmd", "/c " + git_path + " " + git_create_tag_arguments, repo_directory, true);
            }

            if (had_to_create_tags)
            {
                infrastructure.logging.Log.bound_to(this).log_an_info_event_containing("Running cmd /c {0} {1} on {2}", git_path, git_version_arguments, repo_path);
                version_output = run_external_application("cmd", "/c " + git_path + " " + git_version_arguments, repo_directory, true);
                git_describe_output = version_output.ReadToEnd();
            }

            return git_describe_output;
        }
        
        public string get_sha1_hash()
        {
            StreamReader hash_output = null;
            string git_sha1_hash_output = string.Empty;

            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing(Project.BaseDirectory);
            //run_external_application("cmd", git_path + " " + git_version_arguments + " > " + output_file, repo_directory, true);
            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing("Running cmd /c {0} {1} on {2}", git_path, git_sha1_hash_arguments, repo_path);
            hash_output = run_external_application("cmd", "/c " + git_path + " " + git_sha1_hash_arguments, repo_directory, true);
            git_sha1_hash_output = hash_output.ReadToEnd();

            try
            {
                git_sha1_hash_output = git_sha1_hash_output.Split(' ')[0];
            }
            catch (Exception)
            {
                git_sha1_hash_output = "NoHashAvailable";
            }

            return git_sha1_hash_output;
        }

        private int get_revision_number_from(string version_info_full)
        {
            string revision = "0";

            if (!string.IsNullOrEmpty(version_info_full))
            {
                version_info_full = version_info_full.Replace(Environment.NewLine, "");
                string[] version_parts = version_info_full.Split('-');

                if (version_parts.Length == 3)
                {
                    revision = version_parts[1];
                }
            }

            int revision_number = 0;
            int.TryParse(revision,out revision_number);
            
            return revision_number;
        }

        private void save_xml_file(int revision_number,string sha1_hash, string output_file, IFileSystemAccess file_system)
        {
           
            string xml_file = string.Format(@"<?xml version=""1.0"" ?>
<version>
    <revision>{0}</revision>
    <hash>{1}</hash>
</version>", revision_number, sha1_hash.to_lower().Replace("\n", ""));

            File.WriteAllText(output_file, xml_file);
            //file_system.create_file(output_file).
        }

        private StreamReader run_external_application(string app_path, string args, string working_directory, bool wait_for_exit)
        {
            ProcessStartInfo process_info = new ProcessStartInfo(app_path, string.Format(" {0}", args))
                                                {
                                                    UseShellExecute = false,
                                                    WorkingDirectory = working_directory,
                                                    RedirectStandardOutput = true,
                                                    RedirectStandardError = true,
                                                    CreateNoWindow = true
                                                };
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