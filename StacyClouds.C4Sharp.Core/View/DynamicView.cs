using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// A dynamic view, used to describe behaviour between static elements at runtime.
    /// </summary>
    [DataContract]
    public sealed class DynamicView : View
    {

        /// <summary>
        /// Stores the model whose static elements participate in this dynamic view.
        /// </summary>
        public override Model Model { get; set; }

        /// <summary>
        /// Returns the relationships ordered by their dynamic sequence label.
        /// </summary>
        public override ISet<RelationshipView> Relationships
        {
            get
            {
                List<RelationshipView> list = new List<RelationshipView>(base.Relationships);
                bool ordersAreNumeric = true;

                foreach (RelationshipView relationshipView in list)
                {
                    ordersAreNumeric = ordersAreNumeric && isNumeric(relationshipView.Order);
                }

                if (ordersAreNumeric)
                {
                    list.Sort(CompareAsNumber);
                }
                else
                {
                    list.Sort(CompareAsString);
                }

                return new HashSet<RelationshipView>(list);
            }
        }

        private bool isNumeric(string str)
        {
            try
            {
                double.Parse(str);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private int CompareAsNumber(RelationshipView x, RelationshipView y)
        {
            return double.Parse(x.Order).CompareTo(double.Parse(y.Order));
        }

        private int CompareAsString(RelationshipView x, RelationshipView y)
        {
            return x.Order.CompareTo(y.Order); 
        }

        /// <summary>
        /// Returns the default name shown for this dynamic view.
        /// </summary>
        public override string Name
        {
            get
            {
                if (Element != null)
                {
                    return Element.Name + " - Dynamic";
                }
                else
                {
                    return "Dynamic";
                }
            }
        }

        /// <summary>
        /// References the element that defines the scope of the view, when the view is scoped.
        /// </summary>
        public Element Element { get; set; }

        private string _elementId;

        /// <summary>
        /// Stores the ID of the scoped element for serialization.
        /// </summary>
        [DataMember(Name="elementId", EmitDefaultValue=false)]
        public string ElementId {
            get {
                return Element != null ? Element.Id : _elementId;
            }
            set
            {
                _elementId = value;
            }
        }

        private readonly SequenceNumber _sequenceNumber = new SequenceNumber();

        /// <summary>
        /// Initializes a dynamic view during deserialization.
        /// </summary>
        internal DynamicView()
        {
        }

        /// <summary>
        /// Creates an unscoped dynamic view over the supplied model.
        /// </summary>
        /// <param name="model">The model to visualize.</param>
        /// <param name="key">The unique view key.</param>
        /// <param name="description">The view description.</param>
        internal DynamicView(Model model, string key, string description) : base(null, key, description)
        {
            Model = model;
            Element = null;
        }

        /// <summary>
        /// Creates a software-system-scoped dynamic view.
        /// </summary>
        /// <param name="softwareSystem">The software system in scope.</param>
        /// <param name="key">The unique view key.</param>
        /// <param name="description">The view description.</param>
        internal DynamicView(SoftwareSystem softwareSystem, string key, string description) : base(softwareSystem, key, description)
        {
            Model = softwareSystem.Model;
            Element = softwareSystem;
        }

        /// <summary>
        /// Creates a container-scoped dynamic view.
        /// </summary>
        /// <param name="container">The container in scope.</param>
        /// <param name="key">The unique view key.</param>
        /// <param name="description">The view description.</param>
        internal DynamicView(Container container, string key, string description) : base(container.SoftwareSystem, key, description)
        {
            Model = container.Model;
            Element = container;
        }

        protected override void CheckElementCanBeAdded(Element elementToBeAdded)
        {
            if (!(elementToBeAdded is StaticStructureElement))
            {
                throw new ElementNotPermittedInViewException(
                    "Only people, software systems, containers and components can be added to dynamic views.");
            }

            StaticStructureElement staticStructureElementToBeAdded = (StaticStructureElement) elementToBeAdded;

            // people can always be added
            if (staticStructureElementToBeAdded is Person)
            {
                return;
            }

            // if the scope of this dynamic view is a software system, we only want:
            //  - containers
            //  - other software systems
            if (Element is SoftwareSystem)
            {
                if (staticStructureElementToBeAdded.Equals(Element))
                {
                    throw new ElementNotPermittedInViewException(
                        staticStructureElementToBeAdded.Name +
                        " is already the scope of this view and cannot be added to it.");
                }

                if (staticStructureElementToBeAdded is SoftwareSystem ||
                    staticStructureElementToBeAdded is Container) {
                    checkParentAndChildrenHaveNotAlreadyBeenAdded(staticStructureElementToBeAdded);
                } else if (staticStructureElementToBeAdded is Component) {
                    throw new ElementNotPermittedInViewException(
                        "Components can't be added to a dynamic view when the scope is a software system.");
                }
            }

            // dynamic view with container scope:
            //  - other containers
            //  - components
            if (Element is Container) {
                if (staticStructureElementToBeAdded.Equals(Element) ||
                    staticStructureElementToBeAdded.Equals(Element.Parent))
                {
                    throw new ElementNotPermittedInViewException(
                        staticStructureElementToBeAdded.Name +
                        " is already the scope of this view and cannot be added to it.");
                }

                checkParentAndChildrenHaveNotAlreadyBeenAdded(staticStructureElementToBeAdded);
            }

            // dynamic view with no scope
            //  - software systems
            if (Element == null)
            {
                if (!(staticStructureElementToBeAdded is SoftwareSystem)) {
                    throw new ElementNotPermittedInViewException(
                        "Only people and software systems can be added to this dynamic view.");
                }
            }
        }

        private void checkParentAndChildrenHaveNotAlreadyBeenAdded(StaticStructureElement elementToBeAdded) {
            // check the parent hasn't been added already
            ISet<String> elementIds = new HashSet<string>(Elements.Select(ev => ev.Element.Id));

            if (elementToBeAdded.Parent != null) {
                if (elementIds.Contains(elementToBeAdded.Parent.Id)) {
                    throw new ElementNotPermittedInViewException("The parent of " + elementToBeAdded.Name + " is already in this view.");
                }
            }

            // and now check a child hasn't been added already
            ISet<String> elementParentIds = new HashSet<string>(Elements.Where(ev => ev.Element.Parent != null).Select(ev => ev.Element.Parent.Id));

            if (elementParentIds.Contains(elementToBeAdded.Id)) {
                throw new ElementNotPermittedInViewException("The child of " + elementToBeAdded.Name + " is already in this view.");
            }
        }

        /// <summary>
        /// Adds the first matching relationship between the supplied source and destination.
        /// </summary>
        /// <param name="source">The source element.</param>
        /// <param name="destination">The destination element.</param>
        /// <returns>The created relationship view.</returns>
        public RelationshipView Add(StaticStructureElement source, StaticStructureElement destination)
        {
            return Add(source, "", destination);
        }

        /// <summary>
        /// Adds a relationship between the supplied elements with an overridden description.
        /// </summary>
        /// <param name="source">The source element.</param>
        /// <param name="description">The description shown for the interaction.</param>
        /// <param name="destination">The destination element.</param>
        /// <returns>The created relationship view.</returns>
        public RelationshipView Add(StaticStructureElement source, string description, StaticStructureElement destination)
        {
            return Add(source, description, "", destination);
        }

        /// <summary>
        /// Adds a relationship between the supplied elements, selecting a matching model relationship by description and optional technology.
        /// </summary>
        /// <param name="source">The source element.</param>
        /// <param name="description">The description shown for the interaction.</param>
        /// <param name="technology">The technology used to disambiguate matching relationships.</param>
        /// <param name="destination">The destination element.</param>
        /// <returns>The created relationship view.</returns>
        /// <exception cref="ArgumentException">Thrown when a source or destination is missing, or when no matching relationship exists.</exception>
        public RelationshipView Add(StaticStructureElement source, string description, string technology, StaticStructureElement destination)
        {
            if (source == null) {
                throw new ArgumentException("A source element must be specified.");
            }

            if (destination == null) {
                throw new ArgumentException("A destination element must be specified.");
            }

            CheckElementCanBeAdded(source);
            CheckElementCanBeAdded(destination);

            // check that the relationship is in the model before adding it
            // check that the relationship is in the model before adding it
            Relationship relationship = null;

            if (String.IsNullOrEmpty(technology))
            {
                // no technology is specified, so just pick the first relationship we find
                relationship = source.GetEfferentRelationshipWith(destination, description);
                if (relationship == null)
                {
                    relationship = source.GetEfferentRelationshipWith(destination);
                }
            }
            else
            {
                ISet<Relationship> relationships = source.GetEfferentRelationshipsWith(destination);
                foreach (Relationship rel in relationships)
                {
                    if (technology.Equals(rel.Technology))
                    {
                        relationship = rel;
                    }
                }
            }
            
            if (relationship != null)
            {
                AddElement(source, false);
                AddElement(destination, false);

                return AddRelationship(relationship, description, _sequenceNumber.GetNext(), false);
            }
            else
            {
                // perhaps model this as a return/reply/response message instead, if the reverse relationship exists
                relationship = destination.GetEfferentRelationshipWith(source);

                if (relationship != null)
                {
                    AddElement(source, false);
                    AddElement(destination, false);

                    return AddRelationship(relationship, description, _sequenceNumber.GetNext(), true);
                }
                else
                { 
                    throw new ArgumentException("A relationship between " + source.Name + " and " + destination.Name + " does not exist in model.");
                }
            }
        }

        /// <summary>
        /// Adds a specific relationship to this dynamic view, with the original description.
        /// </summary>
        /// <param name="relationship">the Relationship to add</param>
        /// <returns>a RelationshipView</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="relationship"/> is <see langword="null"/>.</exception>
        public RelationshipView Add(Relationship relationship)
         {
            return Add(relationship, "");
        }

        /// <summary>
        /// Adds a specific relationship to this dynamic view, with an overidden description.
        /// </summary>
        /// <param name="relationship">the Relationship to add</param>
        /// <param name="description">the overidden description</param>
        /// <returns>a RelationshipView</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="relationship"/> is <see langword="null"/>.</exception>
        public RelationshipView Add(Relationship relationship, string description)
        {
            if (relationship == null)
            {
                throw new ArgumentException("A relationship must be specified.");
            }

            CheckElementCanBeAdded(relationship.Source);
            CheckElementCanBeAdded(relationship.Destination);

            AddElement(relationship.Source, false);
            AddElement(relationship.Destination, false);

            return AddRelationship(relationship, description, _sequenceNumber.GetNext(), false);
        }
        
        /// <summary>
        /// Adds a relationship and augments it with dynamic-view sequence metadata.
        /// </summary>
        /// <param name="relationship">The model relationship to add.</param>
        /// <param name="description">The description shown for the interaction.</param>
        /// <param name="order">The sequence number assigned to the interaction.</param>
        /// <param name="response">Whether the interaction is a response message.</param>
        /// <returns>The created relationship view, or <see langword="null"/> when the endpoints are not present in the view.</returns>
        internal RelationshipView AddRelationship(Relationship relationship, string description, string order, bool response)
        {
            RelationshipView relationshipView = AddRelationship(relationship);
            if (relationshipView != null)
            {
                relationshipView.Description = description;
                relationshipView.Order = order;
                relationshipView.Response = response;
            }

            return relationshipView;
        }
        
        /// <summary>
        /// Starts a nested parallel numbering branch for subsequent interactions.
        /// </summary>
        public void StartParallelSequence()
        {
            _sequenceNumber.StartParallelSequence();
        }

        /// <summary>
        /// Ends the current parallel numbering branch without carrying the number back to the parent sequence.
        /// </summary>
        public void EndParallelSequence()
        {
            EndParallelSequence(false);
        }

        /// <summary>
        /// Ends the current parallel numbering branch.
        /// </summary>
        /// <param name="endAllParallelSequencesAndContinueNumbering">When <see langword="true"/>, continues numbering from the completed parallel branch.</param>
        public void EndParallelSequence(bool endAllParallelSequencesAndContinueNumbering)
        {
            _sequenceNumber.EndParallelSequence(endAllParallelSequencesAndContinueNumbering);
        }

    }
}