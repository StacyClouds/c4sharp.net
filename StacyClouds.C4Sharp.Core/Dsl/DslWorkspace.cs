using System.Collections.Generic;

namespace StacyClouds.C4Sharp.Dsl
{
    /// <summary>
    /// Represents a simplified, Structurizr DSL-shaped workspace definition for import.
    /// </summary>
    public sealed class DslWorkspace
    {
        /// <summary>
        /// The imported workspace name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The imported workspace description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The model definition to import.
        /// </summary>
        public DslModel Model { get; set; } = new DslModel();

        /// <summary>
        /// The view definitions to import.
        /// </summary>
        public DslViews Views { get; set; } = new DslViews();
    }

    /// <summary>
    /// Represents the DSL-shaped model portion of an imported workspace.
    /// </summary>
    public sealed class DslModel
    {
        /// <summary>
        /// The optional enterprise name for the imported model.
        /// </summary>
        public string Enterprise { get; set; }

        /// <summary>
        /// The people to import.
        /// </summary>
        public IList<DslPerson> People { get; set; } = new List<DslPerson>();

        /// <summary>
        /// The software systems to import.
        /// </summary>
        public IList<DslSoftwareSystem> SoftwareSystems { get; set; } = new List<DslSoftwareSystem>();

        /// <summary>
        /// The relationships to import.
        /// </summary>
        public IList<DslRelationship> Relationships { get; set; } = new List<DslRelationship>();
    }

    /// <summary>
    /// Describes a person to import.
    /// </summary>
    public sealed class DslPerson
    {
        /// <summary>
        /// The optional explicit identifier to preserve.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The person name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The person description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The location to assign to the person.
        /// </summary>
        public Location? Location { get; set; }
    }

    /// <summary>
    /// Describes a software system to import.
    /// </summary>
    public sealed class DslSoftwareSystem
    {
        /// <summary>
        /// The optional explicit identifier to preserve.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The software system name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The software system description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The location to assign to the software system.
        /// </summary>
        public Location? Location { get; set; }

        /// <summary>
        /// The containers to import under the software system.
        /// </summary>
        public IList<DslContainer> Containers { get; set; } = new List<DslContainer>();
    }

    /// <summary>
    /// Describes a container to import.
    /// </summary>
    public sealed class DslContainer
    {
        /// <summary>
        /// The optional explicit identifier to preserve.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The container name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The container description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The container technology.
        /// </summary>
        public string Technology { get; set; }

        /// <summary>
        /// The components to import under the container.
        /// </summary>
        public IList<DslComponent> Components { get; set; } = new List<DslComponent>();
    }

    /// <summary>
    /// Describes a component to import.
    /// </summary>
    public sealed class DslComponent
    {
        /// <summary>
        /// The optional explicit identifier to preserve.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The component name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The component description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The component technology.
        /// </summary>
        public string Technology { get; set; }
    }

    /// <summary>
    /// Describes a relationship to import.
    /// </summary>
    public sealed class DslRelationship
    {
        /// <summary>
        /// The optional explicit identifier to preserve.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The identifier of the relationship source element.
        /// </summary>
        public string SourceId { get; set; }

        /// <summary>
        /// The identifier of the relationship destination element.
        /// </summary>
        public string DestinationId { get; set; }

        /// <summary>
        /// The relationship description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The relationship technology.
        /// </summary>
        public string Technology { get; set; }

        /// <summary>
        /// The interaction style to assign to the relationship.
        /// </summary>
        public InteractionStyle? InteractionStyle { get; set; }

        /// <summary>
        /// The tags to assign to the relationship.
        /// </summary>
        public IList<string> Tags { get; set; } = new List<string>();
    }

    /// <summary>
    /// Configures how a DSL-shaped workspace should be imported.
    /// </summary>
    public sealed class DslImportOptions
    {
        /// <summary>
        /// Overrides the implied relationships strategy used by the imported model.
        /// </summary>
        public IImpliedRelationshipsStrategy ImpliedRelationshipsStrategy { get; set; }
    }
}
