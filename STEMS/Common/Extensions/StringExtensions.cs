namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static string CapitalizeFirstLetter(this string text)
        {
            return char.ToUpper(text[0]) + text[1..];
        }
    }
}
