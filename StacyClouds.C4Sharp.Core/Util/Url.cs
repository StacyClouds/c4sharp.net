using System;

namespace StacyClouds.C4Sharp.Util
{

    /// <summary>
    /// Provides helpers for validating URL strings.
    /// </summary>
    public class Url
    {

        /// <summary>
        /// Determines whether a string can be parsed as an absolute URL.
        /// </summary>
        /// <param name="urlAsString">The string to validate.</param>
        /// <returns><c>true</c> when the string is a non-empty absolute URL; otherwise, <c>false</c>.</returns>
        public static bool IsUrl(string urlAsString)
        {
            if (urlAsString != null && urlAsString.Trim().Length > 0)
            {
                Uri uri;
                return Uri.TryCreate(urlAsString, UriKind.Absolute, out uri);
            }

            return false;
        }

    }

}
