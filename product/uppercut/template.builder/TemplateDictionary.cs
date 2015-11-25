using System.IO;
using System.Xml;

namespace uppercut.template.builder
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public sealed class TemplateDictionary
    {
        public static IDictionary<string, string> create_token_replacement_dictionary(string settings_text)
        {
            IDictionary<string, string> token_replacement_dictionary = new Dictionary<string, string>();
            token_replacement_dictionary.Add("${quote}", "\"");

            using (var text = new StringReader(settings_text))
            using (var reader = XmlReader.Create(text, new XmlReaderSettings { IgnoreWhitespace = true }))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "property")
                            {
                                var name = reader["name"];
                                var value = reader["value"] ?? reader.ReadInnerXml();
                                if (!string.IsNullOrEmpty(value))
                                {
                                    var replaced = replace_tokens_with_other_dictionary_values(value, token_replacement_dictionary);
                                    token_replacement_dictionary.Add("${" + name + "}", replaced);
                                }
                            }
                            break;
                    }
                }
            }

            return token_replacement_dictionary;
        }

        public static IList<Match> get_matches(string settings_text)
        {
            IList<Match> matches = new List<Match>();
            Regex regular_expression =
                new Regex(@"name=""[\w\.\'\`\{\}\s\,\;\:\\\/\$\-\(\)\[\]\@\%]+""\s+value="".*""",
                          RegexOptions.Multiline & RegexOptions.Compiled);

            foreach (Match name_value_match in regular_expression.Matches(settings_text))
            {
                matches.Add(name_value_match);
            }

            return matches;
        }

        public static IList<Match> get_value_token_matches(string value_text)
        {
            IList<Match> matches = new List<Match>();
            Regex regular_expression = new Regex(@"\$\{[\w\.\'\`\s\,\;\:\\\/\$\-\(\)\[\]\@\%]+\}", RegexOptions.Singleline & RegexOptions.Compiled);

            foreach (Match name_value_match in regular_expression.Matches(value_text))
            {
                matches.Add(name_value_match);
            }

            return matches;
        }


        public static string replace_tokens_with_other_dictionary_values(string input, IDictionary<string, string> tokens)
        {
            if (string.IsNullOrEmpty(input)) return input;

            foreach (Match token in get_value_token_matches(input).or_empty_list())
            {
                if (token != null && !string.IsNullOrWhiteSpace(token.Value))
                {
                    string tokenValue = token.Value;
                    if (tokens.ContainsKey(tokenValue))
                    {
                        input = input.Replace(tokenValue, tokens[tokenValue]);
                    }
                }
            }

            return input;
        }

        public static string get_token_value(string input, string token_start, string token_end)
        {
            int position = 0;
            position = move_to_position(input, position, token_start);
            if (position == -1) return string.Empty;

            int position_end = move_to_position(input, position, token_end);
            int end_of_value = (position_end - position) - 1;

            if (position_end < position)
            {
                return string.Empty;
            }

            return input.Substring(position, end_of_value);
        }

        private static int move_to_position(string settings_text, int start_position, string token_find)
        {
            int found_position = settings_text.IndexOf(token_find, start_position);
            if (found_position != -1)
            {
                found_position += token_find.Length;
            }

            return found_position;
        }
    }
}