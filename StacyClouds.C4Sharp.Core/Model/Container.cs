using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// A container (e.g. an application or data store).
    /// </summary>
    [DataContract]
    public sealed class Container : StaticStructureElement, IEquatable<Container>
    {
        /// <summary>
        /// The software system that owns this container.
        /// </summary>
        public override Element Parent { get; set; }

        /// <summary>
        /// The software system that this container belongs to.
        /// </summary>
        public SoftwareSystem SoftwareSystem
        {
            get
            {
                return Parent as SoftwareSystem;
            }
        }

        /// <summary>
        /// The technology associated with this container (e.g. Windows Service).
        /// </summary>
        [DataMember(Name="technology", EmitDefaultValue=false)]
        public string Technology { get; set; }

        private HashSet<Component> _components;

        /// <summary>
        /// The set of components within this container.
        /// </summary>
        [DataMember(Name="components", EmitDefaultValue=false)]
        public ISet<Component> Components
        {
            get
            {
                return new HashSet<Component>(_components);
            }

            set
            {
                _components = new HashSet<Component>(value);
            }
        }
  
        /// <summary>
        /// Gets the canonical name for this container.
        /// </summary>
        public override string CanonicalName
        {
            get
            {
                return new CanonicalNameGenerator().Generate(this);
            }
        }

        /// <summary>
        /// Initializes a container for deserialization.
        /// </summary>
        internal Container()
        {
            _components = new HashSet<Component>();
        }

        /// <summary>
        /// Adds a component with the specified name (unless one exists with the same name already).
        /// </summary>
        /// <param name="name">the name of the component</param>
        /// <returns>a Component instance</returns>
        public Component AddComponent(string name)
        {
            return AddComponent(name, "");
        }

        /// <summary>
        /// Adds a component with the specified name and description (unless one exists with the same name already).
        /// </summary>
        /// <param name="name">the name of the component</param>
        /// <param name="description">a short description/list of responsibilities</param>
        /// <returns>a Component instance</returns>
        public Component AddComponent(string name, string description)
        {
            return AddComponent(name, description, "");
        }

        /// <summary>
        /// Adds a component with the specified name, description and technology (unless one exists with the same name already).
        /// </summary>
        /// <param name="name">the name of the component</param>
        /// <param name="description">a short description/list of responsibilities</param>
        /// <param name="technology">the technology choice</param>
        /// <returns>a Component instance</returns>
        public Component AddComponent(string name, string description, string technology)
        {
            return AddComponent(name, (String)null, description, technology);
        }

        /// <summary>
        /// Adds a component whose primary implementation type is described by a CLR <see cref="Type"/>.
        /// </summary>
        /// <param name="name">The component name.</param>
        /// <param name="type">The CLR type that represents the component.</param>
        /// <param name="description">A short description of the component responsibilities.</param>
        /// <param name="technology">The implementation technology.</param>
        /// <returns>The created component, or <see langword="null"/> if a component with the same name already exists.</returns>
        public Component AddComponent(string name, Type type, string description, string technology)
        {
           return AddComponent(name, type.AssemblyQualifiedName, description, technology);
        }

        /// <summary>
        /// Adds a component whose primary implementation type is described by a fully qualified type name.
        /// </summary>
        /// <param name="name">The component name.</param>
        /// <param name="type">The fully qualified implementation type name.</param>
        /// <param name="description">A short description of the component responsibilities.</param>
        /// <param name="technology">The implementation technology.</param>
        /// <returns>The created component, or <see langword="null"/> if a component with the same name already exists.</returns>
        public Component AddComponent(string name, string type, string description, string technology)
        {
            return Model.AddComponent(this, name, type, description, technology);
        }

        /// <summary>
        /// Adds an existing component instance to this container when it is not already present.
        /// </summary>
        /// <param name="component">The component to add.</param>
        internal void Add(Component component)
        {
            if (GetComponentWithName(component.Name) == null)
            {
                _components.Add(component);
            }
        }

        /// <summary>
        /// Finds a component in this container by name.
        /// </summary>
        /// <param name="name">The component name to search for.</param>
        /// <returns>The matching component, or <see langword="null"/> when no match exists.</returns>
        public Component GetComponentWithName(string name)
        {
            if (name == null)
            {
                return null;
            }

            foreach (Component component in Components)
            {
                if (component.Name == name)
                {
                    return component;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a component in this container by its primary implementation type.
        /// </summary>
        /// <param name="type">The fully qualified type name to search for.</param>
        /// <returns>The matching component, or <see langword="null"/> when no match exists.</returns>
        public Component GetComponentOfType(string type)
        {
            if (type == null)
            {
                return null;
            }

            return _components.Where(c => c.Type == type).FirstOrDefault();
        }

        /// <summary>
        /// Returns the tags that are always applied to containers.
        /// </summary>
        /// <returns>The required container tags.</returns>
        public override List<string> GetRequiredTags()
        {
            return new List<string>
            {
                StacyClouds.C4Sharp.Tags.Element,
                StacyClouds.C4Sharp.Tags.Container
            };
        }

        /// <summary>
        /// Compares this container with another container by canonical identity.
        /// </summary>
        /// <param name="container">The container to compare with.</param>
        /// <returns><see langword="true"/> when both containers represent the same model element; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Container container)
        {
            return this.Equals(container as Element);
        }

    }
}