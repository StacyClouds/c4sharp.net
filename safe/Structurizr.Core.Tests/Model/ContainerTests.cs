using Xunit;
using Shouldly;

namespace Structurizr.Core.Tests
{
    
    public class ContainerTests
    {

        private Workspace workspace;
        private Model model;
        private SoftwareSystem softwareSystem;
        private Container container;

        public ContainerTests()
        {
            workspace = new Workspace("Name", "Description");
            model = workspace.Model;
            softwareSystem = model.AddSoftwareSystem("System", "Description");
            container = softwareSystem.AddContainer("Container", "Description", "Some technology");
        }

        [Fact]
        public void Test_CanonicalName()
        {
            Assert.Equal("Container://System.Container", container.CanonicalName);
        }

        [Fact]
        public void Test_CanonicalName_WhenNameContainsASlashCharacter()
        {
            container.Name = "Name1/Name2";
            Assert.Equal("Container://System.Name1Name2", container.CanonicalName);
        }

        [Fact]
        public void Test_Parent_ReturnsTheParentSoftwareSystem()
        {
            Assert.Equal(softwareSystem, container.Parent);
        }

        [Fact]
        public void Test_SoftwareSystem_ReturnsTheParentSoftwareSystem()
        {
            Assert.Equal(softwareSystem, container.SoftwareSystem);
        }

        [Fact]
        public void Test_RemoveTags_DoesNotRemoveRequiredTags()
        {
            Assert.True(container.Tags.Contains(Tags.Element));
            Assert.True(container.Tags.Contains(Tags.Container));

            container.RemoveTag(Tags.Container);
            container.RemoveTag(Tags.Element);

            Assert.True(container.Tags.Contains(Tags.Element));
            Assert.True(container.Tags.Contains(Tags.Container));
        }

        [Fact]
        public void Test_AddComponent_WithNameOnly_AddsComponentWithEmptyDescriptionAndTechnology()
        {
            Component component = container.AddComponent("Component");
            component.ShouldNotBeNull();
            component.Name.ShouldBe("Component");
            component.Description.ShouldBe("");
            component.Technology.ShouldBe("");
        }

        [Fact]
        public void Test_AddComponent_WithNameAndDescription_AddsComponentWithEmptyTechnology()
        {
            Component component = container.AddComponent("Component", "Description");
            component.ShouldNotBeNull();
            component.Name.ShouldBe("Component");
            component.Description.ShouldBe("Description");
            component.Technology.ShouldBe("");
        }

        [Fact]
        public void Test_GetComponentWithName_ReturnsNull_WhenNameIsNull()
        {
            container.GetComponentWithName(null).ShouldBeNull();
        }

        [Fact]
        public void Test_GetComponentWithName_ReturnsNull_WhenComponentDoesNotExist()
        {
            container.GetComponentWithName("Nonexistent").ShouldBeNull();
        }

        [Fact]
        public void Test_GetComponentWithName_ReturnsComponent_WhenComponentExists()
        {
            Component component = container.AddComponent("MyComponent", "Description");
            container.GetComponentWithName("MyComponent").ShouldBeSameAs(component);
        }

        [Fact]
        public void Test_GetComponentOfType_ReturnsNull_WhenTypeIsNull()
        {
            container.GetComponentOfType(null).ShouldBeNull();
        }

        [Fact]
        public void Test_GetComponentOfType_ReturnsNull_WhenNoComponentWithTypeExists()
        {
            container.GetComponentOfType("com.example.SomeType").ShouldBeNull();
        }

        [Fact]
        public void Test_GetComponentOfType_ReturnsComponent_WhenComponentWithTypeExists()
        {
            Component component = container.AddComponent("Component", "com.example.MyClass, MyAssembly", "Description", "Technology");
            container.GetComponentOfType("com.example.MyClass, MyAssembly").ShouldBeSameAs(component);
        }

    }
}