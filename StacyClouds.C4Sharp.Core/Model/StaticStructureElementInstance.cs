using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{
    
    /// <summary>
    /// Represents a deployment instance of a SoftwareSystem or a Container, which can be added to a DeploymentNode.
    /// </summary>
    [DataContract]
    public abstract class StaticStructureElementInstance : DeploymentElement
    {

        private const int DefaultHealthCheckIntervalInSeconds = 60;
        private const long DefaultHealthCheckTimeoutInMilliseconds = 0;

        /// <summary>
        /// The deployment group used to decide which instances have replicated relationships.
        /// </summary>
        [DataMember(Name = "deploymentGroup", EmitDefaultValue = false)]
        public string DeploymentGroup { get; internal set; }

        /// <summary>
        /// The one-based instance number for the underlying static element within the deployment environment.
        /// </summary>
        [DataMember(Name = "instanceId", EmitDefaultValue = false)]
        public int InstanceId { get; internal set; }

        /// <summary>
        /// The name of the underlying static structure element.
        /// </summary>
        public override string Name
        {
            get
            {
                return getElement().Name;
            }
            internal set
            {
                // no-op
            }
        }

        private HashSet<HttpHealthCheck> _healthChecks = new HashSet<HttpHealthCheck>();

        /// <summary>
        /// The set of health checks associated with this container instance.
        /// </summary>
        [DataMember(Name = "healthChecks", EmitDefaultValue = false)]
        public ISet<HttpHealthCheck> HealthChecks
        {
            get
            {
                return new HashSet<HttpHealthCheck>(_healthChecks);
            }

            internal set
            {
                _healthChecks = new HashSet<HttpHealthCheck>(value);
            }
        }

        /// <summary>
        /// Initializes a static structure element instance for deserialization.
        /// </summary>
        internal StaticStructureElementInstance() {
        }

        /// <summary>
        /// Returns the static structure element represented by this deployment instance.
        /// </summary>
        /// <returns>The underlying static structure element.</returns>
        public abstract StaticStructureElement getElement();

        /// <summary>
        /// Returns the required tags for this instance type.
        /// </summary>
        /// <returns>An empty list because instance-specific required tags are supplied by derived types.</returns>
        public override List<string> GetRequiredTags()
        {
            return new List<string>();
        }

        /// <summary>
        /// Prevents removal of tags inherited from the underlying static structure element.
        /// </summary>
        /// <param name="tag">The tag to remove.</param>
        public override void RemoveTag(string tag)
        {
            // do nothing ... tags cannot be removed from element instances (they should reflect the software system/container they are based upon)
        }

        /// <summary>
        /// Adds a new health check, with the default interval (60 seconds) and timeout (0 milliseconds).
        /// </summary>
        /// <param name="name">The name of the health check.</param>
        /// <param name="url">The URL of the health check.</param>
        /// <returns>A HttpHealthCheck instance.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the name or URL is blank, or when the URL is not valid.
        /// </exception>
        public HttpHealthCheck AddHealthCheck(string name, string url)
        {
            return AddHealthCheck(name, url, DefaultHealthCheckIntervalInSeconds, DefaultHealthCheckTimeoutInMilliseconds);
        }

        /// <summary>
        /// Adds a new health check.
        /// </summary>
        /// <param name="name">The name of the health check.</param>
        /// <param name="url">The URL of the health check.</param>
        /// <param name="interval">The polling interval, in seconds.</param>
        /// <param name="timeout">The timeout, in milliseconds.</param>
        /// <returns>A HttpHealthCheck instance.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the name or URL is invalid, or when <paramref name="interval"/> or <paramref name="timeout"/> is negative.
        /// </exception>
        public HttpHealthCheck AddHealthCheck(string name, string url, int interval, long timeout)
        {
            if (name == null || name.Trim().Length == 0)
            {
                throw new ArgumentException("The name must not be null or empty.");
            }

            if (url == null || url.Trim().Length == 0)
            {
                throw new ArgumentException("The URL must not be null or empty.");
            }

            if (!StacyClouds.C4Sharp.Util.Url.IsUrl(url))
            {
                throw new ArgumentException(url + " is not a valid URL.");
            }

            if (interval < 0)
            {
                throw new ArgumentException("The polling interval must be zero or a positive integer.");
            }

            if (timeout < 0)
            {
                throw new ArgumentException("The timeout must be zero or a positive integer.");
            }

            HttpHealthCheck healthCheck = new HttpHealthCheck(name, url, interval, timeout);
            _healthChecks.Add(healthCheck);

            return healthCheck;
        }
        
        /// <summary>
        /// Adds a relationship between this element instance and an infrastructure node.
        /// </summary>
        /// <param name="destination">the destination InfrastructureNode</param>
        /// <param name="description">a short description of the relationship</param>
        /// <param name="technology">the technology</param>
        /// <returns>a Relationship object</returns>
        public Relationship Uses(InfrastructureNode destination, string description, string technology) {
            return Uses(destination, description, technology, null);
        }

        /// <summary>
        /// Adds a relationship between this element instance and an infrastructure node.
        /// </summary>
        /// <param name="destination">the destination InfrastructureNode</param>
        /// <param name="description">a short description of the relationship</param>
        /// <param name="technology">the technology</param>
        /// <param name="interactionStyle">the interaction style (Synchronous vs Asynchronous)</param>
        /// <returns>a Relationship object</returns>
        public Relationship Uses(InfrastructureNode destination, string description, string technology, InteractionStyle? interactionStyle) {
            return Model.AddRelationship(this, destination, description, technology, interactionStyle);
        }

    }

}