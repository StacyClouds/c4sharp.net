using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{
    
    /// <summary>
    /// This is the superclass for model elements that describe deployment nodes, infrastructure nodes, and container instances.
    /// </summary>
    [DataContract]
    public abstract class DeploymentElement : Element
    {
        /// <summary>
        /// The default deployment environment assigned when none is specified.
        /// </summary>
        internal const string DefaultDeploymentEnvironment = "Default";
        /// <summary>
        /// The default deployment group used for replicated relationships.
        /// </summary>
        internal const string DefaultDeploymentGroup = "Default";

        /// <summary>
        /// The deployment environment that this element belongs to.
        /// </summary>
        [DataMember(Name = "environment", EmitDefaultValue = false)]
        public string Environment { get; internal set; }

        private Element _parent;

        /// <summary>
        /// Gets or sets the parent deployment element in the deployment hierarchy.
        /// </summary>
        public override Element Parent { get; set; }
    }

}