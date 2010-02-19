using bdddoc.core;
using developwithpassion.bdd.contexts;
using developwithpassion.bdd.mbunit;
using developwithpassion.bdd.mbunit.standard;
using developwithpassion.bdd.mbunit.standard.observations;

namespace uppercut.tests.template.builder
{
    using System.Text;
    using MbUnit.Framework;
    using settings;
    using templates;
    using uppercut.template.builder;

    public class TokenReplacerSpecs
    {
        public abstract class concern_for_TokenReplacer : observations_for_a_static_sut
        {
        }

        [Concern(typeof(TokenReplacer))]
        public class when_token_replacer_is_replacing_text : concern_for_TokenReplacer
        {
            protected static string result;

            context c = () => { };
            because b = () =>
            {
                result = TokenReplacer.replace_tokens(
                    new StringBuilder(ConfigTemplate1.Contents),
                    TemplateDictionary.create_token_replacement_dictionary(DevSettings.Contents)
                    );
            };

            [Observation]
            public void should_not_be_null()
            {
                result.should_not_be_null();
            }

            [Observation]
            public void should_not_contain_replaced_token_values_anymore()
            {
                Assert.AreEqual(-1, result.IndexOf("${log.level}"));
            }
        }

        [Concern(typeof(TokenReplacer))]
        public class when_token_replacer_is_replacing_text_multiple_times : concern_for_TokenReplacer
        {
            protected static string result;

            context c = () =>
                            {
                                result = TokenReplacer.replace_tokens(
                                       new StringBuilder(ConfigTemplate1.Contents),
                                       TemplateDictionary.create_token_replacement_dictionary(DevSettings.Contents)
                                       );

                                result = TokenReplacer.replace_tokens(
                                      new StringBuilder(ConfigTemplate1.Contents),
                                      TemplateDictionary.create_token_replacement_dictionary(DrSettings.Contents)
                                      );
                                result = TokenReplacer.replace_tokens(
                                      new StringBuilder(ConfigTemplate1.Contents),
                                      TemplateDictionary.create_token_replacement_dictionary(LocalSettings.Contents)
                                      );

                                result = TokenReplacer.replace_tokens(
                                      new StringBuilder(ConfigTemplate1.Contents),
                                      TemplateDictionary.create_token_replacement_dictionary(ProdSettings.Contents)
                                      );
                                result = TokenReplacer.replace_tokens(
                                      new StringBuilder(ConfigTemplate1.Contents),
                                      TemplateDictionary.create_token_replacement_dictionary(QaSettings.Contents)
                                      );
                                result = TokenReplacer.replace_tokens(
                                     new StringBuilder(ConfigTemplate1.Contents),
                                     TemplateDictionary.create_token_replacement_dictionary(Test2Settings.Contents)
                                     );

                                result = TokenReplacer.replace_tokens(
                                     new StringBuilder(ConfigTemplate1.Contents),
                                     TemplateDictionary.create_token_replacement_dictionary(TestSettings.Contents)
                                     );
                            };
            because b = () =>
            {
            };

            [Observation]
            public void should_not_be_null()
            {
                result.should_not_be_null();
            }

            [Observation]
            public void should_not_contain_replaced_token_values_anymore()
            {
                Assert.AreEqual(-1, result.IndexOf("${log.level}"));
            }

            [Observation]
            public void should_contain_replaced_values_for_the_current_items()
            {
                Assert.AreEqual(17079, result.IndexOf("TESTBondService"));
            }


        }


    }
}