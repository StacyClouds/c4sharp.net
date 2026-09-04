using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// Defines the shared metadata, elements, and relationships for a rendered architecture view.
    /// </summary>
    [DataContract]
    public abstract class View
    {

        /// <summary>
        /// Selects the strategy used when copying layout information from another view instance.
        /// </summary>
        public LayoutMergeStrategy LayoutMergeStrategy = new DefaultLayoutMergeStrategy();
        
        /// <summary>
        /// An identifier for this view.
        /// </summary>
        [DataMember(Name = "key", EmitDefaultValue = false)]
        public string Key { get; set; }

        /// <summary>
        /// References the software system that defines the scope of this view when applicable.
        /// </summary>
        public SoftwareSystem SoftwareSystem { get; set; }

        private string softwareSystemId;

        /// <summary>
        /// The ID of the software system this view is associated with.
        /// </summary>
        [DataMember(Name = "softwareSystemId", EmitDefaultValue = false)]
        public string SoftwareSystemId {
            get
            {
                if (this.SoftwareSystem != null)
                {
                    return this.SoftwareSystem.Id;
                } else
                {
                    return this.softwareSystemId;
                }
            }
            set
            {
                this.softwareSystemId = value;
            }
        }

        /// <summary>
        /// Describes the purpose of the view.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Provides the generated name displayed for this view type.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The title for this view.
        /// </summary>
        [DataMember(Name = "title", EmitDefaultValue = false)]
        public string Title { get; set; }

        /// <summary>
        /// Resolves the model that owns the elements displayed by this view.
        /// </summary>
        /// <remarks>The default setter intentionally ignores assigned values because most view types derive the model from their scope.</remarks>
        public virtual Model Model
        {
            get
            {
                return this.SoftwareSystem.Model;
            }

            set
            {
                // do nothing
            }
        }

        /// <summary>
        /// The paper size that should be used to render this view.
        /// </summary>
        [DataMember(Name = "paperSize", EmitDefaultValue = false)]
        public PaperSize PaperSize { get; set; }

        /// <summary>
        /// The dimensions of the view (in pixels).
        /// </summary>
        [DataMember(Name = "dimensions", EmitDefaultValue = false)]
        public Dimensions Dimensions { get; set; }
        
        private HashSet<ElementView> _elements;

        /// <summary>
        /// The set of elements in this view.
        /// </summary>
        [DataMember(Name = "elements", EmitDefaultValue = false)]
        public ISet<ElementView> Elements
        {
            get
            {
                return new HashSet<ElementView>(_elements);
            }

            internal set
            {
                _elements = new HashSet<ElementView>(value);
            }
        }

        private HashSet<RelationshipView> _relationships;

        /// <summary>
        /// The set of relationships in this view.
        /// </summary>
        [DataMember(Name = "relationships", EmitDefaultValue = false)]
        public virtual ISet<RelationshipView> Relationships
        {
            get
            {
                return new HashSet<RelationshipView>(_relationships);
            }

            internal set
            {
                _relationships = new HashSet<RelationshipView>(value);
            }
        }

        /// <summary>
        /// Initializes the backing collections used by a view during construction or deserialization.
        /// </summary>
        internal View()
        {
            _elements = new HashSet<ElementView>();
            _relationships = new HashSet<RelationshipView>();
        }

        /// <summary>
        /// Creates a view scoped to the supplied software system.
        /// </summary>
        /// <param name="softwareSystem">The software system in scope, or <see langword="null"/> for unscoped views.</param>
        /// <param name="key">The unique identifier for the view.</param>
        /// <param name="description">The description associated with the view.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is blank.</exception>
        internal View(SoftwareSystem softwareSystem, string key, string description) : this()
        {
            this.SoftwareSystem = softwareSystem;
            if (key != null && key.Trim().Length > 0)
            {
                this.Key = key;
            }
            else
            {
                throw new ArgumentException("A key must be specified.");
            }
            this.Description = description;

            _elements = new HashSet<ElementView>();
            _relationships = new HashSet<RelationshipView>();
        }

        protected abstract void CheckElementCanBeAdded(Element element);

        protected void AddElement(Element element, bool addRelationships)
        {
            if (element == null)
            {
                throw new ArgumentException("An element must be specified.");
            }

            if (Model.Contains(element))
            {
                CheckElementCanBeAdded(element);
                _elements.Add(new ElementView(element));

                if (addRelationships)
                {
                    AddRelationships(element);
                }
            }
            else
            {
                throw new ArgumentException("The element named " + element.Name + " does not exist in the model associated with this view.");
            }
        }

        protected void RemoveElement(Element element)
        {
            if (element != null)
            {
                ElementView elementView = new ElementView(element);
                _elements.Remove(elementView);

                _relationships.RemoveWhere(r =>
                            r.Relationship.Source.Equals(element) ||
                            r.Relationship.Destination.Equals(element));
            }
        }

        protected RelationshipView AddRelationship(Relationship relationship)
        {
            if (relationship == null)
            {
                throw new ArgumentException("A relationship must be specified.");
            }

            if (IsElementInView(relationship.Source) && IsElementInView(relationship.Destination))
            {
                RelationshipView relationshipView = new RelationshipView(relationship);
                _relationships.Add(relationshipView);

                return relationshipView;
            }

            return null;
        }

        /// <summary>
        /// Determines whether the supplied element already has an element view in this view.
        /// </summary>
        /// <param name="element">The element to look for.</param>
        /// <returns><see langword="true"/> when the element is present; otherwise, <see langword="false"/>.</returns>
        internal bool IsElementInView(Element element)
        {
            return _elements.Count(ev => ev.Element.Equals(element)) > 0;
        }

        /// <summary>
        /// Gets the element view for the given element.
        /// </summary>
        /// <param name="element">the Element to find the ElementView for</param>
        /// <returns>The matching <see cref="ElementView"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the element is not present in the view.</exception>
        public ElementView GetElementView(Element element)
        {
            return Elements.First(ev => ev.Id.Equals(element.Id));
        }

        /// <summary>
        /// Gets the relationship view for the given relationship.
        /// </summary>
        /// <param name="relationship">the Relationship to find the RelationshipView for</param>
        /// <returns>The matching <see cref="RelationshipView"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the relationship is not present in the view.</exception>
        public RelationshipView GetRelationshipView(Relationship relationship)
        {
            return Relationships.First(ev => ev.Id.Equals(relationship.Id));
        }


        private void AddRelationships(Element element)
        {
            List<Element> elements = new List<Element>();
            foreach (ElementView elementView in this.Elements)
            {
                elements.Add(elementView.Element);
            }

            // add relationships where the destination exists in the view already
            foreach (Relationship relationship in element.Relationships)
            {
                if (elements.Contains(relationship.Destination))
                {
                    this._relationships.Add(new RelationshipView(relationship));
                }
            }

            // add relationships where the source exists in the view already
            foreach (Element e in elements)
            {
                foreach (Relationship relationship in e.Relationships)
                {
                    if (relationship.Destination.Equals(element))
                    {
                        _relationships.Add(new RelationshipView(relationship));
                    }
                }
            }
        }

        /// <summary>
        /// Removes the supplied relationship from the view when it is present.
        /// </summary>
        /// <param name="relationship">The relationship to remove.</param>
        public void Remove(Relationship relationship)
        {
            if (relationship != null)
            {
                RelationshipView relationshipView = new RelationshipView(relationship);
                _relationships.Remove(relationshipView);
            }
        }

        /// <summary>
        /// Attempts to copy the visual layout information (e.g. x,y coordinates) of elements and relationships
        /// from the specified source view into this view.
        /// </summary>
        /// <param name="source">the source view</param>
        public void CopyLayoutInformationFrom(View source)
        {
            LayoutMergeStrategy.CopyLayoutInformation(source, this);
        }

        /// <summary>
        /// Stores the automatic layout settings currently applied to the view.
        /// </summary>
        [DataMember(Name = "automaticLayout", EmitDefaultValue = false)]
        public AutomaticLayout AutomaticLayout { get; internal set; }

        /// <summary>
        /// Enables automatic layout for this view, with some default settings.
        /// </summary>
        public void EnableAutomaticLayout()
        {
            EnableAutomaticLayout(RankDirection.TopBottom, 300, 600, 200, false);
        }

        /// <summary>
        /// Enables the automatic layout for this view, with the specified settings.
        /// </summary>
        /// <param name="rankDirection">the rank direction</param>
        /// <param name="rankSeparation">the separation between ranks (in pixels, a positive integer)</param>
        /// <param name="nodeSeparation">the separation between nodes within the same rank (in pixels, a positive integer)</param>
        /// <param name="edgeSeparation">the separation between edges (in pixels, a positive integer)</param>
        /// <param name="vertices">whether vertices should be created during automatic layout</param>
        public void EnableAutomaticLayout(RankDirection rankDirection, int rankSeparation, int nodeSeparation, int edgeSeparation, bool vertices)
        {
            AutomaticLayout = new AutomaticLayout(rankDirection, rankSeparation, nodeSeparation, edgeSeparation, vertices);
        }

        /// <summary>
        /// Disables automatic layout for this view.
        /// </summary>
        public void DisableAutomaticLayout()
        {
            AutomaticLayout = null;
        }

    }
}