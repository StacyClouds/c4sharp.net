using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// An instance of a model relationship in a View.
    /// </summary>
    [DataContract]
    public sealed class RelationshipView : IEquatable<RelationshipView>
    {

        /// <summary>
        /// References the model relationship represented by this view edge.
        /// </summary>
        public Relationship Relationship { get; set; }

        private string id;

        /// <summary>
        /// The ID of the relationship.
        /// </summary>
        /// <value>The ID of the relationship.</value>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id
        {
            get
            {
                if (this.Relationship != null)
                {
                    return this.Relationship.Id;
                }
                else
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
        /// The order of this relationship (used in dynamic views only; e.g. 1.0, 1.1, 2.0, etc).
        /// </summary>
        [DataMember(Name = "order", EmitDefaultValue = false)]
        public string Order { get; set; }

        /// <summary>
        /// The description of this relationship (used in dynamic views only).
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        private List<Vertex> _vertices = new List<Vertex>();

        /// <summary>
        /// The set of vertices used to render the relationship.
        /// </summary>
        [DataMember(Name = "vertices", EmitDefaultValue = false)]
        public List<Vertex> Vertices
        {
            get
            {
                return new List<Vertex>(_vertices);
            }

            internal set
            {
                _vertices = new List<Vertex>(value);
            }
        }

        /// <summary>
        /// Replaces the connector vertices for this relationship view.
        /// </summary>
        /// <param name="vertices">The ordered connector vertices.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="vertices"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="vertices"/> contains a <see langword="null"/> entry.</exception>
        public void SetVertices(IEnumerable<Vertex> vertices)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException(nameof(vertices));
            }

            List<Vertex> replacement = new List<Vertex>(vertices);
            if (replacement.Any(vertex => vertex == null))
            {
                throw new ArgumentException("Connector vertices cannot contain null values.", nameof(vertices));
            }

            _vertices = replacement;
        }

        /// <summary>
        /// Adds a connector vertex to the end of this relationship view.
        /// </summary>
        /// <param name="vertex">The connector vertex to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="vertex"/> is <see langword="null"/>.</exception>
        public void AddVertex(Vertex vertex)
        {
            if (vertex == null)
            {
                throw new ArgumentNullException(nameof(vertex));
            }

            _vertices.Add(vertex);
        }

        /// <summary>
        /// Removes a connector vertex from this relationship view.
        /// </summary>
        /// <param name="vertex">The connector vertex to remove.</param>
        /// <returns>true if the vertex was removed; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="vertex"/> is <see langword="null"/>.</exception>
        public bool RemoveVertex(Vertex vertex)
        {
            if (vertex == null)
            {
                throw new ArgumentNullException(nameof(vertex));
            }

            return _vertices.Remove(vertex);
        }

        /// <summary>
        /// Removes all connector vertices from this relationship view.
        /// </summary>
        public void ClearVertices()
        {
            _vertices.Clear();
        }

        /// <summary>
        /// The routing of the line.
        /// </summary>
        [DataMember(Name = "routing", EmitDefaultValue = false)]
        public Routing? Routing { get; set; }

        private int? _position;

        /// <summary>
        /// The position of the annotation along the line; 0 (start) to 100 (end).
        /// </summary>
        [DataMember(Name = "position", EmitDefaultValue = false)]
        public int? Position
        {
            get { return _position; }
            set
            {
                if (value != null)
                {
                    if (value < 0)
                    {
                        _position = 0;
                    }
                    else if (value > 100)
                    {
                        _position = 100;
                    }
                    else
                    {
                        _position = value;
                    }
                }
            }
        }

        /// <summary>
        /// Indicates whether this relationship view represents a response message in a dynamic view.
        /// </summary>
        [DataMember(Name = "response", EmitDefaultValue = false)]
        public bool? Response;
        
        /// <summary>
        /// Initializes a relationship view during deserialization.
        /// </summary>
        internal RelationshipView()
        {
            Vertices = new List<Vertex>();
        }

        /// <summary>
        /// Creates a relationship view for the supplied model relationship.
        /// </summary>
        /// <param name="relationship">The relationship to wrap.</param>
        internal RelationshipView(Relationship relationship)
        {
            Vertices = new List<Vertex>();
            this.Relationship = relationship;
        }

        /// <summary>
        /// Returns the wrapped relationship text.
        /// </summary>
        /// <returns>A human-readable representation of the relationship view.</returns>
        public override string ToString()
        {
            return this.Relationship.ToString();
        }

        /// <summary>
        /// Determines whether another object represents the same relationship view.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns><see langword="true"/> when the object is an equivalent <see cref="RelationshipView"/>; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as RelationshipView);
        }

        /// <summary>
        /// Determines whether another relationship view represents the same relationship, order, and description.
        /// </summary>
        /// <param name="relationshipView">The relationship view to compare.</param>
        /// <returns><see langword="true"/> when both views are equivalent; otherwise, <see langword="false"/>.</returns>
        public bool Equals(RelationshipView relationshipView)
        {
            if (relationshipView == null)
            {
                return false;
            }
            if (relationshipView == this)
            {
                return true;
            }

            if (Description != null ? Description != relationshipView.Description : relationshipView.Description != null) return false;
            if (Id != relationshipView.Id) return false;
            return !(Order != null ? Order != relationshipView.Order : relationshipView.Order != null);

        }

        /// <summary>
        /// Returns a hash code based on the relationship identity and dynamic-view metadata.
        /// </summary>
        /// <returns>A hash code for this relationship view.</returns>
        public override int GetHashCode()
        {
            int result = Id.GetHashCode();
            result = 31 * result + (Description != null ? Description.GetHashCode() : 0);
            result = 31 * result + (Order != null ? Order.GetHashCode() : 0);
            return result;
        }

        /// <summary>
        /// Copies the connector layout metadata from another relationship view.
        /// </summary>
        /// <param name="source">The source relationship view that provides layout metadata.</param>
        internal void CopyLayoutInformationFrom(RelationshipView source)
        {
            if (source != null)
            {
                this.Vertices = source.Vertices;
                this.Routing = source.Routing;
                this.Position = source.Position;
            }
        }
    }
}
