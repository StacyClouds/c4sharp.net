using Xunit;
using Shouldly;

namespace StacyClouds.C4Sharp.Core.Tests
{
    public class SoftwareSystemTests
    {
        private Workspace workspace;
        private Model model;
        private SoftwareSystem softwareSystem;

        public SoftwareSystemTests()
        {
            workspace = new Workspace("Name", "Description");
            model = workspace.Model;
            softwareSystem = model.AddSoftwareSystem("Software System", "Description");
        }

        [Fact]
        public void Test_AddContainer_WithNameOnly_AddsContainerWithEmptyDescriptionAndTechnology()
        {
            Container container = softwareSystem.AddContainer("Container");
            container.ShouldNotBeNull();
            container.Name.ShouldBe("Container");
            container.Description.ShouldBe("");
            container.Technology.ShouldBe("");
        }

        [Fact]
        public void Test_AddContainer_WithNameAndDescription_AddsContainerWithEmptyTechnology()
        {
            Container container = softwareSystem.AddContainer("Container", "Description");
            container.ShouldNotBeNull();
            container.Name.ShouldBe("Container");
            container.Description.ShouldBe("Description");
            container.Technology.ShouldBe("");
        }

        [Fact]
        public void Test_GetContainerWithName_ReturnsNull_WhenContainerDoesNotExist()
        {
            softwareSystem.GetContainerWithName("Nonexistent").ShouldBeNull();
        }

        [Fact]
        public void Test_GetContainerWithName_ReturnsContainer_WhenContainerExists()
        {
            Container container = softwareSystem.AddContainer("MyContainer", "Description", "Technology");
            softwareSystem.GetContainerWithName("MyContainer").ShouldBeSameAs(container);
        }

        [Fact]
        public void Test_GetContainerWithId_ReturnsNull_WhenContainerDoesNotExist()
        {
            softwareSystem.GetContainerWithId("999").ShouldBeNull();
        }

        [Fact]
        public void Test_GetContainerWithId_ReturnsContainer_WhenContainerExists()
        {
            Container container = softwareSystem.AddContainer("Container", "Description", "Technology");
            softwareSystem.GetContainerWithId(container.Id).ShouldBeSameAs(container);
        }

        [Fact]
        public void Test_RemoveTags_DoesNotRemoveRequiredTags()
        {
            softwareSystem.Tags.ShouldContain(Tags.Element);
            softwareSystem.Tags.ShouldContain(Tags.SoftwareSystem);

            softwareSystem.RemoveTag(Tags.SoftwareSystem);
            softwareSystem.RemoveTag(Tags.Element);

            softwareSystem.Tags.ShouldContain(Tags.Element);
            softwareSystem.Tags.ShouldContain(Tags.SoftwareSystem);
        }

        [Fact]
        public void Test_Parent_ReturnsNull()
        {
            softwareSystem.Parent.ShouldBeNull();
        }
    }
}
