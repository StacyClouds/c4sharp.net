using System;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Describes an explicit view size in pixels.
    /// </summary>
    [DataContract]
    public class Dimensions
    {

        private int _width;
        
        /// <summary>
        /// Specifies the rendered width, in pixels.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the value is negative.</exception>
        [DataMember(Name = "width", EmitDefaultValue = false)]
        public int Width
        {
            get { return _width; }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("The width must be a positive integer.");
                }

                _width = value;
            }
        }

        private int _height;
        
        /// <summary>
        /// Specifies the rendered height, in pixels.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the value is negative.</exception>
        [DataMember(Name = "height", EmitDefaultValue = false)]
        public int Height
        {
            get { return _height; }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("The height must be a positive integer.");
                }

                _height = value;
            }
        }

        /// <summary>
        /// Initializes an empty dimensions object during deserialization.
        /// </summary>
        internal Dimensions()
        {
        }

        /// <summary>
        /// Creates a size with the supplied width and height.
        /// </summary>
        /// <param name="width">The width in pixels.</param>
        /// <param name="height">The height in pixels.</param>
        public Dimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }
        
    }

}