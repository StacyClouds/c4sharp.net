using Xunit;
using Shouldly;

namespace Structurizr.Core.Tests
{
    public class RelationshipViewTests : AbstractTestBase
    {
        private SoftwareSystem softwareSystem1;
        private SoftwareSystem softwareSystem2;
        private Relationship relationship;

        public RelationshipViewTests()
        {
            softwareSystem1 = Model.AddSoftwareSystem("Software System 1", "Description");
            softwareSystem2 = Model.AddSoftwareSystem("Software System 2", "Description");
            relationship = softwareSystem1.Uses(softwareSystem2, "Uses");
        }

        [Fact]
        public void Test_Position_IsClampedToZero_WhenNegativeValueIsSet()
        {
            RelationshipView view = new RelationshipView(relationship);
            view.Position = -10;
            view.Position.ShouldBe(0);
        }

        [Fact]
        public void Test_Position_IsClampedTo100_WhenValueExceeds100()
        {
            RelationshipView view = new RelationshipView(relationship);
            view.Position = 150;
            view.Position.ShouldBe(100);
        }

        [Fact]
        public void Test_Position_IsSetCorrectly_WhenValueIsWithinRange()
        {
            RelationshipView view = new RelationshipView(relationship);
            view.Position = 50;
            view.Position.ShouldBe(50);
        }

        [Fact]
        public void Test_Position_RemainsNull_WhenNotSet()
        {
            RelationshipView view = new RelationshipView(relationship);
            view.Position.ShouldBeNull();
        }

        [Fact]
        public void Test_Equals_ReturnsFalse_WhenComparingWithNull()
        {
            RelationshipView view = new RelationshipView(relationship);
            view.Equals(null).ShouldBeFalse();
        }

        [Fact]
        public void Test_Equals_ReturnsTrue_WhenComparingWithSelf()
        {
            RelationshipView view = new RelationshipView(relationship);
            view.Equals(view).ShouldBeTrue();
        }

        [Fact]
        public void Test_Equals_ReturnsTrue_WhenTwoViewsHaveTheSameIdAndOrderAndDescription()
        {
            RelationshipView view1 = new RelationshipView(relationship);
            view1.Order = "1";
            view1.Description = "desc";

            RelationshipView view2 = new RelationshipView(relationship);
            view2.Order = "1";
            view2.Description = "desc";

            view1.Equals(view2).ShouldBeTrue();
        }

        [Fact]
        public void Test_Equals_ReturnsFalse_WhenDescriptionsDiffer()
        {
            RelationshipView view1 = new RelationshipView(relationship);
            view1.Description = "desc1";

            RelationshipView view2 = new RelationshipView(relationship);
            view2.Description = "desc2";

            view1.Equals(view2).ShouldBeFalse();
        }

        [Fact]
        public void Test_Equals_ReturnsFalse_WhenOrdersDiffer()
        {
            RelationshipView view1 = new RelationshipView(relationship);
            view1.Order = "1";

            RelationshipView view2 = new RelationshipView(relationship);
            view2.Order = "2";

            view1.Equals(view2).ShouldBeFalse();
        }

        [Fact]
        public void Test_CopyLayoutInformationFrom_CopiesVerticesRoutingAndPosition()
        {
            RelationshipView source = new RelationshipView(relationship);
            source.Routing = Routing.Orthogonal;
            source.Position = 75;

            RelationshipView destination = new RelationshipView(relationship);
            destination.CopyLayoutInformationFrom(source);

            destination.Routing.ShouldBe(Routing.Orthogonal);
            destination.Position.ShouldBe(75);
        }
    }
}
