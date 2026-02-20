using System;
using Xunit;
using Shouldly;

namespace StacyClouds.C4Sharp.Core.Tests
{
    public class ViewSetTests : AbstractTestBase
    {
        [Fact]
        public void Test_CreateSystemLandscapeView_CreatesAViewAndAddsItToTheSet()
        {
            SystemLandscapeView view = Views.CreateSystemLandscapeView("key", "Description");
            view.ShouldNotBeNull();
            view.Key.ShouldBe("key");
            view.Description.ShouldBe("Description");
            Views.SystemLandscapeViews.Count.ShouldBe(1);
            Views.SystemLandscapeViews.ShouldContain(view);
        }

        [Fact]
        public void Test_CreateSystemContextView_CreatesAViewAndAddsItToTheSet()
        {
            SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            SystemContextView view = Views.CreateSystemContextView(softwareSystem, "key", "Description");
            view.ShouldNotBeNull();
            view.Key.ShouldBe("key");
            view.Description.ShouldBe("Description");
            Views.SystemContextViews.Count.ShouldBe(1);
            Views.SystemContextViews.ShouldContain(view);
        }

        [Fact]
        public void Test_CreateContainerView_CreatesAViewAndAddsItToTheSet()
        {
            SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            ContainerView view = Views.CreateContainerView(softwareSystem, "key", "Description");
            view.ShouldNotBeNull();
            view.Key.ShouldBe("key");
            view.Description.ShouldBe("Description");
            Views.ContainerViews.Count.ShouldBe(1);
            Views.ContainerViews.ShouldContain(view);
        }

        [Fact]
        public void Test_CreateComponentView_CreatesAViewAndAddsItToTheSet()
        {
            SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            Container container = softwareSystem.AddContainer("Container", "Description", "Technology");
            ComponentView view = Views.CreateComponentView(container, "key", "Description");
            view.ShouldNotBeNull();
            view.Key.ShouldBe("key");
            view.Description.ShouldBe("Description");
            Views.ComponentViews.Count.ShouldBe(1);
            Views.ComponentViews.ShouldContain(view);
        }

        [Fact]
        public void Test_CreateDynamicView_WithNoScope_CreatesAViewAndAddsItToTheSet()
        {
            DynamicView view = Views.CreateDynamicView("key", "Description");
            view.ShouldNotBeNull();
            view.Key.ShouldBe("key");
            view.Description.ShouldBe("Description");
            Views.DynamicViews.Count.ShouldBe(1);
            Views.DynamicViews.ShouldContain(view);
        }

        [Fact]
        public void Test_CreateDynamicView_WithSoftwareSystemScope_CreatesAViewAndAddsItToTheSet()
        {
            SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            DynamicView view = Views.CreateDynamicView(softwareSystem, "key", "Description");
            view.ShouldNotBeNull();
            view.Key.ShouldBe("key");
            Views.DynamicViews.Count.ShouldBe(1);
            Views.DynamicViews.ShouldContain(view);
        }

        [Fact]
        public void Test_CreateDynamicView_WithContainerScope_CreatesAViewAndAddsItToTheSet()
        {
            SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            Container container = softwareSystem.AddContainer("Container", "Description", "Technology");
            DynamicView view = Views.CreateDynamicView(container, "key", "Description");
            view.ShouldNotBeNull();
            view.Key.ShouldBe("key");
            Views.DynamicViews.Count.ShouldBe(1);
            Views.DynamicViews.ShouldContain(view);
        }

        [Fact]
        public void Test_CreateDeploymentView_WithNoScope_CreatesAViewAndAddsItToTheSet()
        {
            DeploymentView view = Views.CreateDeploymentView("key", "Description");
            view.ShouldNotBeNull();
            view.Key.ShouldBe("key");
            view.Description.ShouldBe("Description");
            Views.DeploymentViews.Count.ShouldBe(1);
            Views.DeploymentViews.ShouldContain(view);
        }

        [Fact]
        public void Test_CreateDeploymentView_WithSoftwareSystemScope_CreatesAViewAndAddsItToTheSet()
        {
            SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            DeploymentView view = Views.CreateDeploymentView(softwareSystem, "key", "Description");
            view.ShouldNotBeNull();
            view.Key.ShouldBe("key");
            view.Description.ShouldBe("Description");
            Views.DeploymentViews.Count.ShouldBe(1);
            Views.DeploymentViews.ShouldContain(view);
        }

        [Fact]
        public void Test_CreateFilteredView_CreatesAFilteredViewAndAddsItToTheSet()
        {
            SystemLandscapeView view = Views.CreateSystemLandscapeView("landscape", "Description");
            FilteredView filteredView = Views.CreateFilteredView(view, "key", "Description", FilterMode.Include, "tag1");
            filteredView.ShouldNotBeNull();
            filteredView.Key.ShouldBe("key");
            Views.FilteredViews.Count.ShouldBe(1);
            Views.FilteredViews.ShouldContain(filteredView);
        }

        [Fact]
        public void Test_CreateView_ThrowsAnException_WhenTheDuplicateKeyIsUsedForSystemLandscapeView()
        {
            Views.CreateSystemLandscapeView("key", "Description");
            Should.Throw<ArgumentException>(() =>
                Views.CreateSystemLandscapeView("key", "Description 2")
            ).Message.ShouldContain("key");
        }

        [Fact]
        public void Test_CreateView_ThrowsAnException_WhenTheDuplicateKeyIsUsedForSystemContextView()
        {
            SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            Views.CreateSystemContextView(softwareSystem, "key", "Description");
            Should.Throw<ArgumentException>(() =>
                Views.CreateSystemContextView(softwareSystem, "key", "Description 2")
            ).Message.ShouldContain("key");
        }

        [Fact]
        public void Test_CreateView_ThrowsAnException_WhenTheDuplicateKeyIsUsedForFilteredView()
        {
            SystemLandscapeView view = Views.CreateSystemLandscapeView("landscape", "Description");
            Views.CreateFilteredView(view, "key", "Description", FilterMode.Include, "tag1");
            Should.Throw<ArgumentException>(() =>
                Views.CreateFilteredView(view, "key", "Description 2", FilterMode.Exclude, "tag2")
            ).Message.ShouldContain("key");
        }

        [Fact]
        public void Test_CreateDynamicView_ThrowsAnException_WhenSoftwareSystemIsNull()
        {
            Should.Throw<ArgumentException>(() =>
                Views.CreateDynamicView((SoftwareSystem)null, "key", "Description")
            ).Message.ShouldBe("Software system must not be null.");
        }

        [Fact]
        public void Test_CreateDynamicView_ThrowsAnException_WhenContainerIsNull()
        {
            Should.Throw<ArgumentException>(() =>
                Views.CreateDynamicView((Container)null, "key", "Description")
            ).Message.ShouldBe("Container must not be null.");
        }

        [Fact]
        public void Test_CreateDeploymentView_ThrowsAnException_WhenSoftwareSystemIsNull()
        {
            Should.Throw<ArgumentException>(() =>
                Views.CreateDeploymentView((SoftwareSystem)null, "key", "Description")
            ).Message.ShouldBe("Software system must not be null.");
        }

        [Fact]
        public void Test_GetViewWithKey_ThrowsAnException_WhenKeyIsNull()
        {
            Should.Throw<ArgumentException>(() =>
                Views.GetViewWithKey(null)
            ).Message.ShouldBe("A key must be specified.");
        }

        [Fact]
        public void Test_GetViewWithKey_ReturnsNull_WhenViewDoesNotExist()
        {
            Views.GetViewWithKey("nonexistent").ShouldBeNull();
        }

        [Fact]
        public void Test_GetViewWithKey_ReturnsSystemLandscapeView()
        {
            SystemLandscapeView view = Views.CreateSystemLandscapeView("key", "Description");
            Views.GetViewWithKey("key").ShouldBeSameAs(view);
        }

        [Fact]
        public void Test_GetViewWithKey_ReturnsSystemContextView()
        {
            SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            SystemContextView view = Views.CreateSystemContextView(softwareSystem, "key", "Description");
            Views.GetViewWithKey("key").ShouldBeSameAs(view);
        }

        [Fact]
        public void Test_GetViewWithKey_ReturnsContainerView()
        {
            SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            ContainerView view = Views.CreateContainerView(softwareSystem, "key", "Description");
            Views.GetViewWithKey("key").ShouldBeSameAs(view);
        }

        [Fact]
        public void Test_GetViewWithKey_ReturnsComponentView()
        {
            SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            Container container = softwareSystem.AddContainer("Container", "Description", "Technology");
            ComponentView view = Views.CreateComponentView(container, "key", "Description");
            Views.GetViewWithKey("key").ShouldBeSameAs(view);
        }

        [Fact]
        public void Test_GetViewWithKey_ReturnsDynamicView()
        {
            DynamicView view = Views.CreateDynamicView("key", "Description");
            Views.GetViewWithKey("key").ShouldBeSameAs(view);
        }

        [Fact]
        public void Test_GetViewWithKey_ReturnsDeploymentView()
        {
            DeploymentView view = Views.CreateDeploymentView("key", "Description");
            Views.GetViewWithKey("key").ShouldBeSameAs(view);
        }

        [Fact]
        public void Test_GetFilteredViewWithKey_ThrowsAnException_WhenKeyIsNull()
        {
            Should.Throw<ArgumentException>(() =>
                Views.GetFilteredViewWithKey(null)
            ).Message.ShouldBe("A key must be specified.");
        }

        [Fact]
        public void Test_GetFilteredViewWithKey_ReturnsNull_WhenViewDoesNotExist()
        {
            Views.GetFilteredViewWithKey("nonexistent").ShouldBeNull();
        }

        [Fact]
        public void Test_GetFilteredViewWithKey_ReturnsTheFilteredView()
        {
            SystemLandscapeView view = Views.CreateSystemLandscapeView("landscape", "Description");
            FilteredView filteredView = Views.CreateFilteredView(view, "key", "Description", FilterMode.Include, "tag1");
            Views.GetFilteredViewWithKey("key").ShouldBeSameAs(filteredView);
        }

        [Fact]
        public void Test_CopyLayoutInformationFrom_CopiesLayoutForMatchingViews()
        {
            Workspace source = new Workspace("Source", "Description");
            SoftwareSystem sourceSoftwareSystem = source.Model.AddSoftwareSystem("Software System", "Description");
            SystemContextView sourceView = source.Views.CreateSystemContextView(sourceSoftwareSystem, "key", "Description");
            sourceView.AddAllElements();
            foreach (ElementView ev in sourceView.Elements) { ev.X = 100; ev.Y = 200; }

            SoftwareSystem destSoftwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            SystemContextView destView = Views.CreateSystemContextView(destSoftwareSystem, "key", "Description");
            destView.AddAllElements();

            Views.CopyLayoutInformationFrom(source.Views);

            destView.Elements.Count.ShouldBeGreaterThan(0);
            foreach (ElementView ev in destView.Elements) { ev.X.ShouldBe(100); ev.Y.ShouldBe(200); }
        }

        [Fact]
        public void Test_GetViewWithKey_ReturnsScopedDeploymentView()
        {
            SoftwareSystem softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            DeploymentView view = Views.CreateDeploymentView(softwareSystem, "key", "Description");
            Views.GetViewWithKey("key").ShouldBeSameAs(view);
        }

        [Fact]
        public void Test_CopyLayoutInformationFrom_CopiesLayoutForContainerViews()
        {
            Workspace source = new Workspace("Source", "Description");
            SoftwareSystem sourceSoftwareSystem = source.Model.AddSoftwareSystem("Software System", "Description");
            sourceSoftwareSystem.AddContainer("Container", "Description", "Technology");
            ContainerView sourceView = source.Views.CreateContainerView(sourceSoftwareSystem, "key", "Description");
            sourceView.AddAllContainers();
            foreach (ElementView ev in sourceView.Elements) { ev.X = 100; ev.Y = 200; }

            SoftwareSystem destSoftwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            destSoftwareSystem.AddContainer("Container", "Description", "Technology");
            ContainerView destView = Views.CreateContainerView(destSoftwareSystem, "key", "Description");
            destView.AddAllContainers();

            Views.CopyLayoutInformationFrom(source.Views);

            destView.Elements.Count.ShouldBeGreaterThan(0);
            foreach (ElementView ev in destView.Elements) { ev.X.ShouldBe(100); ev.Y.ShouldBe(200); }
        }

        [Fact]
        public void Test_CopyLayoutInformationFrom_CopiesLayoutForComponentViews()
        {
            Workspace source = new Workspace("Source", "Description");
            SoftwareSystem sourceSoftwareSystem = source.Model.AddSoftwareSystem("Software System", "Description");
            Container sourceContainer = sourceSoftwareSystem.AddContainer("Container", "Description", "Technology");
            sourceContainer.AddComponent("Component", "Description", "Technology");
            ComponentView sourceView = source.Views.CreateComponentView(sourceContainer, "key", "Description");
            sourceView.AddAllComponents();
            foreach (ElementView ev in sourceView.Elements) { ev.X = 100; ev.Y = 200; }

            SoftwareSystem destSoftwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            Container destContainer = destSoftwareSystem.AddContainer("Container", "Description", "Technology");
            destContainer.AddComponent("Component", "Description", "Technology");
            ComponentView destView = Views.CreateComponentView(destContainer, "key", "Description");
            destView.AddAllComponents();

            Views.CopyLayoutInformationFrom(source.Views);

            destView.Elements.Count.ShouldBeGreaterThan(0);
            foreach (ElementView ev in destView.Elements) { ev.X.ShouldBe(100); ev.Y.ShouldBe(200); }
        }

        [Fact]
        public void Test_CopyLayoutInformationFrom_CopiesLayoutForDynamicViews()
        {
            Workspace source = new Workspace("Source", "Description");
            SoftwareSystem sourceSystemA = source.Model.AddSoftwareSystem("System A", "Description");
            SoftwareSystem sourceSystemB = source.Model.AddSoftwareSystem("System B", "Description");
            sourceSystemA.Uses(sourceSystemB, "uses");
            DynamicView sourceView = source.Views.CreateDynamicView("key", "Description");
            sourceView.Add(sourceSystemA, sourceSystemB);
            foreach (ElementView ev in sourceView.Elements) { ev.X = 100; ev.Y = 200; }

            SoftwareSystem destSystemA = Model.AddSoftwareSystem("System A", "Description");
            SoftwareSystem destSystemB = Model.AddSoftwareSystem("System B", "Description");
            destSystemA.Uses(destSystemB, "uses");
            DynamicView destView = Views.CreateDynamicView("key", "Description");
            destView.Add(destSystemA, destSystemB);

            Views.CopyLayoutInformationFrom(source.Views);

            destView.Elements.Count.ShouldBe(2);
            foreach (ElementView ev in destView.Elements) { ev.X.ShouldBe(100); ev.Y.ShouldBe(200); }
        }

        [Fact]
        public void Test_CopyLayoutInformationFrom_CopiesLayoutForDeploymentViews()
        {
            Workspace source = new Workspace("Source", "Description");
            SoftwareSystem sourceSoftwareSystem = source.Model.AddSoftwareSystem("Software System", "Description");
            Container sourceContainer = sourceSoftwareSystem.AddContainer("Container", "Description", "Technology");
            DeploymentNode sourceDeploymentNode = source.Model.AddDeploymentNode("Deployment Node", "Description", "Technology");
            sourceDeploymentNode.Add(sourceContainer);
            DeploymentView sourceView = source.Views.CreateDeploymentView("key", "Description");
            sourceView.Add(sourceDeploymentNode);
            foreach (ElementView ev in sourceView.Elements) { ev.X = 100; ev.Y = 200; }

            SoftwareSystem destSoftwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            Container destContainer = destSoftwareSystem.AddContainer("Container", "Description", "Technology");
            DeploymentNode destDeploymentNode = Model.AddDeploymentNode("Deployment Node", "Description", "Technology");
            destDeploymentNode.Add(destContainer);
            DeploymentView destView = Views.CreateDeploymentView("key", "Description");
            destView.Add(destDeploymentNode);

            Views.CopyLayoutInformationFrom(source.Views);

            destView.Elements.Count.ShouldBeGreaterThan(0);
            foreach (ElementView ev in destView.Elements) { ev.X.ShouldBe(100); ev.Y.ShouldBe(200); }
        }

        [Fact]
        public void Test_CopyLayoutInformationFrom_CopiesLayoutForSystemLandscapeViews()
        {
            Workspace source = new Workspace("Source", "Description");
            SystemLandscapeView sourceView = source.Views.CreateSystemLandscapeView("key", "Description");
            source.Model.AddSoftwareSystem("Software System", "Description");
            sourceView.AddAllElements();
            foreach (ElementView ev in sourceView.Elements) { ev.X = 50; ev.Y = 75; }

            Model.AddSoftwareSystem("Software System", "Description");
            SystemLandscapeView destView = Views.CreateSystemLandscapeView("key", "Description");
            destView.AddAllElements();

            Views.CopyLayoutInformationFrom(source.Views);

            destView.Elements.Count.ShouldBeGreaterThan(0);
            foreach (ElementView ev in destView.Elements) { ev.X.ShouldBe(50); ev.Y.ShouldBe(75); }
        }
    }
}
