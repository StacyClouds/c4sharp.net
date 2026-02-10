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

        public SoftwareSystem SoftwareSystem { get; internal set; }

        private string _softwareSystemId;

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

        internal SoftwareSystemInstance() {
        }

        internal SoftwareSystemInstance(SoftwareSystem softwareSystem, int instanceId, string environment, string deploymentGroup)
        {
            SoftwareSystem = softwareSystem;
            AddTags(StacyClouds.C4Sharp.Tags.SoftwareSystemInstance);
            InstanceId = instanceId;
            Environment = environment;
            DeploymentGroup = deploymentGroup;
        }

        public override StaticStructureElement getElement()
        {
            return SoftwareSystem;
        }

        public override string CanonicalName
        {
            get { return new CanonicalNameGenerator().Generate(this); }
        }

    }

}