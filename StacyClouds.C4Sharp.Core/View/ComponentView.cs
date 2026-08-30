using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// A component view that shows components inside a container and their direct neighbours.
    /// </summary>
    [DataContract]
    public sealed class ComponentView : StaticView
    {

        /// <summary>
        /// Returns the default name shown for this component view.
        /// </summary>
        public override string Name
        {
            get
            {
                return SoftwareSystem.Name + " - " + Container.Name + " - Components";
            }
        }

        /// <summary>
        /// References the container whose components are in scope for this view.
        /// </summary>
        public Container Container { get; set; }

        private string containerId;

        /// <summary>
        /// The ID of the container this view is associated with.
        /// </summary>
        [DataMember(Name="containerId", EmitDefaultValue=false)]
        public string ContainerId {
            get
            {
                if (Container != null)
                {
                    return Container.Id;
                } else
                {
                    return containerId;
                }
            }
            set
            {
                this.containerId = value;
            }
        }
        
        /// <summary>
        /// Determines whether container boundaries should be visible for "external" components (those outside the container in scope).
        /// </summary>
        [DataMember(Name = "externalContainerBoundariesVisible", EmitDefaultValue = false)]
        public bool? ExternalContainerBoundariesVisible { get; set; }

        /// <summary>
        /// Initializes a component view during deserialization.
        /// </summary>
        internal ComponentView() : base()
        {
        }

        /// <summary>
        /// Creates a component view for the supplied container.
        /// </summary>
        /// <param name="container">The container in scope.</param>
        /// <param name="key">The unique view key.</param>
        /// <param name="description">The view description.</param>
        internal ComponentView(Container container, string key, string description) : base(container.SoftwareSystem,key,  description)
        {
            this.Container = container;
        }

        protected override void CheckElementCanBeAdded(Element element)
        {
            if (element is Person)
            {
                return;
            }

            if (element is SoftwareSystem)
            {
                if (element.Equals(Container.Parent))
                {
                    throw new ElementNotPermittedInViewException("The software system in scope cannot be added to a component view.");
                }
                else
                {
                    return;
                }
            }

            if (element is Container)
            {
                if (element.Equals(Container))
                {
                    throw new ElementNotPermittedInViewException("The container in scope cannot be added to a component view.");
                }
                else
                {
                    return;
                }
            }

            if (element is Component)
            {
                return;
            }

            throw new ElementNotPermittedInViewException("Only people, software systems, containers, and components can be added to a component view.");
        }

        /// <summary>
        /// Adds all people, software systems, containers, and components permitted by this view.
        /// </summary>
        public override void AddAllElements()
        {
            AddAllSoftwareSystems();
            AddAllPeople();
            AddAllContainers();
            AddAllComponents();
        }

        /// <summary>
        /// Adds every container in the scoped software system except the container already in scope.
        /// </summary>
        public void AddAllContainers()
        {
            foreach (Container container in SoftwareSystem.Containers)
            {
                try
                {
                    Add(container);
                }
                catch (ElementNotPermittedInViewException e)
                {
                    // ignore
                }
            }
        }

        /// <summary>
        /// Adds a container to the component view.
        /// </summary>
        /// <param name="container">The container to add.</param>
        public void Add(Container container)
        {
            AddElement(container, true);
        }

        /// <summary>
        /// Removes a container from the component view.
        /// </summary>
        /// <param name="container">The container to remove.</param>
        public void Remove(Container container)
        {
            RemoveElement(container);
        }

        /// <summary>
        /// Adds every component defined inside the scoped container.
        /// </summary>
        public void AddAllComponents()
        {
            foreach (Component component in Container.Components)
            {
                Add(component);
            }
        }

        /// <summary>
        /// Adds a component to the view.
        /// </summary>
        /// <param name="component">The component to add.</param>
        public void Add(Component component)
        {
            if (component != null)
            {
                AddElement(component, true);
            }
        }

        /// <summary>
        /// Removes a component from the view.
        /// </summary>
        /// <param name="component">The component to remove.</param>
        public void Remove(Component component)
        {
            RemoveElement(component);
        }

        /// <summary>
        /// Adds people, software systems, containers and components that are directly related to the given element.
        /// </summary>
        public override void AddNearestNeighbours(Element element)
        {
            AddNearestNeighbours(element, typeof(Person));
            AddNearestNeighbours(element, typeof(SoftwareSystem));
            AddNearestNeighbours(element, typeof(Container));
            AddNearestNeighbours(element, typeof(Component));
        }
        
        /// <summary>
        /// Adds the default set of elements to this view.
        /// </summary>
        public override void AddDefaultElements()
        {
            foreach (Component component in Container.Components)
            {
                Add(component);

                foreach (Container container in SoftwareSystem.Containers)
                {
                    if (container.HasEfferentRelationshipWith(component) || component.HasEfferentRelationshipWith(container))
                    {
                        Add(container);
                    }
                };

                AddNearestNeighbours(component, typeof(Person));
                AddNearestNeighbours(component, typeof(SoftwareSystem));
            }
        }
        
    }
}
