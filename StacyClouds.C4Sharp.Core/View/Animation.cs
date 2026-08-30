using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{
    
    /// <summary>
    /// Describes a single animation step for a static or deployment view.
    /// </summary>
    [DataContract]
    public sealed class Animation
    {
        
        /// <summary>
        /// Indicates the 1-based position of this animation step within the view.
        /// </summary>
        [DataMember(Name = "order", EmitDefaultValue = false)]
        public int Order { get; internal set; }

        private HashSet<string> _elements;

        /// <summary>
        /// Identifies the element IDs revealed by this animation step.
        /// </summary>
        [DataMember(Name = "elements", EmitDefaultValue = false)]
        public ISet<string> Elements
        {
            get
            {
                return new HashSet<string>(_elements);
            }

            internal set
            {
                _elements = new HashSet<string>(value);
            }
        }

        private HashSet<string> _relationships;

        /// <summary>
        /// Identifies the relationship IDs revealed by this animation step.
        /// </summary>
        [DataMember(Name = "relationships", EmitDefaultValue = false)]
        public ISet<string> Relationships
        {
            get
            {
                return new HashSet<string>(_relationships);
            }

            internal set
            {
                _relationships = new HashSet<string>(value);
            }
        }

        /// <summary>
        /// Initializes an empty animation step during deserialization.
        /// </summary>
        internal Animation()
        {
            _elements = new HashSet<string>();
            _relationships = new HashSet<string>();
        }
        
        /// <summary>
        /// Creates an animation step from the supplied elements and relationships.
        /// </summary>
        /// <param name="order">The 1-based order of the step.</param>
        /// <param name="elements">The elements revealed in the step.</param>
        /// <param name="relationships">The relationships revealed in the step.</param>
        internal Animation(int order, ISet<Element> elements, ISet<Relationship> relationships) : this()
        {
            Order = order;

            foreach (Element element in elements)
            {
                _elements.Add(element.Id);
            }

            foreach (Relationship relationship in relationships)
            {
                _relationships.Add(relationship.Id);
            }
        }

    }
}