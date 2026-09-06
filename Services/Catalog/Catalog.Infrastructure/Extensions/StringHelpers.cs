namespace Catalog.Infrastructure.Extensions;

public static class StringHelpers
{
    public static bool EqualsCaseInsensitive(this string a, string b)
    {
        return a.ToLower() == b.ToLower();
    }
}