using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// A relationship between two elements.
    /// </summary>
    [DataContract]
    public sealed class Relationship : ModelItem, IEquatable<Relationship>
    {

        private string _description;

        /// <summary>
        /// A short description of this relationship.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description
        {
            get
            {
                return _description ?? "";
            }

            internal set { _description = value; }
        }

        private string _sourceId;

        /// <summary>
        /// The ID of the source element.
        /// </summary>
        [DataMember(Name = "sourceId", EmitDefaultValue = false)]
        public string SourceId
        {
            get
            {
                if (Source != null)
                {
                    return Source.Id;
                }
                else
                {
                    return _sourceId;
                }
            }
            set
            {
                _sourceId = value;
            }
        }

        /// <summary>
        /// The source element of the relationship.
        /// </summary>
        public Element Source { get; set; }

        private string _destinationId;

        /// <summary>
        /// The ID of the destination element.
        /// </summary>
        [DataMember(Name = "destinationId", EmitDefaultValue = false)]
        public string DestinationId
        {
            get
            {
                if (Destination != null)
                {
                    return Destination.Id;
                }
                else
                {
                    return _destinationId;
                }
            }
            set
            {
                _destinationId = value;
            }
        }

        /// <summary>
        /// The destination element of the relationship.
        /// </summary>
        public Element Destination { get; set; }

        /// <summary>
        /// The technology associated with this relationship (e.g. HTTPS, JDBC, etc).
        /// </summary>
        [DataMember(Name = "technology", EmitDefaultValue = false)]
        public string Technology { get; internal set; }

        private InteractionStyle? _interactionStyle;

        /// <summary>
        /// The identifier of the static relationship that this replicated deployment relationship was derived from.
        /// </summary>
        [DataMember(Name = "linkedRelationshipId", EmitDefaultValue = false)]
        public string LinkedRelationshipId { get; internal set; }

        /// <summary>
        /// The interaction style (synchronous or asynchronous).
        /// </summary>
        [DataMember(Name = "interactionStyle", EmitDefaultValue = false)]
        public InteractionStyle? InteractionStyle
        {
            get
            {
                return _interactionStyle;
            }
            set
            {
                _interactionStyle = value;
            }
        }
        
        private string _url;

        /// <summary>
        /// The URL where more information about this relationship can be found.
        /// </summary>
        [DataMember(Name = "url", EmitDefaultValue = false)]
        public string Url
        {
            get
            {
                return _url;
            }

            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    if (Util.Url.IsUrl(value))
                    { 
                        this._url = value;
                    }
                    else
                    {
                        throw new ArgumentException(value + " is not a valid URL.");
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a relationship for deserialization.
        /// </summary>
        internal Relationship()
        {
        }

        /// <summary>
        /// Initializes a relationship between two elements.
        /// </summary>
        /// <param name="source">The source element.</param>
        /// <param name="destination">The destination element.</param>
        /// <param name="description">The relationship description.</param>
        /// <param name="technology">The relationship technology.</param>
        /// <param name="interactionStyle">The interaction style, if specified.</param>
        /// <param name="tags">Additional tags to apply.</param>
        internal Relationship(Element source, Element destination, string description, string technology, InteractionStyle? interactionStyle, string[] tags) :
            this()
        {
            Source = source;
            Destination = destination;
            Description = description;
            Technology = technology;
            InteractionStyle = interactionStyle;

            AddTags(tags);
        }

        /// <summary>
        /// Returns the built-in tags that apply to this relationship.
        /// </summary>
        /// <returns>
        /// A list containing the relationship tag and, for non-linked relationships,
        /// the appropriate synchronous or asynchronous tag.
        /// </returns>
        public override List<string> GetRequiredTags()
        {
            if (LinkedRelationshipId == null) {
                List<string> tags = new List<string>(); 
                tags.Add(StacyClouds.C4Sharp.Tags.Relationship);
                
                if (InteractionStyle == StacyClouds.C4Sharp.InteractionStyle.Synchronous)
                {
                    tags.Add(StacyClouds.C4Sharp.Tags.Synchronous);
                }
                else if (InteractionStyle == StacyClouds.C4Sharp.InteractionStyle.Asynchronous)
                {
                    tags.Add(StacyClouds.C4Sharp.Tags.Asynchronous);
                }
                
                return tags.ToList();
            } else {
                return new List<string>();
            }
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Compares this relationship with another object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns><see langword="true"/> when <paramref name="obj"/> is a matching <see cref="Relationship"/>; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Relationship);
        }

        /// <summary>
        /// Compares this relationship with another relationship by source, destination, and description.
        /// </summary>
        /// <param name="relationship">The relationship to compare with.</param>
        /// <returns><see langword="true"/> when both relationships connect the same elements with the same description; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Relationship relationship)
        {
            if (relationship == null)
            {
                return false;
            }

            if (relationship == this)
            {
                return true;
            }

            if (!Description.Equals(relationship.Description)) return false;
            if (!Destination.Equals(relationship.Destination)) return false;
            if (!Source.Equals(relationship.Source)) return false;

            return true;
        }

        /// <summary>
        /// Returns a hash code derived from the source, destination, and description.
        /// </summary>
        /// <returns>A hash code for the current relationship.</returns>
        public override int GetHashCode()
        {
            int result = SourceId.GetHashCode();
            result = 31 * result + DestinationId.GetHashCode();
            result = 31*result + Description.GetHashCode();
            return result;
        }

        /// <summary>
        /// Returns a readable representation of the relationship.
        /// </summary>
        /// <returns>A string containing the source, description, and destination.</returns>
        public override string ToString()
        {
            return Source.ToString() + " ---[" + Description + "]---> " + Destination.ToString();
        }

    }
}