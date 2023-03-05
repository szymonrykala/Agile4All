namespace AgileApp.Utils
{
    public static class StringExtensions
    {
        public static string PropertyStringCompare(this string s, string newProperty)
        {
            if (newProperty != null && !string.IsNullOrWhiteSpace(newProperty) && newProperty != s)
                return newProperty;
            return s;
        }
    }
}
