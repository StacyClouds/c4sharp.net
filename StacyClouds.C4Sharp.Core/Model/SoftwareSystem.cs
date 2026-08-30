using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// A software system.
    /// </summary>
    [DataContract]
    public sealed class SoftwareSystem : StaticStructureElement, IEquatable<SoftwareSystem>
    {

        /// <summary>
        /// The location of this software system.
        /// </summary>
        [DataMember(Name="location", EmitDefaultValue=true)]
        public Location Location { get; set; }

        private HashSet<Container> _containers;

        /// <summary>
        /// The set of containers within this software system.
        /// </summary>
        [DataMember(Name="containers", EmitDefaultValue=false)]
        public ISet<Container> Containers
        {
            get
            {
                return new HashSet<Container>(_containers);
            }

            internal set
            {
                _containers = new HashSet<Container>(value);
            }
        }
  
        /// <summary>
        /// Gets the canonical name for this software system.
        /// </summary>
        public override string CanonicalName
        {
            get
            {
                return new CanonicalNameGenerator().Generate(this);
            }
        }

        /// <summary>
        /// Software systems do not have a parent element in the static structure hierarchy.
        /// </summary>
        public override Element Parent
        {
            get
            {
                return null;
            }

            set
            {
            }
        }

        /// <summary>
        /// Initializes a software system for deserialization.
        /// </summary>
        internal SoftwareSystem()
        {
            _containers = new HashSet<Container>();
        }

        /// <summary>
        /// Adds a container with the specified name (unless one exists with the same name already).
        /// </summary>
        /// <param name="name">the name of the container (e.g. "Web Application")</param>
        /// <returns>The created container, or <see langword="null"/> if a container with the same name already exists.</returns>
        public Container AddContainer(string name)
        {
            return AddContainer(name, "");
        }

        /// <summary>
        /// Adds a container with the specified name and description (unless one exists with the same name already).
        /// </summary>
        /// <param name="name">the name of the container (e.g. "Web Application")</param>
        /// <param name="description">a short description/list of responsibilities</param>
        /// <returns>The created container, or <see langword="null"/> if a container with the same name already exists.</returns>
        public Container AddContainer(string name, string description)
        {
            return AddContainer(name, description, "");
        }

        /// <summary>
        /// Adds a container with the specified name, description and technology (unless one exists with the same name already).
        /// </summary>
        /// <param name="name">the name of the container (e.g. "Web Application")</param>
        /// <param name="description">a short description/list of responsibilities</param>
        /// <param name="technology">the technology choice (e.g. "Spring MVC", "Java EE", etc)</param>
        /// <returns>The created container, or <see langword="null"/> if a container with the same name already exists.</returns>
        public Container AddContainer(string name, string description, string technology)
        {
            return Model.AddContainer(this, name, description, technology);
        }

        /// <summary>
        /// Adds an existing container instance to this software system.
        /// </summary>
        /// <param name="container">The container to add.</param>
        internal void Add(Container container)
        {
            _containers.Add(container);
        }

        /// <summary>
        /// Gets the container with the specified name (or null if it doesn't exist).
        /// </summary>
        /// <param name="name">The name of the container to find.</param>
        /// <returns>The matching container, or <see langword="null"/> when no container has that name.</returns>
        public Container GetContainerWithName(string name)
        {
            foreach (Container container in _containers)
            {
                if (container.Name == name)
                {
                    return container;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the container with the specified ID (or null if it doesn't exist).
        /// </summary>
        /// <param name="id">The identifier of the container to find.</param>
        /// <returns>The matching container, or <see langword="null"/> when no container has that identifier.</returns>
        public Container GetContainerWithId(string id)
        {
            foreach (Container container in _containers)
            {
                if (container.Id == id)
                {
                    return container;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the tags that are always applied to software systems.
        /// </summary>
        /// <returns>The required software system tags.</returns>
        public override List<string> GetRequiredTags()
        {
            return new List<string>
            {
                StacyClouds.C4Sharp.Tags.Element,
                StacyClouds.C4Sharp.Tags.SoftwareSystem
            };
        }

        /// <summary>
        /// Compares this software system with another software system by canonical identity.
        /// </summary>
        /// <param name="softwareSystem">The software system to compare with.</param>
        /// <returns><see langword="true"/> when both software systems represent the same model element; otherwise, <see langword="false"/>.</returns>
        public bool Equals(SoftwareSystem softwareSystem)
        {
            return this.Equals(softwareSystem as Element);
        }

    }
}