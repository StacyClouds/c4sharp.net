using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{
    
    /// <summary>
    /// Represents a deployment instance of a Software System, which can be added to a DeploymentNode.
    /// </summary>
    [DataContract]
    public sealed class SoftwareSystemInstance : StaticStructureElementInstance
    {
        /// <summary>
        /// The software system represented by this deployment-time instance.
        /// </summary>
        public SoftwareSystem SoftwareSystem { get; internal set; }

        private string _softwareSystemId;

        /// <summary>
        /// The identifier of the underlying software system.
        /// </summary>
        [DataMember(Name = "softwareSystemId", EmitDefaultValue = false)]
        public string SoftwareSystemId
        {
            get
            {
                if (SoftwareSystem != null)
                {
                    return SoftwareSystem.Id;
                }
                else
                {
                    return _softwareSystemId;
                }
            }
            set { _softwareSystemId = value; }
        }

        /// <summary>
        /// Initializes a software system instance for deserialization.
        /// </summary>
        internal SoftwareSystemInstance() {
        }

        /// <summary>
        /// Initializes a software system instance for the specified deployment node context.
        /// </summary>
        /// <param name="softwareSystem">The software system being deployed.</param>
        /// <param name="instanceId">The instance number within the deployment environment.</param>
        /// <param name="environment">The deployment environment.</param>
        /// <param name="deploymentGroup">The deployment group used for relationship replication.</param>
        internal SoftwareSystemInstance(SoftwareSystem softwareSystem, int instanceId, string environment, string deploymentGroup)
        {
            SoftwareSystem = softwareSystem;
            AddTags(StacyClouds.C4Sharp.Tags.SoftwareSystemInstance);
            InstanceId = instanceId;
            Environment = environment;
            DeploymentGroup = deploymentGroup;
        }

        /// <summary>
        /// Returns the static software system represented by this deployment instance.
        /// </summary>
        /// <returns>The underlying software system.</returns>
        public override StaticStructureElement getElement()
        {
            return SoftwareSystem;
        }

        /// <summary>
        /// Gets the canonical name of this software system instance, including deployment path and instance number.
        /// </summary>
        public override string CanonicalName
        {
            get { return new CanonicalNameGenerator().Generate(this); }
        }

    }

}