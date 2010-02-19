using bdddoc.core;
using developwithpassion.bdd.contexts;
using developwithpassion.bdd.mbunit;
using developwithpassion.bdd.mbunit.standard;
using developwithpassion.bdd.mbunit.standard.observations;

namespace uppercut.tests.template.builder
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using MbUnit.Framework;
    using settings;
    using uppercut.template.builder;

    public class TemplateDictionarySpecs
    {
        public abstract class concern_for_TemplateDictionary : observations_for_a_static_sut
        {
        }

        [Concern(typeof(TemplateDictionary))]
        public class when_matching_for_name_value_pairs_for_the_environment_dictionary : concern_for_TemplateDictionary
        {
            protected static IList<Match> result;

            context c = () => { };

            private because b = () => { };

            [Observation]
            public void should_match_a_name_value_of_just_strings()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob\" value=\"bill\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob\" value=\"bill\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_uppercase()
            {
                result = TemplateDictionary.get_matches("<property name=\"Bob\" value=\"bIll\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"Bob\" value=\"bIll\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_an_extra_space()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob\"  value=\"bill\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob\"  value=\"bill\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_numbers()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob1\" value=\"bill2\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob1\" value=\"bill2\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_underscores()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob_1\" value=\"bill_2\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob_1\" value=\"bill_2\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_periods()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob.1\" value=\"bill.2\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob.1\" value=\"bill.2\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_tick_mark()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob'1\" value=\"bill'2\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob'1\" value=\"bill'2\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_spaces()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob 1\" value=\"bill 2\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob 1\" value=\"bill 2\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_open_brackets()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob{\" value=\"bill{\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob{\" value=\"bill{\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_close_brackets()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob}\" value=\"bill}\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob}\" value=\"bill}\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_back_slashes()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob\\\" value=\"bill\\\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob\\\" value=\"bill\\\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_forward_slashes()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob/\" value=\"bill/\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob/\" value=\"bill/\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_dollar_signs()
            {
                result = TemplateDictionary.get_matches("<property name=\"$bob\" value=\"$bill\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"$bob\" value=\"$bill\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_open_parentheses()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob(\" value=\"bill(\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob(\" value=\"bill(\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_close_parentheses()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob)\" value=\"bill)\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob)\" value=\"bill)\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_open_brace()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob[\" value=\"bill[\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob[\" value=\"bill[\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_close_brace()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob]\" value=\"bill]\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob]\" value=\"bill]\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_percentage()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob]\" value=\"bill%\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob]\" value=\"bill%\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_pound_symbol()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob]\" value=\"bill#\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob]\" value=\"bill#\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_where_value_is_empty()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob]\" value=\"\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob]\" value=\"\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_a_name_value_with_email_address()
            {
                result = TemplateDictionary.get_matches("<property name=\"bob@bob.com\" value=\"bill@bob.com\" />");
                Assert.AreEqual(1, result.Count);
                foreach (Match match in result)
                {
                    Assert.AreEqual("name=\"bob@bob.com\" value=\"bill@bob.com\"", match.ToString());
                }
            }

            [Observation]
            public void should_match_quite_a_few_times_against_a_real_settings_file()
            {
                result = TemplateDictionary.get_matches(DevSettings.Contents);
                Assert.AreEqual(62,result.Count);
            }

        }

        [Concern(typeof(TemplateDictionary))]
        public class when_environment_dictionary_is_adding_a_value_to_the_dictionary_that_points_to_another_key_in_the_dictionary : concern_for_TemplateDictionary
        {
            protected static  string result;

            context c = () => { };

            private because b = () => {  };

            [Observation]
            public void should_replace_the_token_value_with_the_value_from_the_key()
            {
                IDictionary<string, string> tokens = new Dictionary<string,string>();
                tokens.Add(@"${bill}", "ypiiipe");
                result = TemplateDictionary.replace_tokens_with_other_dictionary_values("\\\\${bill}\\yo\"",tokens);
                Assert.AreEqual("\\\\ypiiipe\\yo\"", result);
            }

        }

        [Concern(typeof(TemplateDictionary))]
        public class when_environment_dictionary_is_getting_a_token_value_from_a_string : concern_for_TemplateDictionary
        {
            protected static string result;

            context c = () => { };

            private because b = () => { };

            [Observation]
            public void should_return_the_value_when_the_start_and_end_parts_are_found()
            {
                Assert.AreEqual("bob",TemplateDictionary.get_token_value("<property name=\"bob\" value=\"$bill\" />","name=\"","\""));
            }

            [Observation]
            public void should_return_nothing_when_the_value_when_the_start_is_not_found()
            {
                Assert.AreEqual(string.Empty, TemplateDictionary.get_token_value("<property name=\"bob\" value=\"$bill\" />", "${", "\""));
            }

            [Observation]
            public void should_return_nothing_when_the_value_when_the_end_is_not_found()
            {
                Assert.AreEqual(string.Empty, TemplateDictionary.get_token_value("<property name=\"bob\" value=\"$bill\" />", "name=\"", "Z"));
            }
        }

        [Concern(typeof(TemplateDictionary))]
        public class when_environment_dictionary_is_creating_a_token_replacement_dictionary : concern_for_TemplateDictionary
        {
            protected static IDictionary<string, string> result;

            context c = () => { };

            private because b = () => { result = TemplateDictionary.create_token_replacement_dictionary(DevSettings.Contents); };

            [Observation]
            public void should_create_quite_a_few_matches()
            {
                Assert.AreEqual(62, result.Count);
            }

            [Observation]
            public void should_have_a_key_with_a_replaced_token()
            {
                Assert.AreEqual(@"\\appdevserver\Apps", result["${dirs.app.toplevel}"]);
            }


        }
  
    
    }
}