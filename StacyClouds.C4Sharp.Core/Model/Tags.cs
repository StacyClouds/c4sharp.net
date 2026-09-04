namespace StacyClouds.C4Sharp
{
    /// <summary>
    /// Defines the built-in tags applied to elements and relationships.
    /// </summary>
    public sealed class Tags
    {
        /// <summary>
        /// Applied to every element in the model.
        /// </summary>
        public const string Element = "Element";
        /// <summary>
        /// Applied to every relationship in the model.
        /// </summary>
        public const string Relationship = "Relationship";

        /// <summary>
        /// Applied to person elements.
        /// </summary>
        public const string Person = "Person";
        /// <summary>
        /// Applied to software system elements.
        /// </summary>
        public const string SoftwareSystem = "Software System";
        /// <summary>
        /// Applied to container elements.
        /// </summary>
        public const string Container = "Container";
        /// <summary>
        /// Applied to component elements.
        /// </summary>
        public const string Component = "Component";

        /// <summary>
        /// Applied to synchronous relationships.
        /// </summary>
        public const string Synchronous = "Synchronous";
        /// <summary>
        /// Applied to asynchronous relationships.
        /// </summary>
        public const string Asynchronous = "Asynchronous";

        /// <summary>
        /// Applied to deployment node elements.
        /// </summary>
        public const string DeploymentNode = "Deployment Node";
        /// <summary>
        /// Applied to infrastructure node elements.
        /// </summary>
        public const string InfrastructureNode = "Infrastructure Node";
        /// <summary>
        /// Applied to software system instances in deployment views.
        /// </summary>
        public const string SoftwareSystemInstance = "Software System Instance";
        /// <summary>
        /// Applied to container instances in deployment views.
        /// </summary>
        public const string ContainerInstance = "Container Instance";
        
    }
}
