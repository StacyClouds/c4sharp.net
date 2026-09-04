using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{
    
    /// <summary>
    /// Represents a deployment instance of a Container, which can be added to a DeploymentNode.
    /// </summary>
    [DataContract]
    public sealed class ContainerInstance : StaticStructureElementInstance
    {
        /// <summary>
        /// The container represented by this deployment-time instance.
        /// </summary>
        public Container Container { get; internal set; }

        private string _containerId;

        /// <summary>
        /// The identifier of the underlying container.
        /// </summary>
        [DataMember(Name = "containerId", EmitDefaultValue = false)]
        public string ContainerId
        {
            get
            {
                if (Container != null)
                {
                    return Container.Id;
                }
                else
                {
                    return _containerId;
                }
            }
            set { _containerId = value; }
        }

        /// <summary>
        /// Initializes a container instance for deserialization.
        /// </summary>
        internal ContainerInstance() {
        }

        /// <summary>
        /// Initializes a container instance for the specified deployment node context.
        /// </summary>
        /// <param name="container">The container being deployed.</param>
        /// <param name="instanceId">The instance number within the deployment environment.</param>
        /// <param name="environment">The deployment environment.</param>
        /// <param name="deploymentGroup">The deployment group used for relationship replication.</param>
        internal ContainerInstance(Container container, int instanceId, string environment, string deploymentGroup)
        {
            Container = container;
            AddTags(StacyClouds.C4Sharp.Tags.ContainerInstance);
            InstanceId = instanceId;
            Environment = environment;
            DeploymentGroup = deploymentGroup;
        }

        /// <summary>
        /// Returns the static container represented by this deployment instance.
        /// </summary>
        /// <returns>The underlying container.</returns>
        public override StaticStructureElement getElement()
        {
            return Container;
        }

        /// <summary>
        /// Gets the canonical name of this container instance, including deployment path and instance number.
        /// </summary>
        public override string CanonicalName
        {
            get { return new CanonicalNameGenerator().Generate(this); }
        }

    }

}