using System.Collections.Generic;

namespace StacyClouds.C4Sharp.Dsl
{
    public sealed class DslWorkspace
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DslModel Model { get; set; } = new DslModel();

        public DslViews Views { get; set; } = new DslViews();
    }

    public sealed class DslModel
    {
        public string Enterprise { get; set; }

        public IList<DslPerson> People { get; set; } = new List<DslPerson>();

        public IList<DslSoftwareSystem> SoftwareSystems { get; set; } = new List<DslSoftwareSystem>();

        public IList<DslRelationship> Relationships { get; set; } = new List<DslRelationship>();
    }

    public sealed class DslPerson
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Location? Location { get; set; }
    }

    public sealed class DslSoftwareSystem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Location? Location { get; set; }

        public IList<DslContainer> Containers { get; set; } = new List<DslContainer>();
    }

    public sealed class DslContainer
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Technology { get; set; }

        public IList<DslComponent> Components { get; set; } = new List<DslComponent>();
    }

    public sealed class DslComponent
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Technology { get; set; }
    }

    public sealed class DslRelationship
    {
        public string Id { get; set; }

        public string SourceId { get; set; }

        public string DestinationId { get; set; }

        public string Description { get; set; }

        public string Technology { get; set; }

        public InteractionStyle? InteractionStyle { get; set; }

        public IList<string> Tags { get; set; } = new List<string>();
    }

    public sealed class DslImportOptions
    {
        public IImpliedRelationshipsStrategy ImpliedRelationshipsStrategy { get; set; }
    }
}
