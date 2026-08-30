using System;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// An instance of a model element (Person, Software System, Container or Component) in a View.
    /// </summary>
    [DataContract]
    public sealed class ElementView : IEquatable<ElementView>
    {

        /// <summary>
        /// References the model element represented by this view node.
        /// </summary>
        public Element Element { get; set; }

        private string id;

        /// <summary>
        /// The ID of the element.
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id {
            get
            {
                if (this.Element != null)
                {
                    return this.Element.Id;
                } else
                {
                    return this.id;
                }
            }

            set
            {
                this.id = value;
            }
        }
  
        /// <summary>
        /// The horizontal position of the element when rendered.
        /// </summary>
        [DataMember(Name="x", EmitDefaultValue=true)]
        public int X { get; set; }
  
        /// <summary>
        /// The vertical position of the element when rendered.
        /// </summary>
        [DataMember(Name="y", EmitDefaultValue=true)]
        public int Y { get; set; }
  
        /// <summary>
        /// Initializes an element view during deserialization.
        /// </summary>
        internal ElementView()
        {
        }

        /// <summary>
        /// Creates an element view for the supplied model element.
        /// </summary>
        /// <param name="element">The model element to wrap.</param>
        internal ElementView(Element element)
        {
            this.Element = element;
        }

        /// <summary>
        /// Determines whether another object represents the same element view.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns><see langword="true"/> when the object is an equivalent <see cref="ElementView"/>; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as ElementView);
        }

        /// <summary>
        /// Determines whether another element view represents the same element ID.
        /// </summary>
        /// <param name="elementView">The element view to compare.</param>
        /// <returns><see langword="true"/> when both views refer to the same element ID; otherwise, <see langword="false"/>.</returns>
        public bool Equals(ElementView elementView)
        {
            if (elementView == null)
            {
                return false;
            }
            if (elementView == this)
            {
                return true;
            }

            return this.Id == elementView.Id;
        }

        /// <summary>
        /// Returns a hash code based on the element identifier.
        /// </summary>
        /// <returns>A hash code for this element view.</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        /// <summary>
        /// Returns the wrapped element text when available, otherwise the serialized identifier.
        /// </summary>
        /// <returns>A human-readable representation of the element view.</returns>
        public override string ToString()
        {
            if (this.Element != null) {
                return this.Element.ToString();
            }
            else
            {
                return this.Id;
            }
        }

        /// <summary>
        /// Copies the stored X and Y coordinates from another element view.
        /// </summary>
        /// <param name="source">The source element view that provides layout coordinates.</param>
        internal void CopyLayoutInformationFrom(ElementView source)
        {
            if (source != null)
            {
                this.X = source.X;
                this.Y = source.Y;
            }
        }

    }
}
