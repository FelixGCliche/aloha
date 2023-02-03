using System.Text.RegularExpressions;

namespace Game
{
    // Author: David Dorion
    public static class StringExtensions
    {
        public static string RemoveFileType(this string str)
        {
            return str.Substring(0,str.IndexOf("."));
        }
        
        public static string GetNumbers(this string str)
        {
            return Regex.Replace(str, "[^.0-9]", "");
        }
    }
}