using System.Linq;
using System;
using StacyClouds.C4Sharp.Dsl;
using Xunit;

namespace StacyClouds.C4Sharp.Core.Tests.Dsl
{
    public class DslWorkspaceImporterTests
    {
        [Fact]
        public void Import_creates_workspace_model_and_view_objects()
        {
            DslWorkspace source = CreateBaseWorkspace();

            Workspace workspace = DslWorkspaceImporter.Import(source);

            Assert.Equal("Customer Portal", workspace.Name);
            Assert.Equal("Portal workspace", workspace.Description);
            Assert.NotNull(workspace.Model.GetPersonWithName("User"));
            Assert.NotNull(workspace.Model.GetSoftwareSystemWithName("Portal"));
            Assert.Single(workspace.Views.SystemLandscapeViews);
            Assert.Single(workspace.Views.SystemContextViews);

            SystemLandscapeView landscapeView = workspace.Views.SystemLandscapeViews.Single();
            SystemContextView contextView = workspace.Views.SystemContextViews.Single();
            Assert.Equal(3, landscapeView.Elements.Count);
            Assert.Equal(3, contextView.Elements.Count);
            Assert.Single(contextView.Relationships);
            Assert.Equal(Location.Internal, workspace.Model.GetPersonWithName("User").Location);
            Assert.Equal(Location.External, workspace.Model.GetSoftwareSystemWithName("Portal").Location);
            Assert.Equal("Acme", workspace.Model.Enterprise.Name);
        }

        [Fact]
        public void Import_preserves_explicit_ids_and_generates_stable_ids_for_missing_ids()
        {
            DslWorkspace source = new DslWorkspace
            {
                Name = "Customer Portal",
                Description = "Portal workspace",
                Model =
                {
                    Enterprise = "Acme"
                }
            };

            source.Model.People.Add(new DslPerson
            {
                Id = "user-person",
                Name = "User",
                Description = "A portal user",
                Location = Location.Internal
            });

            source.Model.SoftwareSystems.Add(new DslSoftwareSystem
            {
                Id = "portal-system",
                Name = "Portal",
                Description = "The portal",
                Location = Location.External,
                Containers =
                {
                    new DslContainer
                    {
                        Name = "API",
                        Description = "Public API",
                        Technology = ".NET"
                    }
                }
            });

            source.Model.Relationships.Add(new DslRelationship
            {
                Id = "uses-relationship",
                SourceId = "user-person",
                DestinationId = "portal-system",
                Description = "Uses",
                Tags = { "critical" }
            });

            Workspace firstImport = DslWorkspaceImporter.Import(source);
            Workspace secondImport = DslWorkspaceImporter.Import(source);

            Assert.Equal("user-person", firstImport.Model.GetPersonWithName("User").Id);
            Assert.Equal("portal-system", firstImport.Model.GetSoftwareSystemWithName("Portal").Id);
            Assert.Equal("uses-relationship", firstImport.Model.GetRelationship("uses-relationship").Id);
            Assert.Contains("critical", firstImport.Model.GetRelationship("uses-relationship").GetAllTags());

            string firstGeneratedId = firstImport.Model.GetSoftwareSystemWithName("Portal").GetContainerWithName("API").Id;
            string secondGeneratedId = secondImport.Model.GetSoftwareSystemWithName("Portal").GetContainerWithName("API").Id;

            Assert.False(string.IsNullOrWhiteSpace(firstGeneratedId));
            Assert.Equal(firstGeneratedId, secondGeneratedId);
        }

        [Fact]
        public void Import_preserves_explicit_container_and_component_ids_and_imports_views()
        {
            DslWorkspace source = new DslWorkspace
            {
                Name = "Customer Portal",
                Description = "Portal workspace"
            };

            source.Model.SoftwareSystems.Add(new DslSoftwareSystem
            {
                Id = "portal-system",
                Name = "Portal",
                Containers =
                {
                    new DslContainer
                    {
                        Id = "portal-web",
                        Name = "Web",
                        Components =
                        {
                            new DslComponent
                            {
                                Id = "portal-web-controller",
                                Name = "Controller"
                            }
                        }
                    }
                }
            });

            source.Views.ContainerViews.Add(new DslContainerView
            {
                Key = "containers",
                Description = "Container view",
                SoftwareSystemId = "portal-system",
                ElementIds = { "portal-web" }
            });

            source.Views.ComponentViews.Add(new DslComponentView
            {
                Key = "components",
                Description = "Component view",
                ContainerId = "portal-web",
                ElementIds = { "portal-web", "portal-web-controller" }
            });

            Workspace workspace = DslWorkspaceImporter.Import(source);

            Assert.Equal("portal-web", workspace.Model.GetSoftwareSystemWithName("Portal").GetContainerWithName("Web").Id);
            Assert.Equal("portal-web-controller", workspace.Model.GetSoftwareSystemWithName("Portal").GetContainerWithName("Web").GetComponentWithName("Controller").Id);
            Assert.Single(workspace.Views.ContainerViews);
            Assert.Single(workspace.Views.ComponentViews);
            Assert.Equal(1, workspace.Views.ContainerViews.Single().Elements.Count);
            Assert.Equal(1, workspace.Views.ComponentViews.Single().Elements.Count);
        }

        [Fact]
        public void Import_applies_implied_relationship_strategy()
        {
            DslWorkspace source = new DslWorkspace
            {
                Name = "Billing",
                Description = "Billing workspace"
            };

            source.Model.SoftwareSystems.Add(new DslSoftwareSystem
            {
                Id = "billing-system",
                Name = "Billing",
                Containers =
                {
                    new DslContainer
                    {
                        Id = "billing-web",
                        Name = "Web",
                        Components =
                        {
                            new DslComponent
                            {
                                Id = "billing-web-controller",
                                Name = "Controller"
                            }
                        }
                    },
                    new DslContainer
                    {
                        Id = "billing-db",
                        Name = "Database",
                        Components =
                        {
                            new DslComponent
                            {
                                Id = "billing-db-repository",
                                Name = "Repository"
                            }
                        }
                    }
                }
            });

            source.Model.Relationships.Add(new DslRelationship
            {
                SourceId = "billing-web-controller",
                DestinationId = "billing-db-repository",
                Description = "Reads from"
            });

            Workspace withImpliedRelationships = DslWorkspaceImporter.Import(
                source,
                new DslImportOptions
                {
                    ImpliedRelationshipsStrategy = new CreateImpliedRelationshipsUnlessAnyRelationshipExistsStrategy()
                });

            Workspace withoutImpliedRelationships = DslWorkspaceImporter.Import(source);

            SoftwareSystem billingSystem = withImpliedRelationships.Model.GetSoftwareSystemWithName("Billing");
            Container webContainer = billingSystem.GetContainerWithName("Web");
            Container databaseContainer = billingSystem.GetContainerWithName("Database");

            Assert.True(webContainer.HasEfferentRelationshipWith(databaseContainer));
            Assert.False(withoutImpliedRelationships.Model.GetSoftwareSystemWithName("Billing").GetContainerWithName("Web").HasEfferentRelationshipWith(withoutImpliedRelationships.Model.GetSoftwareSystemWithName("Billing").GetContainerWithName("Database")));
        }

        [Fact]
        public void Import_throws_when_a_reference_cannot_be_resolved()
        {
            DslWorkspace source = new DslWorkspace
            {
                Name = "Broken",
                Description = "Broken workspace"
            };

            source.Model.SoftwareSystems.Add(new DslSoftwareSystem
            {
                Id = "system",
                Name = "System"
            });

            source.Model.Relationships.Add(new DslRelationship
            {
                SourceId = "system",
                DestinationId = "missing",
                Description = "Uses"
            });

            ArgumentException exception = Assert.Throws<ArgumentException>(() => DslWorkspaceImporter.Import(source));

            Assert.Contains("Unable to resolve DSL element", exception.Message);
        }

        private static DslWorkspace CreateBaseWorkspace()
        {
            DslWorkspace source = new DslWorkspace
            {
                Name = "Customer Portal",
                Description = "Portal workspace",
                Model =
                {
                    Enterprise = "Acme"
                }
            };

            source.Model.People.Add(new DslPerson
            {
                Id = "user-person",
                Name = "User",
                Description = "A portal user",
                Location = Location.Internal
            });

            source.Model.SoftwareSystems.Add(new DslSoftwareSystem
            {
                Id = "portal-system",
                Name = "Portal",
                Description = "The portal",
                Location = Location.External
            });

            source.Model.SoftwareSystems.Add(new DslSoftwareSystem
            {
                Id = "auth-system",
                Name = "Auth",
                Description = "Authentication"
            });

            source.Model.Relationships.Add(new DslRelationship
            {
                SourceId = "user-person",
                DestinationId = "portal-system",
                Description = "Uses",
                Tags = { "critical" }
            });

            source.Views.SystemLandscapeViews.Add(new DslSystemLandscapeView
            {
                Key = "landscape",
                Description = "Landscape view",
                ElementIds = { "user-person", "portal-system", "auth-system" }
            });

            source.Views.SystemContextViews.Add(new DslSystemContextView
            {
                Key = "context",
                Description = "Context view",
                SoftwareSystemId = "portal-system",
                ElementIds = { "user-person", "portal-system", "auth-system" }
            });

            return source;
        }
    }
}
