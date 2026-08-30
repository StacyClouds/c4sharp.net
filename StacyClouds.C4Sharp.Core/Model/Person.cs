using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// A person who uses a software system.
    /// </summary>
    [DataContract]
    public sealed class Person : StaticStructureElement, IEquatable<Person>
    {

        /// <summary>
        /// The location of this person.
        /// </summary>
        [DataMember(Name = "location", EmitDefaultValue = true)]
        public Location Location { get; set; }

        /// <summary>
        /// Gets the canonical name for this person.
        /// </summary>
        public override string CanonicalName
        {
            get
            {
                return new CanonicalNameGenerator().Generate(this);
            }
        }

        /// <summary>
        /// Persons do not have a parent element in the static structure hierarchy.
        /// </summary>
        public override Element Parent
        {
            get
            {
                return null;
            }

            set
            {
            }
        }

        /// <summary>
        /// Initializes a person for deserialization.
        /// </summary>
        internal Person()
        {
        }

        /// <summary>
        /// Returns the tags that are always applied to people.
        /// </summary>
        /// <returns>The required person tags.</returns>
        public override List<string> GetRequiredTags()
        {
            return new List<string>
            {
                StacyClouds.C4Sharp.Tags.Element,
                StacyClouds.C4Sharp.Tags.Person
            };
        }

        /// <summary>
        /// Person-to-person delivery relationships are not supported by this API.
        /// </summary>
        /// <param name="destination">The destination person.</param>
        /// <param name="description">The relationship description.</param>
        /// <returns>This method never returns a value.</returns>
        /// <exception cref="InvalidOperationException">Always thrown because person delivery relationships are modelled via <see cref="InteractsWith(Person, string)"/>.</exception>
        public new Relationship Delivers(Person destination, string description)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Person-to-person delivery relationships are not supported by this API.
        /// </summary>
        /// <param name="destination">The destination person.</param>
        /// <param name="description">The relationship description.</param>
        /// <param name="technology">The interaction technology.</param>
        /// <returns>This method never returns a value.</returns>
        /// <exception cref="InvalidOperationException">Always thrown because person delivery relationships are modelled via <see cref="InteractsWith(Person, string, string)"/>.</exception>
        public new Relationship Delivers(Person destination, string description, string technology)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Person-to-person delivery relationships are not supported by this API.
        /// </summary>
        /// <param name="destination">The destination person.</param>
        /// <param name="description">The relationship description.</param>
        /// <param name="technology">The interaction technology.</param>
        /// <param name="interactionStyle">The interaction style.</param>
        /// <returns>This method never returns a value.</returns>
        /// <exception cref="InvalidOperationException">Always thrown because person delivery relationships are modelled via <see cref="InteractsWith(Person, string, string, InteractionStyle?)"/>.</exception>
        public new Relationship Delivers(Person destination, string description, string technology, InteractionStyle interactionStyle)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Adds an interaction between this person and another. 
        /// </summary>
        /// <param name="destination">the Person being interacted with</param>
        /// <param name="description">a description of the interaction</param>
        /// <returns>the resulting Relationship</returns>
        public Relationship InteractsWith(Person destination, string description)
        {
            return InteractsWith(destination, description, null);
        }

        /// <summary>
        /// Adds an interaction between this person and another. 
        /// </summary>
        /// <param name="destination">the Person being interacted with</param>
        /// <param name="description">a description of the interaction</param>
        /// <param name="technology">the technology of the interaction (e.g. Telephone)</param>
        /// <returns>the resulting Relationship</returns>
        public Relationship InteractsWith(Person destination, string description, string technology)
        {
            return InteractsWith(destination, description, technology, null);
        }

        /// <summary>
        /// Adds an interaction between this person and another. 
        /// </summary>
        /// <param name="destination">the Person being interacted with</param>
        /// <param name="description">a description of the interaction</param>
        /// <param name="technology">the technology of the interaction (e.g. Telephone)</param>
        /// <param name="interactionStyle">the interaction style (e.g. Synchronous or Asynchronous)</param>
        /// <returns>the resulting Relationship</returns>
        public Relationship InteractsWith(Person destination, string description, string technology, InteractionStyle? interactionStyle)
        {
            return InteractsWith(destination, description, technology, interactionStyle, new string[0]);
        }

        /// <summary>
        /// Adds an interaction between this person and another. 
        /// </summary>
        /// <param name="destination">the Person being interacted with</param>
        /// <param name="description">a description of the interaction</param>
        /// <param name="technology">the technology of the interaction (e.g. Telephone)</param>
        /// <param name="interactionStyle">the interaction style (e.g. Synchronous or Asynchronous)</param>
        /// <param name="tags">an array of tags</param>
        /// <returns>the resulting Relationship</returns>
        public Relationship InteractsWith(Person destination, string description, string technology, InteractionStyle? interactionStyle, string[] tags)
        {
            return Model.AddRelationship(this, destination, description, technology, interactionStyle, tags);
        }

        /// <summary>
        /// Compares this person with another person by canonical identity.
        /// </summary>
        /// <param name="person">The person to compare with.</param>
        /// <returns><see langword="true"/> when both people represent the same model element; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Person person)
        {
            return this.Equals(person as Element);
        }

    }
}
