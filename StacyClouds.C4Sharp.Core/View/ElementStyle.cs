using System;
using System.Runtime.Serialization;
using StacyClouds.C4Sharp.Util;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// A definition of an element style.
    /// </summary>
    [DataContract]
    public sealed class ElementStyle
    {
        
        /// <summary>
        /// The tag to which this element style applies.
        /// </summary>
        [DataMember(Name="tag", EmitDefaultValue=false)]
        public string Tag { get; set; }
        
        /// <summary>
        /// The width of the element, in pixels.
        /// </summary>
        [DataMember(Name="width", EmitDefaultValue=false)]
        public int? Width { get; set; }
        
        /// <summary>
        /// The height of the element, in pixels.
        /// </summary>
        [DataMember(Name="height", EmitDefaultValue=false)]
        public int? Height { get; set; }

        private string _background;

        /// <summary>
        /// Defines the background color of the element as a six-digit hexadecimal value such as <c>#1168bd</c>.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the value is not a valid hexadecimal color code.</exception>
        [DataMember(Name = "background", EmitDefaultValue = false)]
        public string Background
        {
            get
            {
                return this._background;
            }

            set
            {
                if (StacyClouds.C4Sharp.Color.IsHexColorCode(value))
                {
                    this._background = value.ToLower();
                }
                else
                {
                    throw new ArgumentException("'" + value + "' is not a valid hex color code.");
                }
            }
        }

        private string _stroke;

        /// <summary>
        /// Defines the stroke color of the element as a six-digit hexadecimal value such as <c>#ffffff</c>.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the value is not a valid hexadecimal color code.</exception>
        [DataMember(Name = "stroke", EmitDefaultValue = false)]
        public string Stroke
        {
            get
            {
                return this._stroke;
            }

            set
            {
                if (StacyClouds.C4Sharp.Color.IsHexColorCode(value))
                {
                    this._stroke = value.ToLower();
                }
                else
                {
                    throw new ArgumentException("'" + value + "' is not a valid hex color code.");
                }
            }
        }

        private string _color;

        /// <summary>
        /// Defines the foreground text color as a six-digit hexadecimal value such as <c>#000000</c>.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the value is not a valid hexadecimal color code.</exception>
        [DataMember(Name = "color", EmitDefaultValue = false)]
        public string Color
        {
            get
            {
                return this._color;
            }

            set
            {
                if (StacyClouds.C4Sharp.Color.IsHexColorCode(value))
                {
                    this._color = value.ToLower();
                }
                else
                {
                    throw new ArgumentException("'" + value + "' is not a valid hex color code.");
                }
            }
        }

        /// <summary>
        /// The standard font size used to render text, in pixels.
        /// </summary>
        /// <value>The standard font size used to render text, in pixels.</value>
        [DataMember(Name="fontSize", EmitDefaultValue=false)]
        public int? FontSize { get; set; }

        /// <summary>
        /// The shape used to render the element.
        /// </summary>
        [DataMember(Name="shape", EmitDefaultValue=false)]
        public Shape Shape { get; set; }

        private string _icon;

        /// <summary>
        /// Specifies an icon URL or data URI rendered with the element.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the value is not a URL or image data URI.</exception>
        [DataMember(Name = "icon", EmitDefaultValue = false)]
        public string Icon
        {
            get { return _icon; }
            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    if (Url.IsUrl(value) || value.StartsWith("data:image/"))
                    {
                        _icon = value.Trim();
                    }
                    else {
                        throw new ArgumentException(value + " is not a valid URL.");
                    }
                }
            }
        }

        /// <summary>
        /// The border to use when rendering the element.
        /// </summary>
        [DataMember(Name="border", EmitDefaultValue=false)]
        public Border Border { get; set; }

        private int? _opacity;

        /// <summary>
        /// The opacity of the line/text; 0 to 100.
        /// </summary>
        [DataMember(Name = "opacity", EmitDefaultValue = false)]
        public int? Opacity
        {
            get { return _opacity; }
            set
            {
                if (value != null)
                {
                    if (value < 0)
                    {
                        _opacity = 0;
                    }
                    else if (value > 100)
                    {
                        _opacity = 100;
                    }
                    else {
                        _opacity = value;
                    }
                }
            }
        }

        /// <summary>
        /// A flag to indicate whether the element metadata should be shown or not.
        /// </summary>
        [DataMember(Name = "metadata", EmitDefaultValue = false)]
        public bool? Metadata { get; set; }

        /// <summary>
        /// A flag to indicate whether the element description should be shown or not.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public bool? Description { get; set; }

        /// <summary>
        /// Initializes an element style during deserialization.
        /// </summary>
        internal ElementStyle()
        {
        }

        /// <summary>
        /// Creates an element style for the supplied tag.
        /// </summary>
        /// <param name="tag">The tag this style applies to.</param>
        public ElementStyle(string tag)
        {
            this.Tag = tag;
        }

    }
}