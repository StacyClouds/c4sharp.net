using System;
using System.Runtime.Serialization;
using StacyClouds.C4Sharp.Util;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// Defines workspace-wide branding such as logos and fonts.
    /// </summary>
    [DataContract]
    public sealed class Branding
    {

        /// <summary>
        /// Specifies the font metadata to use when rendering diagrams.
        /// </summary>
        [DataMember(Name = "font", EmitDefaultValue = false)]
        public Font Font;

        private string _logo;

        /// <summary>
        /// Provides a logo URL or data URI that renderers can display with diagrams.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the supplied value is not a URL or image data URI.</exception>
        [DataMember(Name = "logo", EmitDefaultValue = false)]
        public string Logo
        {
            get { return _logo; }
            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    if (Url.IsUrl(value) || value.StartsWith("data:image/"))
                    {
                        _logo = value.Trim();
                    }
                    else {
                        throw new ArgumentException(value + " is not a valid URL.");
                    }
                }
            }
        }

    }

}