namespace uppercut.infrastructure.extensions
{
    public static class StringExtensions
    {
        public static string format_using(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static string to_lower(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            return input.ToLower();
        }

        public static string to_upper(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            return input.ToUpper();
        }

    }
}