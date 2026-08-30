using System;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// Represents a validated background and foreground color combination.
    /// </summary>
    [DataContract]
    public sealed class ColorPair
    {

        private string _background;
        private string _foreground;

        /// <summary>
        /// Initializes an empty color pair during deserialization.
        /// </summary>
        internal ColorPair()
        { }

        /// <summary>
        /// Creates a color pair from the supplied hexadecimal color codes.
        /// </summary>
        /// <param name="background">The background color as a six-digit hexadecimal value.</param>
        /// <param name="foreground">The foreground color as a six-digit hexadecimal value.</param>
        public ColorPair(string background, string foreground)
        {
            this.Background = background;
            this.Foreground = foreground;
        }

        /// <summary>
        /// Defines the background color as a normalized lowercase hexadecimal value.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the value is not a valid hexadecimal color code.</exception>
        [DataMember(Name = "background", EmitDefaultValue = false)]
        public string Background
        {
            get { return this._background; }
            set
            {
                if (Color.IsHexColorCode(value))
                {
                    this._background = value.ToLower();
                }
                else
                {
                    throw new ArgumentException("'" + value + "' is not a valid hex color code.");
                }
            }
        }

        /// <summary>
        /// Defines the foreground color as a normalized lowercase hexadecimal value.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the value is not a valid hexadecimal color code.</exception>
        [DataMember(Name = "foreground", EmitDefaultValue = false)]
        public string Foreground
        {
            get { return this._foreground; }
            set
            {
                if (Color.IsHexColorCode(value))
                {
                    this._foreground = value.ToLower();
                }
                else
                {
                    throw new ArgumentException("'" + value + "' is not a valid hex color code.");
                }
            }
        }

    }

}
