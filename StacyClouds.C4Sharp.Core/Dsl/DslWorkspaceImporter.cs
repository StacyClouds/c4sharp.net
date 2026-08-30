using System;
using System.Collections.Generic;
using System.Linq;

namespace StacyClouds.C4Sharp.Dsl
{
    /// <summary>
    /// Imports simplified DSL-shaped workspace definitions into the core workspace model.
    /// </summary>
    public static class DslWorkspaceImporter
    {
        /// <summary>
        /// Builds a workspace from a DSL-shaped source model and view definition.
        /// </summary>
        /// <param name="source">The DSL workspace definition to import.</param>
        /// <param name="options">Optional import settings that influence model creation.</param>
        /// <returns>A hydrated workspace built from the DSL source.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when referenced elements cannot be resolved during import.</exception>
        public static Workspace Import(DslWorkspace source, DslImportOptions options = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            options ??= new DslImportOptions();

            Workspace workspace = new Workspace(source.Name, source.Description);
            DslIdGenerator idGenerator = new DslIdGenerator();
            workspace.Model.IdGenerator = idGenerator;
            workspace.Model.ImpliedRelationshipsStrategy = options.ImpliedRelationshipsStrategy ?? new DefaultImpliedRelationshipsStrategy();

            Dictionary<string, Element> elementsById = new Dictionary<string, Element>();

            if (source.Model != null)
            {
                if (!string.IsNullOrWhiteSpace(source.Model.Enterprise))
                {
                    workspace.Model.Enterprise = new Enterprise(source.Model.Enterprise);
                }

                ImportPeople(source.Model.People, workspace, idGenerator, elementsById);
                ImportSoftwareSystems(source.Model.SoftwareSystems, workspace, idGenerator, elementsById);
                ImportRelationships(source.Model.Relationships, workspace, idGenerator, elementsById);
            }

            ImportViews(source.Views, workspace, elementsById);

            return workspace;
        }

        private static void ImportPeople(IEnumerable<DslPerson> people, Workspace workspace, DslIdGenerator idGenerator, IDictionary<string, Element> elementsById)
        {
            foreach (DslPerson person in people ?? Enumerable.Empty<DslPerson>())
            {
                Person modelPerson = workspace.Model.AddPerson(person.Location ?? Location.Unspecified, person.Name, person.Description);
                ApplyExplicitId(workspace.Model, modelPerson, person.Id, idGenerator);
                elementsById[modelPerson.Id] = modelPerson;
            }
        }

        private static void ImportSoftwareSystems(IEnumerable<DslSoftwareSystem> softwareSystems, Workspace workspace, DslIdGenerator idGenerator, IDictionary<string, Element> elementsById)
        {
            foreach (DslSoftwareSystem softwareSystem in softwareSystems ?? Enumerable.Empty<DslSoftwareSystem>())
            {
                SoftwareSystem modelSoftwareSystem = workspace.Model.AddSoftwareSystem(softwareSystem.Location ?? Location.Unspecified, softwareSystem.Name, softwareSystem.Description);
                ApplyExplicitId(workspace.Model, modelSoftwareSystem, softwareSystem.Id, idGenerator);
                elementsById[modelSoftwareSystem.Id] = modelSoftwareSystem;

                ImportContainers(softwareSystem.Containers, workspace.Model, modelSoftwareSystem, idGenerator, elementsById);
            }
        }

        private static void ImportContainers(IEnumerable<DslContainer> containers, Model model, SoftwareSystem softwareSystem, DslIdGenerator idGenerator, IDictionary<string, Element> elementsById)
        {
            foreach (DslContainer container in containers ?? Enumerable.Empty<DslContainer>())
            {
                Container modelContainer = softwareSystem.AddContainer(container.Name, container.Description, container.Technology);
                ApplyExplicitId(model, modelContainer, container.Id, idGenerator);
                elementsById[modelContainer.Id] = modelContainer;

                ImportComponents(container.Components, model, modelContainer, idGenerator, elementsById);
            }
        }

        private static void ImportComponents(IEnumerable<DslComponent> components, Model model, Container container, DslIdGenerator idGenerator, IDictionary<string, Element> elementsById)
        {
            foreach (DslComponent component in components ?? Enumerable.Empty<DslComponent>())
            {
                Component modelComponent = container.AddComponent(component.Name, component.Description, component.Technology);
                ApplyExplicitId(model, modelComponent, component.Id, idGenerator);
                elementsById[modelComponent.Id] = modelComponent;
            }
        }

        private static void ImportRelationships(IEnumerable<DslRelationship> relationships, Workspace workspace, DslIdGenerator idGenerator, IDictionary<string, Element> elementsById)
        {
            foreach (DslRelationship relationship in relationships ?? Enumerable.Empty<DslRelationship>())
            {
                Element source = ResolveElement(elementsById, relationship.SourceId);
                Element destination = ResolveElement(elementsById, relationship.DestinationId);

                Relationship modelRelationship = workspace.Model.AddRelationship(source, destination, relationship.Description, relationship.Technology, relationship.InteractionStyle, relationship.Tags?.ToArray() ?? new string[0]);
                ApplyExplicitId(workspace.Model, modelRelationship, relationship.Id, idGenerator);
            }
        }

        private static void ImportViews(DslViews views, Workspace workspace, IDictionary<string, Element> elementsById)
        {
            if (views == null)
            {
                return;
            }

            ImportSystemLandscapeViews(views.SystemLandscapeViews, workspace, elementsById);
            ImportSystemContextViews(views.SystemContextViews, workspace, elementsById);
            ImportContainerViews(views.ContainerViews, workspace, elementsById);
            ImportComponentViews(views.ComponentViews, workspace, elementsById);
        }

        private static void ImportSystemLandscapeViews(IEnumerable<DslSystemLandscapeView> views, Workspace workspace, IDictionary<string, Element> elementsById)
        {
            foreach (DslSystemLandscapeView view in views ?? Enumerable.Empty<DslSystemLandscapeView>())
            {
                SystemLandscapeView modelView = workspace.Views.CreateSystemLandscapeView(view.Key, view.Description);
                AddElementsToView(modelView, view.ElementIds, elementsById);
            }
        }

        private static void ImportSystemContextViews(IEnumerable<DslSystemContextView> views, Workspace workspace, IDictionary<string, Element> elementsById)
        {
            foreach (DslSystemContextView view in views ?? Enumerable.Empty<DslSystemContextView>())
            {
                SoftwareSystem softwareSystem = (SoftwareSystem)ResolveElement(elementsById, view.SoftwareSystemId);
                SystemContextView modelView = workspace.Views.CreateSystemContextView(softwareSystem, view.Key, view.Description);
                AddElementsToView(modelView, view.ElementIds, elementsById);
            }
        }

        private static void ImportContainerViews(IEnumerable<DslContainerView> views, Workspace workspace, IDictionary<string, Element> elementsById)
        {
            foreach (DslContainerView view in views ?? Enumerable.Empty<DslContainerView>())
            {
                SoftwareSystem softwareSystem = (SoftwareSystem)ResolveElement(elementsById, view.SoftwareSystemId);
                ContainerView modelView = workspace.Views.CreateContainerView(softwareSystem, view.Key, view.Description);
                AddElementsToView(modelView, view.ElementIds, elementsById);
            }
        }

        private static void ImportComponentViews(IEnumerable<DslComponentView> views, Workspace workspace, IDictionary<string, Element> elementsById)
        {
            foreach (DslComponentView view in views ?? Enumerable.Empty<DslComponentView>())
            {
                Container container = (Container)ResolveElement(elementsById, view.ContainerId);
                ComponentView modelView = workspace.Views.CreateComponentView(container, view.Key, view.Description);
                AddElementsToView(modelView, view.ElementIds, elementsById);
            }
        }

        private static void AddElementsToView(View view, IEnumerable<string> elementIds, IDictionary<string, Element> elementsById)
        {
            foreach (string elementId in elementIds ?? Enumerable.Empty<string>())
            {
                Element element = ResolveElement(elementsById, elementId);

                if (view is SystemLandscapeView systemLandscapeView)
                {
                    if (element is Person person)
                    {
                        systemLandscapeView.Add(person);
                    }
                    else if (element is SoftwareSystem softwareSystem)
                    {
                        systemLandscapeView.Add(softwareSystem);
                    }
                }
                else if (view is SystemContextView systemContextView)
                {
                    if (element is Person person)
                    {
                        systemContextView.Add(person);
                    }
                    else if (element is SoftwareSystem softwareSystem)
                    {
                        systemContextView.Add(softwareSystem);
                    }
                }
                else if (view is ContainerView containerView)
                {
                    if (element is Person person)
                    {
                        containerView.Add(person);
                    }
                    else if (element is SoftwareSystem softwareSystem)
                    {
                        containerView.Add(softwareSystem);
                    }
                    else if (element is Container container)
                    {
                        containerView.Add(container);
                    }
                }
                else if (view is ComponentView componentView)
                {
                    if (element is Person person)
                    {
                        componentView.Add(person);
                    }
                    else if (element is SoftwareSystem softwareSystem)
                    {
                        componentView.Add(softwareSystem);
                    }
                    if (element is Container container)
                    {
                        if (!container.Equals(componentView.Container))
                        {
                            componentView.Add(container);
                        }
                    }
                    else if (element is Component component)
                    {
                        componentView.Add(component);
                    }
                }
            }
        }

        private static void ApplyExplicitId(Model model, ModelItem item, string explicitId, DslIdGenerator idGenerator)
        {
            if (!string.IsNullOrWhiteSpace(explicitId))
            {
                if (item is Relationship relationship)
                {
                    model.UpdateRelationshipId(relationship, explicitId);
                }
                else if (item is Element element)
                {
                    model.UpdateElementId(element, explicitId);
                }
                else
                {
                    item.Id = explicitId;
                }

                idGenerator.Found(explicitId);
            }
        }

        private static Element ResolveElement(IDictionary<string, Element> elementsById, string elementId)
        {
            if (string.IsNullOrWhiteSpace(elementId))
            {
                throw new ArgumentException("A referenced element identifier must be specified.");
            }

            if (!elementsById.TryGetValue(elementId, out Element element))
            {
                throw new ArgumentException("Unable to resolve DSL element with identifier '" + elementId + "'.");
            }

            return element;
        }
    }
}
