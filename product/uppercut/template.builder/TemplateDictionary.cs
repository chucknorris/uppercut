namespace uppercut.template.builder
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public sealed class TemplateDictionary
    {
        public static IDictionary<string, string> create_token_replacement_dictionary(string settings_text)
        {
            IDictionary<string, string> token_replacement_dictionary = new Dictionary<string, string>();

            foreach (Match name_value_match in get_matches(settings_text))
            {
                string pair_key_text = get_name_value(name_value_match.ToString());
                string pair_value_text = get_pair_value(name_value_match.ToString(), token_replacement_dictionary);
                pair_key_text = @"${" + pair_key_text + @"}";
                if (!token_replacement_dictionary.ContainsKey(pair_key_text))
                    token_replacement_dictionary.Add(pair_key_text, pair_value_text);
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

        public static string get_name_value(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            const string token_key_start = "name=\"";
            const string token_key_end = "\"";

            return get_token_value(input, token_key_start, token_key_end);
        }

        public static string get_pair_value(string input, IDictionary<string, string> tokens)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            const string token_value_start = "value=\"";
            const string token_value_end = "\"";

            return
                replace_tokens_with_other_dictionary_values(get_token_value(input, token_value_start, token_value_end),
                                                            tokens);
        }

        public static string replace_tokens_with_other_dictionary_values(string input,
                                                                         IDictionary<string, string> tokens)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string token = get_token_value(input, "${", "}");
            if (tokens.ContainsKey(@"${" + token + @"}"))
            {
                return input.Replace(@"${" + token + @"}", tokens[@"${" + token + @"}"]);
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