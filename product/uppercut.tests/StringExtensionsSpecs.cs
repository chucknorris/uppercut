namespace uppercut.tests
{
    using bdddoc.core;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;

    [Concern(typeof(StringExtensions))]
    public class when_the_string_extensions_formats_a_string_using_provided_arguments : observations_for_a_static_sut
    {
        static string result;

        because b = () => result = "this is the {0}".format_with(1);

        [Observation]
        public void should_return_the_string_formatted_with_the_arguments()
        {
            result.should_be_equal_to("this is the 1");
        }
    }

    [Concern(typeof(StringExtensions))]
    public class when_the_string_extensions_attempts_to_lower_on_a_regular_string : observations_for_a_static_sut
    {
        static string result;
        private static string test = "BILL1";


        private because b = () => result = test.to_lower();

        [Observation]
        public void should_not_error_out()
        {
            result.should_be_equal_to("bill1");
        }
    }

    [Concern(typeof(StringExtensions))]
    public class when_the_string_extensions_attempts_to_lower_on_an_empty_string : observations_for_a_static_sut
    {
        static string result;
        private static string test = string.Empty;

        private because b = () => result = test.to_lower();

        [Observation]
        public void should_not_error_out()
        {
            result.should_be_equal_to(string.Empty);
        }
    }

    [Concern(typeof(StringExtensions))]
    public class when_the_string_extensions_attempts_to_lower_on_a_null_string : observations_for_a_static_sut
    {
        static string result;
        private static string test = null;

        private because b = () => result = test.to_lower();

        [Observation]
        public void should_not_error_out()
        {
            result.should_be_equal_to(null);
        }
    }

    [Concern(typeof(StringExtensions))]
    public class when_the_string_extensions_attempts_to_upper_on_a_regular_string : observations_for_a_static_sut
    {
        static string result;
        private static string test = "bill1";


        private because b = () => result = test.to_upper();

        [Observation]
        public void should_not_error_out()
        {
            result.should_be_equal_to("BILL1");
        }
    }


    [Concern(typeof(StringExtensions))]
    public class when_the_string_extensions_attempts_to_upper_on_an_empty_string : observations_for_a_static_sut
    {
        static string result;

        private because b = () => result = string.Empty.to_upper();

        [Observation]
        public void should_not_error_out()
        {
            result.should_be_equal_to(string.Empty);
        }
    }

    [Concern(typeof(StringExtensions))]
    public class when_the_string_extensions_attempts_to_upper_on_a_null_string : observations_for_a_static_sut
    {
        static string result;
        private static string test = null;

        private because b = () => result = test.to_upper();

        [Observation]
        public void should_not_error_out()
        {
            result.should_be_equal_to(null);
        }
    }

    [Concern(typeof(StringExtensions))]
    public class when_the_string_extensions_attempts_to_replace_last_instance_on_a_regular_string : observations_for_a_static_sut
    {
        static string result;

        private because b = () => result = @"C:\CodeBuild\DataManager-Debug\build".replace_last_instance_of("build", "build.custom");

        [Observation]
        public void should_not_replace_first_instance()
        {
            result.should_contain(@"C:\CodeBuild\DataManager-Debug");
        }

        [Observation]
        public void should_replace_last_instance()
        {
            result.should_contain(@"DataManager-Debug\build.custom");
        }
    }

    [Concern(typeof(StringExtensions))]
    public class when_the_string_extensions_attempts_to_replace_last_instance_on_a_null_string : observations_for_a_static_sut
    {
        static string result;
        private static string test = null;

        private because b = () => result = test.replace_last_instance_of("build", "build.custom");

        [Observation]
        public void should_not_error_out()
        {
            result.should_be_equal_to(null);
        }
    }
    
    [Concern(typeof(StringExtensions))]
    public class when_the_string_extensions_attempts_to_replace_last_instance_when_no_matches_exist : observations_for_a_static_sut
    {
        static string result;

        private because b = () => result = @"C:\CodeBuild\DataManager-Debug\build".replace_last_instance_of("builds", "build.custom");

        [Observation]
        public void the_original_string_should_be_returned()
        {
            result.should_be_equal_to(@"C:\CodeBuild\DataManager-Debug\build");
        }
    }
}