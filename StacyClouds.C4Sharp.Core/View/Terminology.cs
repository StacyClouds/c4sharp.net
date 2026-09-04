using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp.Core.View
{

    /// <summary>
    /// Provides custom labels for view terminology, such as language-specific translations.
    /// </summary>
    [DataContract]
    public sealed class Terminology
    {

        /// <summary>
        /// Overrides the label used for enterprise boundaries.
        /// </summary>
        [DataMember(Name = "enterprise", EmitDefaultValue = false)]
        public string Enterprise;

        /// <summary>
        /// Overrides the label used for people.
        /// </summary>
        [DataMember(Name = "person", EmitDefaultValue = false)]
        public string Person;

        /// <summary>
        /// Overrides the label used for software systems.
        /// </summary>
        [DataMember(Name = "softwareSystem", EmitDefaultValue = false)]
        public string SoftwareSystem;

        /// <summary>
        /// Overrides the label used for containers.
        /// </summary>
        [DataMember(Name = "container", EmitDefaultValue = false)]
        public string Container;

        /// <summary>
        /// Overrides the label used for components.
        /// </summary>
        [DataMember(Name = "component", EmitDefaultValue = false)]
        public string Component;

        /// <summary>
        /// Overrides the label used for code elements.
        /// </summary>
        [DataMember(Name = "code", EmitDefaultValue = false)]
        public string Code;

        /// <summary>
        /// Overrides the label used for deployment nodes.
        /// </summary>
        [DataMember(Name = "deploymentNode", EmitDefaultValue = false)]
        public string DeploymentNode;

        /// <summary>
        /// Overrides the label used for infrastructure nodes.
        /// </summary>
        [DataMember(Name = "infrastructureNode", EmitDefaultValue = false)]
        public string InfrastructureNode;

        /// <summary>
        /// Overrides the label used for relationships.
        /// </summary>
        [DataMember(Name = "relationship", EmitDefaultValue = false)]
        public string Relationship;

    }
}