using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// A container view.
    /// </summary>
    [DataContract]
    public sealed class ContainerView : StaticView
    {

        /// <summary>
        /// Returns the default name shown for this container view.
        /// </summary>
        public override string Name
        {
            get
            {
                return SoftwareSystem.Name + " - Containers";
            }
        }

        /// <summary>
        /// Determines whether software system boundaries should be visible for "external" containers (those outside the software system in scope).
        /// </summary>
        [DataMember(Name = "externalSoftwareSystemBoundariesVisible", EmitDefaultValue = false)]
        public bool? ExternalSoftwareSystemBoundariesVisible { get; set; }

        /// <summary>
        /// Initializes a container view during deserialization.
        /// </summary>
        internal ContainerView() : base()
        {
        }

        /// <summary>
        /// Creates a container view for the supplied software system.
        /// </summary>
        /// <param name="softwareSystem">The software system in scope.</param>
        /// <param name="key">The unique view key.</param>
        /// <param name="description">The view description.</param>
        internal ContainerView(SoftwareSystem softwareSystem, string key, string description) : base(softwareSystem, key, description)
        {
        }

        protected override void CheckElementCanBeAdded(Element element)
        {
            if (element is Person)
            {
                return;
            }

            if (element is SoftwareSystem)
            {
                if (element.Equals(SoftwareSystem))
                {
                    throw new ElementNotPermittedInViewException("The software system in scope cannot be added to a container view.");
                }
                else
                {
                    return;
                }
            }

            if (element is Container)
            {
                return;
            }

            throw new ElementNotPermittedInViewException("Only people, software systems, and containers can be added to a container view.");
        }

        /// <summary>
        /// Adds all software systems, people and containers to this view.
        /// </summary>
        public override void AddAllElements()
        {
            AddAllSoftwareSystems();
            AddAllPeople();
            AddAllContainers();
        }

        /// <summary>
        /// Adds every container that belongs to the scoped software system.
        /// </summary>
        public void AddAllContainers()
        {
            foreach (Container container in SoftwareSystem.Containers)
            {
                Add(container);
            }
        }

        /// <summary>
        /// Adds a container to the view.
        /// </summary>
        /// <param name="container">The container to add.</param>
        public void Add(Container container)
        {
            AddElement(container, true);
        }

        /// <summary>
        /// Removes a container from the view.
        /// </summary>
        /// <param name="container">The container to remove.</param>
        public void Remove(Container container)
        {
            RemoveElement(container);
        }

        /// <summary>
        /// Adds people, software systems and containers that are directly related to the given element.
        /// </summary>
        public override void AddNearestNeighbours(Element element)
        {
            AddNearestNeighbours(element, typeof(Person));
            AddNearestNeighbours(element, typeof(SoftwareSystem));
            AddNearestNeighbours(element, typeof(Container));
        }
        
        /// <summary>
        /// Adds the default set of elements to this view. 
        /// </summary>
        public override void AddDefaultElements()
        {
            foreach (Container container in SoftwareSystem.Containers)
            {
                Add(container);
                AddNearestNeighbours(container, typeof(Person));
                AddNearestNeighbours(container, typeof(SoftwareSystem));
            }
        }
        
    }
}
