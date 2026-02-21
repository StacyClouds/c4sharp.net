using System;
using Xunit;
using Shouldly;

namespace Structurizr.Core.Tests
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

            SoftwareSystem destSoftwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            SystemContextView destView = Views.CreateSystemContextView(destSoftwareSystem, "key", "Description");
            destView.AddAllElements();

            Views.CopyLayoutInformationFrom(source.Views);
        }
    }
}
