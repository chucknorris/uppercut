namespace uppercut.infrastructure.console
{
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using logging;

    public class CommandRunner
    {
        public static int run(string process, string arguments, bool wait_for_exit, StringDictionary environment_variables)
        {
            Log.bound_to(typeof(CommandRunner)).log_a_debug_event_containing("Attempting to run '{0}' with arguments '{1}'", Path.GetFullPath(process),
                                                                                arguments);

            int exit_code = -1;
            ProcessStartInfo psi = new ProcessStartInfo(Path.GetFullPath(process), arguments)
                                       {
                                           WorkingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                                           UseShellExecute = false,
                                           RedirectStandardOutput = true,
                                           RedirectStandardError = true,
                                           CreateNoWindow = true
                                       };

            StreamReader standard_output;
            StreamReader error_output;

            set_environment_variables(psi, environment_variables);

            using (Process p = new Process())
            {
                p.StartInfo = psi;
                p.Start();

                standard_output = p.StandardOutput;
                error_output = p.StandardError;
                if (wait_for_exit)
                {
                    p.WaitForExit();
                }
                exit_code = p.ExitCode;
            }

            Log.bound_to(typeof(CommandRunner)).log_an_error_event_containing(error_output.ReadToEnd());
            Log.bound_to(typeof(CommandRunner)).log_a_warning_event_containing(standard_output.ReadToEnd());

            return exit_code;
        }

        private static void set_environment_variables(ProcessStartInfo psi, StringDictionary environment_variables)
        {
            foreach (string key in environment_variables.Keys)
            {
                psi.EnvironmentVariables[key] = environment_variables[key];
            }
        }
    }
}