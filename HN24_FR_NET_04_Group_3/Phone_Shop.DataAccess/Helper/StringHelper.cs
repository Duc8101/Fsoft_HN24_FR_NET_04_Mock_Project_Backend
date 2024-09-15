namespace Phone_Shop.DataAccess.Helper
{
    public class StringHelper
    {
        public static bool isStringNullOrEmpty(string? value)
        {
            return value == null || value.Trim().Length == 0;
        }

        public static string? getStringValue(string? value)
        {
            return value == null || value.Trim().Length == 0 ? null : value.Trim();
        }
    }
}
