using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp.Documentation
{

    /// <summary>
    /// Represents a single documentation section attached to the workspace or a model element.
    /// </summary>
    [DataContract]
    public sealed class Section
    {

        /// <summary>
        /// References the model element that owns this section, when the section is element-specific.
        /// </summary>
        public Element Element { get; internal set; }

        private string _elementId;

        /// <summary>
        /// The ID of the element.
        /// </summary>
        [DataMember(Name = "elementId", EmitDefaultValue = false)]
        public string ElementId
        {
            get
            {
                if (this.Element != null)
                {
                    return this.Element.Id;
                }
                else
                {
                    return _elementId;
                }
            }

            set
            {
                _elementId = value;
            }
        }

        /// <summary>
        /// Provides the section title shown in documentation navigation.
        /// </summary>
        [DataMember(Name = "title", EmitDefaultValue = true)]
        public string Title { get; internal set; }

        /// <summary>
        /// (this is for backwards compatibility with older client libraries)
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = true)]
        internal string SectionType
        {
            set { Title = value; }
        }

        /// <summary>
        /// Defines the display order of the section.
        /// </summary>
        [DataMember(Name = "order", EmitDefaultValue = true)]
        public int Order { get; internal set; }
        
        /// <summary>
        /// Identifies the markup format used by <see cref="Content"/>.
        /// </summary>
        [DataMember(Name = "format", EmitDefaultValue = true)]
        public Format Format { get; internal set; }

        /// <summary>
        /// Contains the section body in the configured markup format.
        /// </summary>
        [DataMember(Name = "content", EmitDefaultValue = false)]
        public string Content { get; internal set; }

        /// <summary>
        /// Initializes a section placeholder for serializers.
        /// </summary>
        internal Section() { }

        /// <summary>
        /// Initializes a documentation section.
        /// </summary>
        /// <param name="element">The owning element, or <c>null</c> for a workspace-level section.</param>
        /// <param name="title">The section title.</param>
        /// <param name="order">The display order.</param>
        /// <param name="format">The format of <paramref name="content"/>.</param>
        /// <param name="content">The section body.</param>
        internal Section(Element element, string title, int order, Format format, string content) {
            Element = element;
            Title = title;
            Order = order;
            Format = format;
            Content = content;
        }

        /// <summary>
        /// Determines whether another object represents the same documentation section.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><c>true</c> when <paramref name="obj"/> is an equivalent <see cref="Section"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Section);
        }

        /// <summary>
        /// Determines whether another section has the same scope and title.
        /// </summary>
        /// <param name="section">The section to compare with this instance.</param>
        /// <returns><c>true</c> when both sections refer to the same element scope and title; otherwise, <c>false</c>.</returns>
        public bool Equals(Section section)
        {
            if (section == this)
            {
                return true;
            }

            if (section == null)
            {
                return false;
            }
            
            if (ElementId != null)
            {
                return ElementId.Equals(section.ElementId) && Title == section.Title;
            }
            else
            {
                return Title == section.Title;
            }
        }

        /// <summary>
        /// Computes a hash code from the scoped section title.
        /// </summary>
        /// <returns>A hash code suitable for section set membership.</returns>
        public override int GetHashCode()
        {
            int result = ElementId != null ? ElementId.GetHashCode() : 0;
            result = 31 * result + Title.GetHashCode();
            return result;
        }

    }
}