using System.Collections.Generic;

namespace StacyClouds.C4Sharp.Dsl
{
    /// <summary>
    /// Collects the view definitions that can be imported into a workspace.
    /// </summary>
    public sealed class DslViews
    {
        /// <summary>
        /// The system landscape views to import.
        /// </summary>
        public IList<DslSystemLandscapeView> SystemLandscapeViews { get; set; } = new List<DslSystemLandscapeView>();

        /// <summary>
        /// The system context views to import.
        /// </summary>
        public IList<DslSystemContextView> SystemContextViews { get; set; } = new List<DslSystemContextView>();

        /// <summary>
        /// The container views to import.
        /// </summary>
        public IList<DslContainerView> ContainerViews { get; set; } = new List<DslContainerView>();

        /// <summary>
        /// The component views to import.
        /// </summary>
        public IList<DslComponentView> ComponentViews { get; set; } = new List<DslComponentView>();
    }

    /// <summary>
    /// Provides the common fields shared by imported view definitions.
    /// </summary>
    public abstract class DslViewDefinition
    {
        /// <summary>
        /// The view key to assign in the imported workspace.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The description to assign to the imported view.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The element identifiers to place into the imported view.
        /// </summary>
        public IList<string> ElementIds { get; set; } = new List<string>();
    }

    /// <summary>
    /// Describes a system landscape view to import.
    /// </summary>
    public sealed class DslSystemLandscapeView : DslViewDefinition
    {
    }

    /// <summary>
    /// Describes a system context view to import.
    /// </summary>
    public sealed class DslSystemContextView : DslViewDefinition
    {
        /// <summary>
        /// Identifies the software system that anchors the imported view.
        /// </summary>
        public string SoftwareSystemId { get; set; }
    }

    /// <summary>
    /// Describes a container view to import.
    /// </summary>
    public sealed class DslContainerView : DslViewDefinition
    {
        /// <summary>
        /// Identifies the software system that anchors the imported view.
        /// </summary>
        public string SoftwareSystemId { get; set; }
    }

    /// <summary>
    /// Describes a component view to import.
    /// </summary>
    public sealed class DslComponentView : DslViewDefinition
    {
        /// <summary>
        /// Identifies the container that anchors the imported view.
        /// </summary>
        public string ContainerId { get; set; }
    }
}
