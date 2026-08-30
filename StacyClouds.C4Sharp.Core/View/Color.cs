using System.Text.RegularExpressions;

namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Provides helpers for validating color values used by the view layer.
    /// </summary>
    public class Color
    {

        /// <summary>
        /// Determines whether the supplied value is a six-digit hexadecimal color code.
        /// </summary>
        /// <param name="colorAsString">The color value to validate.</param>
        /// <returns><see langword="true"/> when the value starts with <c>#</c> and contains six hexadecimal digits; otherwise, <see langword="false"/>.</returns>
        public static bool IsHexColorCode(string colorAsString)
        {
            return colorAsString != null && Regex.IsMatch(colorAsString, "^#[A-Fa-f0-9]{6}");
        }


    }
}
