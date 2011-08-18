namespace uppercut
{
    public static class StringExtensions
    {
        public static string format_with(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static string to_lower(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            return input.ToLower();
        }

        public static string to_upper(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            return input.ToUpper();
        }

        public static string replace_last_instance_of(this string input, string toReplace, string replaceWith)
        {
            if (input == null)
            {
                return null;
            }

            int indexOf = input.LastIndexOf(toReplace);
            if (indexOf == -1)
            {
                return input;
            }

            string removedString = input.Remove(indexOf, toReplace.Length);
            string replacedString = removedString.Insert(indexOf, replaceWith);
            return replacedString;
        }
    }
}