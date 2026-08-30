using System;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// Describes an external font resource used by branding.
    /// </summary>
    [DataContract]
    public sealed class Font
    {

        private string _url;

        /// <summary>
        /// Stores the font family name advertised to renderers.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name;

        /// <summary>
        /// Specifies a URL where the font can be obtained.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the value is not a valid URL.</exception>
        [DataMember(Name = "url", EmitDefaultValue = false)]
        public string Url
        {
            get
            {
                return _url;
            }

            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    if (Util.Url.IsUrl(value))
                    {
                        this._url = value;
                    }
                    else
                    {
                        throw new ArgumentException(value + " is not a valid URL.");
                    }
                }
            }
        }

        /// <summary>
        /// Initializes an empty font definition during deserialization.
        /// </summary>
        internal Font()
        {
        }
        
        /// <summary>
        /// Creates a font definition with a family name.
        /// </summary>
        /// <param name="name">The font family name.</param>
        public Font(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Creates a font definition with a family name and downloadable URL.
        /// </summary>
        /// <param name="name">The font family name.</param>
        /// <param name="url">The URL where the font can be obtained.</param>
        public Font(string name, string url)
        {
            this.Name = name;
            this.Url = url;
        }

    }

}
