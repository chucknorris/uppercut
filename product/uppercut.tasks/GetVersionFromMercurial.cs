namespace uppercut.tasks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text.RegularExpressions;
    using Castle.Windsor;
    using infrastructure.containers;
    using infrastructure.extensions;
    using infrastructure.filesystem;
    using infrastructure.logging;
    using infrastructure.logging.custom;
    using log4net;
    using NAnt.Core;
    using NAnt.Core.Attributes;
    using ILog = log4net.ILog;

    [TaskName("getVersionFromMercurial")]
    public class GetVersionFromMercurial : Task
    {
        private readonly ILog the_logger = LogManager.GetLogger(typeof(GetVersionFromMercurial));

        private const string hg_version_arguments = "tip";

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

        [TaskAttribute("hgExecutableWithPath", Required = false)]
        [StringValidator(AllowEmpty = false)]
        public string hg_path { get; set; }

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

            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing("Attempting to resolve version using Mercurial. This will get both number and hash.");

            output_file = Path.Combine(Project.BaseDirectory, output_file);

            try
            {
                string tip_output = get_tip_output();
                string[] changeset_pieces = get_changeset_pieces(tip_output);

                save_xml_file(get_revision_number_from(changeset_pieces), get_sha1_hash_from(changeset_pieces), output_file, file_system);
            }
            catch (Exception exception)
            {
                infrastructure.logging.Log.bound_to(this).log_an_error_event_containing(
                    "{0} encountered an error:{1}{2}",
                    "getVersionFromMercurial",
                    Environment.NewLine,
                    exception);
                throw;
            }
        }

        public string get_tip_output()
        {
            StreamReader version_output = null;
            string hg_tip_output = string.Empty;

            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing(Project.BaseDirectory);
            //run_external_application("cmd", hg_path + " " + hg_version_arguments + " > " + output_file, repo_directory, true);
            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing("Running cmd /c {0} {1} on {2}", hg_path, hg_version_arguments, repo_path);
            version_output = run_external_application("cmd", "/c " + hg_path + " " + hg_version_arguments, repo_directory, true);
            hg_tip_output = version_output.ReadToEnd();

            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing(hg_tip_output);

            return hg_tip_output;
        }

        private string[] get_changeset_pieces(string tip_info)
        {
            string[] changeset_info = { };

            if (!string.IsNullOrEmpty(tip_info))
            {
                tip_info = Regex.Replace(tip_info, "\n", "|");
                string[] version_parts = tip_info.Split('|');

                foreach (string version_part in version_parts)
                {
                    if (version_part.to_lower().Contains("changeset"))
                    {
                        infrastructure.logging.Log.bound_to(this).log_a_debug_event_containing("Changeset={0}",version_part);
                        changeset_info = version_part.Split(':');
                    }
                }
            }

            return changeset_info;
        }

        private int get_revision_number_from(string[] changeset_info)
        {
            string revision = "0";

            infrastructure.logging.Log.bound_to(this).log_a_debug_event_containing("Length of changeset={0}",changeset_info.Length);

            if (changeset_info.Length >= 2)
            {
                revision = changeset_info[1].Replace(" ", "");
            }
            
            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing("Revision string returned \"{0}\"", revision);
            
            int revision_number = 0;
            int.TryParse(revision, out revision_number);

            return revision_number;
        }

        private string get_sha1_hash_from(string[] changeset_info)
        {
            string sha1_hash = "";

            if (changeset_info.Length >=3)
            {
                sha1_hash = changeset_info[2].Replace(" ", "");
            }

            infrastructure.logging.Log.bound_to(this).log_an_info_event_containing("SHA1 returned \"{0}\"", sha1_hash);

            return sha1_hash;
        }

        private void save_xml_file(int revision_number, string sha1_hash, string output_file, IFileSystemAccess file_system)
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
            if (string.IsNullOrEmpty(hg_path))
            {
                hg_path = "hg";
            }
        }
    }
}