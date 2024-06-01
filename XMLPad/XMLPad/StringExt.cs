namespace XMLPad
{
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength, bool useEllipsis)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value.Length <= maxLength ? value : (value.Substring(0, maxLength) + (useEllipsis ? "..." : string.Empty));
        }

    }
}
