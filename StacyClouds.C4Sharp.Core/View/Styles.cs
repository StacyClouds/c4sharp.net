using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// The styles associated with this set of views.
    /// </summary>
    [DataContract]
    public sealed class Styles
    {

        private List<RelationshipStyle> _relationships;

        /// <summary>
        /// The set of relationship styles.
        /// </summary>
        [DataMember(Name="relationships", EmitDefaultValue=false)]
        public IList<RelationshipStyle> Relationships
        {
            get
            {
                return new List<RelationshipStyle>(_relationships);
            }

            internal set
            {
                _relationships = new List<RelationshipStyle>(value);
            }
        }

        private List<ElementStyle> _elements;

        /// <summary>
        /// The set of element styles.
        /// </summary>
        [DataMember(Name="elements", EmitDefaultValue=false)]
        public IList<ElementStyle> Elements
        {
            get
            {
                return new List<ElementStyle>(_elements);
            }

            set
            {
                _elements = new List<ElementStyle>(value);
            }
        }
  
        /// <summary>
        /// Initializes the style collections used by a view configuration.
        /// </summary>
        internal Styles()
        {
            _elements = new List<ElementStyle>();
            _relationships = new List<RelationshipStyle>();
        }

        /// <summary>
        /// Adds an element style when no style for the same tag exists yet.
        /// </summary>
        /// <param name="elementStyle">The element style to add.</param>
        /// <exception cref="ArgumentException">Thrown when another element style already uses the same tag.</exception>
        public void Add(ElementStyle elementStyle)
        {
            if (elementStyle != null)
            {
                if (_elements.Exists(es => es.Tag.Equals(elementStyle.Tag)))
                {
                    throw new ArgumentException("An element style for the tag \"" + elementStyle.Tag + "\" already exists.");
                }

                _elements.Add(elementStyle);
            }
        }

        /// <summary>
        /// Adds a relationship style when no style for the same tag exists yet.
        /// </summary>
        /// <param name="relationshipStyle">The relationship style to add.</param>
        /// <exception cref="ArgumentException">Thrown when another relationship style already uses the same tag.</exception>
        public void Add(RelationshipStyle relationshipStyle)
        {
            if (relationshipStyle != null)
            {
                if (_relationships.Exists(es => es.Tag.Equals(relationshipStyle.Tag)))
                {
                    throw new ArgumentException("A relationship style for the tag \"" + relationshipStyle.Tag + "\" already exists.");
                }

                _relationships.Add(relationshipStyle);
            }
        }

        /// <summary>
        /// Removes all element styles.
        /// </summary>
        public void ClearElementStyles()
        {
            _elements = new List<ElementStyle>();
        }

        /// <summary>
        /// Removes all relationship styles.
        /// </summary>
        public void ClearRelationshipStyles()
        {
            _relationships = new List<RelationshipStyle>();
        }

    }
}