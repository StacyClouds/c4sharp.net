using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp.Documentation
{

    /// <summary>
    /// Represents an image asset that can be embedded from workspace documentation.
    /// </summary>
    [DataContract]
    public sealed class Image
    {

        /// <summary>
        /// The path-like name used to reference the image from documentation.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; internal set; }

        /// <summary>
        /// The Base64-encoded image payload.
        /// </summary>
        [DataMember(Name = "content", EmitDefaultValue = false)]
        public string Content { get; private set; }

        /// <summary>
        /// The image MIME type.
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; private set; }

        /// <summary>
        /// Initializes an image placeholder for serializers.
        /// </summary>
        internal Image() { }

        /// <summary>
        /// Initializes an image asset.
        /// </summary>
        /// <param name="name">The image name used in documentation references.</param>
        /// <param name="content">The Base64-encoded image payload.</param>
        /// <param name="type">The image MIME type.</param>
        internal Image(string name, string content, string type)
        {
            this.Name = name;
            this.Content = content;
            this.Type = type;
        }

    }
}
