using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// A component (a grouping of related functionality behind an interface that runs inside a container).
    /// </summary>
    [DataContract]
    public sealed class Component : StaticStructureElement, IEquatable<Component>
    {
        /// <summary>
        /// The container that owns this component.
        /// </summary>
        public override Element Parent { get; set; }

        /// <summary>
        /// The container that this component belongs to.
        /// </summary>
        public Container Container
        {
            get
            {
                return Parent as Container;
            }
        }

        /// <summary>
        /// The technology associated with this component (e.g. Spring Bean).
        /// </summary>
        [DataMember(Name="technology", EmitDefaultValue=false)]
        public string Technology { get; set; }
          
        /// <summary>
        /// The size of this component (e.g. lines of code).
        /// </summary>
        [DataMember(Name="size", EmitDefaultValue = true)]
        public long Size { get; set; }

        private HashSet<CodeElement> _codeElements;

        /// <summary>
        /// The implementation type (e.g. a fully qualified interface/class name).
        /// </summary>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public ISet<CodeElement> CodeElements
        {
            get
            {
                return new HashSet<CodeElement>(_codeElements);
            }

            internal set
            {
                _codeElements = new HashSet<CodeElement>(value);
            }
        }

        /// <summary>
        /// Initializes a component for deserialization.
        /// </summary>
        internal Component()
        {
            _codeElements = new HashSet<CodeElement>();
        }

        /// <summary>
        /// Gets the canonical name for this component.
        /// </summary>
        public override string CanonicalName
        {
            get
            {
                return new CanonicalNameGenerator().Generate(this);
            }
        }

        /// <summary>
        /// Returns the tags that are always applied to components.
        /// </summary>
        /// <returns>The required component tags.</returns>
        public override List<string> GetRequiredTags()
        {
            return new List<string>
            {
                StacyClouds.C4Sharp.Tags.Element,
                StacyClouds.C4Sharp.Tags.Component
            };
        }

        /// <summary>
        /// Gets or sets the primary implementation type for this component.
        /// </summary>
        public string Type
        {
            get
            {
                CodeElement codeElement = _codeElements.FirstOrDefault(ce => ce.Role == CodeElementRole.Primary);
                return codeElement?.Type;
            }

            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    _codeElements.RemoveWhere(ce => ce.Role == CodeElementRole.Primary);
                    CodeElement codeElement = new CodeElement(value);
                    codeElement.Role = CodeElementRole.Primary;
                    _codeElements.Add(codeElement);
                }
            }
        }

        /// <summary>
        /// Adds a supporting implementation type to this component.
        /// </summary>
        /// <param name="type">The fully qualified type name to associate with the component.</param>
        /// <returns>The created supporting <see cref="CodeElement"/>.</returns>
        public CodeElement AddSupportingType(string type)
        {
            CodeElement codeElement = new CodeElement(type);
            codeElement.Role = CodeElementRole.Supporting;
            _codeElements.Add(codeElement);

            return codeElement;
        }

        /// <summary>
        /// Compares this component with another component by canonical identity.
        /// </summary>
        /// <param name="component">The component to compare with.</param>
        /// <returns><see langword="true"/> when both components represent the same model element; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Component component)
        {
            return this.Equals(component as Element);
        }

    }
}