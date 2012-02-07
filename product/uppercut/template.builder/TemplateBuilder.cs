namespace uppercut.template.builder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using infrastructure.filesystem;
    using infrastructure.logging;

    public sealed class TemplateBuilder
    {
        private readonly IFileSystemAccess file_system;
        private const string FILE_SETTINGS_EXTENSION = ".SETTINGS";

        //todo: move some of this stuff out

        public TemplateBuilder(IFileSystemAccess file_system)
        {
            this.file_system = file_system;
        }

        public bool build_template_files(string settings_files_directory, string base_directory, string working_directory, string template_extension,
                                         bool keep_extension,bool use_environment_subdirectory)
        {
            settings_files_directory = file_system.get_full_path(settings_files_directory);
            base_directory = file_system.get_full_path(base_directory);
            working_directory = file_system.get_full_path(working_directory);

            report_settings_files(settings_files_directory);

            return build_the_files(settings_files_directory, base_directory, working_directory, template_extension, keep_extension, use_environment_subdirectory);
        }

        public void report_settings_files(string settings_files_directory)
        {
            if (!file_system.directory_exists(settings_files_directory))
            {
                return;
            }

            foreach (string settings_file in file_system.get_all_file_name_strings_in(settings_files_directory))
            {
                if (is_settings_file(settings_file))
                {
                    Log.bound_to(this).log_an_info_event_containing(
                        "I found settings for \"{0}\" with settings file \"{1}\".",
                        file_system.get_file_name_without_extension_from(settings_file),
                        file_system.get_file_name_from(settings_file)
                        );
                }
            }
        }

        public bool build_the_files(string settings_files_directory, string base_directory, string working_directory, string template_extension,
                                    bool keep_extension, bool use_environment_subdirectory)
        {
            if (file_system.directory_exists(settings_files_directory))
            {
                copy_template_files(settings_files_directory, base_directory, working_directory, template_extension, keep_extension,use_environment_subdirectory);
                replace_tokens_in_output_files(settings_files_directory, working_directory);
            }

            return true;
        }

        public void copy_template_files(string settings_files_directory, string source_directory, string destination_directory, string template_extension,
                                        bool keep_extension, bool use_environment_subdirectory)
        {
            if (string.IsNullOrEmpty(source_directory) || string.IsNullOrEmpty(destination_directory))
            {
                Log.bound_to(this).log_a_warning_event_containing(
                    "Either you have left out source directory or your destination directory. You specified {0} and {1} respectively. No files will be copied."
                    , source_directory ?? ""
                    , destination_directory ?? ""
                    );
                return;
            }

            file_system.verify_or_create_directory(source_directory);
            file_system.verify_or_create_directory(destination_directory);

            traverse_directories_and_copy_files(settings_files_directory, source_directory, destination_directory, template_extension, keep_extension, use_environment_subdirectory);
        }

        public void traverse_directories_and_copy_files(string settings_files_directory, string source_directory, string destination_directory,
                                                        string template_extension, bool keep_extension, bool use_environment_subdirectory)
        {
            foreach (string file_name in file_system.get_all_file_name_strings_in(source_directory))
            {
                if (is_template_file(file_name, template_extension))
                {
                    foreach (string settings_file in file_system.get_all_file_name_strings_in(settings_files_directory))
                    {
                        if (is_settings_file(settings_file))
                        {
                            string settings_name = file_system.get_file_name_without_extension_from(settings_file);
                            string destination_file_name = keep_extension
                                                        ? file_system.get_file_name_from(file_name)
                                                        : file_system.get_file_name_without_extension_from(file_name);
                            string destination_file_path = String.Format("{0}{1}",
                                                                         destination_directory,
                                                                         use_environment_subdirectory ? "\\" + settings_name: string.Empty);
                            file_system.verify_or_create_directory(destination_file_path);
                            copy_file(file_name, String.Format("{0}\\{1}{2}", destination_file_path, use_environment_subdirectory ? string.Empty : settings_name + ".", destination_file_name));
                        }
                    }
                }
            }
            bool build_files_in_place = source_directory.to_lower() == destination_directory.to_lower();

            foreach (string child_directory in file_system.get_all_directory_name_strings_in(source_directory))
            {
                string child_destination_directory = destination_directory;
                if (build_files_in_place)
                {
                    child_destination_directory = child_directory;
                }
                traverse_directories_and_copy_files(settings_files_directory, child_directory, child_destination_directory, template_extension, keep_extension, use_environment_subdirectory);
            }
        }

        private void copy_file(string source_file_name, string destination_file_name)
        {
            Log.bound_to(this).log_an_info_event_containing(
                "Copying file \"{0}\" to directory \"{1}\" as \"{2}\".",
                file_system.get_file_name_from(source_file_name),
                file_system.get_directory_name_from(destination_file_name),
                file_system.get_file_name_from(destination_file_name));

            file_system.file_copy(source_file_name, destination_file_name, true);
        }

        private void replace_tokens_in_output_files(string settings_files_directory, string destination_directory)
        {
            foreach (string settings_file in file_system.get_all_file_name_strings_in(settings_files_directory))
            {
                if (is_settings_file(settings_file))
                {
                    string settings_name = file_system.get_file_name_without_extension_from(settings_file);

                    string settings_text = string.Empty;
                    using (TextReader file_reader = new StreamReader(settings_file))
                    {
                        settings_text = file_reader.ReadToEnd();
                        file_reader.Close();
                    }

                    //Log.bound_to(this).log_an_info_event_containing( 
                    //        "Starting to create replacement tokens for {0}.", 
                    //        settings);

                    IDictionary<string, string> token_replacement_dictionary = TemplateDictionary.create_token_replacement_dictionary(settings_text);

                    replace_tokens_in_template_files(settings_name, destination_directory, token_replacement_dictionary);
                }
            }
        }

        private void replace_tokens_in_template_files(string settings_name, string destination_directory,
                                                      IDictionary<string, string> token_replacement_dictionary)
        {
            foreach (string file_name in file_system.get_all_file_name_strings_in(destination_directory))
            {
                if (file_is_for_settings(file_system.get_file_name_from(file_name), settings_name))
                {
                    string template_file_text;
                    using (TextReader file_reader = new StreamReader(file_name))
                    {
                        template_file_text = file_reader.ReadToEnd();
                        file_reader.Close();
                    }

                    template_file_text = TokenReplacer.replace_tokens(new StringBuilder(template_file_text), token_replacement_dictionary);

                    using (TextWriter tw = new StreamWriter(file_name))
                    {
                        tw.Write(template_file_text);
                        tw.Close();
                    }
                }
            }

            foreach (string child_directory in file_system.get_all_directory_name_strings_in(destination_directory))
            {
                replace_tokens_in_template_files(settings_name, child_directory, token_replacement_dictionary);
            }
        }

        private bool is_settings_file(string settings_file)
        {
            return (!string.IsNullOrEmpty(settings_file) && file_system.get_file_info_from(settings_file).Extension.to_upper() == FILE_SETTINGS_EXTENSION);
        }

        private bool is_template_file(string file_name, string template_extension)
        {
            return (!string.IsNullOrEmpty(file_name) && file_system.get_file_info_from(file_name).Extension.to_upper() == template_extension.to_upper());
        }

        private static bool file_is_for_settings(string file_name, string settings_name)
        {
            return settings_name.to_lower() == file_name.Substring(0, file_name.IndexOf('.')).to_lower();
        }
    }
}