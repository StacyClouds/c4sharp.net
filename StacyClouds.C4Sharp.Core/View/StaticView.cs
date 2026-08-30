using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// Serves as the base type for views that render a static structural slice of the model.
    /// </summary>
    [DataContract]
    public abstract class StaticView : View
    {

        private IList<Animation> _animations = new List<Animation>();

        /// <summary>
        /// Contains the ordered animation steps configured for this view.
        /// </summary>
        [DataMember(Name = "animations", EmitDefaultValue = false)]
        public IList<Animation> Animations
        {
            get { return new List<Animation>(_animations); }

            internal set { _animations = new List<Animation>(value); }
        }

        /// <summary>
        /// Initializes a static view during deserialization.
        /// </summary>
        internal StaticView() : base()
        {
        }

        /// <summary>
        /// Creates a static view for the supplied software system scope.
        /// </summary>
        /// <param name="softwareSystem">The scoped software system, or <see langword="null"/> for unscoped views.</param>
        /// <param name="key">The unique view key.</param>
        /// <param name="description">The view description.</param>
        internal StaticView(SoftwareSystem softwareSystem, string key, string description) : base(softwareSystem, key, description)
        {
        }

        /// <summary>
        /// Adds every element type permitted by the concrete static view.
        /// </summary>
        public abstract void AddAllElements();

        /// <summary>
        /// Adds all software systems in the model to this view.
        /// </summary>
        public void AddAllSoftwareSystems()
        {
            foreach (SoftwareSystem softwareSystem in this.Model.SoftwareSystems)
            {
                try
                {
                    Add(softwareSystem);
                }
                catch (ElementNotPermittedInViewException e)
                {
                    // ignore
                }
            }
        }

        /// <summary>
        /// Adds the given SoftwareSystem to this view.
        /// </summary>
        /// <param name="softwareSystem">The software system to add.</param>
        public virtual void Add(SoftwareSystem softwareSystem)
        {
            AddElement(softwareSystem, true);
        }

        /// <summary>
        /// Removes the given SoftwareSystem from this view.
        /// </summary>
        /// <param name="softwareSystem">The software system to remove.</param>
        public void Remove(SoftwareSystem softwareSystem)
        {
            RemoveElement(softwareSystem);
        }

        /// <summary>
        /// Adds all people in the model to this view.
        /// </summary>
        public void AddAllPeople()
        {
            foreach (Person person in this.Model.People)
            {
                Add(person);
            }
        }

        /// <summary>
        /// Adds the given Person to this view.
        /// </summary>
        /// <param name="person">The person to add.</param>
        public void Add(Person person)
        {
            AddElement(person, true);
        }

        /// <summary>
        /// Removes the given Person from this view.
        /// </summary>
        /// <param name="person">The person to remove.</param>
        public void Remove(Person person)
        {
            RemoveElement(person);
        }

        /// <summary>
        /// Adds the default set of elements to this view. 
        /// </summary>
        public abstract void AddDefaultElements();

        /// <summary>
        /// Adds the permitted elements that are directly connected to the supplied element.
        /// </summary>
        /// <param name="element">The element whose neighbourhood should be included.</param>
        public abstract void AddNearestNeighbours(Element element);

        protected void AddNearestNeighbours(Element element, Type typeOfElement)
        {
            if (element == null)
            {
                return;
            }

            try
            {
                AddElement(element, true);

                ICollection<Relationship> relationships = Model.Relationships;
                foreach (Relationship relationship in relationships)
                {
                    if (relationship.Source.Equals(element) && relationship.Destination.GetType() == typeOfElement)
                    {
                        try
                        {
                            AddElement(relationship.Destination, true);
                        }
                        catch (ElementNotPermittedInViewException e)
                        {
                            Console.WriteLine(e.Message + " (ignoring " + relationship.Destination.Name + ")");
                        }
                    }

                    if (relationship.Destination.Equals(element) && relationship.Source.GetType() == typeOfElement)
                    {
                        try
                        {
                            AddElement(relationship.Source, true);
                        }
                        catch (ElementNotPermittedInViewException e)
                        {
                            Console.WriteLine(e.Message + " (ignoring " + relationship.Source.Name + ")");
                        }
                    }
                }
            }
            catch (ElementNotPermittedInViewException e)
            {
                Console.WriteLine(e.Message + " (ignoring " + element.Name + ")");
            }
        }
        
        /// <summary>
        /// Adds an animation step that reveals the supplied elements and the relationships to previously revealed elements.
        /// </summary>
        /// <param name="elements">The elements to reveal in the new animation step.</param>
        /// <exception cref="ArgumentException">Thrown when no elements are supplied or none exist in the view.</exception>
        public void AddAnimation(params Element[] elements)
        {
            if (elements == null || elements.Length == 0)
            {
                throw new ArgumentException("One or more elements must be specified.");
            }

            ISet<string> elementIdsInPreviousAnimationSteps = new HashSet<string>();
            ISet<Element> elementsInThisAnimationStep = new HashSet<Element>();
            ISet<Relationship> relationshipsInThisAnimationStep = new HashSet<Relationship>();

            foreach (Element element in elements)
            {
                if (IsElementInView(element))
                {
                    elementIdsInPreviousAnimationSteps.Add(element.Id);
                    elementsInThisAnimationStep.Add(element);
                }
            }

            if (elementsInThisAnimationStep.Count == 0)
            {
                throw new ArgumentException("None of the specified elements exist in this view.");
            }

            foreach (Animation animation in Animations) {
                foreach (string elementId in animation.Elements)
                {
                    elementIdsInPreviousAnimationSteps.Add(elementId);
                }
            }

            foreach (RelationshipView relationshipView in Relationships)
            {
                if (
                    (elementsInThisAnimationStep.Contains(relationshipView.Relationship.Source) && elementIdsInPreviousAnimationSteps.Contains(relationshipView.Relationship.Destination.Id)) ||
                    (elementIdsInPreviousAnimationSteps.Contains(relationshipView.Relationship.Source.Id)) && elementsInThisAnimationStep.Contains(relationshipView.Relationship.Destination)
                   )
                {
                    relationshipsInThisAnimationStep.Add(relationshipView.Relationship);
                }
            }

            _animations.Add(new Animation(Animations.Count + 1, elementsInThisAnimationStep, relationshipsInThisAnimationStep));
        }

        /// <summary>
        /// Adds a specific relationship to this view.
        /// </summary>
        /// <param name="relationship">the Relationship to be added</param>
        /// <returns>a RelationshipView object representing the relationship added</returns>
        public RelationshipView Add(Relationship relationship)
        {
            return AddRelationship(relationship);
        }

    }
}
