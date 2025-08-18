namespace Webamoki.Utils;

public static class StringExtensions
{
    public static string TrimEnd(this string source, string suffixToRemove)
    {
        if (!source.EndsWith(suffixToRemove)) throw new Exception("Invalid Operation");
        return source[..^suffixToRemove.Length];
    }
}