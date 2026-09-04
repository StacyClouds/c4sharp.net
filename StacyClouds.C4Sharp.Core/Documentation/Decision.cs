using System;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp.Documentation
{

    /// <summary>
    /// Represents a single (architecture) decision, as described at http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions
    /// </summary>
    [DataContract]
    public sealed class Decision
    {

        /// <summary>
        /// References the model element that owns this decision, when the decision is scoped below the workspace.
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
        /// Uniquely identifies the decision within its scope.
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; internal set; }

        /// <summary>
        /// Records when the decision was made.
        /// </summary>
        [DataMember(Name = "date", EmitDefaultValue = false)]
        public DateTime Date { get; internal set; }

        /// <summary>
        /// Supplies the short human-readable title for the decision.
        /// </summary>
        [DataMember(Name = "title", EmitDefaultValue = false)]
        public string Title { get; internal set; }

        /// <summary>
        /// Indicates the lifecycle state of the decision.
        /// </summary>
        [DataMember(Name = "status", EmitDefaultValue = true)]
        public DecisionStatus Status { get; internal set; }

        /// <summary>
        /// Describes the markup format used by <see cref="Content"/>.
        /// </summary>
        [DataMember(Name = "format", EmitDefaultValue = true)]
        public Format Format { get; internal set; }

        /// <summary>
        /// Contains the decision body in the configured markup format.
        /// </summary>
        [DataMember(Name = "content", EmitDefaultValue = false)]
        public string Content { get; internal set; }

        /// <summary>
        /// Initializes a decision placeholder for serializers.
        /// </summary>
        internal Decision()
        {
        }

        /// <summary>
        /// Initializes a decision entry.
        /// </summary>
        /// <param name="element">The owning element, or <c>null</c> for a workspace-level decision.</param>
        /// <param name="id">The unique decision identifier.</param>
        /// <param name="date">The date associated with the decision.</param>
        /// <param name="title">The decision title.</param>
        /// <param name="status">The decision status.</param>
        /// <param name="format">The format of <paramref name="content"/>.</param>
        /// <param name="content">The decision body.</param>
        internal Decision(Element element, string id, DateTime date, string title, DecisionStatus status, Format format, string content)
        {
            Element = element;
            Id = id;
            Date = date;
            Title = title;
            Status = status;
            Format = format;
            Content = content;
        }

        /// <summary>
        /// Determines whether another object represents the same decision.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><c>true</c> when <paramref name="obj"/> is an equivalent <see cref="Decision"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Decision);
        }

        /// <summary>
        /// Determines whether another decision has the same scope and identifier.
        /// </summary>
        /// <param name="decision">The decision to compare with this instance.</param>
        /// <returns><c>true</c> when both decisions refer to the same element scope and identifier; otherwise, <c>false</c>.</returns>
        public bool Equals(Decision decision)
        {
            if (decision == this)
            {
                return true;
            }

            if (decision == null)
            {
                return false;
            }

            if (ElementId != null)
            {
                return ElementId.Equals(decision.ElementId) && Id == decision.Id;
            }
            else
            {
                return Id == decision.Id;
            }
        }

        /// <summary>
        /// Computes a hash code from the scoped decision identifier.
        /// </summary>
        /// <returns>A hash code suitable for decision set membership.</returns>
        public override int GetHashCode()
        {
            int result = ElementId != null ? ElementId.GetHashCode() : 0;
            result = 31 * result + Id.GetHashCode();
            return result;
        }


    }

}