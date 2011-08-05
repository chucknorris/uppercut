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

    }
}