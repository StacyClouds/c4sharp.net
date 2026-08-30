using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp
{

    /// <summary>
    /// A software architecture model.
    /// </summary>
    [DataContract]
    public sealed class Model
    {
        /// <summary>
        /// Controls whether additional implied relationships are created when static structure relationships are added.
        /// </summary>
        public IImpliedRelationshipsStrategy ImpliedRelationshipsStrategy = new DefaultImpliedRelationshipsStrategy();

        /// <summary>
        /// The enterprise boundary associated with this model, when one is defined.
        /// </summary>
        [DataMember(Name = "enterprise", EmitDefaultValue = false)]
        public Enterprise Enterprise { get; set; }

        private HashSet<Person> _people;

        /// <summary>
        /// The people defined in the model.
        /// </summary>
        [DataMember(Name = "people", EmitDefaultValue = false)]
        public ISet<Person> People
        {
            get
            {
                return new HashSet<Person>(_people);
            }

            internal set
            {
                _people = new HashSet<Person>(value);
            }
        }

        private HashSet<SoftwareSystem> _softwareSystems;

        /// <summary>
        /// The software systems defined in the model.
        /// </summary>
        [DataMember(Name = "softwareSystems", EmitDefaultValue = false)]
        public ISet<SoftwareSystem> SoftwareSystems
        {
            get
            {
                return new HashSet<SoftwareSystem>(_softwareSystems);
            }

            internal set
            {
                _softwareSystems = new HashSet<SoftwareSystem>(value);
            }
        }

        private HashSet<DeploymentNode> _deploymentNodes;

        /// <summary>
        /// The top-level deployment nodes defined in the model.
        /// </summary>
        [DataMember(Name = "deploymentNodes", EmitDefaultValue = false)]
        public ISet<DeploymentNode> DeploymentNodes
        {
            get
            {
                return new HashSet<DeploymentNode>(_deploymentNodes);
            }

            internal set
            {
                _deploymentNodes = new HashSet<DeploymentNode>(value);
            }
        }

        private readonly Dictionary<string, Element> _elementsById = new Dictionary<string, Element>();
        private readonly Dictionary<string, Relationship> _relationshipsById = new Dictionary<string, Relationship>();

        /// <summary>
        /// All relationships currently registered in the model.
        /// </summary>
        public ICollection<Relationship> Relationships
        {
            get
            {
                return new List<Relationship>(_relationshipsById.Values);
            }
        }

        /// <summary>
        /// Generates unique string identifiers for elements and relationships added to the model.
        /// </summary>
        public IdGenerator IdGenerator = new SequentialIntegerIdGeneratorStrategy();

        /// <summary>
        /// Initializes an empty model for deserialization and workspace construction.
        /// </summary>
        internal Model()
        {
            _people = new HashSet<Person>();
            _softwareSystems = new HashSet<SoftwareSystem>();
            _deploymentNodes = new HashSet<DeploymentNode>();
        }

        /// <summary>
        /// Creates a software system (location is unspecified) and adds it to the model
        /// (unless one exists with the same name already).
        /// </summary>
        /// <param name="name">The name of the software system.</param>
        /// <returns>the SoftwareSystem instance created and added to the model (or null)</returns>
        public SoftwareSystem AddSoftwareSystem(string name)
        {
            return AddSoftwareSystem(Location.Unspecified, name, "");
        }

        /// <summary>
        /// Creates a software system (location is unspecified) and adds it to the model
        /// (unless one exists with the same name already).
        /// </summary>
        /// <param name="name">The name of the software system.</param>
        /// <param name="description">A short description of the software system.</param>
        /// <returns>the SoftwareSystem instance created and added to the model (or null)</returns>
        public SoftwareSystem AddSoftwareSystem(string name, string description)
        {
            return AddSoftwareSystem(Location.Unspecified, name, description);
        }

        /// <summary>
        /// Creates a software system (location is unspecified) and adds it to the model
        /// (unless one exists with the same name already).
        /// </summary>
        /// <param name="location">The location of the software system (e.g. internal, external, etc)</param>
        /// <param name="name">The name of the software system</param>
        /// <param name="description">A short description of the software system.</param>
        /// <returns>the SoftwareSystem instance created and added to the model (or null)</returns>
        public SoftwareSystem AddSoftwareSystem(Location location, string name, string description)
        {
            if (GetSoftwareSystemWithName(name) == null)
            {
                SoftwareSystem softwareSystem = new SoftwareSystem();
                softwareSystem.Location = location;
                softwareSystem.Name = name;
                softwareSystem.Description = description;

                _softwareSystems.Add(softwareSystem);

                softwareSystem.Id = IdGenerator.GenerateId(softwareSystem);
                AddElementToInternalStructures(softwareSystem);

                return softwareSystem;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a person (location is unspecified) and adds it to the model
        /// (unless one exists with the same name already.
        /// </summary>
        /// <param name="name">the name of the person (e.g. "Admin User" or "Bob the Business User")</param>
        /// <returns>the Person instance created and added to the model (or null)</returns>
        public Person AddPerson(string name)
        {
            return AddPerson(Location.Unspecified, name, "");
        }

        /// <summary>
        /// Creates a person (location is unspecified) and adds it to the model
        /// (unless one exists with the same name already.
        /// </summary>
        /// <param name="name">the name of the person (e.g. "Admin User" or "Bob the Business User")</param>
        /// <param name="description">a short description of the person</param>
        /// <returns>the Person instance created and added to the model (or null)</returns>
        public Person AddPerson(string name, string description)
        {
            return AddPerson(Location.Unspecified, name, description);
        }

        /// <summary>
        /// Creates a person (location is unspecified) and adds it to the model
        /// (unless one exisrs with the same name already.
        /// </summary>
        /// <param name="location">the location of the person (e.g. internal, external, etc)</param>
        /// <param name="name">the name of the person (e.g. "Admin User" or "Bob the Business User")</param>
        /// <param name="description">a short description of the person</param>
        /// <returns>the Person instance created and added to the model (or null)</returns>
        public Person AddPerson(Location location, string name, string description)
        {
            if (GetPersonWithName(name) == null)
            {
                Person person = new Person();
                person.Location = location;
                person.Name = name;
                person.Description = description;

                _people.Add(person);

                person.Id = IdGenerator.GenerateId(person);
                AddElementToInternalStructures(person);

                return person;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Creates a container inside the specified software system.
        /// </summary>
        /// <param name="parent">The software system that will own the container.</param>
        /// <param name="name">The container name.</param>
        /// <param name="description">A short description of the container responsibilities.</param>
        /// <param name="technology">The implementation technology.</param>
        /// <returns>The created container, or <see langword="null"/> when a container with the same name already exists.</returns>
        internal Container AddContainer(SoftwareSystem parent, string name, string description, string technology)
        {
            if (parent.GetContainerWithName(name) == null)
            {
                Container container = new Container();
                container.Name = name;
                container.Description = description;
                container.Technology = technology;

                container.Parent = parent;
                parent.Add(container);

                container.Id = IdGenerator.GenerateId(container);
                AddElementToInternalStructures(container);

                return container;
            }
            else {
                return null;
            }
        }
        
        /// <summary>
        /// Creates a deployment instance for a software system and replicates matching relationships.
        /// </summary>
        /// <param name="deploymentNode">The deployment node that will own the instance.</param>
        /// <param name="softwareSystem">The software system being deployed.</param>
        /// <param name="deploymentGroup">The deployment group used to scope replicated relationships.</param>
        /// <returns>The created software system instance.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="softwareSystem"/> is <see langword="null"/>.</exception>
        internal SoftwareSystemInstance AddSoftwareSystemInstance(DeploymentNode deploymentNode, SoftwareSystem softwareSystem, string deploymentGroup)
        {
            if (softwareSystem == null) {
                throw new ArgumentException("A software system must be specified.");
            }

            long instanceNumber = deploymentNode.SoftwareSystemInstances.Count(ssi => ssi.SoftwareSystem.Equals(softwareSystem));
            instanceNumber++;
            SoftwareSystemInstance softwareSystemInstance = new SoftwareSystemInstance(softwareSystem, (int)instanceNumber, deploymentNode.Environment, deploymentGroup);
            softwareSystemInstance.Parent = deploymentNode;
            softwareSystemInstance.Id = IdGenerator.GenerateId(softwareSystemInstance);

            ReplicateElementRelationships(softwareSystemInstance);

            AddElementToInternalStructures(softwareSystemInstance);

            return softwareSystemInstance;
        }

        /// <summary>
        /// Creates a deployment instance for a container and replicates matching relationships.
        /// </summary>
        /// <param name="deploymentNode">The deployment node that will own the instance.</param>
        /// <param name="container">The container being deployed.</param>
        /// <param name="deploymentGroup">The deployment group used to scope replicated relationships.</param>
        /// <returns>The created container instance.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="container"/> is <see langword="null"/>.</exception>
        internal ContainerInstance AddContainerInstance(DeploymentNode deploymentNode, Container container, string deploymentGroup)
        {
            if (container == null) {
                throw new ArgumentException("A container must be specified.");
            }

            long instanceNumber = deploymentNode.ContainerInstances.Count(ci => ci.Container.Equals(container));
            instanceNumber++;
            ContainerInstance containerInstance = new ContainerInstance(container, (int)instanceNumber, deploymentNode.Environment, deploymentGroup);
            containerInstance.Parent = deploymentNode;
            containerInstance.Id = IdGenerator.GenerateId(containerInstance);

            ReplicateElementRelationships(containerInstance);

            AddElementToInternalStructures(containerInstance);

            return containerInstance;
        }

        private void ReplicateElementRelationships(StaticStructureElementInstance elementInstance) {
            StaticStructureElement element = elementInstance.getElement();

            // find all StaticStructureElementInstance objects in the same deployment environment and deployment group
            IEnumerable<StaticStructureElementInstance> elementInstances = GetElements().OfType<StaticStructureElementInstance>().Where(ssei => ssei.Environment.Equals(elementInstance.Environment) && ssei.DeploymentGroup.Equals(elementInstance.DeploymentGroup));

            // and replicate the relationships to/from the element instance
            foreach (StaticStructureElementInstance ssei in elementInstances) {
                StaticStructureElement sse = ssei.getElement();

                foreach (Relationship relationship in element.Relationships) {
                    if (relationship.Destination.Equals(sse)) {
                        Relationship newRelationship = AddRelationship(elementInstance, ssei, relationship.Description, relationship.Technology, relationship.InteractionStyle);
                        if (newRelationship != null) {
                            newRelationship.Tags = null;
                            newRelationship.LinkedRelationshipId = relationship.Id;
                        }
                    }
                }

                foreach (Relationship relationship in sse.Relationships) {
                    if (relationship.Destination.Equals(element)) {
                        Relationship newRelationship = AddRelationship(ssei, elementInstance, relationship.Description, relationship.Technology, relationship.InteractionStyle);
                        if (newRelationship != null) {
                            newRelationship.Tags = null;
                            newRelationship.LinkedRelationshipId = relationship.Id;
                        }
                    }
                }
            }
        }
    
        /// <summary>
        /// Creates a component inside the specified container.
        /// </summary>
        /// <param name="parent">The container that will own the component.</param>
        /// <param name="name">The component name.</param>
        /// <param name="type">The optional fully qualified implementation type name.</param>
        /// <param name="description">A short description of the component responsibilities.</param>
        /// <param name="technology">The implementation technology.</param>
        /// <returns>The created component.</returns>
        /// <exception cref="ArgumentException">Thrown when a component with the same name already exists in the container.</exception>
        internal Component AddComponent(Container parent, string name, string type, string description, string technology)
        {
            if (parent.GetComponentWithName(name) == null)
            {
                Component component = new Component();
                component.Name = name;
                component.Description = description;
                component.Technology = technology;

                if (type != null)
                {
                    component.Type = type;

                }

                component.Parent = parent;
                parent.Add(component);

                component.Id = IdGenerator.GenerateId(component);
                AddElementToInternalStructures(component);

                return component;
            }
             
            throw new ArgumentException("A container named '" + name + "' already exists for this software system.");
        }

        /// <summary>
        /// Adds a top-level deployment node in the default deployment environment.
        /// </summary>
        /// <param name="name">The deployment node name.</param>
        /// <param name="description">A short description of the deployment node.</param>
        /// <param name="technology">The technology or product represented by the deployment node.</param>
        /// <returns>The created deployment node.</returns>
        public DeploymentNode AddDeploymentNode(string name, string description, string technology) {
            return AddDeploymentNode(DeploymentElement.DefaultDeploymentEnvironment, name, description, technology);
        }

        /// <summary>
        /// Adds a top-level deployment node in the default deployment environment.
        /// </summary>
        /// <param name="name">The deployment node name.</param>
        /// <returns>The created deployment node.</returns>
        public DeploymentNode AddDeploymentNode(string name) {
            return AddDeploymentNode(DeploymentElement.DefaultDeploymentEnvironment, name, null, null);
        }

        /// <summary>
        /// Adds a top-level deployment node in the specified deployment environment.
        /// </summary>
        /// <param name="environment">The deployment environment name.</param>
        /// <param name="name">The deployment node name.</param>
        /// <param name="description">A short description of the deployment node.</param>
        /// <param name="technology">The technology or product represented by the deployment node.</param>
        /// <returns>The created deployment node.</returns>
        public DeploymentNode AddDeploymentNode(string environment, string name, string description, string technology) {
            return AddDeploymentNode(environment, name, description, technology, 1);
        }

        /// <summary>
        /// Adds a top-level deployment node with an explicit instance count in the default environment.
        /// </summary>
        /// <param name="name">The deployment node name.</param>
        /// <param name="description">A short description of the deployment node.</param>
        /// <param name="technology">The technology or product represented by the deployment node.</param>
        /// <param name="instances">The number of identical deployment node instances.</param>
        /// <returns>The created deployment node.</returns>
        public DeploymentNode AddDeploymentNode(string name, string description, string technology, int instances) {
            return AddDeploymentNode(DeploymentElement.DefaultDeploymentEnvironment, name, description, technology, instances);
        }

        /// <summary>
        /// Adds a top-level deployment node with an explicit instance count in the specified environment.
        /// </summary>
        /// <param name="environment">The deployment environment name.</param>
        /// <param name="name">The deployment node name.</param>
        /// <param name="description">A short description of the deployment node.</param>
        /// <param name="technology">The technology or product represented by the deployment node.</param>
        /// <param name="instances">The number of identical deployment node instances.</param>
        /// <returns>The created deployment node.</returns>
        public DeploymentNode AddDeploymentNode(string environment, string name, string description, string technology, int instances) {
            return AddDeploymentNode(environment, name, description, technology, instances, null);
        }

        /// <summary>
        /// Adds a top-level deployment node with custom properties in the default environment.
        /// </summary>
        /// <param name="name">The deployment node name.</param>
        /// <param name="description">A short description of the deployment node.</param>
        /// <param name="technology">The technology or product represented by the deployment node.</param>
        /// <param name="instances">The number of identical deployment node instances.</param>
        /// <param name="properties">Custom name-value properties for the deployment node.</param>
        /// <returns>The created deployment node.</returns>
        public DeploymentNode AddDeploymentNode(string name, string description, string technology, int instances, Dictionary<string,string> properties) {
            return AddDeploymentNode(DeploymentElement.DefaultDeploymentEnvironment, name, description, technology, instances, properties);
        }

        /// <summary>
        /// Adds a top-level deployment node with custom properties in the specified environment.
        /// </summary>
        /// <param name="environment">The deployment environment name.</param>
        /// <param name="name">The deployment node name.</param>
        /// <param name="description">A short description of the deployment node.</param>
        /// <param name="technology">The technology or product represented by the deployment node.</param>
        /// <param name="instances">The number of identical deployment node instances.</param>
        /// <param name="properties">Custom name-value properties for the deployment node.</param>
        /// <returns>The created deployment node.</returns>
        public DeploymentNode AddDeploymentNode(string environment, string name, string description, string technology, int instances, Dictionary<string,string> properties) {
            return AddDeploymentNode(null, environment, name, description, technology, instances, properties);
        }

        /// <summary>
        /// Creates a deployment node either at the top level or as a child of another deployment node.
        /// </summary>
        /// <param name="parent">The parent deployment node, or <see langword="null"/> for a top-level node.</param>
        /// <param name="environment">The deployment environment name.</param>
        /// <param name="name">The deployment node name.</param>
        /// <param name="description">A short description of the deployment node.</param>
        /// <param name="technology">The technology or product represented by the deployment node.</param>
        /// <param name="instances">The number of identical deployment node instances.</param>
        /// <param name="properties">Custom name-value properties for the deployment node.</param>
        /// <returns>The created deployment node.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="name"/> is blank or when a deployment or infrastructure node with the same name already exists at the target level.
        /// </exception>
        internal DeploymentNode AddDeploymentNode(DeploymentNode parent, string environment, string name, string description, string technology, int instances, Dictionary<string,string> properties) {
            if (name == null || name.Trim().Length == 0) {
                throw new ArgumentException("A name must be specified.");
            }

            if ((parent == null && GetDeploymentNodeWithName(name, environment) == null) || (parent != null && parent.GetDeploymentNodeWithName(name) == null && parent.GetInfrastructureNodeWithName(name) == null)) {
                DeploymentNode deploymentNode = new DeploymentNode
                {
                    Name = name,
                    Description = description,
                    Technology = technology,
                    Parent = parent,
                    Instances = instances,
                    Environment = environment
                };
                
                if (properties != null) {
                    deploymentNode.Properties = properties;
                }

                if (parent == null) {
                    _deploymentNodes.Add(deploymentNode);
                }

                deploymentNode.Id = IdGenerator.GenerateId(deploymentNode);
                AddElementToInternalStructures(deploymentNode);

                return deploymentNode;
            } else {
                throw new ArgumentException("A deployment/infrastructure node named '" + name + "' already exists.");
            }
        }

        /// <summary>
        /// Creates an infrastructure node under the specified deployment node.
        /// </summary>
        /// <param name="parent">The deployment node that will own the infrastructure node.</param>
        /// <param name="name">The infrastructure node name.</param>
        /// <param name="description">A short description of the infrastructure node.</param>
        /// <param name="technology">The technology or product represented by the infrastructure node.</param>
        /// <param name="properties">Custom name-value properties for the infrastructure node.</param>
        /// <returns>The created infrastructure node.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="name"/> is blank or when a deployment or infrastructure node with the same name already exists under the parent.
        /// </exception>
        internal InfrastructureNode AddInfrastructureNode(DeploymentNode parent, string name, string description, string technology, Dictionary<string,string> properties) {
            if (name == null || name.Trim().Length == 0) {
                throw new ArgumentException("A name must be specified.");
            }

            if (parent.GetDeploymentNodeWithName(name) == null && parent.GetInfrastructureNodeWithName(name) == null) {
                InfrastructureNode infrastructureNode = new InfrastructureNode
                {
                    Name = name,
                    Description = description,
                    Technology = technology,
                    Parent = parent,
                    Environment = parent.Environment
                };
                
                if (properties != null) {
                    infrastructureNode.Properties = properties;
                }

                infrastructureNode.Id = IdGenerator.GenerateId(infrastructureNode);
                AddElementToInternalStructures(infrastructureNode);

                return infrastructureNode;
            } else {
                throw new ArgumentException("A deployment/infrastructure node named '" + name + "' already exists.");
            }
        }

        /// <summary>
        /// Gets the DeploymentNode with the specified name.
        /// </summary>
        /// <param name="name">the name of the deployment node</param>
        /// <param name="environment">the name of the deployment environment</param>
        /// <returns>the DeploymentNode instance with the specified name (or null if it doesn't exist)</returns>
        public DeploymentNode GetDeploymentNodeWithName(string name, string environment)
        {
            return _deploymentNodes.FirstOrDefault(dn => dn.Environment.Equals(environment) && dn.Name.Equals(name));
        }

        /// <summary>
        /// Creates a relationship between two elements without explicitly specifying an interaction style.
        /// </summary>
        /// <param name="source">The source element.</param>
        /// <param name="destination">The destination element.</param>
        /// <param name="description">The relationship description.</param>
        /// <param name="technology">The technology used by the relationship.</param>
        /// <returns>The created relationship, or <see langword="null"/> when an equivalent relationship already exists.</returns>
        internal Relationship AddRelationship(Element source, Element destination, string description, string technology)
        {
            return AddRelationship(source, destination, description, technology, null);
        }

        /// <summary>
        /// Creates a relationship between two elements.
        /// </summary>
        /// <param name="source">The source element.</param>
        /// <param name="destination">The destination element.</param>
        /// <param name="description">The relationship description.</param>
        /// <param name="technology">The technology used by the relationship.</param>
        /// <param name="interactionStyle">The interaction style, if specified.</param>
        /// <returns>The created relationship, or <see langword="null"/> when an equivalent relationship already exists.</returns>
        internal Relationship AddRelationship(Element source, Element destination, string description, string technology, InteractionStyle? interactionStyle)
        {
            return AddRelationship(source, destination, description, technology, interactionStyle, new string[0], true);
        }

        /// <summary>
        /// Creates a relationship between two elements with custom tags.
        /// </summary>
        /// <param name="source">The source element.</param>
        /// <param name="destination">The destination element.</param>
        /// <param name="description">The relationship description.</param>
        /// <param name="technology">The technology used by the relationship.</param>
        /// <param name="interactionStyle">The interaction style, if specified.</param>
        /// <param name="tags">Additional tags to apply.</param>
        /// <returns>The created relationship, or <see langword="null"/> when an equivalent relationship already exists.</returns>
        internal Relationship AddRelationship(Element source, Element destination, string description, string technology, InteractionStyle? interactionStyle, string[] tags)
        {
            return AddRelationship(source, destination, description, technology, interactionStyle, tags, true);
        }

        /// <summary>
        /// Creates a relationship between two elements and optionally triggers implied-relationship generation.
        /// </summary>
        /// <param name="source">The source element.</param>
        /// <param name="destination">The destination element.</param>
        /// <param name="description">The relationship description.</param>
        /// <param name="technology">The technology used by the relationship.</param>
        /// <param name="interactionStyle">The interaction style, if specified.</param>
        /// <param name="tags">Additional tags to apply.</param>
        /// <param name="createImpliedRelationships"><see langword="true"/> to run the configured implied-relationship strategy.</param>
        /// <returns>The created relationship, or <see langword="null"/> when an equivalent relationship already exists.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="destination"/> is <see langword="null"/> or when the relationship would connect a parent and child element.
        /// </exception>
        /// <remarks>
        /// Implied relationships are only created for static structure elements and are delegated to
        /// <see cref="ImpliedRelationshipsStrategy"/>.
        /// </remarks>
        internal Relationship AddRelationship(Element source, Element destination, string description, string technology, InteractionStyle? interactionStyle, string[] tags, bool createImpliedRelationships) {
            
            if (destination == null)
            {
                throw new ArgumentException("The destination must be specified.");
            }

            if (IsChildOf(source, destination) || IsChildOf(destination, source))
            {
                throw new ArgumentException("Relationships cannot be added between parents and children.");
            }

            Relationship relationship = new Relationship(source, destination, description, technology, interactionStyle, tags);
            if (AddRelationship(relationship))
            {

                if (createImpliedRelationships)
                {
                    if
                    (
                        (source is Person || source is SoftwareSystem || source is Container || source is Component) &&
                        (destination is Person || destination is SoftwareSystem || destination is Container || destination is Component)
                        )
                    {
                        ImpliedRelationshipsStrategy.CreateImpliedRelationships(relationship);
                    }
                }

                return relationship;
            }

            return null;
        }

        private bool IsChildOf(Element e1, Element e2)
        {
            if (e1 is Person || e2 is Person) {
                return false;
            }

            Element parent = e2.Parent;
            while (parent != null)
            {
                if (parent.Id.Equals(e1.Id))
                {
                    return true;
                }

                parent = parent.Parent;
            }

            return false;
        }

        private bool AddRelationship(Relationship relationship)
        {
            if (!relationship.Source.Has(relationship))
            {
                relationship.Id = IdGenerator.GenerateId(relationship);
                relationship.Source.AddRelationship(relationship);

                AddRelationshipToInternalStructures(relationship);
                return true;
            }

            return false;
        }

        private void AddRelationshipToInternalStructures(Relationship relationship)
        {
            _relationshipsById.Add(relationship.Id, relationship);
            IdGenerator.Found(relationship.Id);
        }

        /// <summary>
        /// Updates the identifier associated with an existing relationship and refreshes internal lookups.
        /// </summary>
        /// <param name="relationship">The relationship whose ID should change.</param>
        /// <param name="newId">The new identifier value.</param>
        /// <exception cref="ArgumentException">Thrown when the relationship is null or when <paramref name="newId"/> is blank.</exception>
        internal void UpdateRelationshipId(Relationship relationship, string newId)
        {
            if (relationship == null)
            {
                throw new ArgumentException("A relationship must be specified.");
            }

            if (string.IsNullOrWhiteSpace(newId))
            {
                throw new ArgumentException("A new ID must be specified.");
            }

            if (!string.IsNullOrWhiteSpace(relationship.Id))
            {
                _relationshipsById.Remove(relationship.Id);
            }

            relationship.Id = newId;
            _relationshipsById.Add(newId, relationship);
            IdGenerator.Found(newId);
        }

        /// <summary>
        /// Provides a way for the description and technology to be modified on an existing relationship.
        /// </summary>
        /// <param name="relationship">a Relationship instance</param>
        /// <param name="description">the new description</param>
        /// <param name="technology">the new technology</param>
        public void ModifyRelationship(Relationship relationship, String description, String technology)
        {
            if (relationship == null)
            {
                throw new ArgumentException("A relationship must be specified.");
            }

            Relationship newRelationship = new Relationship(relationship.Source, relationship.Destination, description, technology, relationship.InteractionStyle, new string[0]);
            if (!relationship.Source.Has(newRelationship))
            {
                relationship.Description = description;
                relationship.Technology = technology;
            }
            else
            {
                throw new ArgumentException("This relationship exists already: " + newRelationship);
            }
        }

        /// <summary>
        /// Gets the SoftwareSystem instance with the specified name.
        /// </summary>
        /// <param name="name">The software system name to search for.</param>
        /// <returns>A SoftwareSystem instance, or null if one doesn't exist.</returns>
        public SoftwareSystem GetSoftwareSystemWithName(string name)
        {
            foreach (SoftwareSystem softwareSystem in _softwareSystems)
            {
                if (softwareSystem.Name == name)
                {
                    return softwareSystem;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the software system with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the software system to find.</param>
        /// <returns>The matching software system, or <see langword="null"/> if one does not exist.</returns>
        public SoftwareSystem GetSoftwareSystemWithId(string id)
        {
            foreach (SoftwareSystem softwareSystem in _softwareSystems)
            {
                if (softwareSystem.Id == id)
                {
                    return softwareSystem;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the Person instance with the specified name.
        /// </summary>
        /// <param name="name">The person name to search for.</param>
        /// <returns>A Person instance, or null if one doesn't exist.</returns>
        public Person GetPersonWithName(string name)
        {
            foreach (Person person in _people)
            {
                if (person.Name == name)
                {
                    return person;
                }
            }

            return null;
        }

        private void AddElementToInternalStructures(Element element)
        {
            _elementsById.Add(element.Id, element);
            element.Model = this;
            IdGenerator.Found(element.Id);
        }

        /// <summary>
        /// Updates the identifier associated with an existing element and refreshes internal lookups.
        /// </summary>
        /// <param name="element">The element whose ID should change.</param>
        /// <param name="newId">The new identifier value.</param>
        /// <exception cref="ArgumentException">Thrown when the element is null or when <paramref name="newId"/> is blank.</exception>
        internal void UpdateElementId(Element element, string newId)
        {
            if (element == null)
            {
                throw new ArgumentException("An element must be specified.");
            }

            if (string.IsNullOrWhiteSpace(newId))
            {
                throw new ArgumentException("A new ID must be specified.");
            }

            if (!string.IsNullOrWhiteSpace(element.Id))
            {
                _elementsById.Remove(element.Id);
            }

            element.Id = newId;
            _elementsById.Add(newId, element);
            IdGenerator.Found(newId);
        }

        /// <summary>
        /// Determines whether the model already contains the specified element instance.
        /// </summary>
        /// <param name="element">The element to look for.</param>
        /// <returns><see langword="true"/> when the element is registered in the model; otherwise, <see langword="false"/>.</returns>
        public bool Contains(Element element)
        {
            return _elementsById.Values.Contains(element);
        }

        /// <summary>
        /// Rebuilds internal lookup structures after a model has been deserialized.
        /// </summary>
        internal void Hydrate()
        {
            
            // add all of the elements to the model
            foreach (Person person in _people)
            {
                AddElementToInternalStructures(person);
            }

            foreach (SoftwareSystem softwareSystem in _softwareSystems)
            {
                AddElementToInternalStructures(softwareSystem);
                foreach (Container container in softwareSystem.Containers)
                {
                    softwareSystem.Add(container);
                    AddElementToInternalStructures(container);
                    container.Parent = softwareSystem;
                    foreach (Component component in container.Components)
                    {
                        container.Add(component);
                        AddElementToInternalStructures(component);
                        component.Parent = container;
                    }
                }
            }

            _deploymentNodes.ToList().ForEach(dn => HydrateDeploymentNode(dn, null));

            // now hydrate the relationships
            GetElements().ToList().ForEach(HydrateRelationships);
        }

        private void HydrateDeploymentNode(DeploymentNode deploymentNode, DeploymentNode parent)
        {
            deploymentNode.Parent = parent;
            AddElementToInternalStructures(deploymentNode);

            deploymentNode.Children.ToList().ForEach(child => HydrateDeploymentNode(child, deploymentNode));

            foreach (InfrastructureNode infrastructureNode in deploymentNode.InfrastructureNodes)
            {
                infrastructureNode.Parent = deploymentNode;
                AddElementToInternalStructures(infrastructureNode);
            }

            foreach (SoftwareSystemInstance softwareSystemInstance in deploymentNode.SoftwareSystemInstances)
            {
                softwareSystemInstance.SoftwareSystem = (SoftwareSystem)GetElement(softwareSystemInstance.SoftwareSystemId);
                softwareSystemInstance.Parent = deploymentNode;
                AddElementToInternalStructures(softwareSystemInstance);
            }

            foreach (ContainerInstance containerInstance in deploymentNode.ContainerInstances)
            {
                containerInstance.Container = (Container)GetElement(containerInstance.ContainerId);
                containerInstance.Parent = deploymentNode;
                AddElementToInternalStructures(containerInstance);
            }
        }
        
        private void HydrateRelationships(Element element)
        {
            foreach (Relationship relationship in element.Relationships)
            {
                relationship.Source = GetElement(relationship.SourceId);
                relationship.Destination = GetElement(relationship.DestinationId);
                AddRelationshipToInternalStructures(relationship);
            }
        }

        /// <summary>
        /// Gets an element by identifier.
        /// </summary>
        /// <param name="id">The element identifier.</param>
        /// <returns>The matching element.</returns>
        public Element GetElement(string id)
        {
            return _elementsById[id];
        }

        /// <summary>
        /// Gets the element with the specified canonical name.
        /// </summary>
        /// <param name="canonicalName">the canonical name (e.g. /SoftwareSystem/Container)</param>
        /// <returns>the Element with the given canonical name, or null if one doesn't exist</returns>
        public Element GetElementWithCanonicalName(string canonicalName)
        {
            if (string.IsNullOrWhiteSpace(canonicalName))
            {
                throw new ArgumentException("A canonical name must be specified.");
            }

            return _elementsById.Values.FirstOrDefault(x => x.CanonicalName == canonicalName);
        }

        /// <summary>
        /// Returns all elements currently registered in the model.
        /// </summary>
        /// <returns>An enumeration of model elements.</returns>
        public IEnumerable<Element> GetElements()
        {
            return _elementsById.Values;
        }

        /// <summary>
        /// Gets a relationship by identifier.
        /// </summary>
        /// <param name="id">The relationship identifier.</param>
        /// <returns>The matching relationship.</returns>
        public Relationship GetRelationship(string id)
        {
            return _relationshipsById[id];
        }
        
        /// <summary>
        /// Propagates all relationships from children to their parents. For example, if you have two components (AAA and BBB)
        /// in different software systems that have a relationship, calling this method will add the following
        /// additional implied relationships to the model: AAA-&gt;BB AAA--&gt;B AA-&gt;BBB AA-&gt;BB AA-&gt;B A-&gt;BBB A-&gt;BB A-&gt;B.
        /// </summary>
        /// <returns>a set of all implicit relationships</returns>
        public ISet<Relationship> AddImplicitRelationships()
        {
            ISet<Relationship> implicitRelationships = new HashSet<Relationship>();

            string descriptionKey = "D";
            string technologyKey = "T";
            
            // source element -> destination element -> D/T -> possible values
            Dictionary<Element, Dictionary<Element, Dictionary<string, HashSet<string>>>> candidateRelationships = new Dictionary<Element, Dictionary<Element, Dictionary<string, HashSet<string>>>>();
    
            foreach (Relationship relationship in Relationships)
            {
                Element source = relationship.Source;
                Element destination = relationship.Destination;
    
                while (source != null)
                {
                    while (destination != null)
                    {
                        if (!source.HasEfferentRelationshipWith(destination))
                        {
                            if (propagatedRelationshipIsAllowed(source, destination))
                            {
    
                                if (!candidateRelationships.ContainsKey(source)) 
                                {
                                    candidateRelationships.Add(source, new Dictionary<Element, Dictionary<string, HashSet<string>>>());
                                }
    
                                if (!candidateRelationships[source].ContainsKey(destination))
                                {
                                    candidateRelationships[source].Add(destination, new Dictionary<string, HashSet<string>>());
                                    candidateRelationships[source][destination].Add(descriptionKey, new HashSet<string>());
                                    candidateRelationships[source][destination].Add(technologyKey, new HashSet<string>());
                                }
    
                                if (relationship.Description != null)
                                {
                                    candidateRelationships[source][destination][descriptionKey].Add(relationship.Description);
                                }
    
                                if (relationship.Technology != null)
                                {
                                    candidateRelationships[source][destination][technologyKey].Add(relationship.Technology);
                                }
                            }
                        }
    
                        destination = destination.Parent;
                    }
    
                    destination = relationship.Destination;
                    source = source.Parent;
                }
            }
    
            foreach (Element source in candidateRelationships.Keys)
            {
                foreach (Element destination in candidateRelationships[source].Keys)
                {
                    ISet<string> possibleDescriptions = candidateRelationships[source][destination][descriptionKey];
                    ISet<string> possibleTechnologies = candidateRelationships[source][destination][technologyKey];
    
                    string description = "";
                    if (possibleDescriptions.Count == 1)
                    {
                        description = possibleDescriptions.First();
                    }
    
                    string technology = "";
                    if (possibleTechnologies.Count == 1)
                    {
                        technology = possibleTechnologies.First();
                    }
    
                    Relationship implicitRelationship = AddRelationship(source, destination, description, technology);
                    if (implicitRelationship != null)
                    {
                        implicitRelationships.Add(implicitRelationship);
                    }
                }
            }
    
            return implicitRelationships;
        }

        private bool propagatedRelationshipIsAllowed(Element source, Element destination)
        {
            if (source.Equals(destination))
            {
                return false;
            }

            return !(IsChildOf(source, destination) || IsChildOf(destination, source));
        }

    }

}