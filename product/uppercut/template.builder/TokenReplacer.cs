namespace uppercut.template.builder
{
    using System.Collections.Generic;
    using System.Text;

    public sealed class TokenReplacer
    {
        public static string replace_tokens(StringBuilder template_file_text, IDictionary<string, string> token_dictionary)
        {
            foreach (KeyValuePair<string, string> token_pair in token_dictionary)
            {
                template_file_text.Replace(token_pair.Key, token_pair.Value);
            }

            return template_file_text.ToString();
        }
    }
}