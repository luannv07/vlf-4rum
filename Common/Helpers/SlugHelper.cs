namespace VlfForum.Common.Helpers;

public static class SlugHelper
{
    public static string Generate(string text)
    {
        return text
            .ToLower()
            .Replace(" ", "-")
            .Replace(".", "")
            .Replace(",", "");
    }
}