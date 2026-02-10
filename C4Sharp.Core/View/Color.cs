using System.Text.RegularExpressions;

namespace StacyClouds.C4Sharp
{
    public class Color
    {

        public static bool IsHexColorCode(string colorAsString)
        {
            return colorAsString != null && Regex.IsMatch(colorAsString, "^#[A-Fa-f0-9]{6}");
        }


    }
}
