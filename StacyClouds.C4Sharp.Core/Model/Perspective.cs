using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// Represents an architectural perspective, that can be applied to elements and relationships.
    /// See https://www.viewpoints-and-perspectives.info/home/perspectives/ for more details of this concept
    /// </summary>
    [DataContract]
    public sealed class Perspective : IEquatable<Perspective>
    {

        /// <summary>
        /// The name of this perspective.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; internal set; }

        /// <summary>
        /// The content of this perspective.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; internal set; }

        /// <summary>
        /// Initializes a perspective for deserialization.
        /// </summary>
        internal Perspective()
        {
        }

        /// <summary>
        /// Initializes a perspective with a name and description.
        /// </summary>
        /// <param name="name">The perspective name.</param>
        /// <param name="description">The perspective description.</param>
        internal Perspective(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Compares this perspective with another object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns><see langword="true"/> when <paramref name="obj"/> is a matching <see cref="Perspective"/>; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Perspective);
        }

        /// <summary>
        /// Compares this perspective with another by name.
        /// </summary>
        /// <param name="other">The perspective to compare with.</param>
        /// <returns><see langword="true"/> when both perspectives have the same name; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Perspective other)
        {
            return other != null &&
                   Name == other.Name;
        }

        /// <summary>
        /// Returns a hash code derived from the perspective name.
        /// </summary>
        /// <returns>A hash code for the current perspective.</returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

    }
}