using System.Collections.Generic;

namespace StacyClouds.C4Sharp.Dsl
{
    public sealed class DslViews
    {
        public IList<DslSystemLandscapeView> SystemLandscapeViews { get; set; } = new List<DslSystemLandscapeView>();

        public IList<DslSystemContextView> SystemContextViews { get; set; } = new List<DslSystemContextView>();

        public IList<DslContainerView> ContainerViews { get; set; } = new List<DslContainerView>();

        public IList<DslComponentView> ComponentViews { get; set; } = new List<DslComponentView>();
    }

    public abstract class DslViewDefinition
    {
        public string Key { get; set; }

        public string Description { get; set; }

        public IList<string> ElementIds { get; set; } = new List<string>();
    }

    public sealed class DslSystemLandscapeView : DslViewDefinition
    {
    }

    public sealed class DslSystemContextView : DslViewDefinition
    {
        public string SoftwareSystemId { get; set; }
    }

    public sealed class DslContainerView : DslViewDefinition
    {
        public string SoftwareSystemId { get; set; }
    }

    public sealed class DslComponentView : DslViewDefinition
    {
        public string ContainerId { get; set; }
    }
}
