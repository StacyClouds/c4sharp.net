using System;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Represents the enterprise boundary that owns the software systems in a model.
    /// </summary>
    [DataContract]
    public sealed class Enterprise
    {

        /// <summary>
        /// The name of this enterprise.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Initializes an enterprise with the specified display name.
        /// </summary>
        /// <param name="name">The enterprise name.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null, empty, or whitespace.</exception>
        public Enterprise(string name)
        {
            if (name == null || name.Trim().Length == 0)
            {
                throw new ArgumentException("Name must be specified.");
            }

            this.Name = name;
        }

    }
}
