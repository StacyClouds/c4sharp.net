using System.Text;

namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Builds canonical names for model elements based on their position in the model hierarchy.
    /// </summary>
    internal class CanonicalNameGenerator
    {

        private const string CustomElementType = "Custom://";
        private const string PersonType = "Person://";
        private const string SoftwareSystemType = "SoftwareSystem://";
        private const string ContainerType = "Container://";
        private const string ComponentType = "Component://";

        private const string DeploymentNodeType = "DeploymentNode://";
        private const string InfrastructureNodeType = "InfrastructureNode://";
        private const string ContainerInstanceType = "ContainerInstance://";
        private const string SoftwareSystemInstanceType = "SoftwareSystemInstance://";

        private const string StaticCanonicalNameSeperator = ".";
        private const string DeploymentCanonicalNameSeperator = "/";

        private string formatName(Element element)
        {
            return formatName(element.Name);
        }

        private string formatName(string name)
        {
            return name
                .Replace(StaticCanonicalNameSeperator, "")
                .Replace(DeploymentCanonicalNameSeperator, "");
        }

        /// <summary>
        /// Builds the canonical name for a person.
        /// </summary>
        /// <param name="person">The person to format.</param>
        /// <returns>The canonical name for <paramref name="person"/>.</returns>
        internal string Generate(Person person)
        {
            return PersonType + formatName(person);
        }

        /// <summary>
        /// Builds the canonical name for a software system.
        /// </summary>
        /// <param name="softwareSystem">The software system to format.</param>
        /// <returns>The canonical name for <paramref name="softwareSystem"/>.</returns>
        internal string Generate(SoftwareSystem softwareSystem)
        {
            return SoftwareSystemType + formatName(softwareSystem);
        }

        /// <summary>
        /// Builds the canonical name for a container.
        /// </summary>
        /// <param name="container">The container to format.</param>
        /// <returns>The canonical name for <paramref name="container"/>.</returns>
        internal string Generate(Container container)
        {
            return ContainerType + formatName(container.SoftwareSystem) + StaticCanonicalNameSeperator + formatName(container);
        }

        /// <summary>
        /// Builds the canonical name for a component.
        /// </summary>
        /// <param name="component">The component to format.</param>
        /// <returns>The canonical name for <paramref name="component"/>.</returns>
        internal string Generate(Component component)
        {
            return ComponentType + formatName(component.Container.SoftwareSystem) + StaticCanonicalNameSeperator + formatName(component.Container) + StaticCanonicalNameSeperator + formatName(component);
        }

        /// <summary>
        /// Builds the canonical name for a deployment node.
        /// </summary>
        /// <param name="deploymentNode">The deployment node to format.</param>
        /// <returns>The canonical name for <paramref name="deploymentNode"/>.</returns>
        internal string Generate(DeploymentNode deploymentNode)
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(DeploymentNodeType);

            buf.Append(formatName(deploymentNode.Environment));
            buf.Append(DeploymentCanonicalNameSeperator);

            string parents = "";
            DeploymentNode parent = (DeploymentNode)deploymentNode.Parent;
            while (parent != null)
            {
                parents = formatName(parent) + DeploymentCanonicalNameSeperator + parents;
                parent = (DeploymentNode)parent.Parent;
            }

            buf.Append(parents);
            buf.Append(formatName(deploymentNode));

            return buf.ToString();
        }

        /// <summary>
        /// Builds the canonical name for an infrastructure node.
        /// </summary>
        /// <param name="infrastructureNode">The infrastructure node to format.</param>
        /// <returns>The canonical name for <paramref name="infrastructureNode"/>.</returns>
        internal string Generate(InfrastructureNode infrastructureNode)
        {
            string deploymentNodeCanonicalName = Generate((DeploymentNode)infrastructureNode.Parent).Substring(DeploymentNodeType.Length);

            return InfrastructureNodeType + deploymentNodeCanonicalName + DeploymentCanonicalNameSeperator + formatName(infrastructureNode);
        }

        /// <summary>
        /// Builds the canonical name for a software system instance.
        /// </summary>
        /// <param name="softwareSystemInstance">The software system instance to format.</param>
        /// <returns>The canonical name for <paramref name="softwareSystemInstance"/>.</returns>
        internal string Generate(SoftwareSystemInstance softwareSystemInstance)
        {
            string deploymentNodeCanonicalName = Generate((DeploymentNode)softwareSystemInstance.Parent).Substring(DeploymentNodeType.Length);

            return SoftwareSystemInstanceType + deploymentNodeCanonicalName + DeploymentCanonicalNameSeperator + formatName(softwareSystemInstance.SoftwareSystem) + "[" + softwareSystemInstance.InstanceId + "]";
        }

        /// <summary>
        /// Builds the canonical name for a container instance.
        /// </summary>
        /// <param name="containerInstance">The container instance to format.</param>
        /// <returns>The canonical name for <paramref name="containerInstance"/>.</returns>
        internal string Generate(ContainerInstance containerInstance)
        {
            string deploymentNodeCanonicalName = Generate((DeploymentNode)containerInstance.Parent).Substring(DeploymentNodeType.Length);

            return ContainerInstanceType + deploymentNodeCanonicalName + DeploymentCanonicalNameSeperator + Generate(containerInstance.Container).Substring(ContainerType.Length) + "[" + containerInstance.InstanceId + "]";
        }

    }
}
