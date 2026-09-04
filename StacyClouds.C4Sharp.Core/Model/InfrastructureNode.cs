using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{
    
    /// <summary>
    /// Represents an infrastructure node, which is something like:
    ///  - Load balancer
    ///  - Firewall
    ///  - DNS service
    ///  - etc
    /// </summary>
    [DataContract]
    public sealed class InfrastructureNode : DeploymentElement
    {

        private DeploymentNode _parent;

        /// <summary>
        /// The parent DeploymentNode, or null if there is no parent.
        /// </summary>
        public override Element Parent
        {
            get { return _parent; }
            set { _parent = value as DeploymentNode; }
        }
            
        /// <summary>
        /// The technology or product used to provide this infrastructure capability.
        /// </summary>
        [DataMember(Name = "technology", EmitDefaultValue = false)]
        public string Technology { get; set; }

        /// <summary>
        /// Initializes an infrastructure node for deserialization.
        /// </summary>
        internal InfrastructureNode()
        {
        }

        /// <summary>
        /// Returns the tags that are always applied to infrastructure nodes.
        /// </summary>
        /// <returns>The required infrastructure node tags.</returns>
        public override List<string> GetRequiredTags()
        {
            return new List<string>
            {
                StacyClouds.C4Sharp.Tags.Element,
                StacyClouds.C4Sharp.Tags.InfrastructureNode
            };
        }

        /// <summary>
        /// Gets the canonical name for this infrastructure node.
        /// </summary>
        public override string CanonicalName
        {
            get
            {
                return new CanonicalNameGenerator().Generate(this);
            }
        }

        /// <summary>
        /// Adds a relationship between this and another deployment element (deployment node, infrastructure node, or container instance).
        /// </summary>
        /// <param name="destination">the destination DeploymentElement</param>
        /// <param name="description">a short description of the relationship</param>
        /// <param name="technology">the technology</param>
        /// <returns>a Relationship object</returns>
        public Relationship Uses(DeploymentElement destination, string description, string technology)
        {
            return Model.AddRelationship(this, destination, description, technology);
        }

        /// <summary>
        /// Adds a relationship between this and another deployment element (deployment node, infrastructure node, or container instance).
        /// </summary>
        /// <param name="destination">the destination DeploymentElement</param>
        /// <param name="description">a short description of the relationship</param>
        /// <param name="technology">the technology</param>
        /// <param name="interactionStyle">the interaction style (Synchronous vs Asynchronous)</param>
        /// <returns>a Relationship object</returns>
        public Relationship Uses(DeploymentElement destination, string description, string technology, InteractionStyle interactionStyle)
        {
            return Model.AddRelationship(this, destination, description, technology, interactionStyle);
        }

    }
    
}